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
    private readonly Vector3D<double> _w;
    private readonly Vector3D<double> _normal;
    private readonly double _d;
    private readonly Material _mat;

    public Quad(Vector3D<double> origin, Vector3D<double> u, Vector3D<double> v, Material mat)
    {
        Vector3D<double> n = Vector3D.Cross(u, v);

        _q = origin;
        _u = u;
        _v = v;
        _w = n / Vector3D.Dot(n, n);
        _mat = mat;
        _normal = Vector3D.Normalize(n);
        _d = Vector3D.Dot(_normal, _q);

        SetBoundingBox();
    }

    public virtual void SetBoundingBox()
    {
        BoundingBox = new AABB(_q, _q + _u + _v).Pad();
    }

    public override bool Hit(Ray ray, Interval ray_t, ref HitRecord hit_record)
    {
        double denom = Vector3D.Dot(_normal, ray.Direction);

        if (Math.Abs(denom) < 1e-8)
        {
            return false;
        }

        double t = (_d - Vector3D.Dot(_normal, ray.Origin)) / denom;
        if (!ray_t.Contains(t))
        {
            return false;
        }

        Vector3D<double> intersection = ray.At(t);
        Vector3D<double> planar_hitpt_vector = intersection - _q;
        double alpha = Vector3D.Dot(_w, Vector3D.Cross(planar_hitpt_vector, _v));
        double beta = Vector3D.Dot(_w, Vector3D.Cross(_u, planar_hitpt_vector));

        if (!IsInterior(alpha, beta, ref hit_record))
        {
            return false;
        }

        hit_record.T = t;
        hit_record.P = intersection;
        hit_record.Mat = _mat;
        hit_record.SetFaceNormal(ray, _normal);

        return true;
    }

    public virtual bool IsInterior(double a, double b, ref HitRecord hit_record)
    {
        if ((a < 0) || (1 < a) || (b < 0) || (1 < b))
        {
            return false;
        }

        hit_record.U = a;
        hit_record.V = b;

        return true;
    }
}
