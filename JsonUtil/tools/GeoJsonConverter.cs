using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace JsonUtil
{
    public class GeoJsonConverter<Geo> : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Geo);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            String typeName = jObject["type"].Value<string>();
            Type type = Type.GetType(typeName);
            return JsonConvert.DeserializeObject(jObject.ToString(), type);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
