using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePower
{
    class PowerList
    {
        public DateTime time;
        public float value;
        public PowerList(string Time,string Value)
        {
            time = DateTime.Parse(Time);
            value = float.Parse(Value);
        }
    }
    class Power
    {
        public string region { get; set; }
        public string department_num { get; set; }
        public string domitory_num { get; set; }
        public List<PowerList> powerLists { get; set; }

        public Power(string region1,string department,string domitory)
        {
            region = region1;
            department_num = department;
            domitory_num = domitory;
            powerLists = new List<PowerList>();
        }
    }
}
