using Silk.NET.Maths;
using TheNextWeek.Contracts.Materials;
using TheNextWeek.Models;

namespace TheNextWeek.Helpers;

public static class HittableHelper
{
    public static HittableList Box(Vector3D<double> a, Vector3D<double> b, Material mat)
    {
        HittableList sides = new();

        Vector3D<double> min = new(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
        Vector3D<double> max = new(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));

        Vector3D<double> dx = new(max.X - min.X, 0, 0);
        Vector3D<double> dy = new(0, max.Y - min.Y, 0);
        Vector3D<double> dz = new(0, 0, max.Z - min.Z);

        sides.Add(new Quad(new Vector3D<double>(min.X, min.Y, max.Z), dx, dy, mat));
        sides.Add(new Quad(new Vector3D<double>(max.X, min.Y, max.Z), -dz, dy, mat));
        sides.Add(new Quad(new Vector3D<double>(max.X, min.Y, min.Z), -dx, dy, mat));
        sides.Add(new Quad(new Vector3D<double>(min.X, min.Y, min.Z), dz, dy, mat));
        sides.Add(new Quad(new Vector3D<double>(min.X, max.Y, max.Z), dx, -dz, mat));
        sides.Add(new Quad(new Vector3D<double>(min.X, min.Y, min.Z), dx, dz, mat));

        return sides;
    }
}
