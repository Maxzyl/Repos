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
using System.Data;
using System.Data.SqlClient;

namespace TestResultMarkerDip
{
    /// <summary>
    /// Interaction logic for DetailResultData.xaml
    /// </summary>
    public partial class DetailResultData : UserControl
    {
        public DetailResultData(DataTable dt)
        {
            InitializeComponent();
            detailGridControl.ItemsSource = dt;
            detailGridControl.Columns["PassFail"].Visible = false;
        }
    }
}
