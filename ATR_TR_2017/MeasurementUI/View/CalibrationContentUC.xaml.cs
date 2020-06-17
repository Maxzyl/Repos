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
using ModelBaseLib;
using ViewModelBaseLib;
namespace MeasurementUI.View
{
    /// <summary>
    /// Interaction logic for CalibrationContentUC.xaml
    /// </summary>
    //[UICalibrationPara("Last")]
    public partial class CalibrationContentUC : UserControl
    {
        public CalibrationContentUC()
        {
            InitializeComponent();
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CalibrationVM vm = DataContext as CalibrationVM;
            vm.InitAllCal();
            vm.UpdateGuideMsglist();
            
        }
    }
}
