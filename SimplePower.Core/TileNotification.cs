using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.StartScreen;

namespace SimplePower.Core
{
    public class TileNotificationHelper
    {
        public static void ShowToastNotification(string title, string content)
        {
            string xml = $@"
                    <toast scenario='reminder'>
                        <visual>
                            <binding template = 'ToastGeneric'>        
                                <text hint-maxLines='1'>{title}</text>
                                <text>{content}</text>
                                <image placement='appLogoOverride' src='Assets/icon-battery.png'/>
                                <image placement='hero' src='Assets/position.png'/>
                            </binding>
                        </visual>
                    </toast>
                    ";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            ToastNotification toast = new ToastNotification(doc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);


        }

        public static void UpdateToastNotification(ObservableCollection<PowerList> powerLists,float limitation)
        {
            string content = "请尽快充电";
            string title = "";

            if (powerLists.Count!=0)
            {
                if(powerLists[0].Value<limitation)
                {
                    title = string.Format("寝室电量仅剩{0}度", powerLists[0].Value.ToString());
                    ShowToastNotification(title,content);
                }
            }
        }


        public static void CleanTileNotification()
        {
            var notif = TileUpdateManager.CreateTileUpdaterForApplication();
            notif.EnableNotificationQueue(true);
            notif.Clear();
        }

        public static void ShowTileNotification(string Title, string Content, string Image,string updateTime,string add_text,string lastday)
        {
            string xml = $@"
                    <tile>
                     <visual branding='nameAndLogo'>
                       <binding template='TileMedium' displayName='{updateTime}' branding='name' hint-textStacking='center'>
                        <text hint-align='center' hint-style='caption'>{Title}</text>
                        <text hint-align='center' hint-style='title'>{Content}</text>
                        <text hint-align='center' hint-style='caption'>{add_text}</text>
                       </binding>  
 					   <binding template='TileWide' displayName='{updateTime} {Title}' branding='name' hint-textStacking='center'>
 					     <group>
                            <subgroup hint-weight='1'>
                                <text hint-align='center' hint-style='caption'>电量剩余</text>
                                <text hint-align='center' hint-style='title'>{Content}</text>
                            </subgroup>
                            <subgroup hint-weight='1'>
                                <text hint-align='center' hint-style='caption'>昨日消耗</text>
                                <text hint-align='center' hint-style='title'>{lastday}</text>
                            </subgroup>
                         </group>
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
            string title="", content="", updateTime="",add_text="",lastday="";
            string image = "Assets/Refreshicon.png";

            if (powerLists.Count()>0)
            {
                title = string.Format("{0}-{1}", power_info.department_num,power_info.domitory_num);
                content = string.Format("{0}°",powerLists[0].Value);
                updateTime= string.Format("📡{0}", DateTime.Now.ToString("HH:mm"));
                lastday = string.Format("{0}°", (powerLists[0].Value - powerLists[1].Value).ToString("f1"));
                if(powerLists[0].Value>20)
                {
                    add_text = "电量充足";
                }
                else
                {
                    add_text = "尽快充电";
                }
            }

            ShowTileNotification(title, content, image, updateTime,add_text,lastday);
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

