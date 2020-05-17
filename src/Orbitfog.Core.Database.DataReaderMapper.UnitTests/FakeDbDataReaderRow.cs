using System;
using System.Collections.Generic;
using System.Linq;

namespace Orbitfog.Core.Database.DataReaderMapper.UnitTests
{
    public class FakeDbDataReaderRow
    {
        private readonly Dictionary<string, int> nameOrdinalList;
        private readonly Dictionary<int, Type> typeList;
        private readonly Dictionary<int, bool> isDBNullList;
        private readonly Dictionary<int, object> ordinalValueList;

        public FakeDbDataReaderRow()
        {
            nameOrdinalList = new Dictionary<string, int>();
            typeList = new Dictionary<int, Type>();
            isDBNullList = new Dictionary<int, bool>();
            ordinalValueList = new Dictionary<int, object>();
        }

        public FakeDbDataReaderRow AddColumn(Type type, string name, object? value)
        {
            int ordinal = nameOrdinalList.Count;

            nameOrdinalList.Add(name, ordinal);
            typeList.Add(ordinal, type);
            isDBNullList.Add(ordinal, value == null);
            if (value != null)
            {
                ordinalValueList.Add(ordinal, value);
            }

            return this;
        }

        public object GetValue(int ordinal)
        {
            return ordinalValueList[ordinal];
        }

        public object GetValue(string name)
        {
            return ordinalValueList[nameOrdinalList[name]];
        }

        public int FieldCount => nameOrdinalList.Count;

        public string GetName(int ordinal)
        {
            return nameOrdinalList.FirstOrDefault(p => p.Value == ordinal).Key;
        }

        public Type GetType(int ordinal)
        {
            return typeList[ordinal];
        }

        public int GetOrdinal(string name)
        {
            return nameOrdinalList[name];
        }

        public bool IsDBNull(int ordinal)
        {
            return isDBNullList[ordinal];
        }
    }
}
