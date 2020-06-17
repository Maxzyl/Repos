using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using Microsoft.Win32;
using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViewModelBaseLib;

using DevExpress.Xpf.Bars;

namespace ATS_TR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DXWindow
    {
        ViewModelLocator vm = new ViewModelLocator();
        string strLayOutFile = "layout.xml";
        bool ResetUI = false;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = vm.MainWindow;
        }

        private int getMilliseconds()
        {
            try
            {
                object dnow = (DataUtils.DB.GetDataSetFromSQL("select convert(varchar, getdate(),120) as dnow")).Tables[0].Rows[0]["dnow"];
                TimeSpan span = (TimeSpan)(Convert.ToDateTime(dnow) - System.DateTime.Now);
                return span.Milliseconds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(strLayOutFile))
            {
                try
                {
                    File.Delete(strLayOutFile);
                }
                catch (Exception ex)
                {
                    ApplicationUtils.SaveException(ex);
                }
            }
            if (!ResetUI)
            {
                dockManager.SaveLayoutToXml(strLayOutFile);
            }

            DataUtils.StaticInfo.intMilliseconds = getMilliseconds();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            document1.Closed = true;
            if (System.IO.File.Exists(strLayOutFile))
            {
                dockManager.RestoreLayoutFromXml(strLayOutFile);
            }
            UserLogin login = new UserLogin();
            login.Owner = this;
            if (!login.ShowDialog().Value)
            {
                this.Close();
                return;
            }
            login.Close();
            (new ViewModelLocator()).MainWindow.StatusInfo.UserInfo = login.txtUserID.Text + "(" + ((login.IsAdminUser == true) ? "Admin" : "User") + ")";
            (new ViewModelLocator()).MainWindow.StatusInfo.Process = login.Process;
            (new ViewModelLocator()).MainWindow.StatusInfo.Terminal = login.Terminal;
            (new ViewModelLocator()).MainWindow.StatusInfo.Token = login.Token;
            if (login.IsAdminUser == true)
            {
                (new ViewModelLocator()).MainWindow.StatusInfo.IsAdmin = true;
            }
            else
            {
                (new ViewModelLocator()).MainWindow.StatusInfo.IsAdmin = false;
            }
            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
          
            if((new ViewModelLocator()).MainWindow.StatusInfo.IsLocal)
            {
                ActiveItem(string.Format(@"\MeasurementUI.dll;component/View/ProjectMgr_Local.xaml"), "状态文件", "文件");
            }
            else
            {
                if (!string.IsNullOrEmpty(DataUtils.StaticInfo.AutoLoadStateBySN) && DataUtils.StaticInfo.AutoLoadStateBySN.ToLower() == "true")
                {
                    ActiveItem(string.Format(@"\MeasurementUI.dll;component/DataDisplay/ResultDisplay.xaml"), "测试结果", "测试");
                    documentPanelIsVisible("测试");
                }
                else
                {
                    if (LoadFile())
                    {
                        ActiveItem(string.Format(@"\MeasurementUI.dll;component/DataDisplay/ResultDisplay.xaml"), "测试结果", "测试");
                        documentPanelIsVisible("测试");
                    }
                    else
                    {
                        ActiveItem(string.Format(@"\MeasurementUI.dll;component/View/ProjectMgr_Old.xaml"), "状态文件", "文件");
                    }
                }
            }
            LoadExtensionUi();
            //if (dtimer == null)
            //{
            //    dtimer = new System.Windows.Threading.DispatcherTimer();
            //    dtimer.Interval = TimeSpan.FromSeconds(0.1);
            //    dtimer.Tick += dtimer_Tick;
            //    dtimer.Start();
            //}
        }
        //System.Windows.Threading.DispatcherTimer dtimer;
        //void dtimer_Tick(object sender, EventArgs e)
        //{
        //    if (TestStepFactory.blockbuff.Count > 0)
        //    {
        //        Blockinfo tt=null;
        //        if (TestStepFactory.blockbuff.TryTake(out tt))
        //        {
        //            Chart_UC chart = new Chart_UC(tt.Step, tt.SpecIndex,tt.Size);
        //            chart.ShowChart(tt.Step);
        //            tt.Step.bImage = chart.saveChart();
        //            System.IO.MemoryStream ms = new System.IO.MemoryStream(tt.Step.bImage);
        //            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
        //            img.Save("./aa.png");
        //        }
        //    }
        //}
        private void barButtonLogin_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            UserLogin login = new UserLogin();
            login.Owner = this;
            if (!login.ShowDialog().Value)
            {
                // this.Close();
            }
            (new ViewModelLocator()).MainWindow.StatusInfo.UserInfo = login.txtUserID.Text + "(" + ((login.IsAdminUser == true) ? "Admin" : "User") + ")";
            (new ViewModelLocator()).MainWindow.StatusInfo.Process = login.Process;
            (new ViewModelLocator()).MainWindow.StatusInfo.Terminal = login.Terminal;
            if (login.IsAdminUser == true)
            {
                (new ViewModelLocator()).MainWindow.StatusInfo.IsAdmin = true;
            }
            else
            {
                (new ViewModelLocator()).MainWindow.StatusInfo.IsAdmin = false;
            }
            login.Close();
        }

        private void barButtonItem_InstruPanel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            documentPanelIsVisible("连接");
        }
        private void barButtonConnect_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ActiveItem(string.Format(@"\MeasurementUI.dll;component/View/InstruMgrNewView.xaml"), "仪表管理", "连接");
        }

        private void barButtonAddTestStep_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ActiveItem(string.Format(@"\MeasurementUI.dll;component/View/DynamicAddStep.xaml"), "添加测试步骤", "连接");
        }
        private void barButtonItem_Project_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ActiveItem(string.Format(@"\MeasurementUI.dll;component/View/ProjectMgr_Old.xaml"), "状态文件", "文件");
            documentPanelIsVisible("文件");
            //CloseDockItem("校准","本地校准");
            
        }
        private void ActiveItem2(string sitem,string DespName="",string tag="")
        {
            bool isFindfrm = false;
            DocumentGroup dgp = null;
            foreach (DevExpress.Xpf.Docking.BaseLayoutItem bli in dockManager.LayoutRoot.Items)
            {
                if (bli.GetType() == typeof(DevExpress.Xpf.Docking.DocumentGroup))
                {
                    dgp = (DevExpress.Xpf.Docking.DocumentGroup)bli;
                    break;
                }
            }
            if (dgp == null)
            {
                dgp = new DevExpress.Xpf.Docking.DocumentGroup();
                dgp.ClosePageButtonShowMode = DevExpress.Xpf.Docking.ClosePageButtonShowMode.InAllTabPageHeaders;
                dockManager.LayoutRoot.Items.Add(dgp);
            }
            foreach (DevExpress.Xpf.Docking.DocumentPanel tempdp in dgp.Items)
            {
                if (Convert.ToString(tempdp.Caption) == Convert.ToString(DespName == "" ? sitem : DespName))
                {
                    isFindfrm = true;
                    tempdp.IsActive = true;
                    tempdp.AllowFloat = false;
                }
            }
            if (!isFindfrm)
            {
                DevExpress.Xpf.Docking.DocumentPanel dp = new DevExpress.Xpf.Docking.DocumentPanel();
                string[] strs = sitem.Split(';');
                Assembly assembly = Assembly.LoadFile(DataUtils.StaticInfo.ExecutePath + "\\" + strs[0]);
                Type type = assembly.GetType(strs[1]);
                UserControl uc = Activator.CreateInstance(type) as UserControl;
                dp.Content = uc;
                dp.Caption = DespName == "" ? sitem : DespName;
                dp.Tag = tag;
                dgp.Items.Add(dp);
                dp.IsActive = true;
                dp.AllowFloat = false;
                if(dp.Caption.ToString()=="状态文件")
                {
                    dp.ShowCloseButton=false;
                }

            }

        }
        private void ActiveItem(string sitem, string DespName = "", string tag = "")
        {
            bool isFindfrm = false;
            DocumentGroup dgp = null;
            foreach (DevExpress.Xpf.Docking.BaseLayoutItem bli in dockManager.LayoutRoot.Items)
            {
                if (bli.GetType() == typeof(DevExpress.Xpf.Docking.DocumentGroup))
                {
                    dgp = (DevExpress.Xpf.Docking.DocumentGroup)bli;
                    break;
                }
            }
            if (dgp == null)
            {
                dgp = new DevExpress.Xpf.Docking.DocumentGroup();
                dgp.ClosePageButtonShowMode = DevExpress.Xpf.Docking.ClosePageButtonShowMode.InAllTabPageHeaders;
                dockManager.LayoutRoot.Items.Add(dgp);
            }
            foreach (DevExpress.Xpf.Docking.DocumentPanel tempdp in dgp.Items)
            {
                if (Convert.ToString(tempdp.Caption) == Convert.ToString(DespName == "" ? sitem : DespName))
                {
                    isFindfrm = true;
                    tempdp.IsActive = true;
                    tempdp.AllowFloat = false;
                }
            }
            if (!isFindfrm)
            {
                DevExpress.Xpf.Docking.DocumentPanel dp = new DevExpress.Xpf.Docking.DocumentPanel();
                System.Windows.Controls.UserControl uc = DataUtils.CommUtils.GetUserControlFromUrl(DataUtils.StaticInfo.ExecutePath + sitem);
                dp.Content = uc;            
                dp.Caption = DespName == "" ? sitem : DespName;
                dp.Tag = tag; 
                dgp.Items.Add(dp);
                dp.IsActive = true;
                dp.AllowFloat = false;
                if(dp.Caption.ToString()=="状态文件")
                {
                    dp.ShowCloseButton = false;
                }
               
            }
        }

        private void documentPanelIsVisible(string tag)
        {
            DocumentGroup dgp = null;
            foreach (DevExpress.Xpf.Docking.BaseLayoutItem bli in dockManager.LayoutRoot.Items)
            {
                if (bli.GetType() == typeof(DevExpress.Xpf.Docking.DocumentGroup))
                {
                    dgp = (DevExpress.Xpf.Docking.DocumentGroup)bli;
                    foreach (DevExpress.Xpf.Docking.DocumentPanel tempdp in dgp.Items)
                    {
                        if (tempdp.Tag == tag)
                        {
                            tempdp.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            tempdp.Visibility = Visibility.Collapsed;
                        }
                        tempdp.AllowFloat = false;
                    }
                }
            }
        }

        private void barButtonItem_Test_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ActiveItem(string.Format(@"\MeasurementUI.dll;component/DataDisplay/ResultDisplay.xaml"),"测试结果","测试");
            documentPanelIsVisible("测试");
        }

        private void barButtonItem_Set_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //documentPanelIsVisible("设置");
            ActiveItem(string.Format(@"\MeasurementUI.dll;component/View/DutDescSet.xaml"), "其他设置", "设置");
            documentPanelIsVisible("设置");
        }

        private void barButtonItem_Common_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ActiveItem(string.Format(@"\MeasurementUI.dll;component/View/DutDescSet.xaml"), "其他设置", "设置");
            documentPanelIsVisible("设置");
        }

        private void barButtonItem_user_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ActiveItem(string.Format(@"\UserMaintenance\UserMaintenance.dll;component/User.xaml?用户编号={0}", "Admin"), "用户管理", "设置");
        }

        private void barButtonItemCal_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ActiveItem(string.Format(@"\MeasurementUI.dll;component/View/CalibrationUC.xaml"), "校准", "校准");
            documentPanelIsVisible("校准");
        }

        private void barButtonItem_TestResult_ItemClick(object sender, ItemClickEventArgs e)
        {
            ActiveItem(string.Format(@"\MeasurementUI.dll;component/View/ChooseResult.xaml"), "选择测试结果", "测试结果");
            documentPanelIsVisible("测试结果");
            CloseDockItem("校准");
        }

        private void barExtension_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            string str = (sender as BarButtonItem).Content.ToString();
            UITypeInfo uiInfo = TestStepFactory.UITypelist.Where(x => x.Att.DisplayName == str).FirstOrDefault();
            ActiveItem2(uiInfo.Att.UserControlTypeStr,uiInfo.Att.DisplayName,"扩展功能");
        }
        private void barButtonItemUi_ItemClick(object sender,DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            documentPanelIsVisible("扩展功能");
        }

        private void LoadExtensionUi()
        {
            BarSubItem subItem = new BarSubItem() { Content="扩展功能",BarItemDisplayMode=BarItemDisplayMode.ContentAndGlyph,LargeGlyph=new BitmapImage(new Uri( "/ATS_TR;component/Images/扩展信息.png", UriKind.Relative))};
            subItem.ItemClick += barButtonItemUi_ItemClick;
            foreach(var type in TestStepFactory.UITypelist)
            {   
                BarButtonItem item = new BarButtonItem() { Content=type.Att.DisplayName};
                item.ItemClick += barExtension_ItemClick;
                subItem.Items.Add(item);
            }
            mainToolBar.Items.Add(subItem);
        }
        private bool IsLoad;

        //连接数据库
        private bool LoadFile()
        {
            IsLoad = false;
            if ((new ViewModelLocator()).MainWindow.StatusInfo.IsLocal == false && DataUtils.StaticInfo.AutoLoadStateFile.ToUpper() == "TRUE")
            {
                string processID = (new ViewModelLocator()).MainWindow.StatusInfo.Process;
                string currFilePath = AppDomain.CurrentDomain.BaseDirectory;
                string fileName = currFilePath + "configfiles/StateFileID.txt";
                if (System.IO.File.Exists(fileName))
                {
                    (new ViewModelLocator()).MainWindow.StatusInfo.IsLoad = true;
                    if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                    string str = File.ReadAllText(fileName);
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        string id = str.Substring(0, str.Length - 2);
                        if (DataUtils.StaticInfo.MesMode.ToLower() == "true")
                        {
                            if (DataUtils.StaticInfo.ATEStatusFile.ToUpper() == "TRUE")
                            {
                                if ((new ViewModelLocator()).MainWindow.StatusInfo.IsAdmin)
                                {
                                    IsLoad = GetStatusFile(id);
                                }
                                else
                                {
                                    DataTable dt = DataUtils.Interface.GetFile(id, processID, "");
                                    if (dt.Rows.Count >= 1)
                                    {
                                        GetStatusFile(id);
                                    }
                                }
                            }
                            else
                            {
                                IsLoad = GetStatusFile(id);
                            }
                        }
                        else
                        {
                            GetStatusFile(id);
                        }

                    }
                }
                if (DXSplashScreen.IsActive) DXSplashScreen.Close();
                (new ViewModelLocator()).MainWindow.StatusInfo.IsLoad = false;
            }
            return IsLoad;
        }

        private bool GetStatusFile(string id)
        {
            IsLoad = false;
            DataSet ds = null;
            if(DataUtils.StaticInfo.MesMode.ToLower()=="true")
            {
                ds = DataUtils.DB.GetDataSetFromSQL(string.Format("SELECT StateData,Material FROM SYS_TEST_PLAN WHERE FILEID='{0}'", id));
            }
            else
            {
                ds = DataUtils.DB.GetDataSetFromSQL(string.Format("SELECT StateData, FileName as Material FROM ATE_TEST_FILE WHERE FILEID='{0}'", id));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                TestPlanVM MTSM = new ViewModelLocator().CurrentTestPlanVm;
                if (ds.Tables[0].Rows[0]["StateData"] != System.DBNull.Value)
                {
                    byte[] data = (byte[])ds.Tables[0].Rows[0]["StateData"];
                    TestPlan testPlan = new TestPlan();
                    object obj = Interface.DeSerializerStateModel(data, testPlan);
                    testPlan = obj as TestPlan;
                    if (testPlan != null)
                    {
                        MTSM.TestPlan = testPlan;
                        IsLoad = true;
                    }
                }
                else
                {
                    MTSM.TestPlan = new TestPlan();
                }
                MTSM.ApplyTestPlan();
                MTSM.DisplayName = ds.Tables[0].Rows[0]["Material"].ToString();
                (new ViewModelLocator()).MainWindow.StatusInfo.OpenProject = ds.Tables[0].Rows[0]["Material"].ToString();
                (new ViewModelLocator()).MainWindow.StatusInfo.Material = MTSM.DisplayName;
            }
            return IsLoad;
        }

        private void barSysUpdate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Update_File();
        }

        private void Update_File()
        {
            string strSuccessFile = "";
            string strFailFile = "";
            string strMessage = "{0}" + Environment.NewLine + "{1}";
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;
            if (openFile.ShowDialog() == true)
            {
                foreach (string fileName in openFile.FileNames)
                {
                    if (System.IO.Path.GetExtension(fileName).Substring(1).ToUpper() == "SQL")
                    {
                        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                        byte[] filebyte = new byte[file.Length];
                        file.Read(filebyte, 0, filebyte.Length);
                        file.Close();
                        if (filebyte.Length < 3) continue;
                        string strSQL = "";
                        byte[] bomBuffer = new byte[] { 0xef, 0xbb, 0xbf };
                        if (filebyte[0] == bomBuffer[0]
                            && filebyte[1] == bomBuffer[1]
                            && filebyte[2] == bomBuffer[2])
                        {
                            strSQL = System.Text.Encoding.UTF8.GetString(filebyte, 3, filebyte.Length - 3);
                        }
                        string res = DataUtils.DB.ExecSQL(strSQL, "");
                        if (res.StartsWith("OK"))
                        {
                            if (string.IsNullOrWhiteSpace(strSuccessFile)) strSuccessFile = "成功:";
                            strSuccessFile += System.IO.Path.GetFileName(fileName) + "; ";
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(strFailFile)) strFailFile = "失败:";
                            strFailFile += System.IO.Path.GetFileName(fileName) + "; ";
                        }
                        continue;
                    }
                    if (DataUtils.ActivityUtils.UploadActivity(fileName))
                    {
                        if (string.IsNullOrWhiteSpace(strSuccessFile)) strSuccessFile = "成功:";
                        strSuccessFile += System.IO.Path.GetFileName(fileName) + "; ";
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(strFailFile)) strFailFile = "失败:";
                        strFailFile += System.IO.Path.GetFileName(fileName) + "; ";
                    }
                }
                DXMessageBox.Show(string.Format(strMessage, strSuccessFile, strFailFile), "上传文件");
            }
        }

        private void dockManager_DockItemClosing(object sender, DevExpress.Xpf.Docking.Base.ItemCancelEventArgs e)
        {
            DockLayoutManager documentManager = sender as DockLayoutManager;
            var item = documentManager.ActiveDockItem;
            if (item != null && item.ActualCaption != null && item.ActualCaption.ToString() == "测试结果")
            {
                var result = DXMessageBox.Show("是否关闭测试界面？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (item != null && item.ActualCaption != null && item.ActualCaption.ToString() == "本地校准")
            {
                if (TestPlanManager.CurrentTestPlan == null) return;
                var dialogResult = DXMessageBox.Show("是否保存校准？", "提示", MessageBoxButton.OKCancel);
                if (dialogResult == MessageBoxResult.OK)
                {
                    Interface.SaveAllLocalSettings(TestPlanManager.CurrentTestPlan);
                }
            }
        }

        /// <summary>
        /// 点击文件的界面的时候关闭测试界面，也就是下一次重新加载测试界面
        /// </summary>
        private void CloseDockItem(params string[] strs)
        {
            DocumentGroup dgp = null;
            foreach (DevExpress.Xpf.Docking.BaseLayoutItem bli in dockManager.LayoutRoot.Items)
            {
                if (bli.GetType() == typeof(DevExpress.Xpf.Docking.DocumentGroup))
                {
                    dgp = (DevExpress.Xpf.Docking.DocumentGroup)bli;
                    for (int i = 0; i < dgp.Items.Count; i++)
                    {
                        if (dgp.Items[i] as DevExpress.Xpf.Docking.DocumentPanel != null)
                        {
                            DevExpress.Xpf.Docking.DocumentPanel tempdp = dgp.Items[i] as DevExpress.Xpf.Docking.DocumentPanel;
                            for (int j = 0; j < strs.Length; j++)
                            {
                                if (tempdp.ActualCaption != null && tempdp.ActualCaption.ToString() == strs[j])
                                {
                                    dgp.Items.Remove(tempdp);
                                    i--;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = DXMessageBox.Show("是否退出程序？","提示" ,MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void buttonLocalClick(object sender, ItemClickEventArgs e)
        {
            ActiveItem(string.Format(@"\MeasurementUI.dll;component/View/TestPlanLocalSettingView.xaml"), "本地设置和校准", "校准");
            documentPanelIsVisible("校准");
        }

    }
}
