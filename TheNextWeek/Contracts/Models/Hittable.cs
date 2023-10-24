using TheNextWeek.Utils;

namespace TheNextWeek.Contracts.Models;

public abstract class Hittable
{
    public AABB BoundingBox { get; set; }

    public abstract bool Hit(Ray ray, Interval ray_t, ref HitRecord hit_record);
}