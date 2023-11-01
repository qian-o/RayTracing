using Silk.NET.Maths;
using TheNextWeek.Contracts.Materials;
using TheNextWeek.Contracts.Models;
using TheNextWeek.Contracts.Textures;
using TheNextWeek.Helpers;
using TheNextWeek.Materials;
using TheNextWeek.Utils;

namespace TheNextWeek.Models;

public class ConstantMedium : Hittable
{
    private readonly Hittable _boundary;
    private readonly double _negInvDensity;
    private readonly Material _phase_function;

    public ConstantMedium(Hittable boundary, double density, Vector3D<double> color)
    {
        _boundary = boundary;
        _negInvDensity = -1.0 / density;
        _phase_function = new Isotropic(color);
    }

    public ConstantMedium(Hittable boundary, double density, Texture texture)
    {
        _boundary = boundary;
        _negInvDensity = -1.0 / density;
        _phase_function = new Isotropic(texture);
    }

    public override bool Hit(Ray ray, Interval ray_t, ref HitRecord hit_record)
    {
        HitRecord rec1 = default, rec2 = default;

        if (!_boundary.Hit(ray, Interval.Infinite, ref rec1))
        {
            return false;
        }

        if (!_boundary.Hit(ray, new Interval(rec1.T + 0.0001, double.PositiveInfinity), ref rec2))
        {
            return false;
        }

        if (rec1.T < ray_t.Min)
        {
            rec1.T = ray_t.Min;
        }

        if (rec2.T > ray_t.Max)
        {
            rec2.T = ray_t.Max;
        }

        if (rec1.T >= rec2.T)
        {
            return false;
        }

        if (rec1.T < 0)
        {
            rec1.T = 0;
        }

        double ray_length = ray.Direction.Length;
        double distance_inside_boundary = (rec2.T - rec1.T) * ray_length;
        double hit_distance = _negInvDensity * Math.Log(MathHelper.RandomDouble());

        if (hit_distance > distance_inside_boundary)
        {
            return false;
        }

        hit_record.T = rec1.T + hit_distance / ray_length;
        hit_record.P = ray.At(hit_record.T);

        hit_record.Normal = new Vector3D<double>(1, 0, 0);
        hit_record.FrontFace = true;
        hit_record.Mat = _phase_function;

        return true;
    }
}
