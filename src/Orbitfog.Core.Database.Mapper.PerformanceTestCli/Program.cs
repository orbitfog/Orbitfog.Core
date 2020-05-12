using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Orbitfog.Core.Database.Mapper.PerformanceTestCli
{
    class Program
    {
        const int testCount = 5;
        const int row1 = 1;
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

            RunTest("Hand coded - first time", resultList, (int count) =>
            {
                Sql.Test1GetListHandCoded(count);
            });
            Console.WriteLine();

            RunTest("Hand coded - second time", resultList, (int count) =>
            {
                Sql.Test1GetListHandCoded(count);
            });
            Console.WriteLine();

            RunTest("Hand coded - third time", resultList, (int count) =>
            {
                Sql.Test1GetListHandCoded(count);
            });
            Console.WriteLine();

            RunTest("Orbitfog.Core.Database.Mapper - first time", resultList, (int count) =>
            {
                var x = Sql.Test1GetListOrbitfogCoreDatabaseMapper(count);
            });
            Console.WriteLine();

            RunTest("Orbitfog.Core.Database.Mapper - second time", resultList, (int count) =>
            {
                var x = Sql.Test1GetListOrbitfogCoreDatabaseMapper(count);
            });
            Console.WriteLine();

            RunTest("Orbitfog.Core.Database.Mapper - third time", resultList, (int count) =>
            {
                var x = Sql.Test1GetListOrbitfogCoreDatabaseMapper(count);
            });
            Console.WriteLine();

            RunTest("Dapper (Query&lt;T&gt;) - first time", resultList, (int count) =>
            {
                var x = Sql.Test1GetListDapper(count);
            });
            Console.WriteLine();

            RunTest("Dapper (Query&lt;T&gt;) - second time", resultList, (int count) =>
            {
                var x = Sql.Test1GetListDapper(count);
            });
            Console.WriteLine();

            RunTest("Dapper (Query&lt;T&gt;) - third time", resultList, (int count) =>
            {
                var x = Sql.Test1GetListDapper(count);
            });
            Console.WriteLine();

            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine("Average time");
            Console.WriteLine("");
            Console.WriteLine("| Name | 10 rows | 100 rows | 1000 rows | 10000 rows | 100000 rows |");
            Console.WriteLine("|:----|----:|----:|----:|----:|----:|");
            foreach (var item in resultList)
            {
                Console.WriteLine("| " + item.Key + " | " + FormatMs(item.Value.Row10) + " | " + FormatMs(item.Value.Row100) + " | " + FormatMs(item.Value.Row1000) + "  | " + FormatMs(item.Value.Row10000) + " | " + FormatMs(item.Value.Row100000) + " |");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        private static string FormatMs(TimeSpan ts)
        {
            return ts.TotalMilliseconds.ToString("0.00") + " ms";
        }

        private static void RunTest(string testName, Dictionary<string, Result> resultList, Action<int> action)
        {
            var stoper = new Stopwatch();

            resultList.Add(testName, new Result());
            action(row1);

            //10
            GC.Collect();
            for (int i = 0; i < testCount; ++i)
            {
                stoper.Stop();
                stoper.Reset();
                stoper.Start();
                action(row10);
                stoper.Stop();

                resultList[testName].Row10 += stoper.Elapsed;
                Console.WriteLine(testName + " 10: " + FormatMs(stoper.Elapsed));
            }
            resultList[testName].Row10 = resultList[testName].Row10 / testCount;

            //100
            GC.Collect();
            for (int i = 0; i < testCount; ++i)
            {
                stoper.Stop();
                stoper.Reset();
                stoper.Start();
                action(row100);
                stoper.Stop();

                resultList[testName].Row100 += stoper.Elapsed;
                Console.WriteLine(testName + " 100: " + FormatMs(stoper.Elapsed));
            }
            resultList[testName].Row100 = resultList[testName].Row100 / testCount;

            //1000
            GC.Collect();
            for (int i = 0; i < testCount; ++i)
            {
                stoper.Stop();
                stoper.Reset();
                stoper.Start();
                action(row1000);
                stoper.Stop();

                resultList[testName].Row1000 += stoper.Elapsed;
                Console.WriteLine(testName + " 1000: " + FormatMs(stoper.Elapsed));
            }
            resultList[testName].Row1000 = resultList[testName].Row1000 / testCount;

            //10000
            GC.Collect();
            for (int i = 0; i < testCount; ++i)
            {
                stoper.Stop();
                stoper.Reset();
                stoper.Start();
                action(row10000);
                stoper.Stop();

                resultList[testName].Row10000 += stoper.Elapsed;
                Console.WriteLine(testName + " 10000: " + FormatMs(stoper.Elapsed));
            }
            resultList[testName].Row10000 = resultList[testName].Row10000 / testCount;

            //100000
            GC.Collect();
            for (int i = 0; i < testCount; ++i)
            {
                stoper.Stop();
                stoper.Reset();
                stoper.Start();
                action(row100000);
                stoper.Stop();

                resultList[testName].Row100000 += stoper.Elapsed;
                Console.WriteLine(testName + " 100000: " + FormatMs(stoper.Elapsed));
            }
            resultList[testName].Row100000 = resultList[testName].Row100000 / testCount;
        }
    }
}
