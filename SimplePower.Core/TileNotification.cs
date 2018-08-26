using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace SimplePower.Core
{
    public class TileNotification
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
            var notif = Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication();
            notif.EnableNotificationQueue(true);
            notif.Clear();
        }

        public static void ShowTileNotification(string Title, string Content, string Image)
        {
            string xml = $@"
                    <tile>
                     <visual>
                       <binding template='TileSquareText02'>
                        <text id='1'>{Title}</text>
                        <text id='2'>{Content}</text>
                       </binding>  
 					   <binding template='TileWideText02'>
 					     <text id='1'>{Title}</text>
 					     <text id='2'>{Content}</text>
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

        public static void UpdateTitleNotification(List<CountDown> lcd)
        {

            CleanTileNotification();

            List<CountDown> notiCountDowns = new List<CountDown>();
            for (int i = 0; i < lcd.Count; i++)
            {
                for (int j = i; j < lcd.Count; j++)
                {
                    if (TimeHelper.TotalDays(lcd[i].Time) > TimeHelper.TotalDays(lcd[j].Time))
                    {
                        CountDown temp = lcd[i];
                        lcd[i] = lcd[j];
                        lcd[j] = temp;
                    }
                }
            }

            if (lcd.Count > 5)
            {
                int non0index = -1;
                int non0count = 0;
                for (int i = 0; i < lcd.Count; i++)
                {
                    if (TimeHelper.TotalDays(lcd[i].Time) >= 0)
                    {
                        if (non0index == -1) non0index = i;
                        non0count++;
                    }
                }

                if (non0count < 5)
                {
                    notiCountDowns.Add(lcd[lcd.Count - 5]);
                    notiCountDowns.Add(lcd[lcd.Count - 4]);
                    notiCountDowns.Add(lcd[lcd.Count - 3]);
                    notiCountDowns.Add(lcd[lcd.Count - 2]);
                    notiCountDowns.Add(lcd[lcd.Count - 1]);
                }
                else
                {
                    notiCountDowns.Add(lcd[non0index]);
                    notiCountDowns.Add(lcd[non0index + 1]);
                    notiCountDowns.Add(lcd[non0index + 2]);
                    notiCountDowns.Add(lcd[non0index + 3]);
                    notiCountDowns.Add(lcd[non0index + 4]);
                }

            }
            else
            {
                notiCountDowns = lcd;
            }

            foreach (CountDown cd in notiCountDowns)
            {
                string Title = cd.Title;
                int totalDays = TimeHelper.TotalDays(cd.Time);
                string Content = "";
                if (totalDays < 0)
                {
                    Content = "已经过去了" + (0 - totalDays).ToString() + "天";
                }
                else if (totalDays == 0)
                {
                    Content = "就在今天";
                }
                else if (totalDays > 0)
                {
                    Content = "还差" + totalDays.ToString() + "天";
                }
                ShowTileNotification(Title, Content, cd.Picture);
            }
        }

        public async static Task UpdateTitleNotification()
        {
            LocateCutDownHelper lcdh = new LocateCutDownHelper();
            await lcdh.Read();
            UpdateTitleNotification(lcdh.CountDowns);
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

