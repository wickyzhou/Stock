using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;

namespace Common
{
    public static class HttpHelper
    {
        /// <summary>
        /// 将日期转换为N为小数的时间戳,基准 0 对应的是10位 ， 需要13位则 scale =3
        /// </summary>
        /// <param name="time"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static string ConvertDateTimeToTimeStamp(DateTime time, int scale = 0)
        {
            TimeSpan cha = time - TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            double t = (double)cha.TotalSeconds;
            return ((long)(Math.Round(t, scale) * Math.Pow(10, scale))).ToString();
        }

        /// <summary>
        /// 中文转百分号格式
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string UrlEncode(string keyword, Encoding charset)
        {
            return HttpUtility.UrlEncode(keyword, charset);
        }

        /// <summary>
        /// 百分号格式转中文
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string UrlDecode(string keyword, Encoding charset)
        {
            return HttpUtility.UrlDecode(keyword, charset);

        }

        //  string Name = GetEnumName<ActionType>(1);  // 根据传入的属性值返回对应枚举属性名称
        // int value = GetEnumValue<ActionType>("Open");  // 根据传入的属性名称获得对应值
        // int value1 = (int)ActionType.OPEN;  // 直接使用枚举类指定属性值

        public static string GetResponse(HttpClient client, string gatherUrl)
        {
            FileHelper.WriteToDisk($"{gatherUrl}");
            try
            {
                var task = client.GetAsync(gatherUrl);
                var response = task.Result;//在这里会等待task返回。
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var task2 = response.Content.ReadAsStringAsync();
                    return task2.Result;//在这里会等待task返回。
                }
                else
                    return response.StatusCode.ToString();
               // var response = client.GetAsync(gatherUrl).Result;
               //return response.StatusCode == HttpStatusCode.OK ? response.Content.ReadAsStringAsync().Result : "";//在这里会等待task返回。responsebody
            }
            catch(Exception ex)
            {
                FileHelper.WriteToDisk($"获取内容异常：{ex.Message}");
                return "";
            }
          
        }
    }
}
