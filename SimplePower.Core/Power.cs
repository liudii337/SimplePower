using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Core;

namespace SimplePower
{
    public class PowerList: ViewModelBase
    {
        private DateTime time;
        public DateTime Time
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged("Time");
            }
        }

        private float value;
        public float Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        public PowerList(string Time,string Value)
        {
            this.Time = DateTime.Parse(Time);
            this.Value = float.Parse(Value);
        }
    }
    public class Power
    {
        public string region { get; set; }
        public string department_num { get; set; }
        public string domitory_num { get; set; }

        public Power(string region1, string department, string domitory)
        {
            region = region1;
            department_num = department;
            domitory_num = domitory;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;
            Power other = (Power)obj;
            if (region != other.region)
                return false;
            if (department_num != other.department_num)
                return false;
            if (domitory_num != other.domitory_num)
                return false;
            return true;
        }
    }
}
