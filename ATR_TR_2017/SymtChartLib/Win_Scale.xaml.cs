using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using DevExpress.Xpf.Core;

namespace SymtChartLib
{
    /// <summary>
    /// Interaction logic for Win_Scale.xaml
    /// </summary>
    public partial class Win_Scale : Window
    {
        public delegate void TransferValue(int scale, double max, double min);
        public event TransferValue TransferEvent;
        public Win_Scale()
        {
            InitializeComponent();
        }

        public double? ToNullDouble(string str)
        {

            if (str == null)
            {
                return null;
            }
            else
            {
                double d;
                if (double.TryParse(str, out d))
                {
                    return (double?)d;
                }
                else
                    return null;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ToNullDouble(txtMax.Text) == null || ToNullDouble(txtMin.Text) == null)
            {
                DXMessageBox.Show("请输入正确的字符");
            }
            TransferEvent(Convert.ToInt32(txtDiv.Text), Convert.ToDouble(txtMax.Text), Convert.ToDouble(txtMin.Text));
            this.Close();
        }
    }
}
