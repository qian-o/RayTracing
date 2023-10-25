using Silk.NET.Maths;
using TheNextWeek.Contracts.Textures;

namespace TheNextWeek.Textures;

public class CheckerTexture : Texture
{
    private readonly Texture _even;
    private readonly Texture _odd;
    private readonly double _scale;

    public CheckerTexture(Texture even, Texture odd, double scale)
    {
        _even = even;
        _odd = odd;
        _scale = 1.0 / scale;
    }

    public CheckerTexture(Vector3D<double> color1, Vector3D<double> color2, double scale)
    {
        _even = new SolidColor(color1);
        _odd = new SolidColor(color2);
        _scale = 1.0 / scale;
    }

    public override Vector3D<double> Value(double u, double v, Vector3D<double> point)
    {
        int x = (int)Math.Floor(point.X * _scale);
        int y = (int)Math.Floor(point.Y * _scale);
        int z = (int)Math.Floor(point.Z * _scale);

        bool isEven = (x + y + z) % 2 == 0;

        return isEven ? _even.Value(u, v, point) : _odd.Value(u, v, point);
    }
}
