using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using ModelBaseLib;
using System.Windows.Markup;
using ViewModelBaseLib;
namespace MeasurementUI
{
    public class TreeViewDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            DataTemplate template = null;
            var obj = item as ViewModelBaseLib.TreeNodeVM;
            var fe = container as FrameworkElement;
            if(fe !=null)
            {
                if (obj.Type == TreeNodeTypeEnum.ManualConnection)
                {
                    template = fe.FindResource("mcTemplate") as DataTemplate;
                }
                else if(obj.Type==TreeNodeTypeEnum.ParentTestStep)
                {
                    template = fe.FindResource("loopTemplate") as DataTemplate;
                }
                else if(obj.Type==TreeNodeTypeEnum.TestStep)
                {
                    template = fe.FindResource("stepTemplate") as DataTemplate;
                }
                else if(obj.Type==TreeNodeTypeEnum.TestTrace)
                {
                    template = fe.FindResource("traceTemplate") as DataTemplate;
                }
                else if (obj.Type == TreeNodeTypeEnum.PointTestItem)
                {
                    template = fe.FindResource("pointTemplate") as DataTemplate;
                }
                else if (obj.Type == TreeNodeTypeEnum.BoolTestItem)
                {
                    template = fe.FindResource("boolTemplate") as DataTemplate;
                }
                else if(obj.Type==TreeNodeTypeEnum.TestMarker)
                {
                    template = fe.FindResource("markerTemplate") as DataTemplate;
                }
                else if(obj.Type ==TreeNodeTypeEnum.TRTestItem)
                {
                    template = fe.FindResource("trTemplate") as DataTemplate;
                }
            }
            return template;
        }
    }
}
