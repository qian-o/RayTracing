using Silk.NET.Maths;

namespace InOneWeekend.Utils;

public class Ray
{
    public Vector3D<double> Origin { get; set; }

    public Vector3D<double> Direction { get; set; }

    public Ray()
    {
    }

    public Ray(Vector3D<double> origin, Vector3D<double> direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public Vector3D<double> At(double t) => Origin + t * Direction;
}
