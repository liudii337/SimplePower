using SimplePower;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace BackgroundTask
{
    public sealed class LiveTileTask : IBackgroundTask
    {
        private ObservableCollection<PowerList> powerLists { get; set; }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            // TODO: 获取数据，更新磁贴逻辑
            if (Data_storage.read_para("setting_save") != null)
            {
                var setting_save = (bool)Data_storage.read_para("setting_save");
                if (setting_save && Data_storage.read_power() != null)
                {
                    Power power_info = Data_storage.read_power();
                    powerLists = new ObservableCollection<PowerList>();
                    await myhttp.GetPower(power_info, powerLists);



                }
            }

            deferral.Complete();
        }
    }
}
