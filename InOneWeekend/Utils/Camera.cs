using InOneWeekend.Contracts.Models;
using InOneWeekend.Helpers;
using Silk.NET.Maths;

namespace InOneWeekend.Utils;

public class Camera
{
    private double aspect_ratio = 1.0;
    private int image_width = 100;
    private int image_height;
    private int samples_per_pixel = 10;
    private int max_depth = 10;
    private Vector3D<double> center;
    private Vector3D<double> pixel00_loc;
    private Vector3D<double> pixel_delta_u;
    private Vector3D<double> pixel_delta_v;

    public double AspectRatio { get => aspect_ratio; set => aspect_ratio = value; }

    public int ImageWidth { get => image_width; set => image_width = value; }

    public int Samples { get => samples_per_pixel; set => samples_per_pixel = value; }

    public int MaxDepth { get => max_depth; set => max_depth = value; }

    public void Render(Hittable world)
    {
        Initialize();

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
                Vector3D<double> pixel_color = Vector3D<double>.Zero;
                for (int sample = 0; sample < samples_per_pixel; sample++)
                {
                    Ray r = GetRay(i, j);

                    pixel_color += RayColor(r, max_depth, world);
                }

                WriteColor(streamWriter, pixel_color, samples_per_pixel);
            }
        }

        Console.WriteLine("Done.");
    }

    private void Initialize()
    {
        image_height = (int)(image_width / aspect_ratio);
        image_height = image_height < 1 ? 1 : image_height;

        center = Vector3D<double>.Zero;

        // Determine viewport dimensions.
        double focal_length = 1.0;
        double viewport_height = 2.0;
        double viewport_width = viewport_height * ((double)image_width / image_height);

        // Calculate the vectors across the horizontal and down the vertical viewport edges.
        Vector3D<double> viewport_u = new(viewport_width, 0.0, 0.0);
        Vector3D<double> viewport_v = new(0.0, -viewport_height, 0.0);

        // Calculate the horizontal and vertical delta vectors from pixel to pixel.
        pixel_delta_u = viewport_u / image_width;
        pixel_delta_v = viewport_v / image_height;

        // Calculate the location of the upper left pixel.
        Vector3D<double> viewport_upper_left = center - new Vector3D<double>(0.0, 0.0, focal_length) - viewport_u / 2 - viewport_v / 2;
        pixel00_loc = viewport_upper_left + 0.5 * (pixel_delta_u + pixel_delta_v);
    }

    private Ray GetRay(int i, int j)
    {
        // Get a randomly sampled camera ray for the pixel at location i,j.
        Vector3D<double> pixel_center = pixel00_loc + (j * pixel_delta_u) + (i * pixel_delta_v);
        Vector3D<double> pixel_sample = pixel_center + PixelSampleSquare();

        Vector3D<double> ray_origin = center;
        Vector3D<double> ray_direction = pixel_sample - ray_origin;

        return new Ray(ray_origin, ray_direction);
    }

    private Vector3D<double> PixelSampleSquare()
    {
        // Returns a random point in the square surrounding a pixel at the origin.
        var px = -0.5 + MathHelper.RandomDouble();
        var py = -0.5 + MathHelper.RandomDouble();

        return (px * pixel_delta_u) + (py * pixel_delta_v);
    }

    public static Vector3D<double> RayColor(Ray ray, int depth, Hittable world)
    {
        // If we've exceeded the ray bounce limit, no more light is gathered.
        if (depth <= 0)
        {
            return Vector3D<double>.Zero;
        }

        if (world.Hit(ray, new Interval(0.001, double.PositiveInfinity), out HitRecord rec))
        {
            return rec.Mat!.Scatter(ray, rec, out Vector3D<double> attenuation, out Ray scattered)
                ? attenuation * RayColor(scattered, depth - 1, world)
                : Vector3D<double>.Zero;
        }

        Vector3D<double> unit_direction = Vector3D.Normalize(ray.Direction);
        double a = 0.5 * (unit_direction.Y + 1.0);

        return (1.0 - a) * Vector3D<double>.One + a * new Vector3D<double>(0.5, 0.7, 1.0);
    }

    public static double LinearToGamma(double linear_component)
    {
        if (linear_component > 0.0)
        {
            return Math.Sqrt(linear_component);
        }

        return 0.0;
    }

    private static void WriteColor(StreamWriter streamWriter, Vector3D<double> pixelColor, int samples_per_pixel)
    {
        double r = pixelColor.X;
        double g = pixelColor.Y;
        double b = pixelColor.Z;

        // Divide the color by the number of samples.
        double scale = 1.0 / samples_per_pixel;
        r *= scale;
        g *= scale;
        b *= scale;

        // Apply the linear to gamma transform.
        r = LinearToGamma(r);
        g = LinearToGamma(g);
        b = LinearToGamma(b);

        // Write the translated [0,255] value of each color component.
        Interval interval = new(0.0, 0.999);
        streamWriter.WriteLine($"{(int)(255.999f * interval.Clamp(r))} {(int)(255.999f * interval.Clamp(g))} {(int)(255.999f * interval.Clamp(b))}");
    }
}
