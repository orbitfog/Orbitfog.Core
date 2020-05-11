using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Orbitfog.Core.Database.Mapper.PerformanceTestCli
{
    class Program
    {
        const int testCount = 5;
        const int row10 = 10;
        const int row100 = 100;
        const int row1000 = 1000;
        const int row10000 = 10000;
        const int row100000 = 100000;

        private class Result
        {
            public TimeSpan Row10 { get; set; }
            public TimeSpan Row100 { get; set; }
            public TimeSpan Row1000 { get; set; }
            public TimeSpan Row10000 { get; set; }
            public TimeSpan Row100000 { get; set; }
        }

        static void Main()
        {
            var resultList = new Dictionary<string, Result>();

            RunTest("Native - first time", resultList, (int count) =>
            {
                Sql.Test1GetListNative(count);
            });
            Console.WriteLine();

            RunTest("Native - second time", resultList, (int count) =>
            {
                Sql.Test1GetListNative(count);
            });
            Console.WriteLine();

            RunTest("OrbitfogCoreDatabaseMapper - first time", resultList, (int count) =>
            {
                var x = Sql.Test1GetListOrbitfogCoreDatabaseMapper(count);
            });
            Console.WriteLine();

            RunTest("OrbitfogCoreDatabaseMapper - second time", resultList, (int count) =>
            {
                var x = Sql.Test1GetListOrbitfogCoreDatabaseMapper(count);
            });
            Console.WriteLine();

            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine("Average time");
            Console.WriteLine("");
            foreach (var item in resultList)
            {
                Console.WriteLine(item.Key + ":");
                Console.WriteLine("* 10 rows: " + item.Value.Row10.TotalMilliseconds + " ms");
                Console.WriteLine("* 100 rows: " + item.Value.Row100.TotalMilliseconds + " ms");
                Console.WriteLine("* 1000 rows: " + item.Value.Row1000.TotalMilliseconds + " ms");
                Console.WriteLine("* 10000 rows: " + item.Value.Row10000.TotalMilliseconds + " ms");
                Console.WriteLine("* 100000 rows: " + item.Value.Row100000.TotalMilliseconds + " ms");
                Console.WriteLine();
            }

            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        private static void RunTest(string testName, Dictionary<string, Result> resultList, Action<int> action)
        {
            var stoper = new Stopwatch();

            resultList.Add(testName, new Result());
            action(1);

            //10
            for (int i = 0; i < testCount; ++i)
            {
                stoper.Stop();
                stoper.Reset();
                stoper.Start();
                action(row10);
                stoper.Stop();

                resultList[testName].Row10 += stoper.Elapsed;
                Console.WriteLine(testName + " 10: " + stoper.Elapsed.TotalMilliseconds + " ms");
            }
            resultList[testName].Row10 = resultList[testName].Row10 / testCount;

            //100
            for (int i = 0; i < testCount; ++i)
            {
                stoper.Stop();
                stoper.Reset();
                stoper.Start();
                action(row100);
                stoper.Stop();

                resultList[testName].Row100 += stoper.Elapsed;
                Console.WriteLine(testName + " 100: " + stoper.Elapsed.TotalMilliseconds + " ms");
            }
            resultList[testName].Row100 = resultList[testName].Row100 / testCount;

            //1000
            for (int i = 0; i < testCount; ++i)
            {
                stoper.Stop();
                stoper.Reset();
                stoper.Start();
                action(row1000);
                stoper.Stop();

                resultList[testName].Row1000 += stoper.Elapsed;
                Console.WriteLine(testName + " 1000: " + stoper.Elapsed.TotalMilliseconds + " ms");
            }
            resultList[testName].Row1000 = resultList[testName].Row1000 / testCount;

            //10000
            for (int i = 0; i < testCount; ++i)
            {
                stoper.Stop();
                stoper.Reset();
                stoper.Start();
                action(row10000);
                stoper.Stop();

                resultList[testName].Row10000 += stoper.Elapsed;
                Console.WriteLine(testName + " 10000: " + stoper.Elapsed.TotalMilliseconds + " ms");
            }
            resultList[testName].Row10000 = resultList[testName].Row10000 / testCount;

            //100000
            for (int i = 0; i < testCount; ++i)
            {
                stoper.Stop();
                stoper.Reset();
                stoper.Start();
                action(row100000);
                stoper.Stop();

                resultList[testName].Row100000 += stoper.Elapsed;
                Console.WriteLine(testName + " 100000: " + stoper.Elapsed.TotalMilliseconds + " ms");
            }
            resultList[testName].Row100000 = resultList[testName].Row100000 / testCount;
        }
    }
}
