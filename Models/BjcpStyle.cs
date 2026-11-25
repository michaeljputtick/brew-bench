namespace BrewBench.Models;

public class BjcpStyle
{
    public string Id { get; set; } = "";
    public string Number { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";

    public Range Og { get; set; } = new();
    public Range Fg { get; set; } = new();
    public Range Abv { get; set; } = new();
    public Range Ibu { get; set; } = new();
    public Range Srm { get; set; } = new();

    public string Description { get; set; } = "";

    public class Range
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Mid => (Min + Max) / 2;
    }
}

public class IngredientReference
{
    public string Name { get; set; } = "";
    public double DefaultValue { get; set; }
    public double SecondaryValue { get; set; }
    public string Category { get; set; } = "";
}
