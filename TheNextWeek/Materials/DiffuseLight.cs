using Silk.NET.Maths;
using TheNextWeek.Contracts.Materials;
using TheNextWeek.Contracts.Textures;
using TheNextWeek.Textures;
using TheNextWeek.Utils;

namespace TheNextWeek.Materials;

public class DiffuseLight : Material
{
    private readonly Texture _emit;

    public DiffuseLight(Vector3D<double> color) : this(new SolidColor(color))
    {
    }

    public DiffuseLight(Texture emit)
    {
        _emit = emit;
    }

    public override bool Scatter(Ray rayIn, HitRecord hitRecord, out Vector3D<double> attenuation, out Ray scattered)
    {
        attenuation = Vector3D<double>.Zero;
        scattered = new Ray(Vector3D<double>.Zero, Vector3D<double>.Zero);

        return false;
    }

    public override Vector3D<double> Emitted(double u, double v, Vector3D<double> p)
    {
        return _emit.Value(u, v, p);
    }
}
