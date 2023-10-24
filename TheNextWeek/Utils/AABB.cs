using Silk.NET.Maths;

namespace TheNextWeek.Utils;

public class AABB
{
    public static AABB Empty => new(Interval.Empty, Interval.Empty, Interval.Empty);

    public Interval X { get; private set; }

    public Interval Y { get; private set; }

    public Interval Z { get; private set; }

    public AABB(Interval ix, Interval iy, Interval iz)
    {
        X = ix;
        Y = iy;
        Z = iz;

        PadToMinimums();
    }

    public AABB(Vector3D<double> a, Vector3D<double> b)
    {
        X = new Interval(Math.Min(a[0], b[0]), Math.Max(a[0], b[0]));
        Y = new Interval(Math.Min(a[1], b[1]), Math.Max(a[1], b[1]));
        Z = new Interval(Math.Min(a[2], b[2]), Math.Max(a[2], b[2]));

        PadToMinimums();
    }

    public AABB(AABB box0, AABB box1)
    {
        X = new Interval(box0.X, box1.X);
        Y = new Interval(box0.Y, box1.Y);
        Z = new Interval(box0.Z, box1.Z);
    }

    public Interval Axis(int n)
    {
        if (n == 1)
        {
            return Y;
        }

        if (n == 2)
        {
            return Z;
        }

        return X;
    }

    public bool Hit(Ray ray, Interval ray_t)
    {
        for (int a = 0; a < 3; a++)
        {
            double invD = 1.0 / ray.Direction[a];

            double t0 = (Axis(a).Min - ray.Origin[a]) * invD;
            double t1 = (Axis(a).Max - ray.Origin[a]) * invD;

            if (invD < 0.0)
            {
                (t1, t0) = (t0, t1);
            }

            ray_t.Min = t0 > ray_t.Min ? t0 : ray_t.Min;
            ray_t.Max = t1 < ray_t.Max ? t1 : ray_t.Max;

            if (ray_t.Max <= ray_t.Min)
            {
                return false;
            }
        }

        return true;
    }

    private void PadToMinimums()
    {
        double delta = 0.0001;

        if (X.Size < delta)
        {
            X = X.Expand(delta);
        }

        if (Y.Size < delta)
        {
            Y = Y.Expand(delta);
        }

        if (Z.Size < delta)
        {
            Z = Z.Expand(delta);
        }
    }
}
