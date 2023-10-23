using InOneWeekend.Contracts.Chapters;
using InOneWeekend.Materials;
using InOneWeekend.Models;
using InOneWeekend.Utils;
using Silk.NET.Maths;

namespace InOneWeekend.Chapters;

public class Chapter9 : IChapter
{
    public void Run()
    {
        HittableList world = new();
        world.Add(new Sphere(new Vector3D<double>(0.0, 0.0, -1.0), 0.5, Lambertian.Identity));
        world.Add(new Sphere(new Vector3D<double>(0.0, -100.5, -1.0), 100, Lambertian.Identity));

        Camera camera = new()
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            Samples = 100,
            MaxDepth = 50
        };

        camera.Render(world);
    }
}
