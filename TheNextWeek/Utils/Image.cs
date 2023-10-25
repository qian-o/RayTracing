using Silk.NET.Maths;
using StbImageSharp;
using System.Runtime.InteropServices;

namespace TheNextWeek.Utils;

public unsafe class Image : IDisposable
{
    private readonly byte* _image;
    private readonly int _image_width;
    private readonly int _image_height;
    private readonly int _bytes_per_scanline;

    public int Width => _image_width;

    public int Height => _image_height;

    public Image(byte[] data, int image_width, int image_height, int bytes_per_scanline)
    {
        byte* image = (byte*)Marshal.AllocHGlobal(data.Length);
        Marshal.Copy(data, 0, (nint)image, data.Length);

        _image = image;
        _image_width = image_width;
        _image_height = image_height;
        _bytes_per_scanline = bytes_per_scanline;
    }

    public Vector3D<double> Pixel(int x, int y)
    {
        x = Math.Clamp(x, 0, _image_width - 1);
        y = Math.Clamp(y, 0, _image_height - 1);

        byte* pixel = _image + (y * _bytes_per_scanline) + (x * 3);

        return new Vector3D<double>(pixel[0], pixel[1], pixel[2]);
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal((nint)_image);

        GC.SuppressFinalize(this);
    }

    public static Image Load(string path)
    {
        using FileStream file = File.OpenRead(path);
        ImageResult result = ImageResult.FromStream(file, ColorComponents.RedGreenBlue);

        return new Image(result.Data, result.Width, result.Height, result.Width * 3);
    }
}
