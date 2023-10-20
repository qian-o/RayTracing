using InOneWeekend.Utils;

namespace InOneWeekend.Contracts.Models;

public abstract class Hittable
{
    public abstract bool Hit(Ray ray, double ray_tmin, double ray_tmax, out HitRecord hit_record);
}