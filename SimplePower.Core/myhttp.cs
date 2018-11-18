using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SimplePower
{
    public class myhttp
    {
        private static readonly String Empty = "";
        private static String __VIEWSTATE { get; set; }
        private static String __EVENTVALIDATION { get; set; }
        private static List<KeyValuePair<String, String>> paramList { get; set; }
        public static string url_baidu = "https://www.baidu.com";
        public static string url_host = "http://202.114.18.218/main.aspx";
        private static HttpClient http = new HttpClient();

        public async static Task<ObservableCollection<PowerList>> GetPower(Power power)
        {
            var powerLists = new ObservableCollection<PowerList>();
            paramList = new List<KeyValuePair<string, string>>();
            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            var response = await http.GetAsync(url_host);
            string result = response.Content.ReadAsStringAsync().Result;
            //获取到两个参数，传区域
            set_para(result);
            set_paraList("programId", power.region, Empty, Empty, Empty, Empty);

            await Task.Delay(20);
            response =await http.PostAsync(url_host, new FormUrlEncodedContent(paramList));
            result = response.Content.ReadAsStringAsync().Result;
            //传楼栋号
            set_para(result);
            set_paraList("txtyq", power.region, power.department_num, Empty, Empty, Empty);

            await Task.Delay(20);
            response = await http.PostAsync(url_host, new FormUrlEncodedContent(paramList));
            result = response.Content.ReadAsStringAsync().Result;
            //传宿舍号
            set_para(result);
            set_paraList(Empty, power.region, power.department_num, power.domitory_num, "65", "19");

            await Task.Delay(20);
            response = await http.PostAsync(url_host, new FormUrlEncodedContent(paramList));
            result = response.Content.ReadAsStringAsync().Result;

            //初始化文档
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            //查找节点
            var titleNodes = doc.DocumentNode.SelectSingleNode("//table[@rules='all']");
            var list = titleNodes.SelectNodes(@"tr");

            foreach (var i in list)
            {

                var list2 = i.SelectNodes(@"td");
                if (list2 != null)
                {
                    powerLists.Add(new PowerList(list2[1].InnerText, list2[0].InnerText));
                }
            }
            return powerLists;
        }

        public async static Task<ObservableCollection<String>> GetList_region()
        {
            var regionLists = new ObservableCollection<string>();
            paramList = new List<KeyValuePair<string, string>>();
            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            var response = await http.GetAsync(url_host);
            string result = response.Content.ReadAsStringAsync().Result;
            //初始化文档
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            //查找节点
            var titleNodes = doc.DocumentNode.SelectSingleNode("//select[@name='programId']");
            var list = titleNodes.SelectNodes(@"option");

            regionLists.Clear();
            foreach (var i in list)
            {
                if (i != null&&i.InnerText!="-请选择-")
                {regionLists.Add(i.InnerText);}
            }
            return regionLists;
        }

        public async static Task<ObservableCollection<String>> GetList_dormitory(string region)
        {
            var department_Lists = new ObservableCollection<String>();
            paramList = new List<KeyValuePair<string, string>>();
            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            var response = await http.GetAsync(url_host);
            string result = response.Content.ReadAsStringAsync().Result;
            //获取到两个参数，传区域
            set_para(result);
            set_paraList("programId", region, Empty, Empty, Empty, Empty);

            await Task.Delay(20);
            response = await http.PostAsync(url_host, new FormUrlEncodedContent(paramList));
            result = response.Content.ReadAsStringAsync().Result;

            //初始化文档
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            //查找节点
            var titleNodes = doc.DocumentNode.SelectSingleNode("//select[@name='txtyq']");
            var list = titleNodes.SelectNodes(@"option");

            department_Lists.Clear();
            foreach (var i in list)
            {
                if (i != null && i.InnerText != "-请选择-")
                { department_Lists.Add(i.InnerText);}
            }
            return department_Lists;
        }

        private static void set_para(string result)
        {
            __VIEWSTATE = new Regex("id=\"__VIEWSTATE\" value=\"(.*?)\"").Match(result).Groups[1].Value;
            __EVENTVALIDATION = new Regex("id=\"__EVENTVALIDATION\" value=\"(.*?)\"").Match(result).Groups[1].Value;
        }

        private static void set_paraList(string EVENTTARGET,string programId,string txtyq,string Txtroom,string x,string y)
        {
            paramList.Clear();
            paramList.Add(new KeyValuePair<string, string>("__EVENTTARGET", EVENTTARGET));
            paramList.Add(new KeyValuePair<string, string>("__EVENTARGUMENT", ""));
            paramList.Add(new KeyValuePair<string, string>("__LASTFOCUS", ""));

            paramList.Add(new KeyValuePair<string, string>("__VIEWSTATE", __VIEWSTATE));
            paramList.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", __EVENTVALIDATION));

            paramList.Add(new KeyValuePair<string, string>("programId", programId));
            paramList.Add(new KeyValuePair<string, string>("txtyq", txtyq));
            paramList.Add(new KeyValuePair<string, string>("Txtroom", Txtroom));
            paramList.Add(new KeyValuePair<string, string>("ImageButton1.x", x));
            paramList.Add(new KeyValuePair<string, string>("ImageButton1.y", y));

            paramList.Add(new KeyValuePair<string, string>("TextBox2", ""));
            paramList.Add(new KeyValuePair<string, string>("TextBox3", ""));
        }

        public async static Task<bool> TestAsync(string url)
        {
            try
            {
                var cts = new CancellationTokenSource(2000);
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var result = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,cts.Token);
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
