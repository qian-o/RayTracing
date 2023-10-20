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
                double r = (double)j / (imageWidth - 1);
                double g = (double)i / (imageHeight - 1);
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
