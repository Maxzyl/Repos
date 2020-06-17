using DevExpress.Xpf.Core;
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

namespace ATS_TR
{
    /// <summary>
    /// Interaction logic for SplashScreen_AMTS.xaml
    /// </summary>
    public partial class SplashScreen_AMTS : UserControl
    {

        string LastState = "";
        System.Timers.Timer timer;
        public SplashScreen_AMTS()
        {
            InitializeComponent();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            tbRevision.Text = "Revision " + Convert.ToString(fvi.ProductMinorPart);
            tbProduct.Text = Convert.ToString(fvi.ProductName);
            Footer_Text.Text = Convert.ToString(fvi.LegalCopyright);
            timer = new System.Timers.Timer(100);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)delegate
            {
                if (DXSplashScreen.IsActive)
                {
                    if (LastState != "")
                    {
                        LastState = "";
                    }
                    else
                    {
                        Info_TargetUpdated(null, null);
                    }
                }
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        private void Info_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            textBlock9.Text = textBlock8.Text;
            textBlock8.Text = textBlock7.Text;
            textBlock7.Text = textBlock6.Text;
            textBlock6.Text = textBlock5.Text;
            textBlock5.Text = textBlock4.Text;
            textBlock4.Text = textBlock3.Text;
            textBlock3.Text = textBlock2.Text;
            textBlock2.Text = textBlock1.Text;
            LastState = Info.Text;
        }
    }
}
