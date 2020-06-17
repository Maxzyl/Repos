using DevExpress.Xpf.Bars;
using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for ChooseResult.xaml
    /// </summary>
    public partial class ChooseResult : UserControl
    {
        ResultDisplayListenerVM vm=new ResultDisplayListenerVM();
        public ChooseResult()
        {
            InitializeComponent();
            LoadResultType();
            //vm = grid.DataContext as ResultDisplayListenerVM;
            grid.DataContext = vm;
        }
        private void LoadResultType()
        {
            List<ResultDisplayType> types = TestStepFactory.ResultDisplaylist;
            foreach(var type in types)
            {
                if (!string.IsNullOrWhiteSpace(type.Att.DisplayName))
                {
                    BarButtonItem subItem = new BarButtonItem() { Content = type.Att.DisplayName };
                    subItem.ItemClick += barButtonAdd_ItemClick;
                    barButtonAdd.Items.Add(subItem);
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            object obj = Interface.DeSerializerResultVM(vm);
            if (obj != null)
            {
                vm.ResultDisplayInfos = (obj as ResultDisplayListenerVM).ResultDisplayInfos;
            }
        }

        private void barButtonDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(resultListBox.SelectedItem !=null)
            {
                vm.DeleteResultDisplayListener(resultListBox.SelectedItem as ResultDisplayInfo);
            }
            Interface.SaveResultVM(vm);
        }

        private void barButtonAdd_ItemClick(object sender,ItemClickEventArgs e)
        {
            vm.AddResultDisplayListener((sender as BarButtonItem).Content.ToString());
            Interface.SaveResultVM(vm);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
           
        }

    }
}
