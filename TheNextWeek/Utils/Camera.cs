using Silk.NET.Maths;
using TheNextWeek.Contracts.Models;
using TheNextWeek.Helpers;

namespace TheNextWeek.Utils;

public class Camera
{
    private double aspect_ratio = 1.0;
    private int image_width = 100;
    private int image_height;
    private int samples_per_pixel = 10;
    private int max_depth = 10;
    private double fov = 90.0;
    private Vector3D<double> lookfrom = new(0.0, 0.0, -1.0);
    private Vector3D<double> lookat = new(0.0, 0.0, 0.0);
    private Vector3D<double> up = new(0.0, 1.0, 0.0);
    private double defocus_angle = 0.0;
    private double focus_dist = 10.0;
    private Vector3D<double> center;
    private Vector3D<double> pixel00_loc;
    private Vector3D<double> pixel_delta_u;
    private Vector3D<double> pixel_delta_v;
    private Vector3D<double> u;
    private Vector3D<double> v;
    private Vector3D<double> w;
    private Vector3D<double> defocus_disk_u;
    private Vector3D<double> defocus_disk_v;

    public double AspectRatio { get => aspect_ratio; set => aspect_ratio = value; }

    public int ImageWidth { get => image_width; set => image_width = value; }

    public int Samples { get => samples_per_pixel; set => samples_per_pixel = value; }

    public int MaxDepth { get => max_depth; set => max_depth = value; }

    public double Fov { get => fov; set => fov = value; }

    public Vector3D<double> LookFrom { get => lookfrom; set => lookfrom = value; }

    public Vector3D<double> LookAt { get => lookat; set => lookat = value; }

    public Vector3D<double> Up { get => up; set => up = value; }

    public double DefocusAngle { get => defocus_angle; set => defocus_angle = value; }

    public double FocusDist { get => focus_dist; set => focus_dist = value; }

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

        center = lookfrom;

        // Determine viewport dimensions.
        double theta = MathHelper.DegreesToRadians(fov);
        double h = Math.Tan(theta / 2.0);
        double viewport_height = 2.0 * h * focus_dist;
        double viewport_width = viewport_height * ((double)image_width / image_height);

        w = Vector3D.Normalize(lookfrom - lookat);
        u = Vector3D.Normalize(Vector3D.Cross(up, w));
        v = Vector3D.Cross(w, u);

        // Calculate the vectors across the horizontal and down the vertical viewport edges.
        Vector3D<double> viewport_u = viewport_width * u;
        Vector3D<double> viewport_v = viewport_height * -v;

        // Calculate the horizontal and vertical delta vectors from pixel to pixel.
        pixel_delta_u = viewport_u / image_width;
        pixel_delta_v = viewport_v / image_height;

        // Calculate the location of the upper left pixel.
        Vector3D<double> viewport_upper_left = center - (focus_dist * w) - viewport_u / 2 - viewport_v / 2;
        pixel00_loc = viewport_upper_left + 0.5 * (pixel_delta_u + pixel_delta_v);

        // Calculate the defocus disk vectors.
        double defocus_radius = focus_dist * Math.Tan(MathHelper.DegreesToRadians(defocus_angle) / 2.0);
        defocus_disk_u = u * defocus_radius;
        defocus_disk_v = v * defocus_radius;
    }

    private Ray GetRay(int i, int j)
    {
        // Get a randomly sampled camera ray for the pixel at location i,j.
        Vector3D<double> pixel_center = pixel00_loc + (j * pixel_delta_u) + (i * pixel_delta_v);
        Vector3D<double> pixel_sample = pixel_center + PixelSampleSquare();

        Vector3D<double> ray_origin = defocus_angle <= 0.0 ? center : DefocusDiskSample();
        Vector3D<double> ray_direction = pixel_sample - ray_origin;
        double ray_time = MathHelper.RandomDouble();

        return new Ray(ray_origin, ray_direction, ray_time);
    }

    private Vector3D<double> PixelSampleSquare()
    {
        // Returns a random point in the square surrounding a pixel at the origin.
        var px = -0.5 + MathHelper.RandomDouble();
        var py = -0.5 + MathHelper.RandomDouble();

        return (px * pixel_delta_u) + (py * pixel_delta_v);
    }

    private Vector3D<double> DefocusDiskSample()
    {
        Vector3D<double> p = MathHelper.RandomInUnitDisk();

        return center + (p.X * defocus_disk_u) + (p.Y * defocus_disk_v);
    }

    public static Vector3D<double> RayColor(Ray ray, int depth, Hittable world)
    {
        // If we've exceeded the ray bounce limit, no more light is gathered.
        if (depth <= 0)
        {
            return Vector3D<double>.Zero;
        }

        HitRecord rec = new();
        if (world.Hit(ray, new Interval(0.001, double.PositiveInfinity), ref rec))
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
