using Silk.NET.Maths;
using TheNextWeek.Contracts.Textures;

namespace TheNextWeek.Textures;

public class SolidColor : Texture
{
    private readonly Vector3D<double> _color;

    public SolidColor(Vector3D<double> color)
    {
        _color = color;
    }

    public SolidColor(double red, double green, double blue)
    {
        _color = new Vector3D<double>(red, green, blue);
    }

    public override Vector3D<double> Value(double u, double v, Vector3D<double> point)
    {
        return _color;
    }
}
