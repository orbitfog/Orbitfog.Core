using System;

namespace Orbitfog.Core.Database.DataReaderMapper.PerformanceTestCli
{
    public class Test1
    {
        public int Test1Id { get; set; }
        public int Int1Value { get; set; }
        public int Int2Value { get; set; }
        public int Int3Value { get; set; }
        public string String1Value { get; set; }
        public string String2Value { get; set; }
        public string String3Value { get; set; }
        public decimal Decimal1Value { get; set; }
        public decimal Decimal2Value { get; set; }
        public decimal Decimal3Value { get; set; }
        public DateTime DateTime1Value { get; set; }
        public DateTime DateTime2Value { get; set; }
        public DateTime DateTime3Value { get; set; }

        public Test1()
        {
            Test1Id = 0;
            Int1Value = 0;
            Int2Value = 0;
            Int3Value = 0;
            String1Value = string.Empty;
            String2Value = string.Empty;
            String3Value = string.Empty;
            Decimal1Value = 0;
            Decimal2Value = 0;
            Decimal3Value = 0;
            DateTime1Value = DateTime.MinValue;
            DateTime2Value = DateTime.MinValue;
            DateTime3Value = DateTime.MinValue;
        }
    }
}
