using InOneWeekend.Contracts.Chapters;

namespace InOneWeekend.Chapters;

/// <summary>
/// Output an Image
/// </summary>
public class Chapter2 : IChapter
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
                double r = (double)j / (image_width - 1);
                double g = (double)i / (image_height - 1);
                double b = 0.25;

                int ir = (int)(255.999 * r);
                int ig = (int)(255.999 * g);
                int ib = (int)(255.999 * b);

                streamWriter.WriteLine($"{ir} {ig} {ib}");
            }
        }

        Console.WriteLine("Done!");
    }
}
