using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;

namespace GatherStockData
{
    class Program
    {

        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var service = new GatherService();
            string jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Config.json");
            Console.WriteLine("程序运行中，请勿关闭此控制台...");
            ConfigModel config = JsonHelper.Readjson<ConfigModel>(jsonFile);

            if (config.LastGatherDate == DateTime.Now.Date.ToString("yyyyMMdd"))
            {
                if (config.IsTodayInTradingDays == "Y")
                {
                    var itemSources = service.GetItemSource().Where(x => x.Id > 1);
                }

            }
            else //今天第一次采集
            {
                if (DateTime.Now.Hour * 60 + DateTime.Now.Minute > 540) // 【第一次采集必须在交易时间内9点开始？？？】
                {
             
                    var model = service.GetItemSource(1);
                    var tradingDate = GetCurrentTradingDate(model);
                    if (tradingDate.Length == 8 && tradingDate.StartsWith("202"))
                    {
                        //if (tradingDate == config.LastTradingDate) //交易日没变，没有新数据，证明访问过
                        //{
                        //    config.LastGatherDate = DateTime.Now.Date.ToString("yyyyMMdd");
                        //    config.IsTodayInTradingDays = "N";
                        //    JsonHelper.ModifyJsonFile(jsonFile, config);
                        //}
                        if (1==1) //采集交易数据
                        {
                            var itemSources = service.GetItemSource().Where(x => x.Id > 1);
                            foreach (var item in itemSources)
                            {
                                if (item.GroupId == 666) //循环间隔采集
                                {
                                    GatherAndSave(item);
                                }
                                else
                                {
                                    if (DateTime.Now.Hour == 15) //每日3点采集
                                    {

                                    }
                                }

                            }

                            config.LastTradingDate = DateTime.Now.Date.ToString("yyyyMMdd");
                            config.LastGatherDate = DateTime.Now.Date.ToString("yyyyMMdd");
                            config.IsTodayInTradingDays = "Y";
                            JsonHelper.ModifyJsonFile(jsonFile, config);
                        }
                    }
                    else
                        FileHelper.WriteToDisk($"匹配交易日错误 response:{ tradingDate} ||");
                }
            }




            //GatherXiangGangZhongYangJieSuanYouXianGongSiChiCang(750);

            //DayOfWeek day = DateTime.Now.DayOfWeek;
            //int minute = DateTime.Now.Hour * 60 + DateTime.Now.Minute;

            ////判断是否为周末
            //if (day == DayOfWeek.Sunday || day == DayOfWeek.Saturday)
            //{
            //    LogToDisk("周末不采集数据", 0, 0);
            //    return;
            //}
            //GatherHuShenStockData(minute);
            //GatherXiangGangZhongYangJieSuanYouXianGongSiChiCang(minute);

        }

        private static void GatherHuShenStockData(int minute)
        {

            if (((minute >= 540 && minute <= 690) || (minute >= 780 && minute <= 900)))
            {
                // var span = ConvertDateTimeToTimeStamp();
                //HttpClient client = new HttpClient();
                //GetHuShenData(client, minute, span, 0);
            }
        }

