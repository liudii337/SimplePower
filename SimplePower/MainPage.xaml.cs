using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace SimplePower
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public void Button1_Click(object sender, EventArgs e)
        {
            //   string url = "http://localhost:49857/test/WebForm6.aspx";
            string url = "http://localhost:1876/Workflow/MTStart.aspx?processID=359&ApplicantUserId=A150017";

            Request request = new Request();
            request.Method = Method.POST;
            #region  webForm6
            //request.Add("ctl00$ContentPlaceHolder1$TextBox1", "name111");
            //request.Add("ctl00$ContentPlaceHolder1$TextBox2", "name222");
            //request.Add("ctl00$ContentPlaceHolder1$TextBox3", "name3333");
            //request.Add("ctl00$ContentPlaceHolder1$Button1", "Butt11o22n");
            #endregion

            HttpRRHelper html = new HttpRRHelper();
            Response response = html.GetHTML(url, new WebProxy("172.27.1.250", 80));
            Match math = Regex.Match(response.Html, "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(?<val>.*?)\" />", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            string value = math.Groups["val"].Value;

            request.Add("__VIEWSTATE", System.Web.HttpUtility.UrlEncode(value));
            math = Regex.Match(response.Html, "<input type=\"hidden\" name=\"__EVENTVALIDATION\" id=\"__EVENTVALIDATION\" value=\"(?<val>.*?)\" />", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            request.Add("__EVENTVALIDATION", System.Web.HttpUtility.UrlEncode(math.Groups["val"].Value));
            request.Add("ctl00$contentPlace$btnSave", "保存草稿");//为按钮的name名字
                                                              //  response = html.GetHTML(url, true, request, new WebProxy("172.27.1.250",80));
                                                              // OAS.Common.HttpRRHelper.Init().BeginGetHTML(url, true, request);

            response = html.GetHTML(url, true, request, new WebProxy("172.27.1.250", 80));
        }
    }
}
