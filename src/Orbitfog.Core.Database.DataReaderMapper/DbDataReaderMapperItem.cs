using System;
using System.Reflection;

namespace Orbitfog.Core.Database.DataReaderMapper
{
    internal abstract class DbDataReaderMapperItem
    {
        public int Code { get; private set; }
        public bool IsNullable { get; private set; }
        public MethodInfo DbDataReader_GetValue_MethodInfo { get; private set; }
        public abstract Type ItemType { get; }
        public abstract string ItemName { get; }
        public abstract bool IsValueTypeNullable { get; }
        public abstract bool IsValueTypeEnum { get; }

        public DbDataReaderMapperItem(int code, bool isNullable, MethodInfo dbDataReader_GetValue_MethodInfo)
        {
            Code = code;
            IsNullable = isNullable;
            DbDataReader_GetValue_MethodInfo = dbDataReader_GetValue_MethodInfo;
        }
    }
}
