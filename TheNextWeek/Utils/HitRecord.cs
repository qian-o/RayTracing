using Silk.NET.Maths;
using TheNextWeek.Contracts.Materials;

namespace TheNextWeek.Utils;

public struct HitRecord
{
    public Vector3D<double> P;

    public Vector3D<double> Normal;

    public Material? Mat;

    public double T;

    public double U;

    public double V;

    public bool FrontFace;

    public void SetFaceNormal(Ray ray, Vector3D<double> outward_normal)
    {
        // If the ray is coming from the inside of the object, then the normal needs to be reversed.
        FrontFace = Vector3D.Dot(ray.Direction, outward_normal) < 0;
        Normal = FrontFace ? outward_normal : -outward_normal;
    }
}
