using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Tam.Database
{
    public class SqlServerHelper : ISqlServerHelper
    {
        public SqlServerHelper(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("ConnectionString is null or empty.");
            }
            this.ConnectionString = connectionString;
        }

        public SqlServerHelper(string sqlServer, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(sqlServer))
            {
                throw new Exception("Sql Server information is null or empty.");
            }
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new Exception("Database name is null or empty.");
            }
            var connectionBuilder = new SqlConnectionStringBuilder();
            connectionBuilder.DataSource = sqlServer;
            connectionBuilder.InitialCatalog = databaseName;
            connectionBuilder.IntegratedSecurity = true;
            this.ConnectionString = connectionBuilder.ConnectionString;
        }

        public SqlServerHelper(string sqlServer, string databaseName, string userId, string password, bool multipleResultSets = true)
        {
            if (string.IsNullOrWhiteSpace(sqlServer))
            {
                throw new Exception("Sql Server information is null or empty.");
            }
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new Exception("Database name is null or empty.");
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("User id is null or empty.");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Password is null or empty.");
            }
            var connectionBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = sqlServer,
                InitialCatalog = databaseName,
                UserID = userId,
                Password = password,
                MultipleActiveResultSets = multipleResultSets
            };
            this.ConnectionString = connectionBuilder.ConnectionString;
        }

        public string ConnectionString { get; set; }

        public static Object GetAs(SqlDataReader reader, Type objectToReturnType)
        {
            // usage
            //while (rdr.Read())
            //    customerList.Add((Customer)SqlServerHelper.GetAs(rdr, typeof(Customer)));

            // Create a new Object
            Object newObjectToReturn = Activator.CreateInstance(objectToReturnType);
            // Get all the properties in our Object
            PropertyInfo[] props = objectToReturnType.GetProperties();
            // For each property get the data from the reader to the object
            for (int i = 0; i < props.Length; i++)
            {
                objectToReturnType.InvokeMember(props[i].Name, BindingFlags.SetProperty, null, newObjectToReturn, new Object[] { reader[props[i].Name] });
            }
            return newObjectToReturn;
        }

        public void ExcuteNonQuery(string query)
        {
            ExcuteNonQuery(query, null, null);
        }

        public void ExcuteNonQuery(string query, string paraName, object paraValue, CommandType commandType = CommandType.Text)
        {
            ExcuteNonQuery(query, new string[] { paraName }, new object[] { paraValue }, commandType);
        }

        public void ExcuteNonQuery(string query, string[] parasName, object[] parasValue, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = commandType;
                        connection.Open();
                        AddParameter(parasName, parasValue, command);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object ExcuteScalar(string query)
        {
            return ExcuteScalar(query, null, null);
        }

        public object ExcuteScalar(string query, string paraName, object paraValue, CommandType commandType = CommandType.Text)
        {
            return ExcuteScalar(query, new string[] { paraName }, new object[] { paraValue }, commandType);
        }

        public object ExcuteScalar(string query, string[] parasName, object[] parasValue, CommandType commandType = CommandType.Text)
        {
            object returnValue = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = commandType;
                        connection.Open();
                        AddParameter(parasName, parasValue, command);
                        returnValue = command.ExecuteScalar();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public SqlDataReader GetDataReader(string query, CommandType commandType, SqlParameter[] arraySqlParameter)
        {
            SqlDataReader reader = null;
            var connection = new SqlConnection(this.ConnectionString);
            try
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = commandType;
                    connection.Open();
                    if (arraySqlParameter != null && arraySqlParameter.Length > 0)
                    {
                        command.Parameters.AddRange(arraySqlParameter);
                    }
                    reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                return reader;
            }
            catch (Exception)
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    connection.Dispose();
                }
                throw;
            }
        }

        public async Task<SqlDataReader> GetDataReaderAsync(string query, CommandType commandType, SqlParameter[] arraySqlParameter)
        {
            SqlDataReader reader = null;
            var connection = new SqlConnection(this.ConnectionString);
            try
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = commandType;
                    connection.Open();
                    if (arraySqlParameter != null && arraySqlParameter.Length > 0)
                    {
                        command.Parameters.AddRange(arraySqlParameter);
                    }
                    reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                }
                return reader;
            }
            catch (Exception)
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    connection.Dispose();
                }
                throw;
            }
        }
        public DataTable GetDataTable(string query)
        {
            return GetDataTable(query, CommandType.Text, sqlParameters: null);
        }

        /// <summary>
        /// Get DataTable
        /// </summary>
        /// <param name="query">SQL query or StoredProcedure name</param>
        /// <param name="commandType">CommandType of query</param>
        /// <returns></returns>
        public DataTable GetDataTable(string query, CommandType commandType)
        {
            return GetDataTable(query, commandType, sqlParameters: null);
        }

        public DataTable GetDataTable(string query, CommandType commandType, string[] parasName, object[] parasValue)
        {
            var table = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = commandType;
                        var adapter = new SqlDataAdapter(command);
                        AddParameter(parasName, parasValue, command);
                        adapter.Fill(table);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return table;
        }

        public DataTable GetDataTable(string query, CommandType commandType,
            params SqlParameter[] sqlParameters)
        {
            var table = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = commandType;
                        var adapter = new SqlDataAdapter(command);
                        if (sqlParameters.Length > 0)
                        {
                            command.Parameters.AddRange(sqlParameters);
                        }
                        adapter.Fill(table);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return table;
        }

        private static void AddParameter(string[] parasName, object[] parasValue, SqlCommand command)
        {
            if (parasName != null && parasName.Length > 0 && parasName.Length == parasValue.Length)
            {
                for (int i = 0; i < parasName.Length; i++)
                {
                    command.Parameters.AddWithValue(parasName[i], parasValue[i] == null ? DBNull.Value : parasValue[i]);
                }
            }
        }

        public IEnumerable<T> GetManyItems<T>(string storedName, object parameters) where T : class
        {
            IEnumerable<T> items = null;
            using (IDbConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                items = connection.Query<T>(storedName, parameters, commandType: CommandType.StoredProcedure);
            }
            return items;
        }

        public T GetItem<T>(string storedName, object parameters) where T : class
        {
            T item = default(T);
            using (IDbConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                item = connection.Query<T>(storedName, parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return item;
        }

        public async Task<T> GetItemAsync<T>(string storedName, object parameters) where T: class
        {
            T item = default(T);
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<T>(storedName, parameters, commandType: CommandType.StoredProcedure);
                item = result.FirstOrDefault();
            }
            return item;
        }

        public async Task<IEnumerable<T>> GetManyItemsAsync<T>(string storedName, object parameters) where T : class
        {
            IEnumerable<T> items = null;
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                items = await connection.QueryAsync<T>(storedName, parameters, commandType: CommandType.StoredProcedure);
            }
            return items;
        }

    }
}