        private static void GetHuShenData(HttpClient client, int minute, string span, int i)
        {

            // 异步调用
            //HttpResponseMessage response = await client.GetAsync("https://hq.cmschina.com/market/json?funcno=21000&version=1&sort=24&order=1&type=9%253A0%253A2%253A18&curPage=1&rowOfPage=4000&field=1%253A2%253A3%253A21%253A22%253A23%253A24%253A14%253A8%253A13%253A4%253A5%253A9%253A12%253A10%253A11%253A16%253A58%253A6%253A7%253A15%253A17%253A18%253A19%253A25%253A27%253A31%253A28%253A48&timeStamp=" + span);
            //response.EnsureSuccessStatusCode();
            //string responseBody = await response.Content.ReadAsStringAsync();

            // 同步调用
            var task = client.GetAsync("https://hq.cmschina.com/market/json?funcno=21000&version=1&sort=24&order=1&type=9%253A0%253A2%253A18&curPage=1&rowOfPage=4000&field=1%253A2%253A3%253A21%253A22%253A23%253A24%253A14%253A8%253A13%253A4%253A5%253A9%253A12%253A10%253A11%253A16%253A58%253A6%253A7%253A15%253A17%253A18%253A19%253A25%253A27%253A31%253A28%253A48&timeStamp=" + span);

            var rep = task.Result;//在这里会等待task返回。
            while (rep.StatusCode != System.Net.HttpStatusCode.OK && i <= 5)
            {
                i++;
                GetHuShenData(client, minute, span, i);
                Thread.Sleep(5000);
            }

            if (i > 5)
                return;

            var task2 = rep.Content.ReadAsStringAsync();
            var responseBody = task2.Result;//在这里会等待task返回。


            DataTable table = new DataTable();
            table.Columns.Add("GatherDate"); table.Columns.Add("GatherMinute"); table.Columns.Add("GatherTime"); table.Columns.Add("DaLei");

            table.Columns.Add("ZhangFuBaiFenBi"); table.Columns.Add("XianJia"); table.Columns.Add("ZhangDieE"); table.Columns.Add("UnKnow1"); table.Columns.Add("StockName");
            table.Columns.Add("XiaoLei"); table.Columns.Add("StockCode"); table.Columns.Add("ChengJiaoJinE"); table.Columns.Add("HuanShou"); table.Columns.Add("ShiYingLv");
            table.Columns.Add("MaiRuJia"); table.Columns.Add("MaiChuJia"); table.Columns.Add("JinKai"); table.Columns.Add("ZuoShou"); table.Columns.Add("ZuiGao");
            table.Columns.Add("ZuiDi"); table.Columns.Add("ZhenFuBaiFenBi"); table.Columns.Add("RongZiRongQuan"); table.Columns.Add("ChengJiaoLiang"); table.Columns.Add("ZhangSu");
            table.Columns.Add("LiangBi"); table.Columns.Add("JunJia"); table.Columns.Add("NeiPan"); table.Columns.Add("WaiPan"); table.Columns.Add("XianShou");
            table.Columns.Add("LiuTongShiZhi"); table.Columns.Add("ZongShiZhi"); table.Columns.Add("ShouZiMu"); table.Columns.Add("UnKnow2");

            ResponseModel s = JsonConvert.DeserializeObject<ResponseModel>(responseBody);
            foreach (var l1 in s.Results)
            {
                DataRow dr = table.NewRow();
                dr["ZhangFuBaiFenBi"] = l1[0]; dr["XianJia"] = l1[1]; dr["ZhangDieE"] = l1[2]; dr["UnKnow1"] = l1[3]; dr["StockName"] = l1[4];
                dr["XiaoLei"] = l1[5]; dr["StockCode"] = l1[6]; dr["ChengJiaoJinE"] = l1[7]; dr["HuanShou"] = l1[8]; dr["ShiYingLv"] = l1[9];
                dr["MaiRuJia"] = l1[10]; dr["MaiChuJia"] = l1[11]; dr["JinKai"] = l1[12]; dr["ZuoShou"] = l1[13]; dr["ZuiGao"] = l1[14];
                dr["ZuiDi"] = l1[15]; dr["ZhenFuBaiFenBi"] = l1[16]; dr["RongZiRongQuan"] = l1[17]; dr["ChengJiaoLiang"] = l1[18]; dr["ZhangSu"] = l1[19];
                dr["LiangBi"] = l1[20]; dr["JunJia"] = l1[21]; dr["NeiPan"] = l1[22]; dr["WaiPan"] = l1[23]; dr["XianShou"] = l1[24];
                dr["LiuTongShiZhi"] = l1[25]; dr["ZongShiZhi"] = l1[26]; dr["ShouZiMu"] = l1[27]; dr["UnKnow2"] = l1[28];

                dr["GatherDate"] = DateTime.Now.ToString("yyyyMMdd");
                dr["GatherMinute"] = minute.ToString();
                dr["GatherTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dr["DaLei"] = "沪深A股";

                table.Rows.Add(dr);
            }
            SqlHelper.BulkInsert(table, "ZhaoShangZhengQuanJiShiHangQing");

            if (minute == 900)
                SqlHelper.BulkInsert(table, "ZhaoShangZhengQuanJiShiHangQingRiJie");

            LogToDisk("", minute, i);
        }

        private static void GatherXiangGangZhongYangJieSuanYouXianGongSiChiCang(int minute)
        {
            if (minute > 730 && minute < 760)
            {
                HttpClient client = new HttpClient();
                GetXiangGangChiCang(client, 0);
            }
        }

        private static void GetXiangGangChiCang(HttpClient client, int i)
        {
            // 采集1500条数据足够一个周期的发布
            var task = client.GetAsync("http://data.eastmoney.com/DataCenter_V3/gdfx/data.ashx?SortType=RDATE,NDATE&SortRule=3&PageIndex=1&PageSize=1500&jsObj=YepsAjQH&type=NSHDDETAIL&cgbd=0&filter=(SHAREHDCODE=%2780637337%27)&rt=53090259");
            var rep = task.Result;//在这里会等待task返回。

            // 指定Encoding解码
            //var taskaa = client.GetByteArrayAsync("http://data.eastmoney.com/DataCenter_V3/gdfx/data.ashx?SortType=RDATE,NDATE&SortRule=3&PageIndex=1&PageSize=50&jsObj=UhabnIci&type=NSHDDETAIL&cgbd=1&filter=(SHAREHDCODE=%2780637337%27)&rt=53090255");
            //var rep2 = taskaa.Result;
            //var result = Encoding.GetEncoding("gb2312").GetString(rep2);

            while (rep.StatusCode != System.Net.HttpStatusCode.OK && i <= 5)
            {
                i++;
                GetXiangGangChiCang(client, i);
                Thread.Sleep(5000);
            }

            if (i > 5)
                return;

            var task2 = rep.Content.ReadAsStringAsync();
            var responseBody = task2.Result;//在这里会等待task返回。

            Regex regex = new Regex(@".*data:(.*)}");
            //var s = regex.Match(responseBody);
            string json = regex.Match(responseBody).Groups[1].Value.Replace(@"""-""", @"""0""");
            // 转化为List
            //IList<DongFangCaiFuJiGouChiCangModel> lists = new List<DongFangCaiFuJiGouChiCangModel>();
            //lists = JsonConvert.DeserializeObject<IList<DongFangCaiFuJiGouChiCangModel>>(json);

            DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
            SqlHelper.BulkInsert(dt, "XiangGangZhongYangJieSuanChiGuMID");
            LogToDisk($"香港中央结算公司持仓采集完毕", 0, 0);
            SqlHelper.ExecuteNonQuery(@" insert into XiangGangZhongYangJieSuanChiGu (COMPANYCODE,SSNAME,SHAREHDNAME,SHAREHDTYPE,SHARESTYPE,RANK,SCODE,SNAME,RDATE,SHAREHDNUM,LTAG,ZB,NDATE,BZ,BDBL,SHAREHDCODE,SHAREHDRATIO,BDSUM) 
                                        select COMPANYCODE, SSNAME, SHAREHDNAME, SHAREHDTYPE, SHARESTYPE, RANK, SCODE, SNAME, RDATE, SHAREHDNUM, LTAG, ZB, NDATE, BZ, BDBL, SHAREHDCODE, SHAREHDRATIO, BDSUM
                                        from XiangGangZhongYangJieSuanChiGuMID a
                                        where not exists(select 1 from XiangGangZhongYangJieSuanChiGu where RDATE = a.RDATE and SCODE = a.SCODE) ;truncate table XiangGangZhongYangJieSuanChiGuMID; ", null);
        }


        private static void LogToDisk(string note, int minute, int i)
        {
            string fileName = Path.Combine(Directory.GetCurrentDirectory(), "GatherLog.txt");
            FileMode fileMode = File.Exists(fileName) ? FileMode.Append : FileMode.Create;
            using FileStream fileStream = new FileStream(fileName, fileMode, FileAccess.Write);
            using StreamWriter streamWriter = new StreamWriter(fileStream);
            if (string.IsNullOrEmpty(note))
                streamWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} \t 第{minute}分钟,开始采集，重试{i}次");
            else
                streamWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} \t {note}");

        }

