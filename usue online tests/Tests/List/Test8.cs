using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Test_Wrapper;
using usue_online_tests.Tests.Components;

namespace usue_online_tests.Tests.List
{
    public class Test8 : ITestCreator, ITest
    {
        public int TestID { get; set; }
        public string Name { get; } = "Матричная алгебра. Тест 8";
        public string Description { get; }

        private Matrix _a1;
        private Matrix _a2;
        private Matrix _b0;
        private Matrix _b1;
        private Matrix _c;

        public ITest CreateTest(int randomSeed)
        {
            var random = new Random(randomSeed);
            ITest test = new Test8();
            Generate(random);

            test.Text = BuildQuestion1(_a1, _a2).ToString();
            test.Text += BuildQuestion2(new List<Matrix>{_b0, _b1}).ToString();
            test.Text += BuildQuestion3(_c).ToString();

            return test;
        }
        
        private void Generate(Random random)
        {
            _a1 = Matrix.Build(2, 2, random);
            _a2 = Matrix.Build(2, 2, random);
            
            _b0 = Matrix.Build(2, 2, random);
            _b1 = Matrix.Build(2, 2, random);
            
            _c = Matrix.Build(2, 2, random);
        }

        private static StringBuilder BuildQuestion1(Matrix a, Matrix b)
        {
            var str = new StringBuilder("1) (4 б.) Введите неизвестные коэффициенты матриц:");
            str.Append('\n');

            str.Append(@"\(");
            var c = a + b;

            str.Append(a.PrintWithInputs(new List<string>{"1,2"}, "aa"));
            str.Append('+');
            str.Append(b.PrintWithInputs(new List<string>{"2,2"}, "ab"));
            str.Append('=');
            str.Append(c.PrintWithInputs(new List<string>{"1,1","2,1"}, "ac"));

            str.Append(@"\)");
            str.Append('\n');
            return str;
        }

        private static StringBuilder BuildQuestion2(IReadOnlyList<Matrix> m)
        {
            var str = new StringBuilder("2) (8 б.) Введите неизвестные коэффициенты матриц:");
            str.Append('\n');

            str.Append(@"\(");
            
            str.Append("а) ");
            str.Append(m[0].Print());
            str.Append("^t = ");
            str.Append(Matrix.PrintInputBoxes(m[0].GetSize(), "b0"));

            str.Append(", б) ");
            str.Append(Matrix.PrintInputBoxes(m[1].GetSize(), "b1"));
            str.Append("^t = ");
            str.Append(m[1].Print());

            str.Append(@"\)");
            str.Append('\n');

            return str;
        }

        private static StringBuilder BuildQuestion3(Matrix m)
        {
            var str = new StringBuilder("3) (4 б.) Введите числовые коэффициенты матрицы:");

            str.Append('\n');
            str.Append(@"\(");
            str.Append(Matrix.PrintInputBoxes(m.GetSize(), "c"));

            str.Append(@"\begin{pmatrix} p \\ q \end{pmatrix} = ");

            str.Append(@"\begin{pmatrix}");
            for (var i = 1; i <= 2; i++)
            {
                str.Append($"{m[i, 1]:+0;-#}p");
                str.Append($"{m[i, 2]:+0;-#}q");
                str.Append(@"\\");
            }
                
            str.Append(@"\end{pmatrix}");

            str.Append(@"\)");
            str.Append('\n');
            
            return str;
        }

        public int CheckAnswer(int randomSeed, Dictionary<string, string> answers)
        {
            var random = new Random(randomSeed);
            Generate(random);

            var ans = new Dictionary<string, int>
            {
                {"aa12", _a1[1, 2]},
                {"ab22", _a2[2, 2]},
                {"ac11", (_a1 + _a2)[1, 1]},
                {"ac21", (_a1 + _a2)[2, 1]},
            };
            var right = ans
                .Where(a => answers.ContainsKey(a.Key) && !string.IsNullOrEmpty(answers[a.Key]))
                .Count(a => a.Value == Convert.ToInt32(answers[a.Key]));
            
            right += Matrix.CheckMatrix(_b0.Transpose(), answers, "b0");
            right += Matrix.CheckMatrix(_b1.Transpose(), answers, "b1");
            right += Matrix.CheckMatrix(_c, answers, "c");
            
            return right;
        }

        public string Text { get; set; }
        public string[] CheckBoxes { get; set; }
        public List<Image> Pictures { get; set; }
    }
}
