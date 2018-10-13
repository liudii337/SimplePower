using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Charting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SimplePower.Core
{
    public class BarTemplateSelector: DataTemplateSelector
    {
        public DataTemplate Normal_RecTemplate { get; set; }
        public DataTemplate Warn_RecTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            PowerList poweritem =(PowerList) ((CategoricalDataPoint)item).DataItem;
            if(poweritem!=null)
            {
                if (poweritem.Value > 20)
                {
                    return Normal_RecTemplate;
                }
                else
                {
                    return Warn_RecTemplate;
                }
            }
            return null;
        }
    }
}
