using InOneWeekend.Contracts.Chapters;
using Color = Silk.NET.Maths.Vector3D<double>;

namespace InOneWeekend.Chapters;

/// <summary>
/// The vec3 Class
/// </summary>
public class Chapter3 : IChapter
{
    public void Run()
    {
        // Image
        const int imageWidth = 256;
        const int imageHeight = 256;

        // Render
        using FileStream fileStream = File.Create("image.ppm");
        using StreamWriter streamWriter = new(fileStream);

        streamWriter.WriteLine("P3");
        streamWriter.WriteLine($"{imageWidth} {imageHeight}");
        streamWriter.WriteLine("255");

        for (int i = 0; i < imageHeight; i++)
        {
            Console.WriteLine($"Scanlines remaining: {imageHeight - i}");

            for (int j = 0; j < imageWidth; j++)
            {
                Color pixelColor = new((double)j / (imageWidth - 1), (double)i / (imageHeight - 1), 0.25);

                WriteColor(streamWriter, pixelColor);
            }
        }

        Console.WriteLine("Done!");
    }

    private static void WriteColor(StreamWriter streamWriter, Color pixelColor)
    {
        // Write the translated [0,255] value of each color component.
        streamWriter.WriteLine($"{(int)(255.999f * pixelColor.X)} {(int)(255.999f * pixelColor.Y)} {(int)(255.999f * pixelColor.Z)}");
    }
}
