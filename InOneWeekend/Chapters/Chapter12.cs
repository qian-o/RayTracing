using InOneWeekend.Contracts.Chapters;
using InOneWeekend.Materials;
using InOneWeekend.Models;
using InOneWeekend.Utils;
using Silk.NET.Maths;

namespace InOneWeekend.Chapters;

public class Chapter12 : IChapter
{
    public void Run()
    {
        Lambertian material_ground = new(new Vector3D<double>(0.8, 0.8, 0.0));
        Lambertian material_center = new(new Vector3D<double>(0.1, 0.2, 0.5));
        Dielectric material_left = new(1.5);
        Metal material_right = new(new Vector3D<double>(0.8, 0.6, 0.2), 0.0);

        HittableList world = new();
        world.Add(new Sphere(new Vector3D<double>(0.0, -100.5, -1.0), 100, material_ground));
        world.Add(new Sphere(new Vector3D<double>(0.0, 0.0, -1.0), 0.5, material_center));
        world.Add(new Sphere(new Vector3D<double>(-1.0, 0.0, -1.0), 0.5, material_left));
        world.Add(new Sphere(new Vector3D<double>(-1.0, 0.0, -1.0), -0.4, material_left));
        world.Add(new Sphere(new Vector3D<double>(1.0, 0.0, -1.0), 0.5, material_right));

        Camera camera = new()
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            Samples = 100,
            MaxDepth = 50,
            Fov = 90.0,
            LookFrom = new Vector3D<double>(-2.0, 2.0, 1.0),
            LookAt = new Vector3D<double>(0.0, 0.0, -1.0),
            Up = new Vector3D<double>(0.0, 1.0, 0.0)
        };

        camera.Render(world);
    }
}
