using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Orbitfog.Core.Database.Mapper.PerformanceTestCli
{
    public class Sql
    {
        private static string ConnectionString { get; } = "Data Source=localhost; Initial Catalog=Orbitfog.Core.Database.Mapper.PerformanceTestCli; Integrated Security=true;";

        private static string GetCommandText(int count)
        {
            return $"SELECT TOP {count} * FROM [dbo].[Test1]";
        }

        public static List<Test1> Test1GetListHandCoded(int count)
        {
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = GetCommandText(count);

                    using (var reader = command.ExecuteReader())
                    {
                        var resultList = new List<Test1>();

                        if (reader.FieldCount > 0)
                        {
                            var allowDbNullList = GetAllowDbNullList(reader);
                            bool[] allowDBNullList = new bool[allowDbNullList.Count];
                            foreach (var item in allowDbNullList)
                            {
                                allowDBNullList[item.Key] = AllowDBNull(allowDbNullList, item.Key);
                            }

                            Test1 t;

                            while (reader.Read())
                            {
                                t = new Test1();

                                int ordinal = 0;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.Test1Id = default;
                                else t.Test1Id = reader.GetInt32(ordinal);

                                ordinal++;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.Int1Value = default;
                                else t.Int1Value = reader.GetInt32(ordinal);

                                ordinal++;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.Int2Value = default;
                                else t.Int2Value = reader.GetInt32(ordinal);

                                ordinal++;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.Int3Value = default;
                                else t.Int3Value = reader.GetInt32(ordinal);

                                ordinal++;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.String1Value = default;
                                else t.String1Value = reader.GetString(ordinal);

                                ordinal++;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.String2Value = default;
                                else t.String2Value = reader.GetString(ordinal);

                                ordinal++;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.String3Value = default;
                                else t.String3Value = reader.GetString(ordinal);

                                ordinal++;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.Decimal1Value = default;
                                else t.Decimal1Value = reader.GetDecimal(ordinal);

                                ordinal++;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.Decimal2Value = default;
                                else t.Decimal2Value = reader.GetDecimal(ordinal);

                                ordinal++;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.Decimal3Value = default;
                                else t.Decimal3Value = reader.GetDecimal(ordinal);

                                ordinal++;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.DateTime1Value = default;
                                else t.DateTime1Value = reader.GetDateTime(ordinal);

                                ordinal++;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.DateTime2Value = default;
                                else t.DateTime2Value = reader.GetDateTime(ordinal);

                                ordinal++;
                                if (allowDBNullList[ordinal] && reader.IsDBNull(ordinal)) t.DateTime3Value = default;
                                else t.DateTime3Value = reader.GetDateTime(ordinal);

                                resultList.Add(t);
                            }
                        }

                        return resultList;
                    }
                }
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

        public static List<Test1> Test1GetListOrbitfogCoreDatabaseMapper(int count)
        {
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = GetCommandText(count);

                    return DbDataReaderMapper<Test1>.ToList(command.ExecuteReader());
                }
            }
        }

        public static List<Test1> Test1GetListDapper(int count)
        {
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                return connection.Query<Test1>(GetCommandText(count)).ToList();
            }
        }
    }
}
