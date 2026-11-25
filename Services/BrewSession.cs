using BrewBench.Models;

namespace BrewBench.Services;

public class BrewSession
{
    public Recipe? Recipe { get; set; }
    public CalcResult? Calc { get; set; }
    public int CurrentStepIndex { get; set; }
    public List<BrewStep> Steps { get; private set; } = new();
    public DateTime? StartedAt { get; set; }
    public DateTime? StepStartedAt { get; set; }
    public double? ActualMashTemp { get; set; }
    public double? ActualPreBoilGravity { get; set; }
    public double? ActualOg { get; set; }
    public double? ActualFg { get; set; }
    public string Notes { get; set; } = "";

    public event Action? Changed;
    public void NotifyChanged() => Changed?.Invoke();

    public void Begin(Recipe r, CalcResult calc)
    {
        Recipe = r;
        Calc = calc;
        Steps = BuildSteps(r, calc);
        CurrentStepIndex = 0;
        StartedAt = DateTime.Now;
        StepStartedAt = DateTime.Now;
        ActualMashTemp = null;
        ActualPreBoilGravity = null;
        ActualOg = null;
        ActualFg = null;
        Notes = "";
        NotifyChanged();
    }

    public void Advance()
    {
        if (CurrentStepIndex < Steps.Count - 1)
        {
            CurrentStepIndex++;
            StepStartedAt = DateTime.Now;
            NotifyChanged();
        }
    }

    public void GoBack()
    {
        if (CurrentStepIndex > 0)
        {
            CurrentStepIndex--;
            StepStartedAt = DateTime.Now;
            NotifyChanged();
        }
    }

    public BrewStep? Current => CurrentStepIndex < Steps.Count ? Steps[CurrentStepIndex] : null;

