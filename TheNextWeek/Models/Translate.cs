using Silk.NET.Maths;
using TheNextWeek.Contracts.Models;
using TheNextWeek.Utils;

namespace TheNextWeek.Models;

public class Translate : Hittable
{
    private readonly Hittable _object;
    private readonly Vector3D<double> _offset;

    public Translate(Hittable @object, Vector3D<double> offset)
    {
        _object = @object;
        _offset = offset;

        BoundingBox = @object.BoundingBox + offset;
    }

    public override bool Hit(Ray ray, Interval ray_t, ref HitRecord hit_record)
    {
        Ray offset_r = new(ray.Origin - _offset, ray.Direction, ray.Time);

        if (!_object.Hit(offset_r, ray_t, ref hit_record))
        {
            return false;
        }

        hit_record.P += _offset;

        return true;
    }
}
