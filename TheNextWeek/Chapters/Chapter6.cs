using Silk.NET.Maths;
using TheNextWeek.Contracts.Chapters;
using TheNextWeek.Materials;
using TheNextWeek.Models;
using TheNextWeek.Utils;

namespace TheNextWeek.Chapters;

public class Chapter6 : IChapter
{
    public void Run()
    {
        HittableList world = new();

        Lambertian left_red = new(new Vector3D<double>(1.0, 0.2, 0.2));
        Lambertian back_green = new(new Vector3D<double>(0.2, 1.0, 0.2));
        Lambertian right_blue = new(new Vector3D<double>(0.2, 0.2, 1.0));
        Lambertian upper_orange = new(new Vector3D<double>(1.0, 0.5, 0.0));
        Lambertian lower_teal = new(new Vector3D<double>(0.2, 0.8, 0.8));

        world.Add(new Quad(new Vector3D<double>(-3.0, -2.0, 5.0), new Vector3D<double>(0.0, 0.0, -4.0), new Vector3D<double>(0.0, 4.0, 0.0), left_red));
        world.Add(new Quad(new Vector3D<double>(-2.0, -2.0, 0.0), new Vector3D<double>(4.0, 0.0, 0.0), new Vector3D<double>(0.0, 4.0, 0.0), back_green));
        world.Add(new Quad(new Vector3D<double>(3.0, -2.0, 1.0), new Vector3D<double>(0.0, 0.0, 4.0), new Vector3D<double>(0.0, 4.0, 0.0), right_blue));
        world.Add(new Quad(new Vector3D<double>(-2.0, 3.0, 1.0), new Vector3D<double>(4.0, 0.0, 0.0), new Vector3D<double>(0.0, 0.0, 4.0), upper_orange));
        world.Add(new Quad(new Vector3D<double>(-2.0, -3.0, 5.0), new Vector3D<double>(4.0, 0.0, 0.0), new Vector3D<double>(0.0, 0.0, -4.0), lower_teal));

        Camera camera = new()
        {
            AspectRatio = 1.0,
            ImageWidth = 400,
            Samples = 100,
            MaxDepth = 50,
            Background = new Vector3D<double>(0.70, 0.80, 1.00),
            Fov = 80.0,
            LookFrom = new Vector3D<double>(0, 0, 9),
            LookAt = new Vector3D<double>(0, 0, 0),
            Up = new Vector3D<double>(0, 1, 0),
            DefocusAngle = 0.0
        };

        camera.Render(world);
    }
}
