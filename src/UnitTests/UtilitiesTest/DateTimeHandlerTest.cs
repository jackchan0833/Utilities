using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UtilitiesTest
{
    [TestClass]
    public class DateTimeHandlerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var baseDt = new DateTime(2000, 1, 1, 0, 0, 0);
            var watch1 = new System.Diagnostics.Stopwatch();
            watch1.Start();
            int i = 0;
            while(i < 1000000)
            {
                var dt = 2000 == 2000 ? baseDt : new DateTime(2000, 1, 1, 0, 0, 0);
                i++;
            }
            watch1.Stop();
            Console.WriteLine("watch1: " + watch1.ElapsedMilliseconds);
            var watch2 = new System.Diagnostics.Stopwatch();
            watch2.Start();
            int x = 0;
            while (x < 1000000)
            {
                var dt = 2000 == 2010 ? baseDt : new DateTime(2010, 1, 1, 0, 0, 0);
                x++;
            }
            watch2.Stop();
            Console.WriteLine("watch2: " + watch2.ElapsedMilliseconds);
            var watch3 = new System.Diagnostics.Stopwatch();
            watch3.Start();
            int r = 0;
            while (r < 1000000)
            {
                var dt = baseDt.AddYears(2010 - 2000);
                r++;
            }
            watch3.Stop();
            Console.WriteLine("watch3: " + watch3.ElapsedMilliseconds);
        }
    }
}
