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
using System.Data;
using System.Data.SqlClient;

namespace ModelBaseLib
{
    /// <summary>
    /// Interaction logic for ManualLoopWindow.xaml
    /// </summary>
    public partial class ManualLoopWindow : Window
    {
        public delegate void delegatePassIndex(int index);
        public delegatePassIndex PassIndex;
        public ManualLoopWindow(DataTable dt,int index)
        {
            InitializeComponent();
            this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
            gridControl.ItemsSource = dt;
            if(dt.Rows.Count>index)
            {
                gridControl.Focus();
                tableView.FocusedRowHandle = index;
             
            }
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            int index = tableView.FocusedRowHandle;
            if(PassIndex !=null)
            {
                PassIndex.Invoke(index);
            }
            this.Close();
        } 

        private void skipButton_Click(object sender, RoutedEventArgs e)
        {
            int index = gridControl.VisibleRowCount;
            if(PassIndex !=null)
            {
                PassIndex.Invoke(index);
            }
            this.Close();
        }
    }
}
