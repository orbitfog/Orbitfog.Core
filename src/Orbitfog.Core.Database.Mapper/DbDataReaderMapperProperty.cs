using System;
using System.Reflection;

namespace Orbitfog.Core.Database.Mapper
{
    internal class DbDataReaderMapperProperty : DbDataReaderMapperItem
    {
        private PropertyInfo PropertyInfo { get; set; }
        public MethodInfo SetMethod => PropertyInfo.SetMethod;
        public override Type ItemType => PropertyInfo.PropertyType;
        public override string ItemName => PropertyInfo.Name;
        public override bool IsValueTypeNullable => PropertyInfo.PropertyType.IsValueType && IsNullable;
        public override bool IsValueTypeEnum => PropertyInfo.PropertyType.IsEnum;

        public DbDataReaderMapperProperty(int code, bool isNullable, MethodInfo dbDataReader_GetValue_MethodInfo, PropertyInfo propertyInfo)
            : base(code, isNullable, dbDataReader_GetValue_MethodInfo)
        {
            PropertyInfo = propertyInfo;
        }
    }
}