        /* =======================================================       新的程序函数                  */
        /// <summary>
        /// 通用Get获取可以用于正则表达式的内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetResponseToApplingPatternExoression(ItemSourceModel model)
        {
            HttpClient client = new HttpClient();
            //HttpClient client = new HttpClient(new RetryHandler(new HttpClientHandler()));
            // client.Timeout = new TimeSpan(0, 0, 8);
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36");
            client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");

            // "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.116 Safari/537.36"
            return SubstringResponse(HttpHelper.GetResponse(client, InsteadOfUrlParameter(model.GatherUrl)), model.ContentBegin ?? "", model.ContentEnd ?? ""); ;
        }


        /// <summary>
        /// 获取当前交易日期
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string GetCurrentTradingDate(ItemSourceModel model)
        {
            string response = GetResponseToApplingPatternExoression(model);
            if (response.Length > 2)
            {
                Regex rg = new Regex(model.FieldPattern, RegexOptions.IgnoreCase);//指定不区分大小写的匹配
                if (rg.IsMatch(response))
                    return rg.Match(response).Groups[1].Value;
                else
                    return response;
            }
            return response;
        }

        /// <summary>
        /// 根据正则获取DataTable结构
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string GatherAndSave(ItemSourceModel model)
        {
            string response = GetResponseToApplingPatternExoression(model);
            if (response.Length > 2)
            {
                Regex rg = new Regex(model.FieldPattern, RegexOptions.IgnoreCase);//指定不区分大小写的匹配
                if (rg.IsMatch(response))
                {
                    DataTable dataTable = DataTableHelper.CreateDataTableByPattern(model.FieldPattern,model.ExtraFieldPattern);
                    //return rg.Match(response).Groups[1].Value;
                    // 插入到后台
                    MatchCollection matches = rg.Matches(response);
                    for (int i = 0; i < matches.Count; i++)
                    {
                        Match matchitem = matches[i];
                        GroupCollection gc = matchitem.Groups;
                        DataRow dr = dataTable.NewRow();
                        //多少个匹配组就创建多少个列值
                        for (int j = 1; j < gc.Count; j++)
                        {
                            dr[j - 1] = gc[j];
                        }
                       
                        if (!string.IsNullOrEmpty(model.ExtraFieldPattern))
                        {
                            JObject jObject = new JObject(@"{GatherDate:当前日期, GatherDateNumber 当前日期序号, GatherMinuteNumber 当前分钟序号 }");
                            foreach (var item in jObject.Properties())
                            {
                                dr[item.Name] = GetCalculateValue(item.Value.ToObject<string>());
                            }
                        }
                    }

                }
                else
                    return response;
            }
            return response;
        }

