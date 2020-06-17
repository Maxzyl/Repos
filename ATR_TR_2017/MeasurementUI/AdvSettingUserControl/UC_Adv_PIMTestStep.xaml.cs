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
    /// Interaction logic for UC_Adv_PIMTestStep.xaml
    /// </summary>
    public partial class UC_Adv_PIMTestStep : UserControl
    {
        public UC_Adv_PIMTestStep()
        {
            InitializeComponent();
        }

        private void cboCalType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            string strMeasMode = this.cboCalType.SelectedItem.ToString();
            if (strMeasMode == "Point")
            {
                this.spFreq1.Visibility = Visibility.Visible;
                this.spFreq2.Visibility = Visibility.Visible;
                this.spSSFreq1.Visibility = Visibility.Collapsed;
                this.spSSFreq2.Visibility = Visibility.Collapsed;
                this.spTestTime.Visibility = Visibility.Visible;
                this.spTestStep.Visibility = Visibility.Collapsed;
            }
            else if (strMeasMode == "Sweep")
            {
                this.spFreq1.Visibility = Visibility.Collapsed;
                this.spFreq2.Visibility = Visibility.Collapsed;
                this.spSSFreq1.Visibility = Visibility.Visible;
                this.spSSFreq2.Visibility = Visibility.Visible;
                this.spTestTime.Visibility = Visibility.Collapsed;
                this.spTestStep.Visibility = Visibility.Visible;
            }
            else
            {
                this.spFreq1.Visibility = Visibility.Visible;
                this.spFreq2.Visibility = Visibility.Visible;
                this.spSSFreq1.Visibility = Visibility.Collapsed;
                this.spSSFreq2.Visibility = Visibility.Collapsed;
                this.spTestTime.Visibility = Visibility.Visible;
                this.spTestStep.Visibility = Visibility.Collapsed;
            }
        }

        private void FreqEnable_Checked(object sender, RoutedEventArgs e)
        {
            if (this.FreqEnable.IsChecked == true)
            {
                (this.DataContext as ViewModelBaseLib.PIMTestStepVM).setFreqRefresh();
            }
        }
    }
}
