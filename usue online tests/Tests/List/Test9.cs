using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Test_Wrapper;
using usue_online_tests.Tests.Components;

namespace usue_online_tests.Tests.List
{
    public class Test9 : ITestCreator, ITest
    {
        public int TestID { get; set; }
        public string Name { get; } = "Матричная алгебра. Тест 9";
        public string Description { get; }

        private int _aLen;
        private int _aRow;
        private int _aCol;
        private Matrix _b1;
        private Matrix _b2;
        private Matrix _c1;
        private Matrix _c2;
        
        public ITest CreateTest(int randomSeed)
        {
            var random = new Random(randomSeed);
            ITest test = new Test9();
            Generate(random);

            test.Text = BuildQuestion1(_aLen, _aRow, _aCol).ToString();
            test.Text += BuildQuestion2(_b1, _b2, 2, "b").ToString();
            test.Text += BuildQuestion2(_c1, _c2, 3, "c").ToString();

            return test;
        }

        private void Generate(Random random)
        {
            _aLen = random.Next(2, 5);
            _aRow = random.Next(1, 5);
            _aCol = random.Next(1, 5);
            
            _b1 = Matrix.Build(2, 2, random);
            _b2 = Matrix.Build(2, 3, random);
            
            _c1 = Matrix.Build(3, 3, random);
            _c2 = Matrix.Build(3, 2, random);
        }

        private static StringBuilder BuildQuestion1(int len, int a, int b)
        {
            var str = new StringBuilder();
            str.Append(@"1) (12 б.) Введите значения индексов в формуле для \(\textbf{R = GV}:\)");
            str.Append('\n');
            str.Append(@"\(");

            str.Append($"r_{{{a}{b}}} = ");
            for (var i = 0; i < b; i++)
                str.Append($"g_{{<a{i}0><a{i}1>}}v_{{<a{i}2><a{i}3>}}+");

            str.Remove(str.Length - 1, 1);
            
            str.Append(@"\)" + '\n');
            return str;
        }
        
        private static StringBuilder BuildQuestion2(Matrix a, Matrix b, int num, string key)
        {
            var str = new StringBuilder();
            str.Append($"{num}) (6 б.) Введите коэффициенты матрицы:");
            str.Append(@"\(");
            str.Append(a.Print());
            str.Append(b.Print());
            str.Append(" = ");

            str.Append(Matrix.PrintInputBoxes((a * b).GetSize(), key));

            str.Append(@"\)");
            str.Append('\n');
            return str;
        }

        public int CheckAnswer(int randomSeed, Dictionary<string, string> answers)
        {
            var right = 0;
            var random = new Random(randomSeed);
            Generate(random);

            for (var i = 0; i < _aLen; i++)
            for (var j = 0; j < 4; j++)
                if (answers.ContainsKey($"a{i}{j}") && !string.IsNullOrEmpty(answers[$"a{i}{j}"]))
                {
                    var num = Convert.ToInt32(answers[$"a{i}{j}"]);
                    switch (j)
                    {
                        case 0 when num == _aRow:
                        case 1 when num == i + 1:
                        case 2 when num == i + 1:
                        case 3 when num == _aCol:
                            right++;
                            break;
                    }
                }
            
            right += Matrix.CheckMatrix(_b1 * _b2, answers, "b");
            right += Matrix.CheckMatrix(_c1 * _c2, answers, "c");

            return right;
        }

        public string Text { get; set; }
        public string[] CheckBoxes { get; set; }
        public List<Image> Pictures { get; set; }
    }
}
