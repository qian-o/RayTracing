using Silk.NET.Maths;
using TheNextWeek.Contracts.Materials;
using TheNextWeek.Contracts.Textures;
using TheNextWeek.Helpers;
using TheNextWeek.Textures;
using TheNextWeek.Utils;

namespace TheNextWeek.Materials;

public class Lambertian : Material
{
    public static Lambertian Identity { get; } = new(new Vector3D<double>(0.5, 0.5, 0.5));

    private readonly Texture _albedo;

    public Lambertian(Vector3D<double> color)
    {
        _albedo = new SolidColor(color);
    }

    public Lambertian(Texture texture)
    {
        _albedo = texture;
    }

    public override bool Scatter(Ray rayIn, HitRecord hitRecord, out Vector3D<double> attenuation, out Ray scattered)
    {
        Vector3D<double> scatterDirection = hitRecord.Normal + MathHelper.RandomUnitVector();

        if (scatterDirection.NearZero())
        {
            scatterDirection = hitRecord.Normal;
        }

        scattered = new Ray(hitRecord.P, scatterDirection, rayIn.Time);
        attenuation = _albedo.Value(hitRecord.U, hitRecord.V, hitRecord.P);

        return true;
    }
}
