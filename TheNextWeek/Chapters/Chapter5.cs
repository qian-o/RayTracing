using Silk.NET.Maths;
using TheNextWeek.Contracts.Chapters;
using TheNextWeek.Materials;
using TheNextWeek.Models;
using TheNextWeek.Textures;
using TheNextWeek.Utils;

namespace TheNextWeek.Chapters;

public class Chapter5 : IChapter
{
    public void Run()
    {
        HittableList world = new();

        NoiseTexture pertext = new(4.0);
        world.Add(new Sphere(new Vector3D<double>(0, -1000, 0), 1000, new Lambertian(pertext)));
        world.Add(new Sphere(new Vector3D<double>(0, 2, 0), 2, new Lambertian(pertext)));

        Camera camera = new()
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            Samples = 100,
            MaxDepth = 50,
            Background = new Vector3D<double>(0.70, 0.80, 1.00),
            Fov = 20.0,
            LookFrom = new Vector3D<double>(13, 2, 3),
            LookAt = new Vector3D<double>(0, 0, 0),
            Up = new Vector3D<double>(0, 1, 0),
            DefocusAngle = 0.0
        };

        camera.Render(world);
    }
}
