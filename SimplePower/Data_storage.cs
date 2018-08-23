using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SimplePower
{
    class Data_storage
    {
        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public static void save_power(Power value)
        {
            localSettings.Values["region"] = value.region;
            localSettings.Values["department_num"] = value.department_num;
            localSettings.Values["domitory_num"] = value.domitory_num;
        }

        public static Power read_power()
        {
            var region = read_para("region").ToString();
            var department_num = read_para("department_num").ToString();
            var domitory_num = read_para("domitory_num").ToString();
            var power_info = new Power(region,department_num,domitory_num);
            return power_info;
        }

        public static void save_para(string key,object value)
        {
            localSettings.Values[key] = value;
        }

        public static object read_para(string key)
        {
            if (localSettings.Values.ContainsKey(key))
            {
                return localSettings.Values[key];
            }
            else
            {
                return null;
            }
        }

        public static void remove_para(string key)
        {
            if (localSettings.Values.ContainsKey(key))
            {
                localSettings.Values.Remove(key);
            }
            else
            {
                //
            }
        }
    }
}