    private static List<BrewStep> BuildSteps(Recipe r, CalcResult c)
    {
        var s = new List<BrewStep>();
        s.Add(new BrewStep("Setup", BrewStepKind.Checklist,
            $"Gather ingredients, clean equipment, calibrate thermometer. Target batch: {r.BatchSizeGallons:F1} gal."));
        s.Add(new BrewStep("Heat strike water", BrewStepKind.Heat,
            $"Heat {c.StrikeWaterGallons:F2} gal of water to {c.StrikeWaterTempF:F1}°F.",
            targetTempF: c.StrikeWaterTempF));
        s.Add(new BrewStep("Dough in", BrewStepKind.Action,
            "Slowly stir grain into strike water. Avoid dough balls. Confirm mash temp."));

        foreach (var step in r.MashSchedule)
        {
            s.Add(new BrewStep($"Mash: {step.Name}", BrewStepKind.Rest,
                $"Hold at {step.TempF:F0}°F for {step.Minutes} min.",
                targetTempF: step.TempF, restMinutes: step.Minutes));
        }

        s.Add(new BrewStep("Sparge / Lauter", BrewStepKind.Action,
            $"Sparge with {c.SpargeWaterGallons:F2} gal at ~170°F. Collect to {c.PreBoilVolume:F2} gal pre-boil."));
        s.Add(new BrewStep("Pre-boil gravity", BrewStepKind.Reading,
            $"Take refractometer reading. Target pre-boil OG: {c.PreBoilOg:F4}."));
        s.Add(new BrewStep("Begin boil", BrewStepKind.Heat,
            $"Bring kettle to rolling boil. Boil duration: {r.BoilMinutes} min total."));

        // Build hop addition steps based on remaining boil time
        int totalBoil = (int)r.BoilMinutes;
        var hopSchedule = r.Hops
            .Where(h => h.Use == HopUse.Boil || h.Use == HopUse.FirstWort)
            .OrderByDescending(h => h.BoilMinutes)
            .ToList();
        var lateAdditions = r.Grains.Where(g => g.Use != GrainUse.Mash).ToList();

        // Generate timed boil additions in time-remaining order
        var boilEvents = new List<(int remaining, string label)>();
        foreach (var h in hopSchedule)
            boilEvents.Add((h.BoilMinutes, $"Hop: {h.Ounces:F2} oz {h.Name} ({h.AlphaAcid:F1}% AA)"));
        if (lateAdditions.Any(g => g.Use == GrainUse.LateBoil))
            boilEvents.Add((10, "Add late-boil ingredient(s): " + string.Join(", ",
                lateAdditions.Where(g => g.Use == GrainUse.LateBoil).Select(g => $"{g.Pounds:F2} lb {g.Name}"))));

        boilEvents = boilEvents.OrderByDescending(e => e.remaining).ToList();
        int lastRemaining = totalBoil;
        foreach (var ev in boilEvents)
        {
            int gap = lastRemaining - ev.remaining;
            if (gap > 0)
                s.Add(new BrewStep("Boil: wait", BrewStepKind.Wait,
                    $"Continue boil for {gap} min. {ev.remaining} min remaining at end.",
                    restMinutes: gap));
            s.Add(new BrewStep(ev.label, BrewStepKind.Action,
                $"Add now. ({ev.remaining} min remaining in boil.)"));
            lastRemaining = ev.remaining;
        }
        if (lastRemaining > 0)
            s.Add(new BrewStep("Finish boil", BrewStepKind.Wait,
                $"Continue boil for final {lastRemaining} min.",
                restMinutes: lastRemaining));

        // Flameout additions
        var flameoutGrains = r.Grains.Where(g => g.Use == GrainUse.Flameout).ToList();
        var whirlpoolHops = r.Hops.Where(h => h.Use == HopUse.Whirlpool).ToList();
        if (flameoutGrains.Count > 0 || whirlpoolHops.Count > 0)
        {
            var bits = new List<string>();
            bits.AddRange(flameoutGrains.Select(g => $"{g.Pounds:F2} lb {g.Name}"));
            bits.AddRange(whirlpoolHops.Select(h => $"{h.Ounces:F2} oz {h.Name}"));
            s.Add(new BrewStep("Flameout / Whirlpool additions", BrewStepKind.Action,
                "Add at flameout: " + string.Join(", ", bits)));
        }

        s.Add(new BrewStep("Chill", BrewStepKind.Action,
            $"Chill wort to {(r.Yeasts.FirstOrDefault()?.IdealLowF ?? 65):F0}°F (yeast pitch temp)."));
        s.Add(new BrewStep("Transfer to fermenter", BrewStepKind.Action,
            $"Target {r.BatchSizeGallons:F1} gal in fermenter. Aerate well (lager: shake/stone)."));
        s.Add(new BrewStep("OG reading", BrewStepKind.Reading,
            $"Target OG: {c.Og:F4}. Note actual."));
        s.Add(new BrewStep("Pitch yeast", BrewStepKind.Action,
            $"Pitch: {string.Join(", ", r.Yeasts.Select(y => y.Name))}. Ferment at {(r.Yeasts.FirstOrDefault()?.IdealLowF ?? 65):F0}-{(r.Yeasts.FirstOrDefault()?.IdealHighF ?? 70):F0}°F."));
        s.Add(new BrewStep("Cleanup", BrewStepKind.Checklist,
            "Clean kettle, mash tun, hoses. Sanitize anything that touched post-boil wort."));
        s.Add(new BrewStep("Done", BrewStepKind.Done,
            $"Brew day complete. Estimated ABV: {c.Abv:F1}%. Log notes."));

        return s;
    }
}

public enum BrewStepKind { Checklist, Heat, Action, Rest, Wait, Reading, Done }

public class BrewStep
{
    public string Title { get; }
    public BrewStepKind Kind { get; }
    public string Instruction { get; }
    public double? TargetTempF { get; }
    public int? RestMinutes { get; }

    public BrewStep(string title, BrewStepKind kind, string instruction,
        double? targetTempF = null, int? restMinutes = null)
    {
        Title = title;
        Kind = kind;
        Instruction = instruction;
        TargetTempF = targetTempF;
        RestMinutes = restMinutes;
    }
}
