using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Test_Wrapper;
using usue_online_tests.Tests.Components;

namespace usue_online_tests.Tests.List
{
    public class Test11 : ITestCreator, ITest
    {
        public int TestID { get; set; }
        public string Name { get; } = "Матричная алгебра. Тест 11";
        public string Description { get; }

        private int _aLen;
        private int _aRow;
        private int _aCol;
        private Matrix _b0;
        private Matrix _b1;

        public ITest CreateTest(int randomSeed)
        {
            var random = new Random(randomSeed);
            ITest test = new Test11();
            Generate(random);

            test.Text = BuildQuestion1(_aLen, _aRow, _aCol).ToString();
            test.Text += BuildQuestion2(_b0, _b1).ToString();

            return test;
        }
        
        private void Generate(Random random)
        {
            _aLen = random.Next(2, 5);
            _aRow = random.Next(1, 5);
            _aCol = random.Next(1, 5);
            
            _b0 = Matrix.Build(1, 3, random);
            _b1 = Matrix.Build(_b0.GetCols(), 3, random);
        }

        private static StringBuilder BuildQuestion1(int len, int a, int b)
        {
            var str = new StringBuilder();
            str.Append(@"1) (11 б.) Введите значения индексов в формуле для \(\bf{Q = HW^t}:\)");
            str.Append('\n');
            str.Append(@"(здесь \(X^t\) — матрица, транспонированная к \(X\))");
            str.Append('\n');
            str.Append(@"\(");

            str.Append($"q_{{{a}{b}}} = ");
            for (var i = 0; i < len; i++)
                str.Append($"h_{{<a{i}0><a{i}1>}}w_{{<a{i}2><a{i}3>}}+");

            str.Remove(str.Length - 1, 1);
            
            str.Append(@"\)" + '\n');
            return str;
        }

        private static StringBuilder BuildQuestion2(Matrix a, Matrix b)
        {
            var str = new StringBuilder();
            str.Append("2) (11 б.) Заполните поля для ввода, подбирая значения с помощью " +
                       "«умножение на макроуровне» (по строчкам и столбцам):");
            str.Append('\n');
            str.Append(@"\(");
            
            for (var i = 1; i <= a.GetCols(); i++)
            {
                str.Append($"{a[1, i]:+0;-#}");
                str.Append(b.GetRow(i).Print());
            }

            str.Append(" = ");
            str.Append(Matrix.PrintInputBoxes(a.GetSize(), "ba"));
            str.Append(Matrix.PrintInputBoxes(b.GetSize(), "bb"));
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
                        case 0 when num == _aRow:
                        case 1 when num == i + 1:
                        case 2 when num == _aCol:
                        case 3 when num == i + 1:
                            right++;
                            break;
                    }
                }
            
            right += Matrix.CheckMatrix(_b0, answers, "ba");
            right += Matrix.CheckMatrix(_b1, answers, "bb");

            return right;
        }

        public string Text { get; set; }
        public string[] CheckBoxes { get; set; }
        public List<Image> Pictures { get; set; }
    }
}
