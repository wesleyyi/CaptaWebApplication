using System.Runtime.Serialization.Json;

namespace CaptaWebApplication.Utilities
{
    public class DeserializeUtilities
    {
        public T Deserialize<T>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            {
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}
