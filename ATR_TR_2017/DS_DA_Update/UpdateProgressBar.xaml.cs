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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DS_PA_Update
{
    /// <summary>
    /// Interaction logic for UpdateProgressBar.xaml
    /// </summary>
    public partial class UpdateProgressBar : UserControl
    {
        System.Timers.Timer timer;
        public static int? EditValue = null;
        public static double Maximum = 0;
        public static string Content = null;
        public static string info = null;
        public static string msg = null;

        public UpdateProgressBar()
        {
            InitializeComponent();
            txtInfo.Visibility = System.Windows.Visibility.Collapsed;
            timer = new System.Timers.Timer(100);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((System.Action)(() =>
            {
                this.ProBar.EditValue = EditValue;
                this.ProBar.Maximum = Maximum;
                this.ProBar.Content = Content;
                this.Info.Text = info;
                this.txtInfo.Text = msg;
                this.txtInfo.ScrollToEnd();
                if (EditValue == Maximum)
                {
                    this.Info.Text = "本次共更新" + EditValue + "个文件!";
                    //timer.Stop();
                }
            }));
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (txtInfo.Visibility == System.Windows.Visibility.Visible)
            {
                txtInfo.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                txtInfo.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
