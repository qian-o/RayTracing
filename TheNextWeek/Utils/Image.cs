using Silk.NET.Maths;
using StbImageSharp;
using System.Runtime.InteropServices;

namespace TheNextWeek.Utils;

public unsafe class Image : IDisposable
{
    private readonly byte* _image;
    private readonly int _imageWidth;
    private readonly int _imageHeight;
    private readonly int _bytesPerScanline;

    public int Width => _imageWidth;

    public int Height => _imageHeight;

    public Image(byte[] data, int image_width, int image_height, int bytes_per_scanline)
    {
        byte* image = (byte*)Marshal.AllocHGlobal(data.Length);
        Marshal.Copy(data, 0, (nint)image, data.Length);

        _image = image;
        _imageWidth = image_width;
        _imageHeight = image_height;
        _bytesPerScanline = bytes_per_scanline;
    }

    public Vector3D<double> Pixel(int x, int y)
    {
        x = Math.Clamp(x, 0, _imageWidth - 1);
        y = Math.Clamp(y, 0, _imageHeight - 1);

        byte* pixel = _image + (y * _bytesPerScanline) + (x * 3);

        return new Vector3D<double>(pixel[0], pixel[1], pixel[2]);
    }

    public static Image Load(string path)
    {
        using FileStream file = File.OpenRead(path);
        ImageResult result = ImageResult.FromStream(file, ColorComponents.RedGreenBlue);

        return new Image(result.Data, result.Width, result.Height, result.Width * 3);
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal((nint)_image);

        GC.SuppressFinalize(this);
    }
}
