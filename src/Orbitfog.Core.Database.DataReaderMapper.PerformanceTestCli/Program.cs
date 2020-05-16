using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Orbitfog.Core.Database.DataReaderMapper.PerformanceTestCli
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
            public TimeSpan Row1 { get; set; }
            public TimeSpan Row10 { get; set; }
            public TimeSpan Row100 { get; set; }
            public TimeSpan Row1000 { get; set; }
            public TimeSpan Row10000 { get; set; }
            public TimeSpan Row100000 { get; set; }
        }

        static void Main()
        {
            var resultList = new Dictionary<string, Result>();

            RunTest("Hand coded #1", resultList, (int count) =>
            {
                SqlQuery.Test1GetListHandCoded(count);
            });

            RunTest("Hand coded #2", resultList, (int count) =>
            {
                SqlQuery.Test1GetListHandCoded(count);
            });

            RunTest("Hand coded #3", resultList, (int count) =>
            {
                SqlQuery.Test1GetListHandCoded(count);
            });

            RunTest("Orbitfog.Core.Database.DataReaderMapper #1", resultList, (int count) =>
            {
                var x = SqlQuery.Test1GetListOrbitfogCoreDatabaseMapper(count);
            });

            RunTest("Orbitfog.Core.Database.DataReaderMapper #2", resultList, (int count) =>
            {
                var x = SqlQuery.Test1GetListOrbitfogCoreDatabaseMapper(count);
            });

            RunTest("Orbitfog.Core.Database.DataReaderMapper #3", resultList, (int count) =>
            {
                var x = SqlQuery.Test1GetListOrbitfogCoreDatabaseMapper(count);
            });

            RunTest("Dapper (Query&lt;T&gt;) #1", resultList, (int count) =>
            {
                var x = SqlQuery.Test1GetListDapper(count);
            });

            RunTest("Dapper (Query&lt;T&gt;) #2", resultList, (int count) =>
            {
                var x = SqlQuery.Test1GetListDapper(count);
            });

            RunTest("Dapper (Query&lt;T&gt;) #3", resultList, (int count) =>
            {
                var x = SqlQuery.Test1GetListDapper(count);
            });

            RunTest("EntityFrameworkCore #1", resultList, (int count) =>
            {
                var x = SqlQuery.Test1GetListEntityFrameworkCore(count);
            });

            RunTest("EntityFrameworkCore #2", resultList, (int count) =>
            {
                var x = SqlQuery.Test1GetListEntityFrameworkCore(count);
            });

            RunTest("EntityFrameworkCore #3", resultList, (int count) =>
            {
                var x = SqlQuery.Test1GetListEntityFrameworkCore(count);
            });

            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine("Average time");
            Console.WriteLine("");
            Console.WriteLine("| Name | 1 row | 10 rows | 100 rows | 1000 rows | 10000 rows | 100000 rows |");
            Console.WriteLine("|:----|----:|----:|----:|----:|----:|----:|");
            foreach (var item in resultList)
            {
                Console.WriteLine("| " + item.Key + " | " + FormatMs(item.Value.Row1) + " | " + FormatMs(item.Value.Row10) + " | " + FormatMs(item.Value.Row100) + " | " + FormatMs(item.Value.Row1000) + "  | " + FormatMs(item.Value.Row10000) + " | " + FormatMs(item.Value.Row100000) + " |");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        private static string FormatMs(TimeSpan ts)
        {
            return ts.TotalMilliseconds.ToString("0.00");
        }

        private static void RunTest(string testName, Dictionary<string, Result> resultList, Action<int> action)
        {
            var stoper = new Stopwatch();

            resultList.Add(testName, new Result());

            //initialize
            action(row1);
            action(row10);
            action(row100);

            //tests
            resultList[testName].Row1 = RunTest(testName, row1, action);
            resultList[testName].Row10 = RunTest(testName, row10, action);
            resultList[testName].Row100 = RunTest(testName, row100, action);
            resultList[testName].Row1000 = RunTest(testName, row1000, action);
            resultList[testName].Row10000 = RunTest(testName, row10000, action);
            resultList[testName].Row100000 = RunTest(testName, row100000, action);

            Console.WriteLine();
        }

        private static TimeSpan RunTest(string testName, int rowCount, Action<int> action)
        {
            var stoper = new Stopwatch();
            var ts = new TimeSpan();

            for (int i = 0; i < testCount; ++i)
            {
                GC.Collect();

                stoper.Stop();
                stoper.Reset();
                stoper.Start();
                action(rowCount);
                stoper.Stop();

                ts += stoper.Elapsed;
                Console.WriteLine($"{testName} {rowCount}: {FormatMs(stoper.Elapsed)}");
            }
            ts /= testCount;

            return ts;
        }
    }
}
