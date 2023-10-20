using InOneWeekend.Contracts.Chapters;
using InOneWeekend.Models;
using InOneWeekend.Utils;
using Silk.NET.Maths;

namespace InOneWeekend.Chapters;

public class Chapter8 : IChapter
{
    public void Run()
    {
        HittableList world = new();
        world.Add(new Sphere(new Vector3D<double>(0.0, 0.0, -1.0), 0.5));
        world.Add(new Sphere(new Vector3D<double>(0.0, -100.5, -1.0), 100));

        Camera camera = new()
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            Samples = 100
        };

        camera.Render(world);
    }
}
