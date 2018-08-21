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

        public MainPage()
        {
            this.InitializeComponent();
            powerLists = new ObservableCollection<PowerList>();
            var power_info = new Power("西区", "西9舍", "230");
            myhttp.GetPower(power_info,powerLists);
            Task.Delay(10000);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            result_text.Text = powerLists[0].value.ToString();
        }
    }
}
