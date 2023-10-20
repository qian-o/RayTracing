using InOneWeekend.Contracts.Models;
using InOneWeekend.Utils;

namespace InOneWeekend.Models;

public class HittableList : Hittable
{
    public List<Hittable> Objects { get; }

    public HittableList()
    {
        Objects = new List<Hittable>();
    }

    public void Add(Hittable hittable)
    {
        Objects.Add(hittable);
    }

    public void Clear()
    {
        Objects.Clear();
    }

    public override bool Hit(Ray ray, Interval ray_t, out HitRecord hit_record)
    {
        hit_record = new HitRecord();

        bool hit_anything = false;
        double closest_so_far = ray_t.Max;

        foreach (Hittable hittable in Objects)
        {
            if (hittable.Hit(ray, new Interval(ray_t.Min, closest_so_far), out HitRecord temp_rec))
            {
                hit_anything = true;
                closest_so_far = temp_rec.T;
                hit_record = temp_rec;
            }
        }

        return hit_anything;
    }
}
