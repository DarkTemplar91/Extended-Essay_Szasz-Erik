using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ConsoleApp7
{
    
    class Matrix
    {
        private int rows;
        private int size;
        private int columns;
        private double[,] _inMatrix;
        public double this[int row, int col]
        {
            get
            {
                return _inMatrix[row, col];
            }
            set
            {
                _inMatrix[row, col] = value;
            }

        }
        public Matrix(int row, int col)
        {
            rows = row;
            columns = col;
            size = row * col;
            _inMatrix = new double[rows, columns];
        }
        public Matrix()
        {

        }
        public Matrix(double[,] a)
        {
            Initialize(a);
            rows = a.GetLength(0);
            columns = a.GetLength(1);
        }

        public void Initialize(double[,] a) => _inMatrix = a;
        public static implicit operator Matrix(double[,] a) => new Matrix(a);

        public static Matrix CreateColumnMatrix(double[] input)
        {
            Matrix column = new Matrix(1, input.Length);

            for (int n = 0; n < input.Length; n++)
            {
                column[0, n] = input[n];
            }
            return column;
        }
        public static Matrix CreateRowMatrix(double[] input)
        {
            Matrix row = new Matrix(input.Length, 1);
            for (int n = 0; n < input.Length; n++)
            {
                row[n, 0] = input[n];
            }
            return row;
        }

        public void Add(double value)
        {
            for (int n = 0; n < rows; n++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _inMatrix[n, j] += value;
                }
            }
        }
        public void Clear()
        {
            for (int n = 0; n < rows; n++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _inMatrix[n, j] = 0;

                }
            }
        }
        public bool IsVector()
        {

            if (rows == 1 || columns == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool IsEmpty()
        {
            bool empt = false;
            for (int n = 0; n < rows; n++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (_inMatrix[n, j] == 0)
                    {
                        empt = true;
                        break;
                    }
                }
            }
            return empt;
        }
        public double Sum()
        {
            double sum = 0;
            for (int n = 0; n < rows; n++)
            {
                for (int j = 0; j < columns; j++)
                {
                    sum += _inMatrix[n, j];
                }
            }
            return sum;
        }
        public void Randomize(int min, int max)
        {
            Random rn = new Random();
            for (int n = 0; n < rows; n++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _inMatrix[n, j] = rn.Next(min * 100, max * 100) * 0.01;
                }
            }
        }
        public void RandomWeight() 
        {
            Random random = new Random();
            Random sign = new Random();
            for (int n = 0; n < Rows; n++)
            {
                for (int j = 0; j < Col; j++)
                {
                    int s=sign.Next(0, 99);
                    int v = sign.Next(20, 80);
                    if (s <= 49)
                    {
                        _inMatrix[n, j] = -v * 0.01;
                    }
                    else
                    {
                        _inMatrix[n, j] = +v*0.01;
                    }
                }
                
            }
        }
        
        public int GetSize()
        {
            return size;
        }
        public void Display()
        {
            for (int n = 0; n < rows; n++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(_inMatrix[n, j] + "; ");
                }
                Console.WriteLine();
            }
        }
        public int Rows => rows;
        public int Col => columns;
        public static double[] ToPackedArray(Matrix a)
        {
            if (a.IsVector() != true)
            {
                throw new Exception("Matrix must be a type vector to convert to array.");
            }
            double[] packed;
            if (a.Col == 1)
            {
                double[] row = new double[a.Rows];
                for (int n = 0; n < a.Rows; n++)
                {
                    row[n] = a[n, 0];
                }
                packed = row;
            }
            else
            {
                double[] col = new double[a.Rows];
                for (int n = 0; n < a.Rows; n++)
                {
                    col[n] = a[0, n];
                }
                packed = col;
            }
            return packed;
        }
        public Matrix GetCol(int a)
        {
            Matrix output = new Matrix(1, Col);
            for (int n = 0; n < Col; n++)
            {
                output[0, n] = _inMatrix[a, n];
            }
            return output;
        }



    }
    class MatrixMath
    {
        public static Matrix Add(Matrix a, Matrix b)
        {

            if ((a.Col != b.Col) || a.Rows != b.Rows)
            {
                throw new Exception("Matrixes must have the same number of rows and columns.");
            }
            Matrix Add = new Matrix(a.Rows, a.Col);
            for (int n = 0; n < Add.Rows; n++)
            {
                for (int j = 0; j < Add.Col; j++)
                {
                    Add[n, j] = a[n, j] + b[n, j];
                }
            }
            return Add;
        }
        public static Matrix Divide(Matrix a, double scal)
        {
            Matrix div = new Matrix(a.Rows, a.Col);
            for (int n = 0; n < div.Rows; n++)
            {
                for (int j = 0; j < div.Col; j++)
                {
                    div[n, j] = a[n, j] / 2;
                }
            }
            return div;
        }
        public static double DotProduct(Matrix a, Matrix b)
        {
            double dp = 0;
            if (a.IsVector() != true || b.IsVector() != true)
            {

                throw new Exception("Both matrixes should be vectors (type column or row).");

            }
            if (a.GetSize() != b.GetSize())
            {
                throw new Exception("Both Vectors must be of the same size!");
            }
            double[] dot1 = Matrix.ToPackedArray(a);
            double[] dot2 = Matrix.ToPackedArray(b);
            for (int n = 0; n < dot1.Length; n++)
            {
                dp += dot1[n] * dot2[n];
            }
            return dp;
        }
        public static Matrix Multiply(Matrix m, double a)
        {


            for (int n = 0; n < m.Rows; n++)
            {
                for (int j = 0; j < m.Col; j++)
                {
                    m[n, j] = m[n, j] * Convert.ToDouble(a);
                }
            }
            Matrix res = m;
            return res;



        }
        public static Matrix MatrixMultiply(Matrix a, Matrix b)
        {
            if (a.Rows != b.Col)
            {
                throw new Exception("The row of the first matrix should be equal to the column of the second!");
            }
            else
            {


                Matrix result = new Matrix(a.Rows, b.Col);
                for (int n = 0; n < result.Rows; n++)
                {
                    for (int j = 0; j < result.Col; j++)
                    {
                        result[n, j] = 0;
                        for (int k = 0; k < a.Col; k++)
                        {
                            result[n, j] += a[n, k] * b[k, j];
                        }
                    }
                }
                return result;
            }
        }
        public static Matrix Subtract(Matrix a, Matrix b)
        {
            Matrix res = new Matrix(a.Rows, a.Col);
            if ((a.Rows != b.Rows) || (a.Col != b.Col))
            {
                throw new Exception("The number of rows and collumns of the two matrixes should be equal!");
            }
            else
            {
                for (int n = 0; n < res.Rows; n++)
                {
                    for (int j = 0; j < res.Col; j++)
                    {
                        res[n, j] = a[n, j] - b[n, j];
                    }
                }
                return res;
            }

        }
        public static Matrix Transpose(Matrix a)
        {
            Matrix res = new Matrix(a.Col, a.Rows);
            for (int n = 0; n < a.Rows; n++)
            {
                for (int j = 0; j < a.Col; j++)
                {
                    res[j, n] = a[n, j];
                }
            }
            return res;
        }
        public static Matrix Identity(int size)
        {

            Matrix id = new Matrix(size, size);
            for (int n = 0; n < size; n++)
            {
                id[n, n] = 1;
            }
            return id;
        }
        public static double VectorLength(Matrix a)
        {
            if (a.IsVector() == false)
            {
                throw new Exception("Can only take vector lengh of vectors");
            }
            double[] d = Matrix.ToPackedArray(a);
            double result = 0;
            for (int n = 0; n < d.Length; n++)
            {
                result += Math.Pow(d[n], 2);
            }
            return Math.Sqrt(result);


        }
    }

    
    
    
    
}
    
