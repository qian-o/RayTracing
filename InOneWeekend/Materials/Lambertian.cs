using InOneWeekend.Contracts.Materials;
using InOneWeekend.Helpers;
using InOneWeekend.Utils;
using Silk.NET.Maths;

namespace InOneWeekend.Materials;

public class Lambertian : Material
{
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
