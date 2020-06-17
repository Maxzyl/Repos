using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ATS_TR
{
    static class Program
    {
        static double Loadprocess = 0;
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                PresentationTraceSources.Refresh();
                PresentationTraceSources.DataBindingSource.Listeners.Add(new DefaultTraceListener());
                
                PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning | SourceLevels.Error;

                ThemeManager.ApplicationThemeName = Theme.VS2010Name;
               // bool bdownload = DataUtils.ActivityUtils.DownLoadActivity(Loadprocess);
                DXSplashScreen.Show<SplashScreen_AMTS>();
                //DXSplashScreen.Progress(10);
                DXSplashScreen.SetState("Start Loding...");
                DataUtils.StaticInfo.ExecutePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                DataUtils.StaticInfo.LoginUser = "未登陆";
                DataUtils.StaticInfo.LocalModel = ConfigurationManager.AppSettings["LocalModel"] == null ? "" : ConfigurationManager.AppSettings["LocalModel"].ToUpper();
                DataUtils.StaticInfo.AutoConnect = ConfigurationManager.AppSettings["AutoConnect"] == null ? "" : ConfigurationManager.AppSettings["AutoConnect"].ToUpper();
                DataUtils.StaticInfo.ProcessName = ConfigurationManager.AppSettings["ProcessName"] == null ? "" : ConfigurationManager.AppSettings["ProcessName"].ToUpper();
                DataUtils.StaticInfo.AutoLoadStateBySN = ConfigurationManager.AppSettings["AutoLoadStateBySN"] == null ? "" : ConfigurationManager.AppSettings["AutoLoadStateBySN"].ToUpper();
                DataUtils.StaticInfo.AutoLoadStateFile = ConfigurationManager.AppSettings["AutoLoadStateFile"] == null ? "" : ConfigurationManager.AppSettings["AutoLoadStateFile"].ToUpper();
                DataUtils.StaticInfo.ATEKind = ConfigurationManager.AppSettings["ATEKind"] == null ? "" : ConfigurationManager.AppSettings["ATEKind"].ToUpper();
                DataUtils.StaticInfo.ATEStatusFile = ConfigurationManager.AppSettings["ATEStatusFile"] == null ? "" : ConfigurationManager.AppSettings["ATEStatusFile"].ToUpper();
                DataUtils.StaticInfo.MesMode = ConfigurationManager.AppSettings["MesMode"] == null ? "" : ConfigurationManager.AppSettings["MesMode"].ToUpper();
                DataUtils.StaticInfo.ResultString = ConfigurationManager.AppSettings["ResultListerner"] == null ? "" : ConfigurationManager.AppSettings["ResultListerner"].ToUpper().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "").Trim();
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ApplicationUtils.CurrentDomain_AssemblyResolve);
                DXSplashScreen.SetState("Loading The ApplicationTheme");
                DXSplashScreen.SetState("Check File Revisions...");
                DXSplashScreen.Progress(Loadprocess = 5);
                DXSplashScreen.SetState("Loading Application Instance");

                //string url = System.Reflection.Assembly.GetExecutingAssembly().Location + ";component/App.xaml";
                //string uri = "";
                //string param = "";
                //if (GetAssemblyInstanse(url, ref uri, ref param) == null) { throw new Exception("GetAssemblyInstanse is NULL!"); };
                //(System.Windows.Application)System.Windows.Application.LoadComponent(new System.Uri(uri, System.UriKind.Relative));
                System.Windows.Application app = new System.Windows.Application();
                app.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(app_DispatcherUnhandledException);
                DXSplashScreen.SetState("Loading Main FrameWork");
                DXSplashScreen.Progress(Loadprocess = 70);
                System.Windows.Window MainWindow = new MainWindow();
                DXSplashScreen.SetState("Run MainWindow");
                DXSplashScreen.Progress(Loadprocess = 90);
                app.Run(MainWindow);
            }
            catch (Exception ex)
            {
                LOG.WriteLog(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source);
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                ApplicationUtils.SaveException(ex);
                System.Windows.MessageBox.Show(ex.Message);
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
              
            }
        }

        static void app_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            Exception ex = e.Exception;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string str in ex.StackTrace.Split(new char[] { '\n' }))
            {
                if (!str.Contains("at System.")) sb.Append(str);
            }
            ApplicationUtils.SaveException(ex);
            DXMessageBox.Show(ex.Message + Environment.NewLine + sb.ToString(), "系统异常", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            e.Handled = true;
        }
        public static Assembly GetAssemblyInstanse(string url, ref string uri, ref string param)
        {
            string[] urls = url.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (urls.Count() == 0) return null;
            System.Reflection.Assembly MyAssembly = System.Reflection.Assembly.LoadFrom(urls[0]);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(urls[0]);
            if (urls.Count() > 1)
            {
                urls[1] = "/" + fileName + ";" + urls[1];
                int index = urls[1].IndexOf('?');
                uri = index > -1 ? urls[1].Substring(0, index) : urls[1];
                param = index > -1 ? urls[1].Substring(index + 1) : "";
            }
            return MyAssembly;
        }

        private static void DownLoadActivityNoticeStatus(string message, double process, double imin = 0, double imax = 100)
        {
            Loadprocess = process;
            //DXSplashScreen.SetState("Check File: " + message);
            DXSplashScreen.Progress(process, imax);
        }

    }
}
