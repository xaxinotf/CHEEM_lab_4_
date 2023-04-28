
using System;
class Program
{
    static double epsilon = 1e-6;

    static double[,] J(double x, double y, double z)
    {
        double[,] jacobian = new double[3, 3];

        jacobian[0, 0] = 2 * x - 1;
        jacobian[0, 1] = 2 * y;
        jacobian[0, 2] = 2 * z;

        jacobian[1, 0] = 2 * x;
        jacobian[1, 1] = 2 * y - 1;
        jacobian[1, 2] = 2 * z;

        jacobian[2, 0] = 2 * x;
        jacobian[2, 1] = 2 * y;
        jacobian[2, 2] = 2 * z + 1;

        return jacobian;
    }

    static double[,] Inverse(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        double[,] inverse = new double[n, n];
        double det = Determinant(matrix);

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                double[,] submatrix = Submatrix(matrix, i, j);
                inverse[j, i] = Math.Pow(-1, i + j) * Determinant(submatrix) / det;
            }
        }

        return inverse;
    }

    static double Determinant(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        double det = 0;

        if (n == 2)
        {
            det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
        }
        else
        {
            for (int j = 0; j < n; j++)
            {
                double[,] submatrix = Submatrix(matrix, 0, j);
                det += Math.Pow(-1, j) * matrix[0, j] * Determinant(submatrix);
            }
        }

        return det;
    }

    static double[,] Submatrix(double[,] matrix, int rowToRemove, int colToRemove)
    {
        int n = matrix.GetLength(0);
        double[,] submatrix = new double[n - 1, n - 1];

        int rowIndex = 0;
        for (int i = 0; i < n; i++)
        {
            if (i == rowToRemove)
            {
                continue;
            }

            int colIndex = 0;
            for (int j = 0; j < n; j++)
            {
                if (j == colToRemove)
                {
                    continue;
                }

                submatrix[rowIndex, colIndex] = matrix[i, j];
                colIndex++;
            }

            rowIndex++;
        }

        return submatrix;
    }

    static double[] F(double x, double y, double z)
    {
        double[] f = new double[3];

        f[0] = x * x - x + y * y + z * z - 5;
        f[1] = x * x + y * y - y + z * z - 4;
        f[2] = x * x + y * y + z * z + z - 6;

        return f;
    }

    static double Norm(double[] vector)
    {
        double norm = 0;
        for (int i = 0; i < vector.Length; i++)
        {
            norm += vector[i] * vector[i];
        }
        return Math.Sqrt(norm);
    }

    static void Main(string[] args)
    {
        double x = 0, y = 0, z = 0;
        int iter = 0;
        Console.WriteLine("Чебан Богдан лабка номер4_нелiнiйнi системи_варiант_1");
        Console.WriteLine("iter 0: x = {0}, y = {1}, z = {2}", x, y, z);

        while (true)
        {
            double[] f = F(x, y, z);
            double[,] jacobian = J(x, y, z);
            double[,] inverse = Inverse(jacobian);

            double dx = -(inverse[0, 0] * f[0] + inverse[0, 1] * f[1] + inverse[0, 2] * f[2]);
            double dy = -(inverse[1, 0] * f[0] + inverse[1, 1] * f[1] + inverse[1, 2] * f[2]);
            double dz = -(inverse[2, 0] * f[0] + inverse[2, 1] * f[1] + inverse[2, 2] * f[2]);

            x += dx;
            y += dy;
            z += dz;

            double norm = Norm(new double[] { dx, dy, dz });

            iter++;

            Console.WriteLine("iter {0}: x = {1}, y = {2}, z = {3}, norm = {4}", iter, x, y, z, norm);

            if (norm < epsilon)
            {
                break;
            }
        }

        Console.ReadLine();
    }
}
