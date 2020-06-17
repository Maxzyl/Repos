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

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for UC_TestStep.xaml
    /// </summary>
    public partial class UC_TestStep : UserControl
    {
        public UC_TestStep()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tableView.BestFitArea = DevExpress.Xpf.Grid.BestFitArea.Header;
            //tableView.BestFitColumns();
        }
    }
}
