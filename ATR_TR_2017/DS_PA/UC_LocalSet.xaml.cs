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
using System.Windows.Shapes;
using ViewModelBaseLib;

namespace ATS_TR
{
    /// <summary>
    /// Interaction logic for UC_LocalSet.xaml
    /// </summary>
    public partial class UC_LocalSet : Window
    {
        public UC_LocalSet(bool isLocal)
        {
            InitializeComponent();
            this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
            check.IsChecked = isLocal;
           
        }
        public Action Update;
        private void check_Click(object sender, RoutedEventArgs e)
        {
            if (check.IsChecked == true)
            {   
                (new ViewModelLocator()).MainWindow.StatusInfo.IsLocal = true;
                GeneTestSetup.Instance.IsLocal = true;
            }
            else
            {
                (new ViewModelLocator()).MainWindow.StatusInfo.IsLocal = false;
                GeneTestSetup.Instance.IsLocal = false;
                if(Update !=null)
                {
                    Update.Invoke();
                }
            }
        }
    }
}