        private static object GetCalculateValue(string value)
        {
            object returnValue = null;
            switch (value)
            {
                case "当前日期": returnValue= DateTime.Now.Date; break;
                case "当前日期序号": returnValue = DateTime.Now.Date.ToString("yyyyMMdd"); break;
                case "当前分钟序号": returnValue = DateTime.Now.Date.Hour *60 +DateTime.Now.Date.Minute; break;
                default: break;
            }
            return returnValue;
        }


        /// <summary>
        /// 替换掉Url里面的动态参数
        /// </summary>
        /// <param name="gatherUrl"></param>
        /// <returns></returns>
        private static string InsteadOfUrlParameter(string gatherUrl)
        {
            return gatherUrl.Replace("{span13}", HttpHelper.ConvertDateTimeToTimeStamp(DateTime.Now, 3));
        }


        /// <summary>
        /// 通过开始和结束位置，先截取中间的内容，再匹配具体正则
        /// </summary>
        /// <param name="content"></param>
        /// <param name="listbegin"></param>
        /// <param name="listend"></param>
        /// <returns></returns>
        private static string SubstringResponse(string content, string listbegin, string listend)
        {
            if (string.IsNullOrEmpty(listbegin) || string.IsNullOrEmpty(listend))
                return content;
            return Regex.Match(content, listbegin + "(.*?)" + listend, RegexOptions.IgnoreCase).Groups[1].Value;
        }


    }

    public class ResponseModel
    {
        public string ErrorInfo { get; set; }

        public string ErrorNo { get; set; }

        public string[][] Results { get; set; }

    }

}
