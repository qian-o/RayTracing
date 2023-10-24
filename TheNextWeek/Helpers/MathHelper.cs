using Silk.NET.Maths;

namespace TheNextWeek.Helpers;

public static class MathHelper
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

    public static Vector3D<double> Random()
    {
        return new Vector3D<double>(RandomDouble(), RandomDouble(), RandomDouble());
    }

    public static Vector3D<double> Random(double min, double max)
    {
        return new Vector3D<double>(RandomDouble(min, max), RandomDouble(min, max), RandomDouble(min, max));
    }

    public static Vector3D<double> RandomInUnitSphere()
    {
        while (true)
        {
            Vector3D<double> p = Random(-1, 1);
            if (p.LengthSquared < 1)
            {
                return p;
            }
        }
    }

    public static Vector3D<double> RandomUnitVector()
    {
        return Vector3D.Normalize(RandomInUnitSphere());
    }

    public static Vector3D<double> RandomOnHemisphere(Vector3D<double> normal)
    {
        Vector3D<double> on_unit_sphere = RandomUnitVector();

        return Vector3D.Dot(on_unit_sphere, normal) > 0.0 ? on_unit_sphere : -on_unit_sphere;
    }

    public static Vector3D<double> RandomInUnitDisk()
    {
        while (true)
        {
            Vector3D<double> p = new(RandomDouble(-1, 1), RandomDouble(-1, 1), 0);
            if (p.LengthSquared < 1)
            {
                return p;
            }
        }
    }

    public static bool NearZero(this Vector3D<double> vector)
    {
        const double s = 1e-8;
        return Math.Abs(vector.X) < s && Math.Abs(vector.Y) < s && Math.Abs(vector.Z) < s;
    }

    public static Vector3D<double> Refract(Vector3D<double> uv, Vector3D<double> normal, double etai_over_etat)
    {
        double cos_theta = Math.Min(Vector3D.Dot(-uv, normal), 1.0);
        Vector3D<double> r_out_perp = etai_over_etat * (uv + cos_theta * normal);
        Vector3D<double> r_out_parallel = -Math.Sqrt(Math.Abs(1.0 - r_out_perp.LengthSquared)) * normal;

        return r_out_perp + r_out_parallel;
    }
}
