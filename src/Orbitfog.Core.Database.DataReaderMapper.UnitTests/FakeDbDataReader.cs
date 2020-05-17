using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Orbitfog.Core.Database.DataReaderMapper.UnitTests
{
    public class FakeDbDataReader : DbDataReader
    {
        private int rowNumber;
        private readonly List<FakeDbDataReaderRow> list;

        public FakeDbDataReader(List<FakeDbDataReaderRow> list) : base()
        {
            rowNumber = -1;
            this.list = list;
        }

        public override object this[int ordinal] => GetValue(ordinal);

        public override object this[string name] => list[rowNumber].GetValue(name);

        public override int Depth => throw new NotImplementedException();

        public override int FieldCount => list.Count > 0 ? list[rowNumber >= 0 ? rowNumber : 0].FieldCount : 0;

        public override bool HasRows => list.Count > 0;

        public override bool IsClosed => throw new NotImplementedException();

        public override int RecordsAffected => throw new NotImplementedException();

        public override bool GetBoolean(int ordinal)
        {
            return (bool)GetValue(ordinal);
        }

        public override byte GetByte(int ordinal)
        {
            return (byte)GetValue(ordinal);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
        {
            return (long)GetValue(ordinal);
        }

        public override char GetChar(int ordinal)
        {
            return (char)GetValue(ordinal);
        }

        public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            var o = GetFieldType(ordinal);
            if (o == null) return "";
            else return o.Name;
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return (DateTime)GetValue(ordinal);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return (decimal)GetValue(ordinal);
        }

        public override double GetDouble(int ordinal)
        {
            return (double)GetValue(ordinal);
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override Type GetFieldType(int ordinal)
        {
            var o = list[rowNumber >= 0 ? rowNumber : 0].GetType(ordinal);
            return o.GetType();
        }

        public override float GetFloat(int ordinal)
        {
            return (float)GetValue(ordinal);
        }

        public override Guid GetGuid(int ordinal)
        {
            return (Guid)GetValue(ordinal);
        }

        public override short GetInt16(int ordinal)
        {
            return (Int16)GetValue(ordinal);
        }

        public override int GetInt32(int ordinal)
        {
            return (Int32)GetValue(ordinal);
        }

        public override long GetInt64(int ordinal)
        {
            return (Int64)GetValue(ordinal);
        }

        public override string GetName(int ordinal)
        {
            return list[rowNumber >= 0 ? rowNumber : 0].GetName(ordinal);
        }

        public override int GetOrdinal(string name)
        {
            return list[rowNumber].GetOrdinal(name);
        }

        public override string GetString(int ordinal)
        {
            return (string)GetValue(ordinal);
        }

        public override object GetValue(int ordinal)
        {
            var value = list[rowNumber].GetValue(ordinal);
            if (value != null)
            {
                return value;
            }
            else
            {
                throw new Exception($"GetValue return null, ordinal='{ordinal}', rowNumber='{rowNumber}'");
            }
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override bool IsDBNull(int ordinal)
        {
            return list[rowNumber].IsDBNull(ordinal);
        }

        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        public override bool Read()
        {
            rowNumber++;
            return rowNumber < list.Count;
        }

        public override DataTable GetSchemaTable()
        {
            return new DataTable();
        }
    }
}
