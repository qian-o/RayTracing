using TheNextWeek.Contracts.Materials;
using TheNextWeek.Helpers;
using TheNextWeek.Utils;
using Silk.NET.Maths;

namespace TheNextWeek.Materials;

public class Dielectric : Material
{
    // Index of Refraction
    private readonly double _ir;

    public Dielectric(double indexOfRefraction)
    {
        _ir = indexOfRefraction;
    }

    public override bool Scatter(Ray rayIn, HitRecord hitRecord, out Vector3D<double> attenuation, out Ray scattered)
    {
        attenuation = new Vector3D<double>(1.0, 1.0, 1.0);
        double refractionRatio = hitRecord.FrontFace ? (1.0 / _ir) : _ir;

        Vector3D<double> unitDirection = Vector3D.Normalize(rayIn.Direction);
        double cosTheta = Math.Min(Vector3D.Dot(-unitDirection, hitRecord.Normal), 1.0);
        double sinTheta = Math.Sqrt(1.0 - cosTheta * cosTheta);

        bool cannotRefract = refractionRatio * sinTheta > 1.0;
        Vector3D<double> direction;

        if (cannotRefract || Reflectance(cosTheta, refractionRatio) > MathHelper.RandomDouble())
        {
            direction = Vector3D.Reflect(unitDirection, hitRecord.Normal);
        }
        else
        {
            direction = MathHelper.Refract(unitDirection, hitRecord.Normal, refractionRatio);
        }

        scattered = new Ray(hitRecord.P, direction);

        return true;
    }

    private static double Reflectance(double cosine, double refIdx)
    {
        // Use Schlick's approximation for reflectance.
        double r0 = (1 - refIdx) / (1 + refIdx);
        r0 *= r0;
        return r0 + (1 - r0) * Math.Pow((1 - cosine), 5);
    }
}
