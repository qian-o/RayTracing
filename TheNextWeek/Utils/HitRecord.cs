using TheNextWeek.Contracts.Materials;
using Silk.NET.Maths;

namespace TheNextWeek.Utils;

public class HitRecord
{
    public Vector3D<double> P { get; set; }

    public Vector3D<double> Normal { get; set; }

    public Material? Mat { get; set; }

    public double T { get; set; }

    public bool FrontFace { get; set; }

    public void SetFaceNormal(Ray ray, Vector3D<double> outward_normal)
    {
        // If the ray is coming from the inside of the object, then the normal needs to be reversed.
        FrontFace = Vector3D.Dot(ray.Direction, outward_normal) < 0;
        Normal = FrontFace ? outward_normal : -outward_normal;
    }
}
