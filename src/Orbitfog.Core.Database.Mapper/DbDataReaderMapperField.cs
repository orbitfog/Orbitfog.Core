using System;
using System.Reflection;

namespace Orbitfog.Core.Database.Mapper
{
    internal class DbDataReaderMapperField : DbDataReaderMapperItem
    {
        public FieldInfo FieldInfo { get; private set; }
        public override Type ItemType => FieldInfo.FieldType;
        public override string ItemName => FieldInfo.Name;
        public override bool IsValueTypeNullable => FieldInfo.FieldType.IsValueType && IsNullable;
        public override bool IsValueTypeEnum => FieldInfo.FieldType.IsEnum;

        public DbDataReaderMapperField(int code, bool isNullable, MethodInfo dbDataReader_GetValue_MethodInfo, FieldInfo fieldInfo)
            : base(code, isNullable, dbDataReader_GetValue_MethodInfo)
        {
            FieldInfo = fieldInfo;
        }
    }
}
