using InOneWeekend.Contracts.Chapters;
using InOneWeekend.Contracts.Models;
using InOneWeekend.Models;
using InOneWeekend.Utils;
using Silk.NET.Maths;

namespace InOneWeekend.Chapters;

public class Chapter6 : IChapter
{
    public void Run()
    {
        // Image
        const double aspect_ratio = 16.0 / 9.0;
        const int image_width = 400;

        // Calculate the image height, and ensure that it's at least 1.
        int image_height = (int)(image_width / aspect_ratio);
        image_height = image_height < 1 ? 1 : image_height;

        // World
        HittableList world = new();
        world.Add(new Sphere(new Vector3D<double>(0.0, 0.0, -1.0), 0.5));
        world.Add(new Sphere(new Vector3D<double>(0.0, -100.5, -1.0), 100));

        // Camera
        double focal_length = 1.0;
        double viewport_height = 2.0;
        double viewport_width = viewport_height * ((double)image_width / image_height);
        Vector3D<double> camera_center = Vector3D<double>.Zero;

        // Calculate the vectors across the horizontal and down the vertical viewport edges.
        Vector3D<double> viewport_u = new(viewport_width, 0.0, 0.0);
        Vector3D<double> viewport_v = new(0.0, -viewport_height, 0.0);

        // Calculate the horizontal and vertical delta vectors from pixel to pixel.
        Vector3D<double> pixel_delta_u = viewport_u / image_width;
        Vector3D<double> pixel_delta_v = viewport_v / image_height;

        // Calculate the location of the upper left pixel.
        Vector3D<double> viewport_upper_left = camera_center - new Vector3D<double>(0.0, 0.0, focal_length) - viewport_u / 2 - viewport_v / 2;
        Vector3D<double> pixel00_loc = viewport_upper_left + 0.5 * (pixel_delta_u + pixel_delta_v);

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
                Vector3D<double> pixel_center = pixel00_loc + (j * pixel_delta_u) + (i * pixel_delta_v);
                Vector3D<double> ray_direction = pixel_center - camera_center;
                Ray ray = new(camera_center, ray_direction);

                Vector3D<double> pixel_color = RayColor(ray, world);

                WriteColor(streamWriter, pixel_color);
            }
        }

        Console.WriteLine("Done!");
    }

    private static Vector3D<double> RayColor(Ray ray, Hittable world)
    {
        if (world.Hit(ray, 0, double.PositiveInfinity, out HitRecord rec))
        {
            return 0.5 * (rec.Normal + Vector3D<double>.One);
        }

        Vector3D<double> unit_direction = Vector3D.Normalize(ray.Direction);
        double a = 0.5 * (unit_direction.Y + 1.0);

        return (1.0 - a) * Vector3D<double>.One + a * new Vector3D<double>(0.5, 0.7, 1.0);
    }

    private static void WriteColor(StreamWriter streamWriter, Vector3D<double> pixelColor)
    {
        // Write the translated [0,255] value of each color component.
        streamWriter.WriteLine($"{(int)(255.999f * pixelColor.X)} {(int)(255.999f * pixelColor.Y)} {(int)(255.999f * pixelColor.Z)}");
    }
}
