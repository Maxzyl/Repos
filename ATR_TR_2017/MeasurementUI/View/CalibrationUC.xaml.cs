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
using ModelBaseLib;
namespace MeasurementUI.View
{
    /// <summary>
    /// Interaction logic for CalibrationUC.xaml
    /// </summary>
    public partial class CalibrationUC : UserControl
    {
        CalibrationVM vm = new CalibrationVM();
        public CalibrationUC()
        {
            InitializeComponent();
            //if (TestStepFactory.CalibUClist.Where(x => x.GetType() == typeof(CalTestStepSelection)).Count() < 1)
            //{
            //    TestStepFactory.CalibUClist.Add(new CalTestStepSelection());
            //}
            if (TestStepFactory.CalibUClist.Where(x => x.GetType() == typeof(CalibrationContentUC)).Count() < 1)
            {
                TestStepFactory.CalibUClist.Add(new CalibrationContentUC());
            }
            this.DataContext = vm;
            
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPreStep_Click(object sender, RoutedEventArgs e)
        {
            vm.Prev();
        }

        private void btnNextStep_Click(object sender, RoutedEventArgs e)
        {
            vm.Next();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            vm.Confirm();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ModelBaseLib.TestPlanManager.CurrentTestPlan != null)
            {
                ModelBaseLib.TestPlanManager.CurrentTestPlan.BeginCal();
            }
        }
    }
}
