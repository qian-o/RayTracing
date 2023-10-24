using TheNextWeek.Utils;

namespace TheNextWeek.Contracts.Models;

public abstract class Hittable
{
    public abstract bool Hit(Ray ray, Interval ray_t, ref HitRecord hit_record);

    public abstract AABB BoundingBox();
}