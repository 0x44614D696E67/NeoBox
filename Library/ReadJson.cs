using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace NeoBox.Library;
internal class ReadJson
{
    /// <summary>
    /// 读取JSON文件
    /// </summary>
    /// <param name="key">JSON文件中的key值</param>
    /// <returns>JSON文件中的value值</returns>
    public static string ReadJSON(string key)
    {
        string jsonfile = "D://testJson.json";//JSON文件路径

        using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject o = (JObject)JToken.ReadFrom(reader);
                var value = o[key].ToString();
                return value;
            }
        }
    }
}
