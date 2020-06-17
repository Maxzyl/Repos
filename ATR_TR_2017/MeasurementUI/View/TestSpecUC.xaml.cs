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

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for TestSpecUC.xaml
    /// </summary>
    public partial class TestSpecUC : UserControl
    {
        TestPlanVM vm;
        public TestSpecUC(TestPlanVM vm)
        {
            InitializeComponent();
            this.vm = vm;
            this.DataContext = vm;
        }

        private void btnAddSpec_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(txtSpec.Text))
            {
                vm.AddTestSpecs(txtSpec.Text);
            }
        }

        private void btnDelSpec_Click(object sender, RoutedEventArgs e)
        {
            if(listBox.SelectedItem !=null)
            {
                TotalSpecVM spec = listBox.SelectedItem as TotalSpecVM;
                vm.DeletSpecs(spec);
            }
        }

        private void btnAddEnvionm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelEnvionm_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
