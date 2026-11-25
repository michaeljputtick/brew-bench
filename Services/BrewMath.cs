using BrewBench.Models;

namespace BrewBench.Services;

public record CalcResult(
    double Og,
    double Fg,
    double Abv,
    double Ibu,
    double Srm,
    double PreBoilVolume,
    double PreBoilOg,
    double StrikeWaterGallons,
    double StrikeWaterTempF,
    double SpargeWaterGallons,
    double TotalGrainPounds,
    double TotalHopOunces,
    int CaloriesPer12oz,
    double AttenuationApparent);

public static class BrewMath
{
    /// <summary>
    /// Calculate full recipe spec from inputs. Pure function, no side effects.
    /// </summary>
    public static CalcResult Calculate(Recipe r)
    {
        // 1. Volumes
        double boilHours = r.BoilMinutes / 60.0;
        double boilOff = r.BoilOffGalPerHour * boilHours;
        double postBoil = r.BatchSizeGallons + r.TrubLossGallons;
        double preBoil = postBoil + boilOff;

        // 2. OG
        double mashGrainGu = 0;
        double lateAddGu = 0;
        double totalGrainLbs = 0;
        double mashGrainLbs = 0;
        foreach (var g in r.Grains)
        {
            double gu = g.Pounds * g.Ppg;
            totalGrainLbs += g.Pounds;
            if (g.Use == GrainUse.Mash)
            {
                mashGrainGu += gu;
                mashGrainLbs += g.Pounds;
            }
            else
                lateAddGu += gu;
        }
        // Total gravity points delivered to the kettle. Gravity is that divided by the wort volume
        // at that moment: post-boil volume for OG (the fermenter gets the same wort minus trub).
        double kettlePoints = mashGrainGu * r.MashEfficiency + lateAddGu;
        double ogPoints = postBoil > 0 ? kettlePoints / postBoil : 0;
        double og = 1.0 + ogPoints / 1000.0;

        // Pre-boil reading is taken before late-boil and flameout additions go in.
        double preBoilOg = preBoil > 0
            ? 1.0 + (mashGrainGu * r.MashEfficiency / preBoil) / 1000.0
            : og;

        // 3. Attenuation -> FG
        // Apparent attenuation modified by mash temp.
        // Reference: low mash 148F -> +4%, high mash 156F -> -6%.
        double avgYeastAtt = r.Yeasts.Count > 0
            ? r.Yeasts.Average(y => y.ApparentAttenuation)
            : 0.75;

        double mashTempEffectivelyF = r.MashSchedule.Count > 0
            ? r.MashSchedule.Where(s => s.TempF >= 144 && s.TempF <= 162).DefaultIfEmpty(new MashStep()).Average(s => s.TempF)
            : 152.0;

        double mashTempAdjustment = (152.0 - mashTempEffectivelyF) * 0.005; // 0.5% per degree
        // Late sugars are close to fully fermentable, so their share of the extract
        // ferments out completely instead of at the yeast's wort attenuation.
        double sugarFraction = kettlePoints > 0 ? lateAddGu / kettlePoints : 0;
        double sugarBoost = sugarFraction * (1.0 - avgYeastAtt);

        double effectiveAttenuation = Math.Clamp(avgYeastAtt + mashTempAdjustment + sugarBoost, 0.55, 0.95);

        double fgPoints = ogPoints * (1.0 - effectiveAttenuation);
        double fg = 1.0 + fgPoints / 1000.0;

        double abv = (og - fg) * 131.25;

        // 4. IBU - Tinseth
        double ibu = 0;
        double totalHopOz = 0;
        double bignessFactor = 1.65 * Math.Pow(0.000125, og - 1.0);
        foreach (var h in r.Hops)
        {
            totalHopOz += h.Ounces;
            if (h.Use == HopUse.DryHop) continue;

            double t = h.BoilMinutes;
            if (h.Use == HopUse.Whirlpool) t = Math.Min(t, 20); // approximate whirlpool isomerization
            double timeFactor = (1.0 - Math.Exp(-0.04 * t)) / 4.15;
            double util = bignessFactor * timeFactor;
            double aau = h.Ounces * h.AlphaAcid;
            // Concentration in the post-boil wort, which is what ends up in the glass.
            ibu += postBoil > 0 ? aau * util * 74.89 / postBoil : 0;
        }

        // 5. SRM - Morey's equation
        // MCU = (lb * Lovibond) / gallons of post-boil wort
        double mcu = postBoil > 0 ? r.Grains.Sum(g => g.Pounds * g.Lovibond) / postBoil : 0;
        double srm = 1.4922 * Math.Pow(mcu, 0.6859);

        // 6. Water (mash grains only; late sugars never see the mash)
        double strikeWater = mashGrainLbs * r.MashThicknessQtPerLb / 4.0; // gallons
        double strikeTempF = StrikeTemperature(r.GrainTempF, MashTargetF(r), r.MashThicknessQtPerLb);
        // sparge to reach pre-boil; absorption ~0.125 gal/lb of grain
        double grainAbsorption = mashGrainLbs * 0.125;
        double sparge = Math.Max(0, preBoil - (strikeWater - grainAbsorption));

        // 7. Calories per 12 oz (Brewer's Friend / ASBC-derived formulas)
        double calAlcohol = 1881.22 * fg * (og - fg) / (1.775 - og);
        double calCarbs = 3550.0 * fg * ((0.1808 * og) + (0.8192 * fg) - 1.0004);
        int caloriesPer12oz = (int)Math.Round(Math.Max(0, calAlcohol) + Math.Max(0, calCarbs));

        return new CalcResult(
            Og: Math.Round(og, 4),
            Fg: Math.Round(fg, 4),
            Abv: Math.Round(abv, 2),
            Ibu: Math.Round(ibu, 1),
            Srm: Math.Round(srm, 1),
            PreBoilVolume: Math.Round(preBoil, 2),
            PreBoilOg: Math.Round(preBoilOg, 4),
            StrikeWaterGallons: Math.Round(strikeWater, 2),
            StrikeWaterTempF: Math.Round(strikeTempF, 1),
            SpargeWaterGallons: Math.Round(sparge, 2),
            TotalGrainPounds: Math.Round(totalGrainLbs, 2),
            TotalHopOunces: Math.Round(totalHopOz, 2),
            CaloriesPer12oz: caloriesPer12oz,
            AttenuationApparent: Math.Round(effectiveAttenuation, 3));
    }

