using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        private ObservableCollection<PowerList> powerLists { get; set; }
        private ObservableCollection<String> region_Lists { get; set; }
        private ObservableCollection<String> department_Lists { get; set; }


        public MainPage()
        {
            this.InitializeComponent();
            powerLists = new ObservableCollection<PowerList>();
            region_Lists = new ObservableCollection<string>();
            department_Lists = new ObservableCollection<string>();

            myhttp.GetList_region(region_Lists);

            Task.Delay(10000);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var region_selection = (string)region_box.SelectedItem;
            var department_selection = (string)department_box.SelectedItem;
            var domitory_selection = donitory_box.Text;


            var power_info = new Power("西区", "西9舍", "230");
            myhttp.GetPower(power_info, powerLists);

        }

        private void region_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = (string)region_box.SelectedItem;
            myhttp.GetList_dormitory(selection, department_Lists);
        }
    }
}
