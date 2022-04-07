using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace usue_online_tests.Tests.Components
{
    public class Matrix
    {
        private readonly Tuple<int, int> _size;
        private int[,] _data;
        public int Determinant => GetDeterminant();

        public int this[int row, int col]
        {
            get => _data[row - 1, col - 1];
            set => _data[row - 1, col - 1] = value;
        }

        public Matrix(int rows, int cols)
        {
            _size = new Tuple<int, int>(rows, cols);
            _data = new int[rows, cols];
        }
         
        public Matrix(Tuple<int, int> size)
        {
            var (cols, rows) = _size = size;
            _data = new int[cols, rows];
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            var aSize = a.GetSize();
            var bSize = b.GetSize();
            if (!aSize.Equals(bSize))
                throw new Exception();
            
            var result = new Matrix(aSize);

            var (rows, cols) = aSize;
            for (var i = 1; i <= rows; i++)
            for (var j = 1; j <= cols; j++)
                result[i, j] = a[i, j] + b[i, j];

            return result;
        }
        
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.GetCols() != b.GetRows())
                throw new Exception();

            var size = new Tuple<int, int>(a.GetRows(), b.GetCols());
            var result = new Matrix(size);

            var (rows, cols) = size;
            for (var i = 1; i <= rows; i++)
            for (var j = 1; j <= cols; j++)
            {
                var t = 0;
                for (var k = 1; k <= a.GetCols(); k++)
                     t += a[i, k] * b[k, j];
                result[i, j] = t;
            }

            return result;
        }
        
        public static Matrix operator *(double a, Matrix b)
        {
            var result = new Matrix(b.GetSize());
            var (rows, cols) = b.GetSize();
            for (var i = 1; i <= rows; i++)
            for (var j = 1; j <= cols; j++)
                result[i, j] = (int)(a * b[i, j]);

            return result;
        }

        public static Matrix operator *(Matrix a, double b) => b * a;

        public void CorrectMatrixToNonZerDeterminant()
        {
            if (Determinant != 0) return;
            if (this[1, 2] > 0)
                this[1, 2] += 1;
            else
                this[1, 2] -= 1;
        }
        
        public Matrix Transpose()
        {
            var result = new Matrix(_size);
            var (cols, rows) = _size;
            for (var i = 1; i <= cols; ++i)
            for (var j = 1; j <= rows; ++j)
                result[j, i] = this[i, j];

            return result;
        }
        
        public Matrix GetMinor(int row, int col)
        {
            var rows = Enumerable.Range(1, GetRows()).ToList(); rows.RemoveAt(row - 1);
            var cols = Enumerable.Range(1, GetCols()).ToList(); cols.RemoveAt(col - 1);
            return GetMinor(rows, cols);
        }
        
        public Matrix GetMinor(IReadOnlyList<int> rows, IReadOnlyList<int> cols)
        {
            var result = new Matrix(rows.Count, cols.Count);

            for (var i = 1; i <= rows.Count; i++)
            for (var j = 1; j <= cols.Count; j++)
                result[i, j] = this[rows[i - 1], cols[j - 1]];

            return result;
        }

        public int GetAlgebraicAddition(int row, int col)
            => (int) Math.Pow(-1, row + col) * GetMinor(row, col).Determinant;

        public Matrix GetAdjointMatrix()
        {
            var result = new Matrix(GetSize());

            for (var i = 1; i <= GetRows(); i++)
            for (var j = 1; j <= GetCols(); j++)
                result[j, i] = GetAlgebraicAddition(i, j);

            return result;
        }

        public Matrix GetInverseMatrix() => 1.0 / Determinant * GetAdjointMatrix();
        
        public int GetDeterminant()
        {
            if (GetCols() != GetRows())
                throw new Exception();

            if (GetCols() == 1)
                return this[1, 1];

            if (GetCols() == 2)
                return this[1, 1] * this[2, 2] - this[1, 2] * this[2, 1];
            
            var result = 0;
            var r = GetRows();
            for (var i = 1; i <= GetCols(); ++i)
                result += this[r, i] * GetAlgebraicAddition(r, i);

            return result;
        }
        
        public Tuple<int, int> GetSize() => _size;

        public int GetRows() => _size.Item1;
        public int GetCols() => _size.Item2;

        public string PrintWithInputs(List<string> skip, string key = "")
        {
            var(cols, rows) = GetSize();
            var str = new StringBuilder();
            str.Append(@"\begin{pmatrix}");

            for (var i = 1; i <= cols; i++)
            {
                for (var j = 1; j <= rows; j++)
                    str.Append(skip.Contains($"{i},{j}") ? $"<{key}{i}{j}>&" : $"{this[i, j]}&");

                str.Remove(str.Length - 1, 1);
                str.Append(@"\\");
            }
            str.Remove(str.Length - 2, 2);
            str.Append(@"\end{pmatrix}");
            
            return str.ToString();
        }
        
        public string Print()
        {
            var str = new StringBuilder();
            str.Append(@"\begin{pmatrix}");

            for (var i = 1; i <= GetRows(); i++)
            {
                for (var j = 1; j <= GetCols(); j++)
                    str.Append($"{this[i, j]}&");

                str.Remove(str.Length - 1, 1);
                str.Append(@"\\");
            }
            str.Remove(str.Length - 2, 2);
            str.Append(@"\end{pmatrix}");
            
            return str.ToString();
        }

        public static Matrix Build(int rows, int cols, Random random) 
            => Build(new Tuple<int, int>(rows, cols), random);
        
        public static Matrix Build(Tuple<int, int> size, Random random)
        {
            var matrix = new Matrix(size);

            for (var i = 1; i <= matrix.GetRows(); i++)
            for (var j = 1; j <= matrix.GetCols(); j++)
                do
                    matrix[i, j] = random.Next(-5, 5);
                while (matrix[i, j] == 0);
            
            return matrix;
        }
        
        public static int CheckMatrix(Matrix matrix, IReadOnlyDictionary<string, string> answers, string key)
        {
            var right = 0;
            for (var i = 1; i <= matrix.GetRows(); i++)
            for (var j = 1; j <= matrix.GetCols(); j++)
                if (answers.ContainsKey($"{key}{i}{j}") && !string.IsNullOrEmpty(answers[$"{key}{i}{j}"]))
                    if (matrix[i, j] == Convert.ToInt32(answers[$"{key}{i}{j}"]))
                        right++;

            return right;
        }
        
        public static StringBuilder PrintInputBoxes(Tuple<int, int> size, string key)
        {
            var str = new StringBuilder();
            str.Append(@"\begin{pmatrix}");
            var (rows, cols) = size;
            for (var i = 1; i <= rows; i++)
            {
                for (var j = 1; j <= cols; j++)
                    str.Append($"<{key}{i}{j}>&");
                str.Remove(str.Length - 1, 1);
                str.Append(@"\\");
            }
            
            str.Append(@"\end{pmatrix}");
            return str;
        }
    }
}