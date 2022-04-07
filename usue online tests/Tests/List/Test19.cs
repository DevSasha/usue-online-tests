using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Test_Wrapper;
using usue_online_tests.Tests.Components;

namespace usue_online_tests.Tests.List
{
    public class Test19 : ITestCreator, ITest
    {
        public int TestID { get; set; }
        public string Name { get; } = "Матричная алгебра. Тест 19";
        public string Description { get; }
        
        private Matrix _a;
        private Matrix _x;

        public ITest CreateTest(int randomSeed)
        {
            var random = new Random(randomSeed);
            ITest test = new Test19();
            Generate(random);

            test.Text = BuildQuestion1(_a, _x).ToString();
            return test;
        }
        
        private void Generate(Random random)
        {
            var size = random.Next(2, 4);
            _a = Matrix.Build(size, size, random);
            _x = Matrix.Build(size, size, random);
            _a.CorrectMatrixToNonZerDeterminant();
        }

        private static StringBuilder BuildQuestion1(Matrix a, Matrix x)
        {
            var str = new StringBuilder();
            var adjoint = a.GetAdjointMatrix();
            var b = a * x;
            
            str.Append("(45 б.) Если матричное уравнение");
            str.Append(@"\(");
            str.Append(a.Print());
            str.Append("X=");
            str.Append(b.Print());
            str.Append(@"\)");
            
            str.Append("умножить слева на");
            str.Append(@"\(");
            
            str.Append(adjoint.Print());
            str.Append(@"\)");

            str.Append('\n');
            str.Append("Получим равенство");
            
            str.Append(@"\(");
            str.Append(Matrix.PrintInputBoxes((adjoint * a).GetSize(), "aa"));
            str.Append("X=");
            str.Append(Matrix.PrintInputBoxes((adjoint * b).GetSize(), "ax"));
            str.Append(@"\)");

            str.Append(", а если умножить не слева, а справа, получим");
            
            str.Append(@"\(");
            str.Append(Matrix.PrintInputBoxes(a.GetSize(), "ba"));
            str.Append('X');
            str.Append(Matrix.PrintInputBoxes(adjoint.GetSize(), "bb"));
            str.Append('=');
            str.Append(Matrix.PrintInputBoxes((b * adjoint).GetSize(), "bc"));
            str.Append(@"\)");
            
            str.Append('\n');
            return str;
        }

        public int CheckAnswer(int randomSeed, Dictionary<string, string> answers)
        {
            var random = new Random(randomSeed);
            Generate(random);
            var right = 0;
            
            var adjoint = _a.GetAdjointMatrix();
            right += Matrix.CheckMatrix(adjoint * _a, answers, "aa");
            right += Matrix.CheckMatrix(adjoint * _a * _x, answers, "ax");
            right += Matrix.CheckMatrix(_a, answers, "ba");
            right += Matrix.CheckMatrix(adjoint, answers, "bb");
            right += Matrix.CheckMatrix(_a * _x * adjoint, answers, "bc");

            return right;
        }

        public string Text { get; set; }
        public string[] CheckBoxes { get; set; }
        public List<Image> Pictures { get; set; }
    }
}