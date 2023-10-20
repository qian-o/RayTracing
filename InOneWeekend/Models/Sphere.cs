using InOneWeekend.Contracts.Models;
using InOneWeekend.Utils;
using Silk.NET.Maths;

namespace InOneWeekend.Models;

public class Sphere : Hittable
{
    public Vector3D<double> Center { get; }

    public double Radius { get; }

    public Sphere(Vector3D<double> center, double radius)
    {
        Center = center;
        Radius = radius;
    }

    public override bool Hit(Ray ray, double ray_tmin, double ray_tmax, out HitRecord hit_record)
    {
        hit_record = new HitRecord();

        Vector3D<double> oc = Center - ray.Origin;
        double a = ray.Direction.LengthSquared;
        double h = Vector3D.Dot(ray.Direction, oc);
        double c = oc.LengthSquared - Radius * Radius;

        double discriminant = h * h - a * c;
        if (discriminant < 0)
        {
            return false;
        }

        double sqrtd = Math.Sqrt(discriminant);

        // Find the nearest root that lies in the acceptable range.
        double root = (h - sqrtd) / a;
        if (root <= ray_tmin || ray_tmax <= root)
        {
            root = (h + sqrtd) / a;

            if (root <= ray_tmin || ray_tmax <= root)
            {
                return false;
            }
        }

        hit_record.T = root;
        hit_record.P = ray.At(hit_record.T);
        Vector3D<double> outward_normal = (hit_record.P - Center) / Radius;
        hit_record.SetFaceNormal(ray, outward_normal);

        return true;
    }
}
