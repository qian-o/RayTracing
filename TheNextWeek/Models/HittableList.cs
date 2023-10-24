using TheNextWeek.Contracts.Models;
using TheNextWeek.Utils;

namespace TheNextWeek.Models;

public class HittableList : Hittable
{
    private AABB bbox = AABB.Empty;

    public List<Hittable> Objects { get; }

    public HittableList()
    {
        Objects = new List<Hittable>();
    }

    public HittableList(Hittable hittable) : this()
    {
        Add(hittable);
    }

    public void Add(Hittable hittable)
    {
        Objects.Add(hittable);

        bbox = new AABB(bbox, hittable.BoundingBox());
    }

    public void Clear()
    {
        Objects.Clear();
    }

    public override bool Hit(Ray ray, Interval ray_t, ref HitRecord hit_record)
    {
        HitRecord temp_rec = new();
        bool hit_anything = false;
        double closest_so_far = ray_t.Max;

        foreach (Hittable hittable in Objects)
        {
            if (hittable.Hit(ray, new Interval(ray_t.Min, closest_so_far), ref temp_rec))
            {
                hit_anything = true;
                closest_so_far = temp_rec.T;
                hit_record = temp_rec;
            }
        }

        return hit_anything;
    }

    public override AABB BoundingBox()
    {
        return bbox;
    }
}
