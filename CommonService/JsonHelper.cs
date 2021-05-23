using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Common
{
    public static class JsonHelper
    {
        public static T DeserializeObject<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static object DeserializeObject(string jsonString, Type type)
        {
            return JsonConvert.DeserializeObject(jsonString, type);
        }

        /// <summary>
        /// 对象序列化String
        /// 时间默认格式默认：yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj)
        {
            IsoDateTimeConverter timeCoverter = new IsoDateTimeConverter();
            timeCoverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, timeCoverter);
        }

        /// <summary>
        /// 日期格式
        /// timeFormat：yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="timeFormat"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj, string timeFormat)
        {
            IsoDateTimeConverter timeCoverter = new IsoDateTimeConverter();
            timeCoverter.DateTimeFormat = timeFormat;
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, timeCoverter);
        }

        public static string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }


        /// <summary>
        /// 读取Json文件
        /// </summary>
        /// <param name="key">JSON文件中的key值</param>
        /// <returns>JSON文件中的value值</returns>
        public static T Readjson<T>(string jsonfile)
        {
            using (StreamReader file = File.OpenText(jsonfile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    var t = Activator.CreateInstance<T>();
                   // JObject o = (JObject)JToken.ReadFrom(reader);
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    foreach (PropertyInfo propertyInfo in t.GetType().GetProperties())
                    {
                        var value = o[propertyInfo.Name].ToObject<string>();
                        propertyInfo.SetValue(t, value, null);
                    }
                    return t;
                }
            }
        }
        
        /// <summary>
        /// 修改Json文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullName"></param>
        /// <param name="model"></param>
        public static void ModifyJsonFile<T>(string fullName,T model)
        {
            if (File.Exists(fullName))
            {
                string jsonString = File.ReadAllText(fullName, Encoding.Default);//读取文件
                JObject jobject = JObject.Parse(jsonString);//解析成json
                //jobject["AppKey"] = this.TbKey.Text;//替换需要的文件
                //jobject["AppSecret"] = this.TbSecret.Text;//替换需要的文件
                foreach (PropertyInfo pi in model.GetType().GetProperties())
                {
                    jobject[pi.Name] = pi.GetValue(model, null).ToString();
                } 
                string convertString = Convert.ToString(jobject);//将json装换为string
                File.WriteAllText(fullName, convertString);//将内容写进jon文件中
            }

        }
    }
}
