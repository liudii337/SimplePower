using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimplePower
{
    class myhttp
    {
        private static readonly String Empty = "";
        private static String __VIEWSTATE { get; set; }
        private static String __EVENTVALIDATION { get; set; }
        private static List<KeyValuePair<String, String>> paramList { get; set; }

        public async static Task GetPower(Power power)
        {
            var http = new HttpClient();
            string url = "http://202.114.18.218/main.aspx";
            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            var response = await http.GetAsync(url);
            string result = response.Content.ReadAsStringAsync().Result;
            //获取到两个参数，传区域
            set_para(result);
            set_paraList("programId", power.region, Empty, Empty, Empty, Empty);

            response =await http.PostAsync(url, new FormUrlEncodedContent(paramList));
            result = response.Content.ReadAsStringAsync().Result;
            //传楼栋号
            set_para(result);
            set_paraList("txtyq", power.region, power.department_num, Empty, Empty, Empty);

            response = await http.PostAsync(url, new FormUrlEncodedContent(paramList));
            result = response.Content.ReadAsStringAsync().Result;
            //传宿舍号
            set_para(result);
            set_paraList(Empty, power.region, power.department_num, power.domitory_num, "65", "19");

            response = await http.PostAsync(url, new FormUrlEncodedContent(paramList));
            result = response.Content.ReadAsStringAsync().Result;

            //初始化文档
            HtmlDocument doc = new HtmlDocument();
            doc.Load(result);
            //查找节点
            HtmlNodeCollection titleNodes = doc.DocumentNode.SelectNodes("//table[@rules='all']");


        }

        private static void set_para(string result)
        {
            __VIEWSTATE = new Regex("id=\"__VIEWSTATE\" value=\"(.*?)\"").Match(result).Groups[1].Value;
            __EVENTVALIDATION = new Regex("id=\"__EVENTVALIDATION\" value=\"(.*?)\"").Match(result).Groups[1].Value;
        }

        private static void set_paraList(string EVENTTARGET,string programId,string txtyq,string Txtroom,string x,string y)
        {
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
    }
}
