using Silk.NET.Maths;
using System.Runtime.InteropServices;
using TheNextWeek.Helpers;

namespace TheNextWeek.Utils;

public unsafe class Perlin : IDisposable
{
    private const int PointCount = 256;

    private readonly Vector3D<double>* _ranvec;
    private readonly int* _permX;
    private readonly int* _permY;
    private readonly int* _permZ;

    public Perlin()
    {
        Vector3D<double>* ranvec = (Vector3D<double>*)Marshal.AllocHGlobal(PointCount * sizeof(Vector3D<double>));
        int* permX = (int*)Marshal.AllocHGlobal(PointCount * sizeof(int));
        int* permY = (int*)Marshal.AllocHGlobal(PointCount * sizeof(int));
        int* permZ = (int*)Marshal.AllocHGlobal(PointCount * sizeof(int));

        for (int i = 0; i < PointCount; i++)
        {
            ranvec[i] = Vector3D.Normalize(MathHelper.Random(-1, 1));
            permX[i] = i;
            permY[i] = i;
            permZ[i] = i;
        }

        Permute(permX, PointCount);
        Permute(permY, PointCount);
        Permute(permZ, PointCount);

        _ranvec = ranvec;
        _permX = permX;
        _permY = permY;
        _permZ = permZ;
    }

    public double Noise(Vector3D<double> p)
    {
        double u = p.X - Math.Floor(p.X);
        double v = p.Y - Math.Floor(p.Y);
        double w = p.Z - Math.Floor(p.Z);

        double i = (int)Math.Floor(p.X);
        double j = (int)Math.Floor(p.Y);
        double k = (int)Math.Floor(p.Z);

        Vector3D<double>[][][] c = new Vector3D<double>[2][][];

        for (int di = 0; di < 2; di++)
        {
            c[di] = new Vector3D<double>[2][];
            for (int dj = 0; dj < 2; dj++)
            {
                c[di][dj] = new Vector3D<double>[2];
                for (int dk = 0; dk < 2; dk++)
                {
                    c[di][dj][dk] = _ranvec[_permX[(int)(i + di) & 255] ^ _permY[(int)(j + dj) & 255] ^ _permZ[(int)(k + dk) & 255]];
                }
            }
        }

        return TrilinearInterp(c, u, v, w);
    }

    public double Turb(Vector3D<double> p, int depth)
    {
        double accum = 0.0;
        Vector3D<double> tempP = p;
        double weight = 1.0;

        for (int i = 0; i < depth; i++)
        {
            accum += weight * Noise(tempP);
            weight *= 0.5;
            tempP *= 2;
        }

        return Math.Abs(accum);
    }

    private static void Permute(int* p, int n)
    {
        for (int i = n - 1; i > 0; i--)
        {
            int target = MathHelper.RandomInt(0, i);

            (p[target], p[i]) = (p[i], p[target]);
        }
    }

    private static double TrilinearInterp(Vector3D<double>[][][] c, double u, double v, double w)
    {
        var uu = u * u * (3 - 2 * u);
        var vv = v * v * (3 - 2 * v);
        var ww = w * w * (3 - 2 * w);
        double accum = 0.0;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    Vector3D<double> weight = new(u - i, v - j, w - k);

                    accum += ((i * uu) + ((1 - i) * (1 - uu))) * ((j * vv) + ((1 - j) * (1 - vv))) * ((k * ww) + ((1 - k) * (1 - ww))) * Vector3D.Dot(c[i][j][k], weight);
                }
            }
        }

        return accum;
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal((nint)_ranvec);
        Marshal.FreeHGlobal((nint)_permX);
        Marshal.FreeHGlobal((nint)_permY);
        Marshal.FreeHGlobal((nint)_permZ);

        GC.SuppressFinalize(this);
    }
}
