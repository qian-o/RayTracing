namespace TheNextWeek.Utils;

public struct Interval
{
    public static readonly Interval Empty = new(double.PositiveInfinity, double.NegativeInfinity);

    public static readonly Interval Infinite = new(double.NegativeInfinity, double.PositiveInfinity);

    public double Min;

    public double Max;

    public readonly double Size => Max - Min;

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

    public static Interval operator +(Interval interval, double delta)
    {
        return new Interval(interval.Min + delta, interval.Max + delta);
    }

    public static Interval operator +(double delta, Interval interval)
    {
        return interval + delta;
    }

    public readonly bool Contains(double value) => Min <= value && value <= Max;

    public readonly bool Surrounds(double value) => Min < value && value < Max;

    public readonly double Clamp(double value) => Math.Clamp(value, Min, Max);

    public readonly Interval Expand(double delta)
    {
        double padding = delta / 2.0;

        return new Interval(Min - padding, Max + padding);
    }
}
