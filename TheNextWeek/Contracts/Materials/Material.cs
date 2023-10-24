using TheNextWeek.Utils;
using Silk.NET.Maths;

namespace TheNextWeek.Contracts.Materials;

public abstract class Material
{
    public abstract bool Scatter(Ray rayIn, HitRecord hitRecord, out Vector3D<double> attenuation, out Ray scattered);
}
