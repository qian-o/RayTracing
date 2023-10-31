using Silk.NET.Maths;
using TheNextWeek.Contracts.Chapters;
using TheNextWeek.Helpers;
using TheNextWeek.Materials;
using TheNextWeek.Models;
using TheNextWeek.Textures;
using TheNextWeek.Utils;

namespace TheNextWeek.Chapters;

public class Chapter8 : IChapter
{
    public void Run()
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

        world.Add(HittableHelper.Box(new Vector3D<double>(130.0, 0.0, 65.0), new Vector3D<double>(295.0, 165.0, 230.0), white));
        world.Add(HittableHelper.Box(new Vector3D<double>(265.0, 0.0, 295.0), new Vector3D<double>(430.0, 330.0, 460.0), white));

        Camera camera = new()
        {
            AspectRatio = 1.0,
            ImageWidth = 600,
            Samples = 200,
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
