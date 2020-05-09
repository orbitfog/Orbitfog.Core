using System;
using System.Collections.Generic;

namespace Orbitfog.Core.Database.Mapper.UnitTests
{
    public class FakeDbDataReaderRow
    {
        private readonly Dictionary<string, object> nameValueList;
        private readonly Dictionary<int, object> ordinalValueList;
        private readonly Dictionary<int, string> ordinalNameList;
        private readonly Dictionary<string, int> nameOrdinalList;
        private readonly Dictionary<int, Type> typeList;

        public FakeDbDataReaderRow()
        {
            nameValueList = new Dictionary<string, object>();
            ordinalValueList = new Dictionary<int, object>();
            ordinalNameList = new Dictionary<int, string>();
            nameOrdinalList = new Dictionary<string, int>();
            typeList = new Dictionary<int, Type>();
        }

        public FakeDbDataReaderRow AddColumn(Type type, string name, object value)
        {
            int ordinal = nameValueList.Count;

            nameValueList.Add(name, value);
            ordinalValueList.Add(ordinal, value);
            ordinalNameList.Add(ordinal, name);
            nameOrdinalList.Add(name, ordinal);
            typeList.Add(ordinal, type);

            return this;
        }

        public object GetValue(int ordinal)
        {
            return ordinalValueList[ordinal];
        }

        public object GetValue(string name)
        {
            return nameValueList[name];
        }

        public int FieldCount => ordinalValueList.Count;

        public string GetName(int ordinal)
        {
            return ordinalNameList[ordinal];
        }

        public Type GetType(int ordinal)
        {
            return typeList[ordinal];
        }

        public int GetOrdinal(string name)
        {
            return nameOrdinalList[name];
        }
    }
}
