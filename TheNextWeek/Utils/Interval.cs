namespace TheNextWeek.Utils;

public class Interval
{
    public double Min { get; set; }

    public double Max { get; set; }

    public double Size => Max - Min;

    public Interval() : this(double.PositiveInfinity, double.NegativeInfinity)
    {
    }

    public Interval(double min, double max)
    {
        Min = min;
        Max = max;
    }

    public Interval(Interval a, Interval b)
    {
        Min = Math.Min(a.Min, b.Min);
        Max = Math.Max(a.Max, b.Max);
    }

    public bool Contains(double value) => Min <= value && value <= Max;

    public bool Surrounds(double value) => Min < value && value < Max;

    public double Clamp(double value) => Math.Clamp(value, Min, Max);

    public Interval Expand(double delta)
    {
        double padding = delta / 2.0;

        return new Interval(Min - padding, Max + padding);
    }
}