    // Strike water has to hit the first rest (dough-in); later steps are reached by heating or infusion.
    private static double MashTargetF(Recipe r) =>
        r.MashSchedule.Count > 0 ? r.MashSchedule.First().TempF : 152.0;

    /// <summary>
    /// Strike water temperature using Palmer's formula.
    /// Strike = (0.2 / R) * (T_target - T_grain) + T_target
    /// where R = qts water per lb grain
    /// </summary>
    public static double StrikeTemperature(double grainTempF, double targetMashF, double thicknessQtPerLb)
    {
        double r = thicknessQtPerLb;
        return (0.2 / r) * (targetMashF - grainTempF) + targetMashF;
    }

    /// <summary>
    /// Lock-and-solve: scale grain bill proportionally to hit a target ABV.
    /// Returns a new Recipe with scaled grain weights.
    /// </summary>
    public static Recipe SolveForAbv(Recipe r, double targetAbv)
    {
        var current = Calculate(r);
        if (current.Abv <= 0) return r;
        double scale = targetAbv / current.Abv;
        var copy = r.Clone();
        copy.Id = r.Id;
        copy.IsPreset = r.IsPreset;
        foreach (var g in copy.Grains)
            g.Pounds = Math.Round(g.Pounds * scale, 2);
        return copy;
    }

    public static Recipe SolveForOg(Recipe r, double targetOg)
    {
        var current = Calculate(r);
        double currentPts = (current.Og - 1) * 1000;
        double targetPts = (targetOg - 1) * 1000;
        if (currentPts <= 0) return r;
        double scale = targetPts / currentPts;
        var copy = r.Clone();
        copy.Id = r.Id;
        copy.IsPreset = r.IsPreset;
        foreach (var g in copy.Grains)
            g.Pounds = Math.Round(g.Pounds * scale, 2);
        return copy;
    }

    public static Recipe SolveForIbu(Recipe r, double targetIbu)
    {
        var current = Calculate(r);
        if (current.Ibu <= 0 || r.Hops.Count == 0) return r;
        double scale = targetIbu / current.Ibu;
        var copy = r.Clone();
        copy.Id = r.Id;
        copy.IsPreset = r.IsPreset;
        foreach (var h in copy.Hops)
            h.Ounces = Math.Round(h.Ounces * scale, 2);
        return copy;
    }

    public static Recipe SolveForBatchSize(Recipe r, double newGallons)
    {
        if (r.BatchSizeGallons <= 0) return r;
        // Scale by post-boil volume: trub loss is fixed, so scaling by batch size alone would drift OG, IBU, and color.
        double scale = (newGallons + r.TrubLossGallons) / (r.BatchSizeGallons + r.TrubLossGallons);
        var copy = r.Clone();
        copy.Id = r.Id;
        copy.IsPreset = r.IsPreset;
        copy.BatchSizeGallons = newGallons;
        foreach (var g in copy.Grains) g.Pounds = Math.Round(g.Pounds * scale, 3);
        foreach (var h in copy.Hops) h.Ounces = Math.Round(h.Ounces * scale, 2);
        return copy;
    }
}
