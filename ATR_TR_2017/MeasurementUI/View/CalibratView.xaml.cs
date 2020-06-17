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

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for CalibratView.xaml
    /// </summary>
    public partial class CalibratView : UserControl
    {
        MainCalVM vm = new MainCalVM();
        public CalibratView()
        {
            InitializeComponent();
            grid.DataContext = vm;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TestPlanVM testPlanVM = (new ViewModelLocator()).CurrentTestPlanVm;
            vm.InitialData(testPlanVM);
        }
    }
}
