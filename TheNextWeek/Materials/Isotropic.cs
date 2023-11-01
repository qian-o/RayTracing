using Silk.NET.Maths;
using TheNextWeek.Contracts.Materials;
using TheNextWeek.Contracts.Textures;
using TheNextWeek.Helpers;
using TheNextWeek.Textures;
using TheNextWeek.Utils;

namespace TheNextWeek.Materials;

public class Isotropic : Material
{
    private readonly Texture _albedo;

    public Isotropic(Vector3D<double> color) : this(new SolidColor(color))
    {
    }

    public Isotropic(Texture albedo)
    {
        _albedo = albedo;
    }

    public override bool Scatter(Ray rayIn, HitRecord hitRecord, out Vector3D<double> attenuation, out Ray scattered)
    {
        scattered = new Ray(hitRecord.P, MathHelper.RandomUnitVector(), rayIn.Time);
        attenuation = _albedo.Value(hitRecord.U, hitRecord.V, hitRecord.P);

        return true;
    }
}
