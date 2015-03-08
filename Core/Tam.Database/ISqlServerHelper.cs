using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Database
{
    public interface ISqlServerHelper
    {
        string ConnectionString { get; set; }
        void ExcuteNonQuery(string query);
        void ExcuteNonQuery(string query, string paraName, object paraValue, CommandType commandType = CommandType.Text);
        void ExcuteNonQuery(string query, string[] parasName, object[] parasValue, CommandType commandType = CommandType.Text);
        object ExcuteScalar(string query);
        object ExcuteScalar(string query, string paraName, object paraValue, CommandType commandType = CommandType.Text);
        object ExcuteScalar(string query, string[] parasName, object[] parasValue, CommandType commandType = CommandType.Text);
        IEnumerable<T> GetManyItems<T>(string storedName, object parameters) where T : class;
        T GetItem<T>(string storedName, object parameters) where T : class;
        Task<IEnumerable<T>> GetManyItemsAsync<T>(string storedName, object parameters) where T : class;
        Task<T> GetItemAsync<T>(string storedName, object parameters) where T : class;
        SqlDataReader GetDataReader(string query, CommandType commandType, System.Data.SqlClient.SqlParameter[] arraySqlParameter);
        Task<System.Data.SqlClient.SqlDataReader> GetDataReaderAsync(string query, CommandType commandType, System.Data.SqlClient.SqlParameter[] arraySqlParameter);
        DataTable GetDataTable(string query);
        DataTable GetDataTable(string query, CommandType commandType);
        DataTable GetDataTable(string query, CommandType commandType, params System.Data.SqlClient.SqlParameter[] sqlParameters);
        DataTable GetDataTable(string query, CommandType commandType, string[] parasName, object[] parasValue);

    }
}
