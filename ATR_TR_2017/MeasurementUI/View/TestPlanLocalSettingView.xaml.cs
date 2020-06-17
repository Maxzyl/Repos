using DevExpress.Xpf.Core;
using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModelBaseLib;

namespace MeasurementUI.View
{
    /// <summary>
    /// Interaction logic for TestPlanLocalSettingView.xaml
    /// </summary>
    public partial class TestPlanLocalSettingView : UserControl
    {
        public TestPlanLocalSettingView()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as TestPlanLocalSettingVM).SaveAllSettings();
        }

        private void ViewUnloaded(object sender, RoutedEventArgs e)
        {
            if (TestPlanManager.CurrentTestPlan == null) return;
            var dialogResult = DXMessageBox.Show("是否保存校准？", "提示", MessageBoxButton.OKCancel);
            if(dialogResult == MessageBoxResult.OK)
            {
                (DataContext as TestPlanLocalSettingVM).SaveAllSettings();
            }
        }

        private void viewLoaded(object sender, RoutedEventArgs e)
        {
            (DataContext as TestPlanLocalSettingVM).OnViewLoad();
        }

        
    }

    public class LocalSettingViewTemplateSelector:DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = new DataTemplate();
            return null;
        }
    }
}
