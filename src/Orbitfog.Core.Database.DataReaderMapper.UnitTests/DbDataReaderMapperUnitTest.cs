using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Orbitfog.Core.Database.DataReaderMapper.UnitTests
{
    public class DbDataReaderMapperUnitTest
    {
        private enum SomeEnum
        {
            A = 10,
            B = 11,
            C = 12
        }

        private class ClassA
        {
            public string? StringValue { get; set; }
            public bool BoolValue { get; set; }
            public int IntValue { get; set; }
            public int? IntNullableValue { get; set; }
            public Guid GuidValue { get; set; }
            public DateTime DateTimeValue { get; set; }
            public byte[]? ByteArrayValue { get; set; }
            public SomeEnum EnumValue { get; set; }
        }

        private class ClassB
        {
        }

        private class ClassC
        {
#pragma warning disable 0649
            public string? StringValue;
            public bool BoolValue;
            public int IntValue;
            public int? IntNullableValue;
            public Guid GuidValue;
            public DateTime DateTimeValue;
            public byte[]? ByteArrayValue;
            public SomeEnum EnumValue;
#pragma warning restore 0649
        }

        private class ClassD
        {
            public int IntValue1 { get; set; }
#pragma warning disable 0649
            public int IntValue2;
#pragma warning restore 0649
        }

        [Fact]
        public void ToList_ClassA_1()
        {
            int i = 10;
            var fakeDatabaseDataList = new List<FakeDbDataReaderRow>()
            {
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i)
                .AddColumn(typeof(bool), "BoolValue", false)
                .AddColumn(typeof(int), "IntValue", i)
                .AddColumn(typeof(Guid), "GuidValue", Guid.NewGuid())
                .AddColumn(typeof(DateTime), "DateTimeValue", DateTime.Today)
                .AddColumn(typeof(byte[]), "ByteArrayValue", new byte[] { 5, 8 })
                .AddColumn(typeof(int), "EnumValue", i++),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i)
                .AddColumn(typeof(bool), "BoolValue", null)
                .AddColumn(typeof(int), "IntValue", i)
                .AddColumn(typeof(Guid), "GuidValue", null)
                .AddColumn(typeof(DateTime), "DateTimeValue", null)
                .AddColumn(typeof(byte[]), "ByteArrayValue", null)
                .AddColumn(typeof(int), "EnumValue", i++),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i)
                .AddColumn(typeof(bool), "BoolValue", true)
                .AddColumn(typeof(int), "IntValue", i)
                .AddColumn(typeof(Guid), "GuidValue", null)
                .AddColumn(typeof(DateTime), "DateTimeValue", null)
                .AddColumn(typeof(byte[]), "ByteArrayValue", null)
                .AddColumn(typeof(int), "EnumValue", i++)
            };

            var list = DbDataReaderMapper<ClassA>.ToList(new FakeDbDataReader(fakeDatabaseDataList));

            Assert.Equal(3, list.Count);

            Assert.Equal("ble12", list[2].StringValue);

            Assert.False(list[0].BoolValue);
            Assert.False(list[1].BoolValue);
            Assert.True(list[2].BoolValue);

            Assert.NotEqual(Guid.Empty, list[0].GuidValue);
            Assert.Equal(Guid.Empty, list[1].GuidValue);

            Assert.Equal(DateTime.Today, list[0].DateTimeValue);
            Assert.Equal(default, list[1].DateTimeValue);

            Assert.True((new byte[] { 5, 8 }).SequenceEqual(list[0].ByteArrayValue!));
            Assert.Null(list[1].ByteArrayValue);

            Assert.Equal(SomeEnum.A, list[0].EnumValue);
            Assert.Equal(SomeEnum.B, list[1].EnumValue);
            Assert.Equal(SomeEnum.C, list[2].EnumValue);
        }

        [Fact]
        public void ToList_ClassC_1()
        {
            int i = 10;
            var fakeDatabaseDataList = new List<FakeDbDataReaderRow>()
            {
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i)
                .AddColumn(typeof(bool), "BoolValue", false)
                .AddColumn(typeof(int), "IntValue", i)
                .AddColumn(typeof(Guid), "GuidValue", Guid.NewGuid())
                .AddColumn(typeof(DateTime), "DateTimeValue", DateTime.Today)
                .AddColumn(typeof(byte[]), "ByteArrayValue", new byte[] { 5, 8 })
                .AddColumn(typeof(int), "EnumValue", i++),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i)
                .AddColumn(typeof(bool), "BoolValue", null)
                .AddColumn(typeof(int), "IntValue", i)
                .AddColumn(typeof(Guid), "GuidValue", null)
                .AddColumn(typeof(DateTime), "DateTimeValue", null)
                .AddColumn(typeof(byte[]), "ByteArrayValue", null)
                .AddColumn(typeof(int), "EnumValue", i++),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i)
                .AddColumn(typeof(bool), "BoolValue", true)
                .AddColumn(typeof(int), "IntValue", i)
                .AddColumn(typeof(Guid), "GuidValue", null)
                .AddColumn(typeof(DateTime), "DateTimeValue", null)
                .AddColumn(typeof(byte[]), "ByteArrayValue", null)
                .AddColumn(typeof(int), "EnumValue", i++)
            };

            var list = DbDataReaderMapper<ClassC>.ToList(new FakeDbDataReader(fakeDatabaseDataList));

            Assert.Equal(3, list.Count);

            Assert.Equal("ble12", list[2].StringValue);

            Assert.False(list[0].BoolValue);
            Assert.False(list[1].BoolValue);
            Assert.True(list[2].BoolValue);

            Assert.NotEqual(Guid.Empty, list[0].GuidValue);
            Assert.Equal(Guid.Empty, list[1].GuidValue);

            Assert.Equal(DateTime.Today, list[0].DateTimeValue);
            Assert.Equal(default, list[1].DateTimeValue);

            Assert.True((new byte[] { 5, 8 }).SequenceEqual(list[0].ByteArrayValue!));
            Assert.Null(list[1].ByteArrayValue);

            Assert.Equal(SomeEnum.A, list[0].EnumValue);
            Assert.Equal(SomeEnum.B, list[1].EnumValue);
            Assert.Equal(SomeEnum.C, list[2].EnumValue);
        }

        [Fact]
        public void ToList_ClassB_1()
        {
            int i = 10;
            var fakeDatabaseDataList = new List<FakeDbDataReaderRow>()
            {
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i)
                .AddColumn(typeof(bool), "BoolValue", true)
                .AddColumn(typeof(int), "IntValue", i++),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i)
                .AddColumn(typeof(bool), "BoolValue", true)
                .AddColumn(typeof(int), "IntValue", i++),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i)
                .AddColumn(typeof(bool), "BoolValue", true)
                .AddColumn(typeof(int), "IntValue", i++),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i)
                .AddColumn(typeof(bool), "BoolValue", true)
                .AddColumn(typeof(int), "IntValue", i++)
            };

            var list = DbDataReaderMapper<ClassB>.ToList(new FakeDbDataReader(fakeDatabaseDataList));

            Assert.Equal(4, list.Count);
        }

        [Fact]
        public void ToList_ClassB_2()
        {
            var fakeDatabaseDataList = new List<FakeDbDataReaderRow>();

            var list = DbDataReaderMapper<ClassB>.ToList(new FakeDbDataReader(fakeDatabaseDataList));

            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public void ToList_int_1()
        {
            int i = 10;
            var fakeDatabaseDataList = new List<FakeDbDataReaderRow>()
            {
                new FakeDbDataReaderRow()
                .AddColumn(typeof(int), "IntValue", i++),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(int), "IntValue", i++),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(int), "IntValue", i++),
            };

            var list = DbDataReaderMapper<int>.ToList(new FakeDbDataReader(fakeDatabaseDataList));

            Assert.Equal(3, list.Count);
            Assert.Equal(10, list[0]);
            Assert.Equal(11, list[1]);
            Assert.Equal(12, list[2]);
        }

        [Fact]
        public void ToList_int__1()
        {
            int i = 10;
            var fakeDatabaseDataList = new List<FakeDbDataReaderRow>()
            {
                new FakeDbDataReaderRow()
                .AddColumn(typeof(int), "IntValue", i++),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(int), "IntValue", null),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(int), "IntValue", i++),
            };

            var list = DbDataReaderMapper<int?>.ToList(new FakeDbDataReader(fakeDatabaseDataList));

            Assert.Equal(3, list.Count);
            Assert.Equal(10, list[0]);
            Assert.False(list[1].HasValue);
            Assert.Equal(11, list[2]);
        }

        [Fact]
        public void ToList_string_1()
        {
            int i = 10;
            var fakeDatabaseDataList = new List<FakeDbDataReaderRow>()
            {
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i++),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i++),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(string), "StringValue", "ble" + i++),
            };

            var list = DbDataReaderMapper<string>.ToList(new FakeDbDataReader(fakeDatabaseDataList));

            Assert.Equal(3, list.Count);
            Assert.Equal("ble10", list[0]);
            Assert.Equal("ble11", list[1]);
            Assert.Equal("ble12", list[2]);
        }

        [Fact]
        public void ToList_SomeEnum_1()
        {
            var fakeDatabaseDataList = new List<FakeDbDataReaderRow>()
            {
                new FakeDbDataReaderRow()
                .AddColumn(typeof(int), "StringValue", 10),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(int), "StringValue", 11),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(int), "StringValue", 12),
            };

            var list = DbDataReaderMapper<SomeEnum>.ToList(new FakeDbDataReader(fakeDatabaseDataList));

            Assert.Equal(3, list.Count);
            Assert.Equal(SomeEnum.A, list[0]);
            Assert.Equal(SomeEnum.B, list[1]);
            Assert.Equal(SomeEnum.C, list[2]);
        }

        [Fact]
        public void Initialize_1()
        {
            DbDataReaderMapper<string>.Initialize();
            DbDataReaderMapper<ClassA>.Initialize();
        }

        [Fact]
        public void ToList_ClassD_1()
        {
            int i = 0;
            var fakeDatabaseDataList = new List<FakeDbDataReaderRow>()
            {
                new FakeDbDataReaderRow()
                .AddColumn(typeof(int), "INTvalue1", ++i)
                .AddColumn(typeof(int), "IntVALUE2", ++i),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(int), "INTvalue1", ++i)
                .AddColumn(typeof(int), "IntVALUE2", ++i),
                new FakeDbDataReaderRow()
                .AddColumn(typeof(int), "INTvalue1", ++i)
                .AddColumn(typeof(int), "IntVALUE2", ++i),
            };

            DbDataReaderMapper<ClassD>.Initialize(configuration =>
            {
                configuration.CaseSensitive = false;
            });

            var list = DbDataReaderMapper<ClassD>.ToList(new FakeDbDataReader(fakeDatabaseDataList));

            Assert.Equal(3, list.Count);

            Assert.Equal(1, list[0].IntValue1);
            Assert.Equal(2, list[0].IntValue2);

            Assert.Equal(3, list[1].IntValue1);
            Assert.Equal(4, list[1].IntValue2);

            Assert.Equal(5, list[2].IntValue1);
            Assert.Equal(6, list[2].IntValue2);
        }
    }
}
