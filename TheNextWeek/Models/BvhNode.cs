using TheNextWeek.Contracts.Models;
using TheNextWeek.Helpers;
using TheNextWeek.Utils;

namespace TheNextWeek.Models;

public class BvhNode : Hittable
{
    private readonly Hittable _left;
    private readonly Hittable _right;
    private readonly AABB _bbox;

    public BvhNode(HittableList list) : this(list.Objects, 0, list.Objects.Count)
    {
    }

    public BvhNode(List<Hittable> src_objects, int start, int end)
    {
        List<Hittable> objects = new(src_objects);

        int axis = MathHelper.RandomInt(0, 2);

        Func<Hittable, Hittable, int> comparator = axis switch
        {
            0 => BoxXCompare,
            1 => BoxYCompare,
            _ => BoxZCompare
        };

        int object_span = end - start;

        if (object_span == 1)
        {
            _left = _right = objects[start];
        }
        else if (object_span == 2)
        {
            if (comparator(objects[start], objects[start + 1]) == -1)
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
            BoxComparer boxComparer = new(comparator);

            objects.Sort(start, object_span, boxComparer);

            int mid = start + object_span / 2;

            _left = new BvhNode(objects, start, mid);
            _right = new BvhNode(objects, mid, end);
        }

        _bbox = new AABB(_left.BoundingBox(), _right.BoundingBox());
    }

    public override bool Hit(Ray ray, Interval ray_t, ref HitRecord hit_record)
    {
        if (!_bbox.Hit(ray, ray_t))
        {
            return false;
        }

        bool hit_left = _left.Hit(ray, ray_t, ref hit_record);
        bool hit_right = _right.Hit(ray, new Interval(ray_t.Min, hit_left ? hit_record.T : ray_t.Max), ref hit_record);

        return hit_left || hit_right;
    }

    public override AABB BoundingBox()
    {
        return _bbox;
    }

    private static int BoxCompare(Hittable a, Hittable b, int axis)
    {
        return a.BoundingBox().Axis(axis).Min.CompareTo(b.BoundingBox().Axis(axis).Min);
    }

    private static int BoxXCompare(Hittable a, Hittable b)
    {
        return BoxCompare(a, b, 0);
    }

    private static int BoxYCompare(Hittable a, Hittable b)
    {
        return BoxCompare(a, b, 1);
    }

    private static int BoxZCompare(Hittable a, Hittable b)
    {
        return BoxCompare(a, b, 2);
    }
}

public class BoxComparer : IComparer<Hittable>
{
    private readonly Func<Hittable, Hittable, int> _comparator;

    public BoxComparer(Func<Hittable, Hittable, int> comparator)
    {
        _comparator = comparator;
    }

    public int Compare(Hittable? x, Hittable? y)
    {
        return _comparator(x!, y!);
    }
}
