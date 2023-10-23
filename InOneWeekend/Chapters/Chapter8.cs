using InOneWeekend.Contracts.Chapters;
using InOneWeekend.Materials;
using InOneWeekend.Models;
using InOneWeekend.Utils;
using Silk.NET.Maths;

namespace InOneWeekend.Chapters;

public class Chapter8 : IChapter
{
    public void Run()
    {
        Lambertian material_center = new(new Vector3D<double>(0.7, 0.3, 0.3));

        HittableList world = new();
        world.Add(new Sphere(new Vector3D<double>(0.0, 0.0, -1.0), 0.5, material_center));
        world.Add(new Sphere(new Vector3D<double>(0.0, -100.5, -1.0), 100, material_center));

        Camera camera = new()
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            Samples = 100
        };

        camera.Render(world);
    }
}
