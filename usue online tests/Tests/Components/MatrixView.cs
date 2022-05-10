using System;
using System.Collections.Generic;
using System.Text;

namespace usue_online_tests.Tests.Components
{
    public class MatrixView
    {
        public enum MatrixType
        {
            Plain = 0,          /*   ...   */
            Parentheses = 1,    /*  (...)  */
            Brackets = 2,       /*  [...]  */
            Braces = 3,         /*  {...}  */
            Pipes = 4,          /*  |...|  */
            DoublePipes = 5,    /* ||...|| */
        }
        private static readonly string[] LatexMatrixTypes = {
            "matrix",
            "pmatrix",
            "bmatrix",
            "Bmatrix",
            "vmatrix",
            "Vmatrix",
        };
        private readonly Matrix _matrix;
        private List<string> _inputs;
        private MatrixType _type = MatrixType.Parentheses; // Default

        public MatrixView(Matrix matrix)
        {
            _matrix = matrix;
        }
        
        public MatrixView SetType(MatrixType type)
        {
            _type = type;
            return this;
        }
        
        public MatrixView SetInputs(IEnumerable<string> skip)
        {
            _inputs = new List<string>(skip);
            return this;
        }
        
        public int CheckMatrix(IReadOnlyDictionary<string, string> answers, string key)
        {
            var right = 0;
            
            for (var i = 1; i <= _matrix.GetRows(); i++)
            for (var j = 1; j <= _matrix.GetCols(); j++)
                if ((_inputs == null || _inputs.Contains($"{i},{j}"))
                    && answers.ContainsKey($"{key}{i}{j}")
                    && !string.IsNullOrEmpty(answers[$"{key}{i}{j}"])
                    && _matrix[i, j] == Convert.ToInt32(answers[$"{key}{i}{j}"])) ++right;

            return right;
        }

        private static string MatrixBegin(MatrixType type) => @$"\begin{{{LatexMatrixTypes[(int)type]}}}";
        private static string MatrixEnd(MatrixType type) => @$"\end{{{LatexMatrixTypes[(int)type]}}}";
        
        public string Print()
        {
            var str = new StringBuilder();
            str.Append(MatrixBegin(_type));

            for (var i = 1; i <= _matrix.GetRows(); i++)
            {
                for (var j = 1; j <= _matrix.GetCols(); j++)
                    str.Append($"{_matrix[i, j]}&");

                str.Remove(str.Length - 1, 1);
                str.Append(@"\\");
            }
            str.Remove(str.Length - 2, 2);
            str.Append(MatrixEnd(_type));
            
            return str.ToString();
        }

        public string PrintInputs(string key)
        {
            var str = new StringBuilder();
            str.Append(MatrixBegin(_type));

            for (var i = 1; i <= _matrix.GetCols(); i++)
            {
                for (var j = 1; j <= _matrix.GetRows(); j++)
                    if (_inputs != null)
                        str.Append(_inputs.Contains($"{i},{j}") ? $"<{key}{i}{j}>&" : $"{_matrix[i, j]}&");
                    else
                        str.Append($"<{key}{i}{j}>&");
        
                str.Remove(str.Length - 1, 1);
                str.Append(@"\\");
            }
            str.Remove(str.Length - 2, 2);
            str.Append(MatrixEnd(_type));
            
            return str.ToString();
        }
    }
}