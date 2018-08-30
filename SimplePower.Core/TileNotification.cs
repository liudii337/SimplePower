using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI;
using Windows.UI.Notifications;


namespace SimplePower.Core
{
    public class TileNotificationHelper
    {
        public static void ShowToastNotification(string text, NotificationAudioNames audioName)
        {
            // 1. create element
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            // 2. provide text
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(text));

            // 3. provide image
            //XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
            //((XmlElement)toastImageAttributes[0]).SetAttribute("src", $"ms-appx:///assets/{assetsImageFileName}");
            //((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");

            // 4. duration
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "short");

            // 5. audio
            XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", $"ms-winsoundevent:Notification.{audioName.ToString().Replace("_", ".")}");
            toastNode.AppendChild(audio);

            // 6. app launch parameter
            //((XmlElement)toastNode).SetAttribute("launch", "{\"type\":\"toast\",\"param1\":\"12345\",\"param2\":\"67890\"}");

            // 7. send toast
            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public static void ShowToastNotification(string text, string parameter, NotificationAudioNames audioName)
        {
            // 1. create element
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            // 2. provide text
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(text));

            // 3. provide image
            //XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
            //((XmlElement)toastImageAttributes[0]).SetAttribute("src", $"ms-appx:///assets/{assetsImageFileName}");
            //((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");

            // 4. duration
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "short");

            // 5. audio
            XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", $"ms-winsoundevent:Notification.{audioName.ToString().Replace("_", ".")}");
            toastNode.AppendChild(audio);

            // 6. app launch parameter
            ((XmlElement)toastNode).SetAttribute("launch", parameter);

            // 7. send toast
            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public static void ShowToastNotification(string text, string parameter)
        {
            ShowToastNotification(text, parameter, NotificationAudioNames.Default);
        }

        public static void ShowToastNotification(string text)
        {
            ShowToastNotification(text, NotificationAudioNames.Default);
        }


        public static void CleanTileNotification()
        {
            var notif = TileUpdateManager.CreateTileUpdaterForApplication();
            notif.EnableNotificationQueue(true);
            notif.Clear();
        }

        public static void ShowTileNotification(string Title, string Content, string Image,string updateTime,string add_text)
        {
            string xml = $@"
                    <tile>
                     <visual branding='nameAndLogo'>
                       <binding template='TileMedium' displayName='{updateTime}' branding='name' hint-textStacking='center'>
                        <text hint-align='center' hint-style='caption'>{Title}</text>
                        <text hint-align='center' hint-style='title'>{Content}</text>
                        <text hint-align='center' hint-style='caption'>{add_text}</text>
                       </binding>  
 					   <binding template='TileWideText02'>
 					     <text id='1'>{Title}</text>
 					     <text id='2'><SymbolIcon Symbol='Refresh'/></text>
                        </binding>  
					    <binding template='TileSquare310x310ImageAndTextOverlay02'>
					      <image id='1' src='{Image}' alt='alt text'/>
					      <text id='1'>{Title}</text>
					      <text id='2'>{Content}</text>
					    </binding>  
                     </visual>
                    </tile>
                    ";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            string nowTimeString = DateTime.Now.ToString();

            // Assign date/time values through XmlDocument to avoid any xml escaping issues
            foreach (XmlElement textEl in doc.SelectNodes("//text").OfType<XmlElement>())
                if (textEl.InnerText.Length == 0)
                    textEl.InnerText = nowTimeString;

            TileNotification notification = new TileNotification(doc);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);



        }

        public static void UpdateTitleNotification(Power power_info, ObservableCollection<PowerList> powerLists)
        {

            CleanTileNotification();
            string title="", content="", updateTime="",add_text="";
            string image = "Assets/Refreshicon.png";

            if (powerLists.Count()>0)
            {
                title = string.Format("{0}-{1}", power_info.department_num,power_info.domitory_num);
                content = string.Format("{0}°",powerLists[0].value);
                updateTime= string.Format("📡{0}:{1}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString());
                if(powerLists[0].value>20)
                {
                    add_text = "电量充足";
                }
                else
                {
                    add_text = "尽快充电";
                }
            }

            ShowTileNotification(title, content, image, updateTime,add_text);
        }

        public async static Task UpdateTitleNotification()
        {
            //UpdateTitleNotification(lcdh.CountDowns);
        }

    }

    public enum NotificationAudioNames
    {
        Default,
        IM,
        Mail,
        Reminder,
        SMS,
        Looping_Alarm,
        Looping_Alarm2,
        Looping_Alarm3,
        Looping_Alarm4,
        Looping_Alarm5,
        Looping_Alarm6,
        Looping_Alarm7,
        Looping_Alarm8,
        Looping_Alarm9,
        Looping_Alarm10,
        Looping_Call,
        Looping_Call2,
        Looping_Call3,
        Looping_Call4,
        Looping_Call5,
        Looping_Call6,
        Looping_Call7,
        Looping_Call8,
        Looping_Call9,
        Looping_Call10,
    }
}

