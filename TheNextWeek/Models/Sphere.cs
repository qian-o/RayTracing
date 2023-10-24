using Silk.NET.Maths;
using TheNextWeek.Contracts.Materials;
using TheNextWeek.Contracts.Models;
using TheNextWeek.Utils;

namespace TheNextWeek.Models;

public class Sphere : Hittable
{
    private readonly Vector3D<double> _center;
    private readonly double _radius;
    private readonly Material _mat;
    private readonly bool _isMoving;
    private readonly Vector3D<double> _centerVec;

    public Sphere(Vector3D<double> center, double radius, Material mat)
    {
        _center = center;
        _radius = radius;
        _mat = mat;
        _isMoving = false;
    }

    public Sphere(Vector3D<double> center1, Vector3D<double> center2, double radius, Material mat)
    {
        _center = center1;
        _centerVec = center2 - center1;
        _radius = radius;
        _mat = mat;
        _isMoving = true;
    }

    public override bool Hit(Ray ray, Interval ray_t, out HitRecord hit_record)
    {
        hit_record = new HitRecord();

        Vector3D<double> center = _isMoving ? Center(ray.Time) : _center;
        Vector3D<double> oc = center - ray.Origin;
        double a = ray.Direction.LengthSquared;
        double h = Vector3D.Dot(ray.Direction, oc);
        double c = oc.LengthSquared - _radius * _radius;

        double discriminant = h * h - a * c;
        if (discriminant < 0)
        {
            return false;
        }

        double sqrtd = Math.Sqrt(discriminant);

        // Find the nearest root that lies in the acceptable range.
        double root = (h - sqrtd) / a;
        if (!ray_t.Surrounds(root))
        {
            root = (h + sqrtd) / a;

            if (!ray_t.Surrounds(root))
            {
                return false;
            }
        }

        hit_record.T = root;
        hit_record.P = ray.At(hit_record.T);
        Vector3D<double> outward_normal = (hit_record.P - center) / _radius;
        hit_record.SetFaceNormal(ray, outward_normal);
        hit_record.Mat = _mat;

        return true;
    }

    private Vector3D<double> Center(double time)
    {
        if (!_isMoving)
        {
            return _center;
        }

        return _center + time * _centerVec;
    }
}
