using Silk.NET.Maths;
using TheNextWeek.Contracts.Materials;
using TheNextWeek.Helpers;
using TheNextWeek.Utils;

namespace TheNextWeek.Materials;

public class Lambertian : Material
{
    public static Lambertian Identity { get; } = new(new Vector3D<double>(0.5, 0.5, 0.5));

    private readonly Vector3D<double> _albedo;

    public Lambertian(Vector3D<double> albedo)
    {
        _albedo = albedo;
    }

    public override bool Scatter(Ray rayIn, HitRecord hitRecord, out Vector3D<double> attenuation, out Ray scattered)
    {
        Vector3D<double> scatterDirection = hitRecord.Normal + MathHelper.RandomUnitVector();

        if (scatterDirection.NearZero())
        {
            scatterDirection = hitRecord.Normal;
        }

        scattered = new Ray(hitRecord.P, scatterDirection);
        attenuation = _albedo;

        return true;
    }
}
