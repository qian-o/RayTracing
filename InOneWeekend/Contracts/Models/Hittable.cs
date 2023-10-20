using InOneWeekend.Utils;

namespace InOneWeekend.Contracts.Models;

public abstract class Hittable
{
    public abstract bool Hit(Ray ray, Interval ray_t, out HitRecord hit_record);
}