using Silk.NET.Maths;

namespace TheNextWeek.Contracts.Textures;

public abstract class Texture
{
    public abstract Vector3D<double> Value(double u, double v, Vector3D<double> point);
}
