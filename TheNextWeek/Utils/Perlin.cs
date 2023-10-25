using Silk.NET.Maths;
using System.Runtime.InteropServices;
using TheNextWeek.Helpers;

namespace TheNextWeek.Utils;

public unsafe class Perlin : IDisposable
{
    private const int PointCount = 256;

    private readonly double* _randomDouble;
    private readonly int* _permX;
    private readonly int* _permY;
    private readonly int* _permZ;

    public Perlin()
    {
        double* randomDouble = (double*)Marshal.AllocHGlobal(PointCount * sizeof(double));
        int* permX = (int*)Marshal.AllocHGlobal(PointCount * sizeof(int));
        int* permY = (int*)Marshal.AllocHGlobal(PointCount * sizeof(int));
        int* permZ = (int*)Marshal.AllocHGlobal(PointCount * sizeof(int));

        for (int i = 0; i < PointCount; i++)
        {
            randomDouble[i] = MathHelper.RandomDouble();
            permX[i] = i;
            permY[i] = i;
            permZ[i] = i;
        }

        Permute(permX, PointCount);
        Permute(permY, PointCount);
        Permute(permZ, PointCount);

        _randomDouble = randomDouble;
        _permX = permX;
        _permY = permY;
        _permZ = permZ;
    }

    public double Noise(Vector3D<double> p)
    {
        double u = p.X - Math.Floor(p.X);
        double v = p.Y - Math.Floor(p.Y);
        double w = p.Z - Math.Floor(p.Z);
        u = u * u * (3 - 2 * u);
        v = v * v * (3 - 2 * v);
        w = w * w * (3 - 2 * w);

        double i = (int)Math.Floor(p.X);
        double j = (int)Math.Floor(p.Y);
        double k = (int)Math.Floor(p.Z);

        double[][][] c = new double[2][][];

        for (int di = 0; di < 2; di++)
        {
            c[di] = new double[2][];
            for (int dj = 0; dj < 2; dj++)
            {
                c[di][dj] = new double[2];
                for (int dk = 0; dk < 2; dk++)
                {
                    c[di][dj][dk] = _randomDouble[_permX[(int)(i + di) & 255] ^ _permY[(int)(j + dj) & 255] ^ _permZ[(int)(k + dk) & 255]];
                }
            }
        }

        return TrilinearInterp(c, u, v, w);
    }

    private static void Permute(int* p, int n)
    {
        for (int i = n - 1; i > 0; i--)
        {
            int target = MathHelper.RandomInt(0, i);

            (p[target], p[i]) = (p[i], p[target]);
        }
    }

    private static double TrilinearInterp(double[][][] c, double u, double v, double w)
    {
        double accum = 0.0;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    accum += ((i * u) + (1 - i) * (1 - u)) * ((j * v) + (1 - j) * (1 - v)) * ((k * w) + (1 - k) * (1 - w)) * c[i][j][k];
                }
            }
        }

        return accum;
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal((IntPtr)_randomDouble);
        Marshal.FreeHGlobal((IntPtr)_permX);
        Marshal.FreeHGlobal((IntPtr)_permY);
        Marshal.FreeHGlobal((IntPtr)_permZ);

        GC.SuppressFinalize(this);
    }
}
