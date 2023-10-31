using Silk.NET.Maths;
using TheNextWeek.Contracts.Chapters;
using TheNextWeek.Contracts.Materials;
using TheNextWeek.Helpers;
using TheNextWeek.Materials;
using TheNextWeek.Models;
using TheNextWeek.Utils;

namespace TheNextWeek.Chapters;

public class Chapter3 : IChapter
{
    public void Run()
    {
        HittableList world = new();

        Lambertian ground_material = new(new Vector3D<double>(0.5, 0.5, 0.5));
        world.Add(new Sphere(new Vector3D<double>(0, -1000, 0), 1000, ground_material));

        for (int a = -11; a < 11; a++)
        {
            for (int b = -11; b < 11; b++)
            {
                double choose_mat = MathHelper.RandomDouble();
                Vector3D<double> center = new(a + 0.9 * MathHelper.RandomDouble(), 0.2, b + 0.9 * MathHelper.RandomDouble());

                if ((center - new Vector3D<double>(4.0, 0.2, 0.0)).Length > 0.9)
                {
                    Material sphere_material;

                    if (choose_mat < 0.8)
                    {
                        // diffuse
                        Vector3D<double> albedo = MathHelper.Random() * MathHelper.Random();
                        sphere_material = new Lambertian(albedo);
                        Vector3D<double> center2 = center + new Vector3D<double>(0, MathHelper.RandomDouble(0, 0.5), 0);
                        world.Add(new Sphere(center, center2, 0.2, sphere_material));
                    }
                    else if (choose_mat < 0.95)
                    {
                        // metal
                        Vector3D<double> albedo = MathHelper.Random(0.5, 1);
                        double fuzz = MathHelper.RandomDouble(0, 0.5);
                        sphere_material = new Metal(albedo, fuzz);
                        world.Add(new Sphere(center, 0.2, sphere_material));
                    }
                    else
                    {
                        // glass
                        sphere_material = new Dielectric(1.5);
                        world.Add(new Sphere(center, 0.2, sphere_material));
                    }
                }
            }
        }

        Dielectric material1 = new(1.5);
        world.Add(new Sphere(new Vector3D<double>(0, 1, 0), 1.0, material1));

        Lambertian material2 = new(new Vector3D<double>(0.4, 0.2, 0.1));
        world.Add(new Sphere(new Vector3D<double>(-4, 1, 0), 1.0, material2));

        Metal material3 = new(new Vector3D<double>(0.7, 0.6, 0.5), 0.0);
        world.Add(new Sphere(new Vector3D<double>(4, 1, 0), 1.0, material3));

        world = new HittableList(new BvhNode(world));

        Camera camera = new()
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            Samples = 100,
            MaxDepth = 50,
            Background = new Vector3D<double>(0.70, 0.80, 1.00),
            Fov = 20.0,
            LookFrom = new Vector3D<double>(13.0, 2.0, 3.0),
            LookAt = new Vector3D<double>(0.0, 0.0, -1.0),
            Up = new Vector3D<double>(0.0, 1.0, 0.0),
            DefocusAngle = 0.6,
            FocusDist = 10.0
        };

        camera.Render(world);
    }
}
