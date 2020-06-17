using DevExpress.Xpf.Core;
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

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for OthersSetUp.xaml
    /// </summary>
    public partial class DutDescSet : UserControl
    {
        public DutDescSet()
        {
            InitializeComponent();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            saveSet();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            saveSet();
        }  
        private void saveSet()
        {
            GeneTestSetup gt = grid1.DataContext as GeneTestSetup;
            string currFilePath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = currFilePath + "/configfiles/generalSetup.xml";
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
            string result = DataUtils.CommUtils.SerializeData(typeof(GeneTestSetup), gt, fileName);
            if (!"OK".Equals(result))
            {
                DXMessageBox.Show(result.ToString());
            }
        }
    }
}
