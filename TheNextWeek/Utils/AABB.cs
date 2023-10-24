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
        X = new Interval(Math.Min(a.X,b.X), Math.Max(a.X, b.X));
        Y = new Interval(Math.Min(a.Y, b.Y), Math.Max(a.Y, b.Y));
        Z = new Interval(Math.Min(a.Z, b.Z), Math.Max(a.Z, b.Z));

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
        for (int i = 0; i < 3; i++)
        {
            double invD = 1.0 / ray.Direction[i];

            double t0 = (Axis(i).Min - ray.Origin[i]) * invD;
            double t1 = (Axis(i).Max - ray.Origin[i]) * invD;

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
