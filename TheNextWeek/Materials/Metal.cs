using Silk.NET.Maths;
using TheNextWeek.Contracts.Materials;
using TheNextWeek.Helpers;
using TheNextWeek.Utils;

namespace TheNextWeek.Materials;

public class Metal : Material
{
    public static Metal Identity { get; } = new(new Vector3D<double>(0.5, 0.5, 0.5), 0.5);

    private readonly Vector3D<double> _albedo;
    private readonly double _fuzz;

    public Metal(Vector3D<double> albedo, double fuzz)
    {
        _albedo = albedo;
        _fuzz = fuzz < 1 ? fuzz : 1;
    }

    public override bool Scatter(Ray rayIn, HitRecord hitRecord, out Vector3D<double> attenuation, out Ray scattered)
    {
        Vector3D<double> reflected = Vector3D.Reflect(Vector3D.Normalize(rayIn.Direction), hitRecord.Normal);
        scattered = new Ray(hitRecord.P, reflected + _fuzz * MathHelper.RandomUnitVector());
        attenuation = _albedo;

        return Vector3D.Dot(scattered.Direction, hitRecord.Normal) > 0;
    }
}
