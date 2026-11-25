using BrewBench.Models;

namespace BrewBench.Services;

public record SaltAdditions(
    double GypsumGrams,
    double CalciumChlorideGrams,
    double EpsomSaltGrams,
    double BakingSodaGrams,
    double ChalkGrams,
    double TableSaltGrams,
    double LacticAcidMl,
    WaterProfile Result);

public static class WaterMath
{
    // Salt contributions per gram per gallon (ppm)
    // Gypsum (CaSO4·2H2O): Ca 61.5, SO4 147.4
    // Calcium Chloride (CaCl2·2H2O): Ca 72, Cl 127
    // Epsom Salt (MgSO4·7H2O): Mg 26, SO4 103
    // Baking Soda (NaHCO3): Na 72, HCO3 191
    // Chalk (CaCO3): Ca 105.5, HCO3 158 (limited solubility)
    // Table Salt (NaCl): Na 103, Cl 160

    private const double PpmPerGramPerGallon = 1.0;

    public static SaltAdditions Recommend(
        WaterProfile source,
        WaterProfile target,
        double mashGallons,
        double totalGallons)
    {
        // Treat total water with salts; use mash gallons for calculation reference
        // We'll calculate additions for total water volume
        double V = totalGallons;
        if (V <= 0) V = 1;

        // Target deltas (ppm to add)
        double dCa = Math.Max(0, target.Calcium - source.Calcium);
        double dMg = Math.Max(0, target.Magnesium - source.Magnesium);
        double dNa = Math.Max(0, target.Sodium - source.Sodium);
        double dSO4 = Math.Max(0, target.Sulfate - source.Sulfate);
        double dCl = Math.Max(0, target.Chloride - source.Chloride);
        double dHCO3 = Math.Max(0, target.Bicarbonate - source.Bicarbonate);

        // Excess source bicarbonate -> lactic acid suggestion
        double excessHCO3 = Math.Max(0, source.Bicarbonate - target.Bicarbonate);

        // Naive solver: greedy fill of each ion using preferred salts.
        // 1. Mg first via Epsom (only Mg source we have)
        double epsom = dMg / 26.0 * V;
        double soFromEpsom = epsom / V * 103.0;

        // 2. Na via baking soda if HCO3 also needed, else table salt
        double bakingSoda = 0, tableSalt = 0;
        if (dNa > 0 && dHCO3 > 0)
        {
            // Use baking soda for both up to whichever is limiting
            double bsForNa = dNa / 72.0 * V;
            double bsForHCO3 = dHCO3 / 191.0 * V;
            bakingSoda = Math.Min(bsForNa, bsForHCO3);
            double naLeft = dNa - bakingSoda / V * 72.0;
            if (naLeft > 0)
                tableSalt = naLeft / 103.0 * V;
        }
        else if (dNa > 0)
        {
            tableSalt = dNa / 103.0 * V;
        }
        else if (dHCO3 > 0)
        {
            // need bicarbonate, no sodium desired - chalk (but limited solubility)
            // for simplicity use baking soda anyway
            bakingSoda = dHCO3 / 191.0 * V;
        }

        double naFromTableSalt = tableSalt / V * 103.0;
        double clFromTableSalt = tableSalt / V * 160.0;
        double naFromBaking = bakingSoda / V * 72.0;
        double hco3FromBaking = bakingSoda / V * 191.0;

        // 3. Remaining Ca + Cl + SO4 via gypsum and calcium chloride
        double remCl = Math.Max(0, dCl - clFromTableSalt);
        double remSO4 = Math.Max(0, dSO4 - soFromEpsom);

        // Each contributes calcium too. Solve a small system:
        // calciumChloride grams * 72/V = Ca contribution; * 127/V = Cl contribution
        // gypsum grams * 61.5/V = Ca contribution; * 147.4/V = SO4 contribution
        // Strategy: hit Cl exactly with CaCl2, SO4 exactly with gypsum, accept resulting Ca
        double calciumChloride = remCl / 127.0 * V;
        double gypsum = remSO4 / 147.4 * V;
        double caFromCaCl = calciumChloride / V * 72.0;
        double caFromGypsum = gypsum / V * 61.5;
        double totalCa = source.Calcium + caFromCaCl + caFromGypsum;

        // If Ca still under target, add a bit more gypsum (most common Ca source)
        if (totalCa < target.Calcium)
        {
            double caShortfall = target.Calcium - totalCa;
            gypsum += caShortfall / 61.5 * V;
            caFromGypsum = gypsum / V * 61.5;
        }

        // Lactic acid 88%: about 11.8 mEq/mL, roughly 97% dissociated at mash pH, so ~11.5 mEq/mL usable.
        // Each mEq/L of bicarbonate to remove is 61 ppm HCO3. Neutralizes the excess across the total water.
        const double LitersPerGallon = 3.785;
        const double LacticMeqPerMl = 11.5;
        double lacticMl = excessHCO3 > 0
            ? Math.Round(excessHCO3 / 61.02 * V * LitersPerGallon / LacticMeqPerMl, 1)
            : 0;

        // Build resulting profile
        var result = new WaterProfile
        {
            Name = "Calculated",
            Calcium = Math.Round(source.Calcium + caFromCaCl + caFromGypsum, 1),
            Magnesium = Math.Round(source.Magnesium + epsom / V * 26.0, 1),
            Sodium = Math.Round(source.Sodium + naFromTableSalt + naFromBaking, 1),
            Sulfate = Math.Round(source.Sulfate + gypsum / V * 147.4 + soFromEpsom, 1),
            Chloride = Math.Round(source.Chloride + calciumChloride / V * 127.0 + clFromTableSalt, 1),
            Bicarbonate = Math.Round(Math.Max(0, source.Bicarbonate + hco3FromBaking - excessHCO3), 1),
        };

        return new SaltAdditions(
            GypsumGrams: Math.Round(gypsum, 2),
            CalciumChlorideGrams: Math.Round(calciumChloride, 2),
            EpsomSaltGrams: Math.Round(epsom, 2),
            BakingSodaGrams: Math.Round(bakingSoda, 2),
            ChalkGrams: 0,
            TableSaltGrams: Math.Round(tableSalt, 2),
            LacticAcidMl: lacticMl,
            Result: result);
    }
}
