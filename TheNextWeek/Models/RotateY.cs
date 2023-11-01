using Silk.NET.Maths;
using TheNextWeek.Contracts.Models;
using TheNextWeek.Helpers;
using TheNextWeek.Utils;

namespace TheNextWeek.Models;

public class RotateY : Hittable
{
    private readonly Hittable _object;
    private readonly double _sin_theta;
    private readonly double _cos_theta;

    public RotateY(Hittable @object, double angle)
    {
        _object = @object;

        double radians = MathHelper.DegreesToRadians(angle);

        _sin_theta = Math.Sin(radians);
        _cos_theta = Math.Cos(radians);

        AABB boundingBox = @object.BoundingBox;

        Vector3D<double> min = new(double.PositiveInfinity);
        Vector3D<double> max = new(double.NegativeInfinity);

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    double x = (i * boundingBox.X.Max) + ((1 - j) * boundingBox.X.Min);
                    double y = (j * boundingBox.Y.Max) + ((1 - j) * boundingBox.Y.Min);
                    double z = (k * boundingBox.Z.Max) + ((1 - k) * boundingBox.Z.Min);

                    double new_x = (_cos_theta * x) + (_sin_theta * z);
                    double new_z = (-_sin_theta * x) + (_cos_theta * z);

                    Vector3D<double> tester = new(new_x, y, new_z);

                    for (int c = 0; c < 3; c++)
                    {
                        min.Set(c, Math.Min(min[c], tester[c]));
                        max.Set(c, Math.Max(max[c], tester[c]));
                    }
                }
            }
        }

        BoundingBox = new(min, max);
    }

    public override bool Hit(Ray ray, Interval ray_t, ref HitRecord hit_record)
    {
        Vector3D<double> origin = ray.Origin;
        Vector3D<double> direction = ray.Direction;

        origin.Set(0, (_cos_theta * ray.Origin[0]) - (_sin_theta * ray.Origin[2]));
        origin.Set(2, (_sin_theta * ray.Origin[0]) + (_cos_theta * ray.Origin[2]));

        direction.Set(0, (_cos_theta * ray.Direction[0]) - (_sin_theta * ray.Direction[2]));
        direction.Set(2, (_sin_theta * ray.Direction[0]) + (_cos_theta * ray.Direction[2]));

        Ray rotated_ray = new(origin, direction, ray.Time);

        if (!_object.Hit(rotated_ray, ray_t, ref hit_record))
        {
            return false;
        }

        Vector3D<double> p = hit_record.P;
        p.Set(0, (_cos_theta * hit_record.P[0]) + (_sin_theta * hit_record.P[2]));
        p.Set(2, (-_sin_theta * hit_record.P[0]) + (_cos_theta * hit_record.P[2]));

        Vector3D<double> normal = hit_record.Normal;
        normal.Set(0, (_cos_theta * hit_record.Normal[0]) + (_sin_theta * hit_record.Normal[2]));
        normal.Set(2, (-_sin_theta * hit_record.Normal[0]) + (_cos_theta * hit_record.Normal[2]));

        hit_record.P = p;
        hit_record.Normal = normal;

        return true;
    }
}
