using System;
using System.Data.Common;
using System.Reflection;

namespace Orbitfog.Core.Database.DataReaderMapper
{
    internal static class DbDataReaderDefinitions
    {
        public static readonly MethodInfo? Read = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.Read), new Type[] { });
        public static readonly MethodInfo? get_FieldCount = typeof(DbDataReader).GetRuntimeProperty(nameof(DbDataReader.FieldCount))?.GetMethod;
        public static readonly MethodInfo? get_HasRows = typeof(DbDataReader).GetRuntimeProperty(nameof(DbDataReader.HasRows))?.GetMethod;
        public static readonly MethodInfo? IsDBNull = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.IsDBNull), new Type[] { typeof(int) });
        public static readonly MethodInfo? GetBoolean = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.GetBoolean), new Type[] { typeof(int) });
        public static readonly MethodInfo? GetDateTime = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.GetDateTime), new Type[] { typeof(int) });
        public static readonly MethodInfo? GetDecimal = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.GetDecimal), new Type[] { typeof(int) });
        public static readonly MethodInfo? GetDouble = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.GetDouble), new Type[] { typeof(int) });
        public static readonly MethodInfo? GetInt16 = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.GetInt16), new Type[] { typeof(int) });
        public static readonly MethodInfo? GetInt32 = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.GetInt32), new Type[] { typeof(int) });
        public static readonly MethodInfo? GetInt64 = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.GetInt64), new Type[] { typeof(int) });
        public static readonly MethodInfo? GetFloat = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.GetFloat), new Type[] { typeof(int) });
        public static readonly MethodInfo? GetString = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.GetString), new Type[] { typeof(int) });
        public static readonly MethodInfo? GetValue = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.GetValue), new Type[] { typeof(int) });
        public static readonly MethodInfo? GetGuid = typeof(DbDataReader).GetRuntimeMethod(nameof(DbDataReader.GetGuid), new Type[] { typeof(int) });
    }
}
