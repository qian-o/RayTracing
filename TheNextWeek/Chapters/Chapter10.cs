using Silk.NET.Maths;
using TheNextWeek.Contracts.Chapters;
using TheNextWeek.Contracts.Models;
using TheNextWeek.Contracts.Textures;
using TheNextWeek.Helpers;
using TheNextWeek.Materials;
using TheNextWeek.Models;
using TheNextWeek.Textures;
using TheNextWeek.Utils;

namespace TheNextWeek.Chapters;

public class Chapter10 : IChapter
{
    public void Run()
    {
        // FinalScene(400, 250, 4);

        FinalScene(800, 10000, 40);
    }

    private static void FinalScene(int imageWidth, int samples, int maxDepth)
    {
        HittableList boxes1 = new();
        Lambertian ground = new(new Vector3D<double>(0.48, 0.83, 0.53));

        int boxesPerSide = 20;
        for (int i = 0; i < boxesPerSide; i++)
        {
            for (int j = 0; j < boxesPerSide; j++)
            {
                double w = 100.0;
                double x0 = -1000.0 + i * w;
                double z0 = -1000.0 + j * w;
                double y0 = 0.0;
                double x1 = x0 + w;
                double y1 = MathHelper.RandomDouble(1, 101);
                double z1 = z0 + w;

                boxes1.Add(HittableHelper.Box(new Vector3D<double>(x0, y0, z0), new Vector3D<double>(x1, y1, z1), ground));
            }
        }

        HittableList world = new();

        world.Add(new BvhNode(boxes1));

        DiffuseLight light = new(new Vector3D<double>(7, 7, 7));
        world.Add(new Quad(new Vector3D<double>(123, 554, 147), new Vector3D<double>(300, 0, 0), new Vector3D<double>(0, 0, 265), light));

        Vector3D<double> center1 = new(400, 400, 200);
        Vector3D<double> center2 = center1 + new Vector3D<double>(30, 0, 0);
        Lambertian sphereMaterial = new(new Vector3D<double>(0.7, 0.3, 0.1));
        world.Add(new Sphere(center1, center2, 50, sphereMaterial));

        world.Add(new Sphere(new Vector3D<double>(260, 150, 45), 50, new Dielectric(1.5)));
        world.Add(new Sphere(new Vector3D<double>(0, 150, 145), 50, new Metal(new Vector3D<double>(0.8, 0.8, 0.9), 1.0)));

        Hittable boundary = new Sphere(new Vector3D<double>(360, 150, 145), 70, new Dielectric(1.5));
        world.Add(boundary);
        world.Add(new ConstantMedium(boundary, 0.2, new Vector3D<double>(0.2, 0.4, 0.9)));
        boundary = new Sphere(new Vector3D<double>(0, 0, 0), 5000, new Dielectric(1.5));
        world.Add(new ConstantMedium(boundary, 0.0001, new Vector3D<double>(1, 1, 1)));

        Lambertian emat = new(new ImageTexture("Resources/Images/earthmap.jpg".FormatFilePath()));
        world.Add(new Sphere(new Vector3D<double>(400, 200, 400), 100, emat));
        Texture pertext = new NoiseTexture(0.1);
        world.Add(new Sphere(new Vector3D<double>(220, 280, 300), 80, new Lambertian(pertext)));

        HittableList boxes2 = new();
        Lambertian white = new(new Vector3D<double>(0.73, 0.73, 0.73));
        int ns = 1000;
        for (int j = 0; j < ns; j++)
        {
            boxes2.Add(new Sphere(MathHelper.Random(0, 165), 10, white));
        }

        world.Add(new Translate(new RotateY(new BvhNode(boxes2), 15), new Vector3D<double>(-100, 270, 395)));

        Camera camera = new()
        {
            AspectRatio = 1.0,
            ImageWidth = imageWidth,
            Samples = samples,
            MaxDepth = maxDepth,
            Background = new Vector3D<double>(0.0, 0.0, 0.0),
            Fov = 40.0,
            LookFrom = new Vector3D<double>(478.0, 278.0, -600.0),
            LookAt = new Vector3D<double>(278.0, 278.0, 0.0),
            Up = new Vector3D<double>(0.0, 1.0, 0.0),
            DefocusAngle = 0.0
        };

        camera.Render(world);
    }
}
