using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Test_Wrapper;
using usue_online_tests.Tests.Components;

namespace usue_online_tests.Tests.List
{
    public class Test22 : ITestCreator, ITest
    {
        public int TestID { get; set; }
        public string Name { get; } = "Матричная алгебра. Тест 22";
        public string Description { get; }
        
        private Matrix _a;
        private Matrix _x;

        public ITest CreateTest(int randomSeed)
        {
            var random = new Random(randomSeed);
            ITest test = new Test22();
            Generate(random);

            test.Text = BuildQuestion1(_a, _x).ToString();
            return test;
        }
        
        private void Generate(Random random)
        {
            _x = Matrix.Build(random.Next(2, 4), random.Next(2, 4), random);
            _a = Matrix.BuildDet1(_x.GetCols(), random);
        }

        private static StringBuilder BuildQuestion1(Matrix a, Matrix x)
        {
            var str = new StringBuilder();
            
            str.Append("(45 б.) Решите матричное уравнение");
            str.Append(@"\(");
            str.Append('X');
            str.Append(a.Print());
            str.Append('=');
            str.Append((x * a).Print());
            str.Append(@"\):");

            str.Append('\n');
            str.Append(@"\(");
            str.Append("X=");
            str.Append(Matrix.PrintInputBoxes(x.GetSize(), "x"));
            str.Append(@"\)");

            str.Append(". При этом");
            
            str.Append(@"\(");
            str.Append(a.Print());
            str.Append("^{-1}=");
            str.Append(Matrix.PrintInputBoxes(a.GetSize(), "aa"));
            str.Append(@"\)");

            str.Append('\n');
            return str;
        }

        public int CheckAnswer(int randomSeed, Dictionary<string, string> answers)
        {
            var random = new Random(randomSeed);
            Generate(random);
            var right = 0;
            
            right += Matrix.CheckMatrix(_x, answers, "x");
            right += Matrix.CheckMatrix(_a.GetInverseMatrix(), answers, "aa");

            return right;
        }

        public string Text { get; set; }
        public string[] CheckBoxes { get; set; }
        public List<Image> Pictures { get; set; }
    }
}
