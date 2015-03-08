using Jil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace Tam.JsonManager
{
    public static class JsonUtil
    {
        private static Dictionary<string, object> GetItem(List<string> columnsName, SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            for (int i = 0; i < columnsName.Count; i++)
            {
                // Format is: "column name: value"
                result.Add(columnsName[i], reader[columnsName[i]]);
            }
            return result;
        }

        private static IEnumerable<Dictionary<string, object>> SerializeReader(SqlDataReader reader)
        {
            var result = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                cols.Add(reader.GetName(i));
            }
            while (reader.Read())
            {
                result.Add(GetItem(cols, reader));
            }
            return result;
        }

        public static string GetJsonString(SqlDataReader reader)
        {
            // http://stackoverflow.com/questions/5083709/convert-from-sqldatareader-to-json
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            // use NewtonSoft
            string jsonString = JsonConvert.SerializeObject(SerializeReader(reader), Formatting.Indented);
            return jsonString;
        }

        public static dynamic DeserializeDynamic(string serializedData)
        {
            // https://github.com/kevin-montrose/Jil
            using (var input = new StringReader(serializedData))
            {
                // use Jil Json
                var result = JSON.DeserializeDynamic(input);
                return result;
            }
        }
    }
}