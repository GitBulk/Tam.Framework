using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Tam.JsonManager
{
    public class NewtonSoftJsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings settings;

        public NewtonSoftJsonSerializer()
        {
            this.settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            this.settings.Converters.Add(new StringEnumConverter());
        }

        public string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data, this.settings);
        }

        public T Deserialize<T>(string serializedData)
        {
            return JsonConvert.DeserializeObject<T>(serializedData, this.settings);
        }
    }
}