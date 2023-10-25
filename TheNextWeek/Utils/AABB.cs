using Silk.NET.Maths;

namespace TheNextWeek.Utils;

public struct AABB
{
    public static readonly AABB Empty = new(Interval.Empty, Interval.Empty, Interval.Empty);

    public static readonly AABB Infinite = new(Interval.Infinite, Interval.Infinite, Interval.Infinite);

    public Interval X;

    public Interval Y;

    public Interval Z;

    public readonly Interval this[int i]
    {
        get
        {
            return i switch
            {
                0 => X,
                1 => Y,
                2 => Z,
                _ => throw new ArgumentOutOfRangeException(nameof(i))
            };
        }
    }

    public AABB(Interval ix, Interval iy, Interval iz)
    {
        X = ix;
        Y = iy;
        Z = iz;
    }

    public AABB(Vector3D<double> a, Vector3D<double> b)
    {
        X = new Interval(Math.Min(a[0], b[0]), Math.Max(a[0], b[0]));
        Y = new Interval(Math.Min(a[1], b[1]), Math.Max(a[1], b[1]));
        Z = new Interval(Math.Min(a[2], b[2]), Math.Max(a[2], b[2]));
    }

    public AABB(AABB box0, AABB box1)
    {
        X = new Interval(box0.X, box1.X);
        Y = new Interval(box0.Y, box1.Y);
        Z = new Interval(box0.Z, box1.Z);
    }

    public readonly bool Hit(Ray ray, Interval ray_t)
    {
        for (int a = 0; a < 3; a++)
        {
            double invD = 1.0 / ray.Direction[a];

            double t0 = (this[a].Min - ray.Origin[a]) * invD;
            double t1 = (this[a].Max - ray.Origin[a]) * invD;

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

    public readonly AABB Pad()
    {
        double delta = 0.0001;

        Interval newX = X.Size >= delta ? X : X.Expand(delta);
        Interval newY = Y.Size >= delta ? Y : Y.Expand(delta);
        Interval newZ = Z.Size >= delta ? Z : Z.Expand(delta);

        return new AABB(newX, newY, newZ);
    }

    public readonly int LongestAxis()
    {
        if (X.Size > Y.Size)
        {
            return X.Size > Z.Size ? 0 : 2;
        }
        else
        {
            return Y.Size > Z.Size ? 1 : 2;
        }
    }
}
