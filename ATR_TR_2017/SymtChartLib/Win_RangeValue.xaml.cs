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
using System.Windows.Shapes;

namespace SymtChartLib
{
    /// <summary>
    /// Interaction logic for Win_RangeValue.xaml
    /// </summary>
    public partial class Win_RangeValue : Window
    {
        public List<double> XValuelist = new List<double>();
        public double[] XValue;
        public double[] YValue;
        public Win_RangeValue()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbStart.ItemsSource = XValuelist;
            cmbStop.ItemsSource = XValuelist;
        }

        private void cmbStart_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            CalculateValue();
        }

        private void cmbStop_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            CalculateValue();
        }
        private void CalculateValue()
        {
            double avgValue;
            int startIndex = 0;
            int stopIndex = 0;
            double count = 0;
            double maxTemp = 0;
            double minTemp = 0;
            if (cmbStart.EditValue != null && cmbStop.EditValue != null && (Convert.ToDouble(cmbStart.EditValue) < Convert.ToDouble(cmbStop.EditValue)))
            {
                for (int i = 0; i <= XValue.Length - 1; i++)
                {
                    if (XValue[i] == Convert.ToDouble(cmbStart.EditValue))
                    {
                        startIndex = i;
                    }
                    else if (XValue[i] == Convert.ToDouble(cmbStop.EditValue))
                    {
                        stopIndex = i;
                    }
                }
                maxTemp = YValue[startIndex];
                minTemp = YValue[startIndex];
                for (int j = startIndex; j <= stopIndex; j++)
                {
                    count += YValue[j];
                    if (maxTemp <= YValue[j])
                    {
                        maxTemp = YValue[j];
                    }
                    if (minTemp >= YValue[j])
                    {
                        minTemp = YValue[j];
                    }
                }
                avgValue = count / (stopIndex - startIndex + 1);
                txtMax.Text = maxTemp.ToString();
                txtMin.Text = minTemp.ToString();
                txtAvg.Text = avgValue.ToString();
            }
            else
            {
                txtAvg.Text = "";
                txtMax.Text = "";
                txtMin.Text = "";
            }
        }
    }
}
