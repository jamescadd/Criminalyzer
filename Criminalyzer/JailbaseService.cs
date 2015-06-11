using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Criminalyzer
{
    static class JailbaseService
    {
        public static Task<Jailbase> GetMugshots()
        {
            string mugshotUrl = "http://www.jailbase.com/api/1/recent/?source_id=az-mcso";
            return GetResponseJson<Jailbase>(new Uri(mugshotUrl));
        }

        public static async Task<T> GetResponseJson<T>(Uri uri)
        {
            var request = WebRequest.CreateHttp(uri);
            var response = await request.GetResponseAsync();
            //var responseString = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();

            //var res = Windows.Data.Json.JsonObject.Parse(responseString);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            var obj = serializer.ReadObject(response.GetResponseStream());

            return (T)obj;

            //var jsonSerializer = new JsonSerializer();
            //var result = jsonSerializer.Deserialize<T>(new JsonTextReader(new StringReader(responseString)));
            //return result;

            //return default(T);
        }
    }
}
