using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Test_Wrapper;
using usue_online_tests.Tests.Components;

namespace usue_online_tests.Tests.List
{
    public class Test10 : ITestCreator, ITest
    {
        public int TestID { get; set; }
        public string Name { get; } = "Матричная алгебра. Тест 10";
        public string Description { get; }

        private int _aLen;
        private int _aRow;
        private int _aCol;
        private Matrix _b0;
        private Matrix _b1;
        private int[] _cNums;
        private List<Matrix> _cMatrices;

        public ITest CreateTest(int randomSeed)
        {
            var random = new Random(randomSeed);
            ITest test = new Test10();
            Generate(random);

            test.Text = BuildQuestion1(_aLen, _aRow, _aCol).ToString();
            test.Text += BuildQuestion2(_b0, _b1, "b").ToString();
            test.Text += BuildQuestion3(_cNums, _cMatrices).ToString();

            return test;
        }
        
        private void Generate(Random random)
        {
            _aLen = random.Next(2, 5);
            _aRow = random.Next(1, 5);
            _aCol = random.Next(1, 5);
            
            _b0 = Matrix.Build(random.Next(2, 4), random.Next(2, 4), random);
            _b1 = Matrix.Build(_b0.GetCols(), random.Next(2, 4), random);

            var length = 3;
            _cNums = new int[length];
            _cMatrices = new List<Matrix>();
            for (var i = 0; i < length; ++i)
            {
                _cNums[i] = random.NonZeroNext(-10, 11);
                _cMatrices.Add(Matrix.Build(2, 2, random));
            }
        }

        private static StringBuilder BuildQuestion1(int len, int a, int b)
        {
            var str = new StringBuilder();
            str.Append(@"1) (12 б.) Введите значения индексов в формуле для \(\bf{P = F^tU}:\)");
            str.Append('\n');
            str.Append(@"(здесь \(X^t\) — матрица, транспонированная к \(X\))");
            str.Append('\n');
            str.Append(@"\(");

            str.Append($"p_{{{a}{b}}} = ");
            for (var i = 0; i < len; i++)
                str.Append($"f_{{<a{i}0><a{i}1>}}u_{{<a{i}2><a{i}3>}}+");

            str.Remove(str.Length - 1, 1);
            
            str.Append(@"\)" + '\n');
            return str;
        }

        private static StringBuilder BuildQuestion2(Matrix a, Matrix b, string key)
        {
            var str = new StringBuilder();
            str.Append("2) (6 б.) Введите коэффициенты матрицы:");
            str.Append('\n');
            str.Append(@"\(");
            str.Append(a.Print());
            str.Append(b.Print());
            str.Append(" = ");
            str.Append(Matrix.PrintInputBoxes((a * b).GetSize(), key));
            str.Append(@"\)");
            str.Append('\n');
            return str;
        }

        private static StringBuilder BuildQuestion3(IReadOnlyList<int> nums, IReadOnlyList<Matrix> matrices)
        {
            var str = new StringBuilder("3) (4 б.) ");

            str.Append(@"\(");
            var m = new Matrix(matrices[0].GetSize());
            for (var i = 0; i < nums.Count; i++)
            {
                m += nums[i] * matrices[i];
                str.Append($"{nums[i]:+0;-#}");
                var a = ((i + 1) / m.GetCols() + 1);
                var b = (i + 3) % m.GetCols() + 1;
                str.Append(matrices[i].PrintWithInputs(new List<string>{$"{a},{b}"}, $"c{i}_"));
            }
            
            str.Append('=');
            str.Append(m.PrintWithInputs(new List<string>{"1,1"}, $"c{nums.Count}_"));
            
            str.Append(@"\)");
            str.Append('\n');
            return str;
        }

        public int CheckAnswer(int randomSeed, Dictionary<string, string> answers)
        {
            var random = new Random(randomSeed);
            Generate(random);
            var right = 0;
            
            for (var i = 0; i < _aLen; i++)
            for (var j = 0; j < 4; j++)
                if (answers.ContainsKey($"a{i}{j}") && !string.IsNullOrEmpty(answers[$"a{i}{j}"]))
                {
                    var num = Convert.ToInt32(answers[$"a{i}{j}"]);
                    switch (j)
                    {
                        case 0 when num == i + 1:
                        case 1 when num == _aRow:
                        case 2 when num == i + 1:
                        case 3 when num == _aCol:
                            right++;
                            break;
                    }
                }
            
            right += Matrix.CheckMatrix(_b0 * _b1, answers, "b");
            
            var m = new Matrix(_cMatrices[0].GetSize());
            var ans = new Dictionary<string, int>();
            for (var i = 0; i < _cNums.Length; i++)
            {
                m += _cNums[i] * _cMatrices[i];
                var a = ((i + 1) / m.GetCols() + 1);
                var b = (i + 3) % m.GetCols() + 1;
                ans.Add($"c{i}_{a}{b}", _cMatrices[i][a, b]);
            }
            ans.Add($"c{_cNums.Length}_11", m[1, 1]);
            right += ans
                .Where(a => answers.ContainsKey(a.Key) && !string.IsNullOrEmpty(answers[a.Key]))
                .Count(a => a.Value == Convert.ToInt32(answers[a.Key]));
            
            return right;
        }

        public string Text { get; set; }
        public string[] CheckBoxes { get; set; }
        public List<Image> Pictures { get; set; }
    }
}
