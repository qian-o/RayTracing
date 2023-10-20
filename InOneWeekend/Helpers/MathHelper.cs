namespace InOneWeekend.Helpers;

public class MathHelper
{
    private static readonly Random _random = new();

    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    public static double RandomDouble()
    {
        return _random.NextDouble();
    }

    public static double RandomDouble(double min, double max)
    {
        return min + (max - min) * _random.NextDouble();
    }
}
