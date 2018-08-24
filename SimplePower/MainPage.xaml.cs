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
        private Power power_info { get; set; }
        private bool setting_save = false;
        private bool start_mode = false;


        public MainPage()
        {
            this.InitializeComponent();
            powerLists = new ObservableCollection<PowerList>();
            region_Lists = new ObservableCollection<string>();
            department_Lists = new ObservableCollection<string>();
            Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);

        }

        private void App_Suspending(object sender, SuspendingEventArgs e)
        {
            if(setting_save)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var region_selection = (string)region_box.SelectedItem;
            var department_selection = (string)department_box.SelectedItem;
            var domitory_selection = donitory_box.Text;

            power_info = new Power(region_selection, department_selection, domitory_selection);

            myhttp.GetPower(power_info, powerLists);
        }

        private void region_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!start_mode)
            {
                var selection = (string)region_box.SelectedItem;
                myhttp.GetList_dormitory(selection, department_Lists);
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
            myhttp.GetList_region(region_Lists);

            if (Data_storage.read_para("setting_save") != null)
            {
                setting_save = (bool)Data_storage.read_para("setting_save");
                Save_box.IsChecked = setting_save;
                if (setting_save && Data_storage.read_power() != null)
                {
                    power_info = (Power)Data_storage.read_power();
                    myhttp.GetPower(power_info, powerLists);

                    //region_box.SelectedValue = power_info.region;

                    await myhttp.GetList_dormitory(power_info.region, department_Lists);

                    region_box.SelectedIndex = region_box.Items.IndexOf(power_info.region);
                    department_box.SelectedIndex = department_box.Items.IndexOf(power_info.department_num);
                    donitory_box.Text = power_info.domitory_num;
                }
            }
            start_mode = false;
        }
    }
}
