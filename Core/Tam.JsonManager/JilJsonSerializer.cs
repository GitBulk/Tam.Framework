using Jil;
using System.IO;

namespace Tam.JsonManager
{
    public class JilJsonSerializer : ISerializer
    {
        public JilJsonSerializer()
        { }

        public string Serialize<T>(T data)
        {
            using (var output = new StringWriter())
            {
                JSON.Serialize(data, output);
                return output.ToString();
            }
        }

        public T Deserialize<T>(string serializedData)
        {
            using (var input = new StringReader(serializedData))
            {
                var result = JSON.Deserialize<T>(input);
                return result;
            }
        }
    }
}