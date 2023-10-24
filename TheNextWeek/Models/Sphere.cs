using TheNextWeek.Contracts.Materials;
using TheNextWeek.Contracts.Models;
using TheNextWeek.Utils;
using Silk.NET.Maths;

namespace TheNextWeek.Models;

public class Sphere : Hittable
{
    public Vector3D<double> Center { get; }

    public double Radius { get; }

    public Material Mat { get; }

    public Sphere(Vector3D<double> center, double radius, Material mat)
    {
        Center = center;
        Radius = radius;
        Mat = mat;
    }

    public override bool Hit(Ray ray, Interval ray_t, out HitRecord hit_record)
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
        Vector3D<double> outward_normal = (hit_record.P - Center) / Radius;
        hit_record.SetFaceNormal(ray, outward_normal);
        hit_record.Mat = Mat;

        return true;
    }
}
