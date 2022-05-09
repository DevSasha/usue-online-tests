using System;

namespace usue_online_tests.Tests.Components
{
    internal static class RandomUtil
    {
        public static int NonZeroNext(this Random random, int min, int max)
        {
            var number = 0;
            while (number == 0)
                number = random.Next(min, max);
            return number;
        }
    }
}