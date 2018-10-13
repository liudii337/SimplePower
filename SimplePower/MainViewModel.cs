using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Core;

namespace SimplePower
{
    class MainViewModel : ViewModelBase
    {
        private ObservableCollection<PowerList> powerLists;
        public ObservableCollection<PowerList> PowerLists
        {
            get { return powerLists; }
            set
            {
                if (this.powerLists != value)
                {
                    powerLists = value;
                    this.OnPropertyChanged("PowerLists");
                }
            }
        }

        private ObservableCollection<String> region_Lists;
        public ObservableCollection<String> Region_Lists
        {
            get { return region_Lists; }
            set
            {
                if (this.region_Lists != value)
                {
                    region_Lists = value;
                    this.OnPropertyChanged("Region_Lists");
                }
            }
        }

        private ObservableCollection<String> department_Lists;
        public ObservableCollection<String> Department_Lists
        {
            get { return department_Lists; }
            set
            {
                if (this.department_Lists != value)
                {
                    department_Lists = value;
                    this.OnPropertyChanged("Department_Lists");
                }
            }
        }

    }
}
