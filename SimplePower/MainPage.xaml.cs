﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SimplePower;
using SimplePower.Core;
using Windows.UI.ViewManagement;
using Windows.Foundation.Metadata;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Xaml.Media.Animation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace SimplePower
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainViewModel MainVM = new MainViewModel();

        private Power power_info { get; set; }
        private float limitation=20;
        private bool setting_save = false;
        private bool start_mode = false;
        private bool tile_enable = false;
        StatusBar statusBar;

        public MainPage()
        {
            this.InitializeComponent();
            SetStatusBar();

            Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);
            this.Mychart.DataContext = MainVM;
        }

        private void App_Suspending(object sender, SuspendingEventArgs e)
        {
            if (!Button.IsEnabled)
                return;
            else if(setting_save && MainVM.Message!= "该宿舍不存在")
            {
                var region_selection = (string)region_box.SelectedItem;
                var department_selection = (string)department_box.SelectedItem;
                var domitory_selection = donitory_box.Text;

                power_info = new Power(region_selection, department_selection, domitory_selection);

                Data_storage.save_power(power_info);
            }
            else
                Data_storage.remove_power();

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var region_selection = (string)region_box.SelectedItem;
            var department_selection = (string)department_box.SelectedItem;
            var domitory_selection = donitory_box.Text;

            var power_info_format = new Power(region_selection, department_selection, domitory_selection);

            if(power_info!=null)
            {
                if (power_info.Equals(power_info_format))
                    return;
            }

            power_info = power_info_format;
            if (region_selection == null && department_selection == null && domitory_selection == "薛楠楠")
                MainVM.Message = "你咋知道我家女神名字？";
            else if(region_selection == null && department_selection == null && domitory_selection == "楠楠")
            {
                MainVM.Message = "这个名字最好听了~";
            }
            else if (region_selection == null && department_selection == null && domitory_selection == "楠宝")
            {
                MainVM.Message = "全称应该是可爱温柔楠楠大宝宝";
            }
            else
            {
                MainVM.Message = "";
                try
                {
                    MainVM.PowerLists= await myhttp.GetPower(power_info);
                }
                catch
                {
                    MainVM.PowerLists.Clear();
                    MainVM.Message = "该宿舍不存在";
                    return;
                }
                if (tile_enable)
                { TileNotificationHelper.UpdateTitleNotification(power_info, MainVM.PowerLists); }
            }

        }

        private async void region_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!start_mode)
            {
                var selection = (string)region_box.SelectedItem;
                MainVM.Department_Lists =await myhttp.GetList_dormitory(selection);
            }
        }

        private void Save_setting_Checked(object sender, RoutedEventArgs e)
        {
            if(!start_mode)
            {
                //保存数据
                setting_save = true;
                Data_storage.save_para("setting_save", true);
            }
        }

        private void Save_setting_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!start_mode)
            {
                //保存数据
                setting_save = false;
                Data_storage.save_para("setting_save", false);
            }
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            start_mode = true;

            if (Data_storage.read_para("setting_save") != null)
            {
                setting_save = (bool)Data_storage.read_para("setting_save");
                Save_box.IsChecked = setting_save;
            }
            if (Data_storage.read_para("tile_enable") != null)
            {
                tile_enable = (bool)Data_storage.read_para("tile_enable");
                Tile_box.IsChecked = tile_enable;
            }

            if (!await MainVM.CheckNetwork())
            {
                Button.IsEnabled = false;
                return;
            }
            MainVM.Region_Lists = await myhttp.GetList_region();

            if (setting_save && Data_storage.read_power() != null)
            {
                power_info = (Power)Data_storage.read_power();
                MainVM.PowerLists = await myhttp.GetPower(power_info);

                if (tile_enable)
                { TileNotificationHelper.UpdateTitleNotification(power_info, MainVM.PowerLists); }
                else
                { TileNotificationHelper.CleanTileNotification(); }
                
                MainVM.Department_Lists= await myhttp.GetList_dormitory(power_info.region);
                region_box.SelectedIndex = region_box.Items.IndexOf(power_info.region);
                department_box.SelectedIndex = department_box.Items.IndexOf(power_info.department_num);
                donitory_box.Text = power_info.domitory_num;
            }
            start_mode = false;
        }

        private void Tile_box_Checked(object sender, RoutedEventArgs e)
        {
            if (!start_mode)
            {
                //保存数据
                tile_enable = true;
                Data_storage.save_para("tile_enable", true);
            }
        }

        private void Tile_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!start_mode)
            {
                //保存数据
                tile_enable = false;
                Data_storage.save_para("tile_enable", false);
                TileNotificationHelper.CleanTileNotification();
            }
        }

        private void SetStatusBar()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.ButtonBackgroundColor = Colors.Transparent;
                    titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.ButtonInactiveForegroundColor = Colors.White;
                    titleBar.BackgroundColor = Colors.Transparent;
                    titleBar.ForegroundColor = Colors.White;

                    coreTitleBar.ExtendViewIntoTitleBar = true;
                }
            }

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                statusBar = StatusBar.GetForCurrentView();
                statusBar.ForegroundColor = Colors.Black;
                statusBar.BackgroundOpacity = 0;

                var applicationView = ApplicationView.GetForCurrentView();
                applicationView.SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            }
        }

        private void rootgrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Storyboard sb = ((Grid)sender).Resources["LoadStory"] as Storyboard;
            sb.Begin();
        }
    }
}
