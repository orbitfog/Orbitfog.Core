﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Orbitfog.Core.Database.Mapper
{
    public class DbDataReaderMapper<TResultItem>
    {
        private static readonly MethodInfo List_Add = typeof(List<TResultItem>).GetMethod(nameof(List<TResultItem>.Add), new Type[] { typeof(TResultItem) });
        private static readonly MethodInfo DbDataReaderMapper_GetAllowDbNullList = typeof(DbDataReaderMapper<TResultItem>).GetMethod(nameof(GetAllowDbNullList), BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo DbDataReaderMapper_AllowDBNull = typeof(DbDataReaderMapper<TResultItem>).GetMethod(nameof(AllowDBNull), BindingFlags.NonPublic | BindingFlags.Static);

        private delegate List<TResultItem> ToListHandle(DbDataReader dbDataReader);

        private static readonly Lazy<ToListHandle> toList = new Lazy<ToListHandle>(() =>
        {
            if (IsTypeForSingleColumn())
            {
                return SingleColumn;
            }
            else
            {
                return BuildMultiColumn();
            }
        });

        private DbDataReaderMapper()
        {
        }

        public static void Initialize()
        {
            toList.Value(null);
        }

        public static List<TResultItem> ToList(DbDataReader dbDataReader)
        {
            return toList.Value(dbDataReader);
        }

        private static ToListHandle BuildMultiColumn()
        {
            var itmList = GetItemList();

            var returnType = typeof(List<TResultItem>);
            var returnValue = Expression.Label(returnType, "returnValue");
            var rowList = Expression.Variable(returnType, "rowList");
            var dbDataReader = Expression.Parameter(typeof(DbDataReader), "dbDataReader");
            var readerMap = Expression.Variable(typeof(int[]), "readerMap");
            var columnSchema = Expression.Variable(typeof(Dictionary<int, DbColumn>), "columnSchema");
            var checkNulls = Expression.Variable(typeof(bool[]), "checkNulls");
            var i = Expression.Variable(typeof(int), "i");

            var expression = Expression.Block(
                                                new[] { rowList, readerMap, columnSchema, checkNulls, i },
                                                Expression.Assign(rowList, Expression.New(returnType)),
                                                Expression.IfThen(
                                                    Expression.NotEqual(dbDataReader, Expression.Constant(null)),
                                                    Expression.Block(
                                                        Expression.Assign(
                                                            columnSchema,
                                                            Expression.Call(null, DbDataReaderMapper_GetAllowDbNullList, dbDataReader)
                                                        ),
                                                        Expression.IfThen(
                                                            Expression.Call(dbDataReader, DbDataReaderDefinitions.get_HasRows),
                                                            Expression.Block(
                                                                Expression.Assign(readerMap, Expression.NewArrayBounds(typeof(int), Expression.Call(dbDataReader, DbDataReaderDefinitions.get_FieldCount))),
                                                                Expression.Assign(checkNulls, Expression.NewArrayBounds(typeof(bool), Expression.Call(dbDataReader, DbDataReaderDefinitions.get_FieldCount))),
                                                                For(
                                                                    i,
                                                                    Expression.Call(dbDataReader, DbDataReaderDefinitions.get_FieldCount),
                                                                    Expression.Block(
                                                                        Expression.Assign(
                                                                            Expression.ArrayAccess(checkNulls, i),
                                                                            Expression.Call(null, DbDataReaderMapper_AllowDBNull, columnSchema, i)
                                                                        ),
                                                                        BuildInitSwitch(dbDataReader, i, readerMap, itmList)
                                                                    )
                                                                ),
                                                                While(
                                                                    Expression.Call(dbDataReader, DbDataReaderDefinitions.Read),
                                                                    BuildReadRow(dbDataReader, rowList, checkNulls, readerMap, itmList)
                                                                )
                                                            )
                                                        )
                                                    )
                                                ),
                                                Expression.Return(returnValue, rowList),
                                                Expression.Label(returnValue, Expression.Default(returnType))
                                            );

            return Expression.Lambda<ToListHandle>(expression, dbDataReader).Compile();
        }

        private static List<DbDataReaderMapperItem> GetItemList()
        {
            var ttype = typeof(TResultItem);
            int code = 1;
            var resultList = new List<DbDataReaderMapperItem>();
            foreach (var propertyInfo in ttype.GetRuntimeProperties())
            {
                var obj_set_MethodInfo = propertyInfo.SetMethod;
                if (ValidateProperty(propertyInfo, obj_set_MethodInfo, 1))
                {
                    ProcessType(propertyInfo.PropertyType, out bool isNullable, out MethodInfo dbDataReader_GetValue_MethodInfo);
                    if (dbDataReader_GetValue_MethodInfo != null)
                    {
                        resultList.Add(new DbDataReaderMapperProperty(code, isNullable, dbDataReader_GetValue_MethodInfo, propertyInfo));
                        code++;
                    }
                }
            }
            foreach (var fieldInfo in ttype.GetRuntimeFields())
            {
                if (ValidateField(fieldInfo))
                {
                    ProcessType(fieldInfo.FieldType, out bool isNullable, out MethodInfo dbDataReader_GetValue_MethodInfo);
                    if (dbDataReader_GetValue_MethodInfo != null)
                    {
                        resultList.Add(new DbDataReaderMapperField(code, isNullable, dbDataReader_GetValue_MethodInfo, fieldInfo));
                        code++;
                    }
                }
            }

            return resultList;
        }

        private static void ProcessType(Type propertyType, out bool isNullable, out MethodInfo dbDataReader_GetValue_MethodInfo)
        {
            isNullable = false;

            if (propertyType.IsClass || propertyType.IsArray)
            {
                isNullable = true;
            }
            else
            {
                var underlyingType = Nullable.GetUnderlyingType(propertyType);
                if (underlyingType != null)
                {
                    isNullable = true;
                    propertyType = underlyingType;
                }
            }
            dbDataReader_GetValue_MethodInfo = DbDataReade_GetValue_MethodInfo(propertyType);
        }

        private static Expression BuildInitSwitch(ParameterExpression dbDataReader, ParameterExpression i, Expression readerMap, List<DbDataReaderMapperItem> itemList)
        {
            if (itemList.Count <= 0)
            {
                return Expression.Empty();
            }

            var breakLabel = Expression.Label();

            var switchCaseList = new List<SwitchCase>();
            foreach (var item in itemList)
            {
                switchCaseList.Add(
                        Expression.SwitchCase(
                                                Expression.Block(
                                                    Expression.Assign(
                                                        Expression.ArrayAccess(readerMap, i),
                                                        Expression.Constant(item.Code)
                                                    ),
                                                    Expression.Break(breakLabel)
                                                ),
                                                Expression.Constant(item.ItemName)
                                            )
                    );
            }

            var switchExpression =
                Expression.Block(
                    Expression.Switch(
                        Expression.Call(dbDataReader, DbDataReaderDefinitions.GetName, i),
                        switchCaseList.ToArray()
                    ),
                    Expression.Label(breakLabel)
                );

            return switchExpression;
        }

        private static Expression BuildReadRow(ParameterExpression dbDataReader, ParameterExpression rowList, ParameterExpression checkNulls, ParameterExpression readerMap, List<DbDataReaderMapperItem> itemList)
        {
            var obj = Expression.Parameter(typeof(TResultItem), "obj");
            var i = Expression.Variable(typeof(int), "i");

            return Expression.Block(
                                        new[] { obj, i },
                                        Expression.Assign(obj, Expression.New(typeof(TResultItem))),
                                        For(
                                            i,
                                            Expression.Call(dbDataReader, DbDataReaderDefinitions.get_FieldCount),
                                            BuildReadRowSwitch(dbDataReader, i, obj, checkNulls, readerMap, itemList)
                                        ),
                                        Expression.Call(rowList, List_Add, obj)
                                    );
        }

        private static Expression BuildReadRowSwitch(ParameterExpression dbDataReader, ParameterExpression i, ParameterExpression obj, ParameterExpression checkNulls, ParameterExpression readerMap, List<DbDataReaderMapperItem> itemList)
        {
            if (itemList.Count <= 0)
            {
                return Expression.Empty();
            }

            var breakLabel = Expression.Label();

            var switchCaseList = new List<SwitchCase>();
            foreach (var item in itemList)
            {
                switchCaseList.Add(
                        Expression.SwitchCase(
                            Expression.Block(
                                BuildReadRowCase(dbDataReader, i, obj, checkNulls, item),
                                Expression.Break(breakLabel)
                            ),
                            Expression.Constant(item.Code)
                        )
                    );
            }

            var switchExpression =
                Expression.Block(
                    Expression.Switch(
                        Expression.ArrayAccess(readerMap, i),
                        switchCaseList.ToArray()
                    ),
                    Expression.Label(breakLabel)
                );

            return switchExpression;
        }

        private static Expression BuildReadRowCase(ParameterExpression dbDataReader, ParameterExpression i, ParameterExpression obj, ParameterExpression checkNulls, DbDataReaderMapperItem item)
        {
            return Expression.IfThenElse(
                                            Expression.AndAlso(
                                                Expression.ArrayAccess(checkNulls, i),
                                                Expression.Call(dbDataReader, DbDataReaderDefinitions.IsDBNull, i)
                                            ),
                                            item is DbDataReaderMapperProperty ? BuildReadRowCaseIfTrue(obj, (DbDataReaderMapperProperty)item) : BuildReadRowCaseIfTrue(obj, (DbDataReaderMapperField)item),
                                            item is DbDataReaderMapperProperty ? BuildReadRowCaseIfFalse(dbDataReader, i, obj, (DbDataReaderMapperProperty)item) : BuildReadRowCaseIfFalse(dbDataReader, i, obj, (DbDataReaderMapperField)item)
                                         );
        }

        private static Expression BuildReadRowCaseIfTrue(ParameterExpression obj, DbDataReaderMapperProperty property)
        {
            return Expression.Call(
                                        obj,
                                        property.SetMethod,
                                        ConvertNull(property.ItemType, property.IsNullable)
                                    );
        }

        private static Expression BuildReadRowCaseIfTrue(ParameterExpression obj, DbDataReaderMapperField field)
        {
            return Expression.Assign(
                                        Expression.Field(obj, field.FieldInfo),
                                        ConvertNull(field.ItemType, field.IsNullable)
                                    );
        }

        private static Expression BuildReadRowCaseIfFalse(ParameterExpression dbDataReader, ParameterExpression i, ParameterExpression obj, DbDataReaderMapperProperty property)
        {
            return Expression.Call(
                                        obj,
                                        property.SetMethod,
                                        NewMethod(dbDataReader, i, property)
                                    );
        }

        private static Expression BuildReadRowCaseIfFalse(ParameterExpression dbDataReader, ParameterExpression i, ParameterExpression obj, DbDataReaderMapperField field)
        {
            return Expression.Assign(
                                        Expression.Field(obj, field.FieldInfo),
                                        NewMethod(dbDataReader, i, field)
                                    );
        }

        private static Expression NewMethod(ParameterExpression dbDataReader, ParameterExpression i, DbDataReaderMapperItem item)
        {
            return (
                        item.DbDataReader_GetValue_MethodInfo == DbDataReaderDefinitions.GetValue || item.IsValueTypeNullable || item.IsValueTypeEnum ?
                        (Expression)Expression.Convert(
                            Expression.Call(dbDataReader, item.DbDataReader_GetValue_MethodInfo, i),
                            item.ItemType
                        ) :
                        (Expression)Expression.Call(dbDataReader, item.DbDataReader_GetValue_MethodInfo, i)
                    );
        }

        private static Expression ConvertNull(Type type, bool isNullable)
        {
            if (isNullable)
            {
                return Expression.Convert(Expression.Constant(null), type);
            }
            else
            {
                return Expression.Default(type);
            }
        }

        private static Dictionary<int, DbColumn> GetAllowDbNullList(DbDataReader dbDataReader)
        {
            if (dbDataReader.CanGetColumnSchema())
            {
                var columnSchema = dbDataReader.GetColumnSchema();
                if (columnSchema.Count > 0)
                {
                    return columnSchema.ToList()
                                .FindAll(p => p.ColumnOrdinal.HasValue && p.AllowDBNull.HasValue)
                                .ToDictionary(p => p.ColumnOrdinal.Value);
                }
            }
            return new Dictionary<int, DbColumn>();
        }

        private static bool AllowDBNull(Dictionary<int, DbColumn> columnSchema, int i)
        {
            if (columnSchema.TryGetValue(i, out DbColumn dbColumn) && dbColumn != null && dbColumn.AllowDBNull.HasValue)
            {
                return dbColumn.AllowDBNull.Value;
            }
            else
            {
                return true;
            }
        }

        private static bool ValidateProperty(PropertyInfo pi, MethodInfo mi, int parameterCount)
        {
            return pi != null && mi != null && !pi.PropertyType.IsPointer && mi.IsPublic && !mi.IsStatic && mi.GetParameters().Length == parameterCount;
        }

        private static bool ValidateField(FieldInfo fi)
        {
            return fi != null && !fi.FieldType.IsPointer && fi.IsPublic && !fi.IsStatic;
        }

        private static MethodInfo DbDataReade_GetValue_MethodInfo(Type type)
        {
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return DbDataReaderDefinitions.GetBoolean;
                case TypeCode.DateTime:
                    return DbDataReaderDefinitions.GetDateTime;
                case TypeCode.Decimal:
                    return DbDataReaderDefinitions.GetDecimal;
                case TypeCode.Double:
                    return DbDataReaderDefinitions.GetDouble;
                case TypeCode.Int16:
                    return DbDataReaderDefinitions.GetInt16;
                case TypeCode.Int32:
                    return DbDataReaderDefinitions.GetInt32;
                case TypeCode.Int64:
                    return DbDataReaderDefinitions.GetInt64;
                case TypeCode.Single:
                    return DbDataReaderDefinitions.GetFloat;
                case TypeCode.String:
                    return DbDataReaderDefinitions.GetString;
                default:
                    if (type == typeof(Guid))
                    {
                        return DbDataReaderDefinitions.GetGuid;
                    }
                    else
                    {
                        return DbDataReaderDefinitions.GetValue;
                    }
            }
        }

        private static bool IsTypeForSingleColumn()
        {
            var type = typeof(TResultItem);

            if (type == typeof(string)) return true;

            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                type = underlyingType;
            }

            if (type.IsEnum) return true;
            if (type == typeof(bool)) return true;
            if (type == typeof(byte)) return true;
            if (type == typeof(sbyte)) return true;
            if (type == typeof(byte[])) return true;
            if (type == typeof(char)) return true;
            if (type == typeof(decimal)) return true;
            if (type == typeof(double)) return true;
            if (type == typeof(float)) return true;
            if (type == typeof(int)) return true;
            if (type == typeof(uint)) return true;
            if (type == typeof(long)) return true;
            if (type == typeof(ulong)) return true;
            if (type == typeof(short)) return true;
            if (type == typeof(ushort)) return true;
            if (type == typeof(DateTime)) return true;
            if (type == typeof(DateTimeOffset)) return true;
            if (type == typeof(Guid)) return true;

            return false;
        }

        private static List<TResultItem> SingleColumn(DbDataReader dbDataReader)
        {
            var resultList = new List<TResultItem>();

            if (dbDataReader != null)
            {
                if (dbDataReader.FieldCount > 0)
                {
                    var type = GetUnderlyingType();
                    var typeCode = Type.GetTypeCode(type);
                    TResultItem t;
                    object obj;
                    const int ordinal = 0;

                    while (dbDataReader.Read())
                    {
                        if (dbDataReader.IsDBNull(ordinal))
                        {
                            t = default;
                        }
                        else
                        {
                            obj = GetValue(dbDataReader, ordinal, type, typeCode);

                            if (obj is TResultItem)
                            {
                                t = (TResultItem)obj;
                            }
                            else
                            {
                                throw new Exception("Incompatible type, read '" + obj.GetType().Name + "' and set to '" + typeof(TResultItem).Name + "'");
                            }
                        }
                        resultList.Add(t);
                    }
                }
            }

            return resultList;
        }

        private static Type GetUnderlyingType()
        {
            var type = typeof(TResultItem);
            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                type = underlyingType;
            }
            return type;
        }

        private static object GetValue(DbDataReader dbDataReader, int ordinal, Type type, TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return dbDataReader.GetBoolean(ordinal);

                case TypeCode.Char:
                    return dbDataReader.GetChar(ordinal);

                case TypeCode.DateTime:
                    return dbDataReader.GetDateTime(ordinal);

                case TypeCode.Decimal:
                    return dbDataReader.GetDecimal(ordinal);

                case TypeCode.Double:
                    return dbDataReader.GetDouble(ordinal);

                case TypeCode.Int16:
                    if (type.IsEnum)
                    {
                        return (TResultItem)(object)dbDataReader.GetInt16(ordinal);
                    }
                    else
                    {
                        return dbDataReader.GetInt16(ordinal);
                    }

                case TypeCode.Int32:
                    if (type.IsEnum)
                    {
                        return (TResultItem)(object)dbDataReader.GetInt32(ordinal);
                    }
                    else
                    {
                        return dbDataReader.GetInt32(ordinal);
                    }

                case TypeCode.Int64:
                    if (type.IsEnum)
                    {
                        return (TResultItem)(object)dbDataReader.GetInt64(ordinal);
                    }
                    else
                    {
                        return dbDataReader.GetInt64(ordinal);
                    }

                case TypeCode.Single:
                    return dbDataReader.GetFloat(ordinal);

                case TypeCode.String:
                    return dbDataReader.GetString(ordinal);

                default:
                    return dbDataReader.GetValue(ordinal);
            }
        }

        /// <summary>
        /// This is equivalent to the following statement:
        /// while (loopCondition)
        /// {
        ///     loopContent
        /// }
        /// </summary>
        private static Expression While(Expression loopCondition, Expression loopContent)
        {
            var breakLabel = Expression.Label();

            var whileLoop = Expression.Loop(
                Expression.IfThenElse(
                    loopCondition,
                    loopContent,
                    Expression.Break(breakLabel)
                ),
                breakLabel
            );

            return whileLoop;
        }

        /// <summary>
        /// This is equivalent to the following statement:
        /// for (loopVar = 0; i < loopMax; ++i)
        /// {
        ///     loopContent
        /// }
        /// </summary>
        private static Expression For(ParameterExpression loopVar, Expression loopMax, Expression loopContent)
        {
            return For(loopVar, Expression.Constant(0, typeof(int)), Expression.LessThan(loopVar, loopMax), Expression.PreIncrementAssign(loopVar), loopContent);
        }

        /// <summary>
        /// This is equivalent to the following statement:
        /// for (loopVar = initValue; loopCondition; increment)
        /// {
        ///     loopContent
        /// }
        /// </summary>
        private static Expression For(ParameterExpression loopVar, Expression initValue, Expression loopCondition, Expression increment, Expression loopContent)
        {
            var breakLabel = Expression.Label();

            var forLoop = Expression.Block(
                new[] { loopVar },
                Expression.Assign(loopVar, initValue),
                Expression.Loop(
                    Expression.IfThenElse(
                        loopCondition,
                        Expression.Block(
                            loopContent,
                            increment
                        ),
                        Expression.Break(breakLabel)
                    ),
                    breakLabel
                )
            );

            return forLoop;
        }
    }
}