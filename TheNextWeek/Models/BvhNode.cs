using TheNextWeek.Contracts.Models;
using TheNextWeek.Utils;

namespace TheNextWeek.Models;

public class BvhNode : Hittable
{
    private readonly Hittable _left;
    private readonly Hittable _right;

    public BvhNode(HittableList list) : this(list.Objects, 0, list.Objects.Count)
    {
    }

    public BvhNode(List<Hittable> src_objects, int start, int end)
    {
        BoundingBox = AABB.Empty;
        for (int i = start; i < end; i++)
        {
            BoundingBox = new AABB(BoundingBox, src_objects[i].BoundingBox);
        }

        int axis = BoundingBox.LongestAxis();

        List<Hittable> objects = new(src_objects);

        int object_span = end - start;

        if (object_span == 1)
        {
            _left = _right = objects[start];
        }
        else if (object_span == 2)
        {
            if (BoxCompare(objects[start], objects[start + 1], axis) == -1)
            {
                _left = objects[start];
                _right = objects[start + 1];
            }
            else
            {
                _left = objects[start + 1];
                _right = objects[start];
            }
        }
        else
        {
            objects.Sort(start, object_span, Comparer<Hittable>.Create((a, b) => BoxCompare(a, b, axis)));

            int mid = start + object_span / 2;

            _left = new BvhNode(objects, start, mid);
            _right = new BvhNode(objects, mid, end);
        }
    }

    public override bool Hit(Ray ray, Interval ray_t, ref HitRecord hit_record)
    {
        if (!BoundingBox.Hit(ray, ray_t))
        {
            return false;
        }

        bool hit_left = _left.Hit(ray, ray_t, ref hit_record);
        bool hit_right = _right.Hit(ray, new Interval(ray_t.Min, hit_left ? hit_record.T : ray_t.Max), ref hit_record);

        return hit_left || hit_right;
    }

    private static int BoxCompare(Hittable a, Hittable b, int axis)
    {
        return a.BoundingBox[axis].Min.CompareTo(b.BoundingBox[axis].Min);
    }
}
