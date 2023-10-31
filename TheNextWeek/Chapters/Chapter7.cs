using Silk.NET.Maths;
using TheNextWeek.Contracts.Chapters;
using TheNextWeek.Materials;
using TheNextWeek.Models;
using TheNextWeek.Textures;
using TheNextWeek.Utils;

namespace TheNextWeek.Chapters;

public class Chapter7 : IChapter
{
    public void Run()
    {
        int index = 2;

        switch (index)
        {
            case 1:
                SimpleLight();
                break;
            case 2:
                CornellBox();
                break;
            default:
                SimpleLight();
                break;
        }
    }

    private static void SimpleLight()
    {
        HittableList world = new();

        NoiseTexture pertext = new(4);
        world.Add(new Sphere(new Vector3D<double>(0.0, -1000.0, 0.0), 1000.0, new Lambertian(pertext)));
        world.Add(new Sphere(new Vector3D<double>(0.0, 2.0, 0.0), 2.0, new Lambertian(pertext)));

        DiffuseLight difflight = new(new SolidColor(new Vector3D<double>(4.0, 4.0, 4.0)));
        world.Add(new Sphere(new Vector3D<double>(0.0, 7.0, 0.0), 2.0, difflight));
        world.Add(new Quad(new Vector3D<double>(3.0, 1.0, -2.0), new Vector3D<double>(2.0, 0.0, 0.0), new Vector3D<double>(0.0, 2.0, 0.0), difflight));

        Camera camera = new()
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            Samples = 100,
            MaxDepth = 50,
            Background = new Vector3D<double>(0.0, 0.0, 0.0),
            Fov = 20.0,
            LookFrom = new Vector3D<double>(26.0, 3.0, 6.0),
            LookAt = new Vector3D<double>(0.0, 2.0, 0.0),
            Up = new Vector3D<double>(0.0, 1.0, 0.0),
            DefocusAngle = 0.0
        };

        camera.Render(world);
    }

    private static void CornellBox()
    {
        HittableList world = new();

        Lambertian red = new(new SolidColor(new Vector3D<double>(0.65, 0.05, 0.05)));
        Lambertian white = new(new SolidColor(new Vector3D<double>(0.73, 0.73, 0.73)));
        Lambertian green = new(new SolidColor(new Vector3D<double>(0.12, 0.45, 0.15)));
        DiffuseLight light = new(new SolidColor(new Vector3D<double>(15.0, 15.0, 15.0)));

        world.Add(new Quad(new Vector3D<double>(555.0, 0.0, 0.0), new Vector3D<double>(0.0, 555.0, 0.0), new Vector3D<double>(0.0, 0.0, 555.0), green));
        world.Add(new Quad(new Vector3D<double>(0.0, 0.0, 0.0), new Vector3D<double>(0.0, 555.0, 0.0), new Vector3D<double>(0.0, 0.0, 555.0), red));
        world.Add(new Quad(new Vector3D<double>(343.0, 554.0, 332.0), new Vector3D<double>(-130.0, 0.0, 0.0), new Vector3D<double>(0.0, 0.0, -105.0), light));
        world.Add(new Quad(new Vector3D<double>(0.0, 0.0, 0.0), new Vector3D<double>(555.0, 0.0, 0.0), new Vector3D<double>(0.0, 0.0, 555.0), white));
        world.Add(new Quad(new Vector3D<double>(555.0, 555.0, 555.0), new Vector3D<double>(-555.0, 0.0, 0.0), new Vector3D<double>(0.0, 0.0, -555.0), white));
        world.Add(new Quad(new Vector3D<double>(0.0, 0.0, 555.0), new Vector3D<double>(555.0, 0.0, 0.0), new Vector3D<double>(0.0, 555.0, 0.0), white));

        Camera camera = new()
        {
            AspectRatio = 1.0,
            ImageWidth = 600,
            Samples = 500,
            MaxDepth = 50,
            Background = new Vector3D<double>(0.0, 0.0, 0.0),
            Fov = 40.0,
            LookFrom = new Vector3D<double>(278.0, 278.0, -800.0),
            LookAt = new Vector3D<double>(278.0, 278.0, 0.0),
            Up = new Vector3D<double>(0.0, 1.0, 0.0),
            DefocusAngle = 0.0
        };

        camera.Render(world);
    }
}
