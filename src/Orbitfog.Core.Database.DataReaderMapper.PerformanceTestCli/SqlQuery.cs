using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Orbitfog.Core.Database.DataReaderMapper.PerformanceTestCli
{
    public class SqlQuery
    {
        public static string ConnectionString { get; } = "Data Source=localhost; Initial Catalog=Orbitfog.Core.Database.DataReaderMapper.PerformanceTestCli; Integrated Security=true;";

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
                            var notCheckNullList = GetNotCheckNullList(reader);

                            Test1 t;
                            int ordinal;

                            while (reader.Read())
                            {
                                t = new Test1();

                                ordinal = 0;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.Test1Id = reader.GetInt32(ordinal);

                                ordinal++;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.Int1Value = reader.GetInt32(ordinal);

                                ordinal++;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.Int2Value = reader.GetInt32(ordinal);

                                ordinal++;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.Int3Value = reader.GetInt32(ordinal);

                                ordinal++;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.String1Value = reader.GetString(ordinal);

                                ordinal++;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.String2Value = reader.GetString(ordinal);

                                ordinal++;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.String3Value = reader.GetString(ordinal);

                                ordinal++;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.Decimal1Value = reader.GetDecimal(ordinal);

                                ordinal++;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.Decimal2Value = reader.GetDecimal(ordinal);

                                ordinal++;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.Decimal3Value = reader.GetDecimal(ordinal);

                                ordinal++;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.DateTime1Value = reader.GetDateTime(ordinal);

                                ordinal++;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.DateTime2Value = reader.GetDateTime(ordinal);

                                ordinal++;
                                if (notCheckNullList[ordinal] || !reader.IsDBNull(ordinal)) t.DateTime3Value = reader.GetDateTime(ordinal);

                                resultList.Add(t);
                            }
                        }

                        return resultList;
                    }
                }
            }
        }

        private static bool[] GetNotCheckNullList(DbDataReader dbDataReader)
        {
            var resultList = new bool[dbDataReader.FieldCount];
            for (int i = 0; i < resultList.Length; i++)
            {
                resultList[i] = true;
            }

            if (dbDataReader.CanGetColumnSchema())
            {
                var columnSchema = dbDataReader.GetColumnSchema();
                if (columnSchema.Count > 0)
                {
                    for (int i = 0; i < columnSchema.Count; i++)
                    {
                        var dbColumn = columnSchema[i];
                        if (dbColumn.ColumnOrdinal.HasValue && dbColumn.AllowDBNull.HasValue)
                        {
                            resultList[dbColumn.ColumnOrdinal.Value] = !dbColumn.AllowDBNull.Value;
                        }
                    }
                }
            }

            return resultList;
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

        public static List<Test1> Test1GetListEntityFrameworkCore(int count)
        {
            using (var context = new EfDbContext())
            {
                return context.Test1.Take(count).ToList();
            }
        }
    }
}
