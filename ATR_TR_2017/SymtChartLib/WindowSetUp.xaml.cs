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
    /// Interaction logic for WindowSetUp.xaml
    /// </summary>
    public partial class WindowSetUp : Window
    {
        public WindowSetUp()
        {
            InitializeComponent();
            this.DataContext = GeneTestSetup.Instance;
            //txtDatadigital.Text = GeneTestSetup.Instance.DataDisplayDigits.ToString();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           // GeneTestSetup.Instance.DataDisplayDigits = Convert.ToInt32(txtDatadigital.Text);
        }
    }
}
