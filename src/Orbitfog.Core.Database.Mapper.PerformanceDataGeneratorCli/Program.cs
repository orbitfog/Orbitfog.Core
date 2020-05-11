using System;
using System.IO;

namespace Orbitfog.Core.Database.Mapper.PerformanceDataGeneratorCli
{
    class Program
    {
        static void Main()
        {
            var numberFormatInfoDot = new System.Globalization.NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };
            var dtFormat = "yyyy-MM-dd HH:mm:ss";

            var sqlFileName = Path.Combine(Directory.GetCurrentDirectory(), "Table1.sql");

            using (var file = new StreamWriter(sqlFileName, false))
            {
                file.Write(@"
CREATE TABLE [dbo].[Test1](
	[Test1Id] [int] NOT NULL,
	[Int1Value] [int] NOT NULL,
	[Int2Value] [int] NOT NULL,
	[Int3Value] [int] NOT NULL,
	[String1Value] [nvarchar](1000) NOT NULL,
	[String2Value] [nvarchar](1000) NOT NULL,
	[String3Value] [nvarchar](1000) NOT NULL,
	[Decimal1Value] [decimal](14, 4) NOT NULL,
	[Decimal2Value] [decimal](14, 4) NOT NULL,
	[Decimal3Value] [decimal](14, 4) NOT NULL,
	[DateTime1Value] [datetime] NOT NULL,
	[DateTime2Value] [datetime] NOT NULL,
	[DateTime3Value] [datetime] NOT NULL,
 CONSTRAINT [PK_Test1] PRIMARY KEY CLUSTERED 
(
	[Test1Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
");

                string s = " n087 6vc 9n t6c  8mn8-957nvkJGJHKGG78o7098yb0875(M({&B_9 n07 b5 vbfdg;fdgjfd  dgfdg987-[53166v341s31 fs3df34536b453 045y45n35575675n67";
                DateTime dt = new DateTime(1970, 1, 1);
                decimal d = 0.1234M;
                for (int i = 1; i <= 100000; ++i)
                {
                    file.Write($@"
INSERT INTO [dbo].[Test1] ([Test1Id],[Int1Value],[Int2Value],[Int3Value],[String1Value],[String2Value],[String3Value],[Decimal1Value],[Decimal2Value],[Decimal3Value],[DateTime1Value],[DateTime2Value],[DateTime3Value])
VALUES ({i},{i + 1},{i + 2},{i + 3},'{s + 1}','{s + 2}','{s + 3}',{(++d).ToString(numberFormatInfoDot)},{(++d).ToString(numberFormatInfoDot)},{(++d).ToString(numberFormatInfoDot)},'{dt.AddMinutes(i + 1).ToString(dtFormat)}','{dt.AddMinutes(i + 2).ToString(dtFormat)}','{dt.AddMinutes(i + 3).ToString(dtFormat)}')
GO
");
                }
            }
            Console.WriteLine("SQL file saved in: " + sqlFileName);

            Console.WriteLine();
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }
    }
}
