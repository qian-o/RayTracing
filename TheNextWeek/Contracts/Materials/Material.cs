using Silk.NET.Maths;
using TheNextWeek.Utils;

namespace TheNextWeek.Contracts.Materials;

public abstract class Material
{
    public abstract bool Scatter(Ray rayIn, HitRecord hitRecord, out Vector3D<double> attenuation, out Ray scattered);

    public virtual Vector3D<double> Emitted(double u, double v, Vector3D<double> p)
    {
        return Vector3D<double>.Zero;
    }
}
