using Silk.NET.Maths;

namespace TheNextWeek.Utils;

public class Ray
{
    public Vector3D<double> Origin { get; }

    public Vector3D<double> Direction { get; }

    public double Time { get; }

    public Ray(Vector3D<double> origin, Vector3D<double> direction)
    {
        Origin = origin;
        Direction = direction;
        Time = 0.0;
    }

    public Ray(Vector3D<double> origin, Vector3D<double> direction, double time)
    {
        Origin = origin;
        Direction = direction;
        Time = time;
    }

    public Vector3D<double> At(double t) => Origin + t * Direction;
}
