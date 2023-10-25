using Silk.NET.Maths;
using TheNextWeek.Contracts.Textures;
using TheNextWeek.Utils;

namespace TheNextWeek.Textures;

public class ImageTexture : Texture
{
    private readonly Image _image;

    public ImageTexture(string path)
    {
        _image = Image.Load(path);
    }

    public override Vector3D<double> Value(double u, double v, Vector3D<double> point)
    {
        u = new Interval(0.0, 1.0).Clamp(u);
        v = 1.0 - new Interval(0.0, 1.0).Clamp(v);  // Flip V to image coordinates

        int i = (int)(u * _image.Width);
        int j = (int)(v * _image.Height);
        Vector3D<double> pixel = _image.Pixel(i, j);

        return new Vector3D<double>(pixel.X / 255.0, pixel.Y / 255.0, pixel.Z / 255.0);
    }
}
