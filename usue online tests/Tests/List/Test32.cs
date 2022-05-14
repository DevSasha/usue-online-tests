using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using Test_Wrapper;
using usue_online_tests.Tests.Components;

namespace usue_online_tests.Tests.List
{
    public class Test32 : ITestCreator, ITest
    {
        public int TestID { get; set; }
        public string Name { get; } = "Матричная алгебра. Тест 32";
        public string Description { get; }
        
        private Matrix _matrix;
        private Matrix _gauss1;
        private Matrix _gauss2;

        private List<string> _list1;
        private List<string> _list2;

        public ITest CreateTest(int randomSeed)
        {
            var random = new Random(randomSeed);
            ITest test = new Test32();
            Generate(random);

            test.Text = BuildQuestion().ToString();
            return test;
        }
        
        private void Generate(Random rnd)
        {
            _matrix = new Matrix(4, 4);
            for (var i = 1; i <= Math.Min(_matrix.GetRows(), _matrix.GetCols()); ++i)
                _matrix[i, i] = 1;
            
            var kCol = Matrix.Build(_matrix.GetRows(), 1, rnd);
            kCol[kCol.GetRows(), 1] = 0;
            _matrix.SetCol(_matrix.GetCols(), kCol);
            
            for (var row = 1; row <= _matrix.GetRows() - 1; ++row)
            for (var j = row + 1; j <= _matrix.GetRows() - 1; ++j)
                _matrix.SetRow(row, _matrix.GetRow(row) + rnd.NonZeroNext(-4, 5) * _matrix.GetRow(j));

            _gauss2 = 1 * _matrix;
            
            for (var row = _matrix.GetRows(); row > 1; --row)
            for (var j = row - 1; j > 1; --j)
                _matrix.SetRow(row, rnd.NonZeroNext(-4, 5) * _matrix.GetRow(row) + rnd.NonZeroNext(-4, 5) * _matrix.GetRow(j));
            _matrix.SetRow(2, rnd.NonZeroNext(-4, 5) * _matrix.GetRow(2));
            
            _gauss1 = 1 * _matrix;

            for (var row = _matrix.GetRows(); row > 1; --row)
                _matrix.SetRow(row, _matrix.GetRow(row) + rnd.NonZeroNext(-4, 5) * _matrix.GetRow(1));
            
            _matrix.SetRow(1, rnd.NonZeroNext(-4, 5) * _matrix.GetRow(1));

            _list1 = new List<string>();
            for (var i = 2; i <= _gauss1.GetCols(); ++i)
            for (var j = 1; j <= _gauss1.GetRows(); ++j)
                _list1.Add($"{j},{i}");

            _list2 = new List<string>(_list1);
            for (var j = 2; j <= _gauss1.GetRows(); ++j)
                _list2.Remove($"{j},2");
        }

        private StringBuilder BuildQuestion()
        {
            var str = new StringBuilder();
            
            str.Append("(22 б.) Найти ранг матрицы как строчный её ранг:\n");

            str.Append(@"\(");
            str.Append(new MatrixView(_matrix).Print());
            str.Append(@"\sim");
            str.Append(new MatrixView(_gauss1).SetInputs(_list1).PrintInputs("g1_"));
            str.Append(@"\sim");
            str.Append(new MatrixView(_gauss2).SetInputs(_list2).PrintInputs("g2_"));
            str.Append(@"\)");
            str.Append('\n');
            str.Append(@"\(");
            str.Append("Rg");
            str.Append(new MatrixView(_matrix).Print());
            str.Append('=');
            str.Append("<rg>");
            str.Append(@"\)");

            return str;
        }

        public int CheckAnswer(int randomSeed, Dictionary<string, string> answers)
        {
            var random = new Random(randomSeed);
            Generate(random);
            var right = 0;

            if (answers.ContainsKey("rg") &&
                !string.IsNullOrEmpty(answers["rg"]) &&
                Convert.ToInt32(answers["rg"]) == 3) ++right; // TODO _matrix.Rg()

            right += new MatrixView(_gauss1).SetInputs(_list1).CheckMatrix(answers, "g1_");
            right += new MatrixView(_gauss2).SetInputs(_list2).CheckMatrix(answers, "g2_");

            return right;
        }

        public string Text { get; set; }
        public string[] CheckBoxes { get; set; }
        public List<Image> Pictures { get; set; }
    }
}
