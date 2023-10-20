namespace InOneWeekend.Utils;

public class Interval
{
    public static Interval Empty => new(double.PositiveInfinity, double.NegativeInfinity);

    public static Interval UnIverse => new(double.NegativeInfinity, double.PositiveInfinity);

    public double Min { get; set; }

    public double Max { get; set; }

    public double Size => Max - Min;

    public Interval() : this(-double.NegativeInfinity, double.PositiveInfinity)
    {
    }

    public Interval(double min, double max)
    {
        Min = min;
        Max = max;
    }

    public bool Contains(double value) => Min <= value && value <= Max;

    public bool Surrounds(double value) => Min < value && value < Max;

    public double Clamp(double value) => Math.Clamp(value, Min, Max);
}
