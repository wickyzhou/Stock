using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Reflection;
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

        public static string GetResponse(string gatherUrl)
        {
            FileHelper.WriteToDisk($"{gatherUrl}");
       
            try
            {
                //var task = client.GetAsync(gatherUrl);
                //var response = task.Result;//在这里会等待task返回。
                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                //    var task2 = response.Content.ReadAsStringAsync();
                //    return task2.Result;//在这里会等待task返回。
                //}
                //else
                //    return response.StatusCode.ToString();
                //HttpClient client = new HttpClient(new RetryHandler(new HttpClientHandler()));
                // client.Timeout = new TimeSpan(0, 0, 8);


                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36");
                client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                var response = client.GetAsync(gatherUrl).Result;
                return response.StatusCode == HttpStatusCode.OK ? response.Content.ReadAsStringAsync().Result : $"{gatherUrl} \t {response.StatusCode}";//在这里会等待task返回。responsebody
            }
            catch(Exception ex)
            {
                FileHelper.WriteToDisk($"获取内容异常：{ex.Message}");
                return "";
            }
        }


        private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36";

        private static Encoding requestEncoding = Encoding.UTF8;

        /// <summary>
        /// Get方式访问数据
        /// </summary>
        /// <param name="url">访问的Url</param>
        /// <param name="HttpWebResponseString">响应字符串</param>
        /// <param name="timeout">毫秒单位的超时</param>
        /// <param name="requestHeader">header格式  name:value   多个用回车换行</param>
        /// <returns></returns>
        public static bool HttpGet(string url, out string HttpWebResponseString, int timeout, string requestHeader = "")
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebResponseString = "";
            try
            {
                HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = DefaultUserAgent;
                httpWebRequest.Timeout = timeout;
                if (!string.IsNullOrEmpty(requestHeader))
                    AddRequestHeaders(httpWebRequest, requestHeader);
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                HttpWebResponseString = ReadHttpWebResponse(httpWebResponse);
                return true;
            }
            catch (Exception ex)
            {
                HttpWebResponseString = ex.ToString();
                return false;
            }
        }

        public static bool HttpPost(string url, byte[] Data, out string HttpWebResponseString, int timeout, string requestHeader = "")
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebResponseString = "";
            HttpWebRequest httpWebRequest = null;
            Stream stream = null;
            try
            {
                httpWebRequest = (WebRequest.Create(url) as HttpWebRequest);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.UserAgent = DefaultUserAgent;
                httpWebRequest.Timeout = timeout;
                if (!string.IsNullOrEmpty(requestHeader))
                    AddRequestHeaders(httpWebRequest, requestHeader);
                if (Data != null)
                {
                    requestEncoding.GetString(Data);
                    stream = httpWebRequest.GetRequestStream();
                    stream.Write(Data, 0, Data.Length);
                }
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                HttpWebResponseString = ReadHttpWebResponse(httpWebResponse);
                return true;
            }
            catch (Exception ex)
            {
                HttpWebResponseString = ex.ToString();
                return false;
            }
            finally
            {
                stream?.Close();
            }
        }

        public static string ReadHttpWebResponse(HttpWebResponse HttpWebResponse)
        {
            Stream stream = null;
            StreamReader streamReader = null;
          
            try
            {
                stream = HttpWebResponse.GetResponseStream();
                streamReader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
                return streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                streamReader?.Close();
                stream?.Close();
                HttpWebResponse?.Close();
            }
        }

        /// <summary>
        /// 根据t_item_source_list表的list_request_headers 和 info_request_headers 内容添加到HttpRequest.Headers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestHeaders"></param>
        public static void AddRequestHeaders(HttpWebRequest request, string requestHeaders)
        {
            //&& request.CookieContainer.GetCookies(request.RequestUri) == null
            try
            {   //将头部格式不改变的加入  不需要替换中间的-； Content-Type  √    ContentType ×
                if (requestHeaders.Length > 0)
                {
                    foreach (var item in requestHeaders.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        SetHeaderValue(request.Headers, item.Split(new char[] { ':', '：' }, 2)[0].Trim(), item.Split(new char[] { ':', '：' }, 2)[1].Trim());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("requestHeader格式错误，多个Header用回车换行分割，单个Header用冒号分隔。示例：Referrer Policy: unsafe-url \r\n accept-encoding: gzip, deflate, br", e);
            }


        }

        /// <summary>
        /// 将字串形式的Headers，添加到HttpRequest.Headers对象中
        /// </summary>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }

        }




        /// <summary>
        /// GET方式获取网址响应
        /// </summary>
        /// <param name="url"></param>
        /// <param name="charset"></param>
        /// <param name="isException"></param>
        /// <returns></returns>
        public static string HttpGetX(string url, out bool isException, string charset, string requestHeaders = "")
        {
            HttpWebRequest myRequest = null; HttpWebResponse myResponse = null;
            string responseText;
            try
            {
                Util.SetCertificatePolicy();//验证安全，未能为 SSL/TLS 安全通道建立信任关系
                //System.GC.Collect();//避免GetRequestStream()超时
                myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Timeout = 60000;
                myRequest.Method = "GET";
                myRequest.AllowAutoRedirect = false;
                myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.116 Safari/537.36";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ProtocolVersion = HttpVersion.Version11;
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                myRequest.CachePolicy = noCachePolicy;
                SetRequestHeadersForGetData(myRequest, requestHeaders, "cookievalue");
                try
                {
                    responseText = GetResponseString(myRequest, out isException, charset);
                    return responseText;
                }
                catch (Exception e)
                {
                    isException = true;
                    return e.Message.ToString();
                }
            }
            catch (Exception e)
            {
                isException = true;
                return e.Message.ToString();
            }
            finally
            {
                //避免超时,释放资源
                if (myResponse != null)
                    myResponse.Close();
                if (myRequest != null)
                    myRequest.Abort();
            }
        }


        /// <summary>
        /// 将响应读取为文本数据，以填写的charset为主
        /// </summary>
        /// <param name="myResponse"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        private static string GetDecodedTextFromResponse(HttpWebResponse myResponse, string charset)
        {
            StreamReader reader; string responseText; Encoding encoding;
            try
            {
                encoding = Encoding.GetEncoding(charset);
            }
            catch
            {
                encoding = GetResponseEncoding(myResponse.CharacterSet);
            }

            if (myResponse.ContentEncoding != null && myResponse.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                reader = new StreamReader(new GZipStream(myResponse.GetResponseStream(), CompressionMode.Decompress), encoding);
            else
                reader = new StreamReader(myResponse.GetResponseStream(), encoding);
            responseText = reader.ReadToEnd();
            reader.Close();
            return responseText;
        }
        /// <summary>
        /// 设置GetData方法的Headers
        /// </summary>
        /// <param name="myRequest"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="cookieStrings"></param>
        private static void SetRequestHeadersForGetData(HttpWebRequest myRequest, string requestHeaders, string cookieStrings)
        {
            SetRequestHeadersForCommon(myRequest, requestHeaders, cookieStrings);

        }


        /// <summary>
        /// 设置GetData和PostData的通用Headers
        /// </summary>
        /// <param name="myRequest"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="cookieStrings"></param>
        private static void SetRequestHeadersForCommon(HttpWebRequest myRequest, string requestHeaders, string cookieStrings)
        {

            AddRequestHeaders(myRequest, requestHeaders);
            //强制取消缓存，同浏览器Disable Cache
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            myRequest.CachePolicy = noCachePolicy;
            myRequest.AllowAutoRedirect = true;
            myRequest.Proxy = null;
            myRequest.UseDefaultCredentials = true;
            //多线程超时解决办法。对于前面多个request。其都是keepalive为true，以及多个response也没有close
            ServicePointManager.DefaultConnectionLimit = 10;
            //如果没有填写头部连接属性则默认关闭【基础连接已经关闭: 服务器关闭了本应保持活动状态的连接。】
            if (myRequest.Headers["Connection"] == null)
            {
                myRequest.KeepAlive = false;
            }

            if (myRequest.Headers["User-Agent"] == null)
            {
                myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.116 Safari/537.36";
            }

            if (cookieStrings.Length > 0)
            {
                if (myRequest.Headers["Cookie"] != null)
                {
                    myRequest.Headers.Remove("Cookie");
                }
                myRequest.Headers.Add("Cookie", cookieStrings);
            }

        }

        /// <summary>
        /// 获取HttpWebResponse中获取的字符集，将其Stream编译为对应的编码response.CharacterSet
        /// </summary>
        /// <param name="responseCharSet"></param>
        /// <returns></returns>
        private static Encoding GetResponseEncoding(string responseCharSet)
        {
            Encoding encoding;
            switch (responseCharSet.ToLower())           //小写
            {
                case "gbk":
                    encoding = Encoding.GetEncoding("GBK");
                    break;
                case "gb2312":
                    encoding = Encoding.GetEncoding("GB2312");
                    break;
                case "utf-8":
                    encoding = Encoding.UTF8;
                    break;
                case "iso-8859-1":
                    encoding = Encoding.GetEncoding("GBK"); //GB2312              
                    break;
                case "big5":
                    encoding = Encoding.GetEncoding("Big5");
                    break;
                default:
                    encoding = Encoding.UTF8;
                    break;
            }
            return encoding;
        }

        /// <summary>
        /// 获取响应文本字符串，返回是否异常
        /// </summary>
        /// <param name="myRequest"></param>
        /// <param name="isException"></param>
        /// <returns></returns>
        private static string GetResponseString(HttpWebRequest myRequest, out bool isException, string charset)
        {
            string responseText;
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            if (myResponse.StatusCode == HttpStatusCode.OK && myResponse.ContentLength < 10240 * 10240)
            {
                responseText = GetDecodedTextFromResponse(myResponse, charset);
                isException = false;

                ////获取响应__VIEWSTATE
                //if (viewStateStringGlobal.Length > 0)
                //    viewStateStringGlobal = GetViewStateParametersStringFormmater(responseText);
            }
            else
            {
                isException = true;
                responseText = $"访问状态为:{myResponse.StatusCode}，请检查是否有重定向等";
            }
            return responseText;
        }
    }
}
