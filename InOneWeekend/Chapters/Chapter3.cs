using InOneWeekend.Contracts.Chapters;
using Silk.NET.Maths;

namespace InOneWeekend.Chapters;

/// <summary>
/// The vec3 Class
/// </summary>
public class Chapter3 : IChapter
{
    public void Run()
    {
        // Image
        const int image_width = 256;
        const int image_height = 256;

        // Render
        using FileStream fileStream = File.Create("image.ppm");
        using StreamWriter streamWriter = new(fileStream);

        streamWriter.WriteLine("P3");
        streamWriter.WriteLine($"{image_width} {image_height}");
        streamWriter.WriteLine("255");

        for (int i = 0; i < image_height; i++)
        {
            Console.WriteLine($"Scanlines remaining: {image_height - i}");

            for (int j = 0; j < image_width; j++)
            {
                Vector3D<double> pixelColor = new((double)j / (image_width - 1), (double)i / (image_height - 1), 0.25);

                WriteColor(streamWriter, pixelColor);
            }
        }

        Console.WriteLine("Done!");
    }

    private static void WriteColor(StreamWriter streamWriter, Vector3D<double> pixelColor)
    {
        // Write the translated [0,255] value of each color component.
        streamWriter.WriteLine($"{(int)(255.999f * pixelColor.X)} {(int)(255.999f * pixelColor.Y)} {(int)(255.999f * pixelColor.Z)}");
    }
}
