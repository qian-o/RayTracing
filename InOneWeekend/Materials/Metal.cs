using InOneWeekend.Contracts.Materials;
using InOneWeekend.Helpers;
using InOneWeekend.Utils;
using Silk.NET.Maths;

namespace InOneWeekend.Materials;

public class Metal : Material
{
    private readonly Vector3D<double> _albedo;

    public Metal(Vector3D<double> albedo)
    {
        _albedo = albedo;
    }

    public override bool Scatter(Ray rayIn, HitRecord hitRecord, out Vector3D<double> attenuation, out Ray scattered)
    {
        Vector3D<double> reflected = MathHelper.Reflect(Vector3D.Normalize(rayIn.Direction), hitRecord.Normal);
        scattered = new Ray(hitRecord.P, reflected);
        attenuation = _albedo;

        return true;
    }
}
