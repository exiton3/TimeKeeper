namespace Framework.Common
{
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using Dtos;

    public class JsonSerializer
    {
        public string Serialize(IEnumerable<MessageDto> list)
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Serialize(list);
        }

        public List<Dictionary<string, string>> Deserialize(string inputData)
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Deserialize<List<Dictionary<string, string>>>(inputData);

        }
    }
}