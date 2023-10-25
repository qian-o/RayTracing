using Silk.NET.Maths;
using TheNextWeek.Contracts.Textures;
using TheNextWeek.Utils;

namespace TheNextWeek.Textures;

public class NoiseTexture : Texture
{
    private readonly Perlin _noise;
    private readonly double _scale;

    public NoiseTexture()
    {
        _noise = new Perlin();
        _scale = 1.0;
    }

    public NoiseTexture(double scale)
    {
        _noise = new Perlin();
        _scale = scale;
    }

    public override Vector3D<double> Value(double u, double v, Vector3D<double> point)
    {
        Vector3D<double> s = point * _scale;

        return Vector3D<double>.One * 0.5 * (1.0 + Math.Sin(s.Z + 10 * _noise.Turb(s, 7)));
    }
}
