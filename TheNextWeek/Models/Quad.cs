using Silk.NET.Maths;
using TheNextWeek.Contracts.Materials;
using TheNextWeek.Contracts.Models;
using TheNextWeek.Utils;

namespace TheNextWeek.Models;

public class Quad : Hittable
{
    private readonly Vector3D<double> _q;
    private readonly Vector3D<double> _u;
    private readonly Vector3D<double> _v;
    private readonly Material _mat;

    public Quad(Vector3D<double> origin, Vector3D<double> u, Vector3D<double> v, Material mat)
    {
        _q = origin;
        _u = u;
        _v = v;
        _mat = mat;
    }

    public virtual void SetBoundingBox()
    {
        BoundingBox = new AABB(_q, _q + _u + _v).Pad();
    }

    public override bool Hit(Ray ray, Interval ray_t, ref HitRecord hit_record)
    {
        return false;
    }
}
