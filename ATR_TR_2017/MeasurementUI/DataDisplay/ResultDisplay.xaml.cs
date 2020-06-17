using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ViewModelBaseLib;
using System.IO;
using System.Windows.Threading;
using System.Data;
using System.Reflection;
using System.Xml.Linq;
using DevExpress.Xpf.Editors;
using System.Threading;

//using Symtant.InstruDriver.AISG;

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for ResultDisplay.xaml
    /// </summary>
    public partial class ResultDisplay : UserControl
    {
        TestPlanVM vm;
        bool IsPause = false;
        bool IsTestSuccess = GeneTestSetup.Instance.IsTestSuccess;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel2;
      //  private List<IResultListerner> listenerlist = new List<IResultListerner>();
        private RouteLine.RouteLine routeLine;
        private List<TraceEditValue> TraceEditValuelist = new List<TraceEditValue>();
        SNInputUserControl snInputUserControl = null;
        DataUtils.SNRule snRule = null;
        DispatcherTimer dtimer2 = null;
        public ResultDisplay()
        {
            InitializeComponent();
            //LoadLayoutFromDB();
            //saveLayoutToXml();
            snRule = new DataUtils.SNRule();
            vm = grid.DataContext as TestPlanVM;
            vm.UpdateData = new Action<int>(UpdateTestData);
            vm.StepStart = new Action<int>(StepStart);
            vm.ManualConnStart = new Action(ManualConnStart);
            vm.TestSeqFinish = new Action(TestPlan_SeqFish);
            vm.PointFinish = new Action<int, int, int>(PointFinish);
            AddDelegateToLoopStep();

            panel2 = new System.Windows.Forms.Panel();
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(0, 726);
            this.panel2.TabIndex = 130;
            tabPage1 = new System.Windows.Forms.TabPage();
            tabPage1.AutoScroll = true;
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(0, 699);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "工艺路径";

            this.tabcontrol.Controls.Add(tabPage1);
            routeLine = new RouteLine.RouteLine(panel2);
            getAllResultDisplaylistener();
            getSNContentControl();
            getRemarksInfo();
        }

        private void AddDelegateToLoopStep()
        { 
            foreach(var manual in vm.TestPlan.ManualConnectionList)
            {
                foreach(var step in manual.TestStepList)
                {
                    if(step is LoopTestStep)
                    {
                        if((step as LoopTestStep).UpdateChildTestStep == null)
                        {
                            (step as LoopTestStep).UpdateChildTestStep += UpdateChildTestStepData;
                        }
                        foreach(var subStep in Interface.GetAllLoopTestStep(step as LoopTestStep))
                        {
                            subStep.UpdateChildTestStep += UpdateChildTestStepData;
                        }
                    }
                }
            }
        }

        private void getAllResultDisplaylistener()
        {
            //ResultDisplayListenerVM resultVM = new ResultDisplayListenerVM();
            //object obj = Interface.DeSerializerResultVM(resultVM);
            //vm.listenerlist.Clear();
            //if (obj != null)
            //{
            //    resultVM = obj as ResultDisplayListenerVM;
            //    foreach (var item in resultVM.ResultDisplayInfos)
            //    {
            //        ResultDisplayType resultType = TestStepFactory.ResultDisplaylist.Where(x => x.Att.DisplayName == item.Name).FirstOrDefault();
            //        if (resultType != null)
            //        {
            //            getResultDisplayUserControl(resultType);
            //        }
            //    }
            //}
            //else
            //{
            vm.listenerlist.Clear();
            foreach (var resultType in TestStepFactory.ResultDisplaylist)
            {
                getResultDisplayUserControl(resultType);
            }
            // }
        }

        private void getResultDisplayUserControl(ResultDisplayType resultType)
        {
            var exsitedListernerTypeNameList = vm.listenerlist.Select(x => x.GetType().FullName);

            Assembly assembly = Assembly.LoadFile(resultType.AssemblyPath);
            string fullName = resultType.ResultType.FullName;
            Type modelType = assembly.GetType(fullName);
            object resultObj = null;
            //if (exsitedListernerTypeNameList.Contains(fullName))
            //{
            //    resultObj = vm.listenerlist.Where(x => x.GetType().FullName == fullName).FirstOrDefault();
            //}
            //else
            //{
            resultObj = Activator.CreateInstance(modelType) as object;
            if (resultObj is IResultListerner)
            {
                IResultListerner listener = resultObj as IResultListerner;
                listener.TestPlan = vm.TestPlan;
                listener.SpecIndex = vm.SelectedSpecIndex;
                vm.listenerlist.Add(listener);
            }


            if (resultObj is UserControl)
            {
                UserControl uc = resultObj as UserControl;
                DocumentPanel panel = new DocumentPanel();
                panel.Caption = resultType.Att.DisplayName;
                panel.Content = uc;
                documentGroup1.Insert(0, panel);
                documentGroup1.SelectedTabIndex = 0;
            }

        }

        private void getSNContentControl()
        {
            if (vm.IsMultiDut)
            {
                MutilSNInputUserControl snUserControl = new MutilSNInputUserControl();
                contentControl.Content = snUserControl;
            }
            else
            {
                snInputUserControl = new SNInputUserControl();
                contentControl.Content = snInputUserControl;
                snInputUserControl.txtInputBarCode.KeyDown += txtInputBarCode_KeyDown;
            }
        }

        private void getRemarksInfo()
        {
            vm.TestDescriptionList.Clear();
            try
            {
                XElement rooEl = XElement.Load(@"./configfiles/RemarksInfo.xml");
                var xmlTestDescriptionList = rooEl.Elements("TestDescription");
                foreach(var testDescription in xmlTestDescriptionList)
                {
                    TestDescription desp = new TestDescription();
                    desp.DescType = TestStepFactory.GetAttibute(testDescription,"type");
                    desp.DescName = TestStepFactory.GetAttibute(testDescription,"displayName");
                    desp.DescValue = TestStepFactory.GetAttibute(testDescription,"value");
                    string source = TestStepFactory.GetAttibute(testDescription,"source");  
                    if(!string.IsNullOrWhiteSpace(source))
                    {
                        desp.DescSource = source.Split(',').ToList();
                    }
                    vm.TestDescriptionList.Add(desp);
                }
                getRemarksUserControl();
            }
            catch(Exception ex)
            {
                DataUtils.LOGINFO.WriteError(ex.Message+Environment.NewLine+ex.StackTrace+Environment.NewLine+ex.Source);
            }
        }

        private void getRemarksUserControl()
        {
            int length = 0;
            if(vm.TestDescriptionList.Count() > 0)
            {
                var qry = (from s in vm.TestDescriptionList orderby s.DescName.Length descending select s).First();
                length = qry.DescName.Length * 12;
            }
            foreach(TestDescription desp in vm.TestDescriptionList)
            {
                StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(3, 1.5, 3, 1.5) };
                Label label = new Label() { Content = desp.DescName, Width = length, Margin = new Thickness(0, 0, 5, 0)};
                panel.Children.Add(label);
                if(desp.DescSource != null)
                {   
                    ComboBoxEdit comBox = new ComboBoxEdit() {Width = 140,ItemsSource = desp.DescSource};
                    Binding bind = new Binding(){Source=desp,Path=new PropertyPath("DescValue"),Mode=BindingMode.TwoWay};
                    BindingOperations.SetBinding(comBox,ComboBoxEdit.EditValueProperty,bind);
                    panel.Children.Add(comBox);
                }
                else
                {
                    TextEdit text = new TextEdit(){Width = 140};
                    Binding bind = new Binding() { Source = desp, Path = new PropertyPath("DescValue"), Mode = BindingMode.TwoWay };
                    BindingOperations.SetBinding(text, TextEdit.EditValueProperty, bind);
                    panel.Children.Add(text);
                }
                remarksPanel.Children.Add(panel);
            }
        }

        private void dockLayoutManager1_DockOperationCompleted(object sender, DevExpress.Xpf.Docking.Base.DockOperationCompletedEventArgs e)
        {
            SaveLayoutToDB();
        }
        private void LoadLayoutFromDB()
        {
            string result = DataUtils.ActivityUtils.LoadUserLayoutByActivity(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.Name, this.GetType().FullName, DataUtils.StaticInfo.LoginUser, new List<DockLayoutManager>() { dockLayoutManager1 });
            if (documentPanel2.Visibility == Visibility.Visible)
            {
                documentPanel2.Visibility = Visibility.Collapsed;
                documentPanel1.Visibility = Visibility.Visible;
            }
        }
        private void SaveLayoutToDB()
        {
            string result = DataUtils.ActivityUtils.UpdataUserLayoutByActivity(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.Name, this.GetType().FullName, DataUtils.StaticInfo.LoginUser, new List<DockLayoutManager>() { dockLayoutManager1 });
        }

        /// <summary>
        /// 没有用到
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        private bool GetSNData(string SN)
        {
            routeLine.GetRouteBySN(SN);

            //亨鑫使用
            string token = (new ViewModelLocator()).MainWindow.StatusInfo.Token;

            string resValue = ServiceInterface.DataService.CheckINBYSN(token, SN);

            if (resValue.StartsWith("ER"))
            {
                DXMessageBox.Show("序号：[" + SN + "]处理异常！异常消息：[" + resValue + "]");
                return false;
            }

            if (false)
            {
                TestPlanVM MTSM = new ViewModelLocator().CurrentTestPlanVm;
                byte[] StateFile = Interface.SerializerStateModel(MTSM.TestPlan);

                string StateFileHasCode = Interface.GetMD5HashFromByte(StateFile);
                bool CheckStatFileBySn = ServiceInterface.DataService.CheckStatFileBySn(token, SN, StateFileHasCode);
                if (!CheckStatFileBySn)
                {
                    return true;
                }

                byte[] bStatFile = ServiceInterface.DataService.DownLoadStatFile(token, SN);
                
                if (bStatFile != null)
                {
                    TestPlanVM testPlan = Interface.DeSerializerStateModel(bStatFile, testPlan) as TestPlanVM;
                    if (testPlan != null)
                    {
                        MTSM.TestPlan = testPlan.TestPlan;
                    }
                }
                else
                {
                    MTSM.TestPlan = new TestPlan();
                }
                MTSM.ApplyTestPlan();

                vm = MTSM;
                vm.UpdateData = new Action<int>(UpdateTestData); 
            }

            return true;
        }

        private bool CheckDeviceSN(string SN)
        {
            if (GeneTestSetup.Instance.IsSimulated)
            {
                return true;
            }
            else
            {
                BarcodeInput BI = new BarcodeInput();
                BI.ShowDialog();
                string strInput = "";
                if (BI.DialogResult == true)
                {
                    strInput = BI.txtInputBarCode.Text;
                }
                else
                {
                    return false;
                }

                return true;
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if ((new ViewModelLocator()).MainWindow.StatusInfo.IsLocal == false)
            {
                routeLine.GetRouteBySN(vm.SN);
                if (!string.IsNullOrEmpty(DataUtils.StaticInfo.AutoLoadStateBySN) && DataUtils.StaticInfo.AutoLoadStateBySN.ToLower() == "true")
                {
                    if(!LoadStateFileBySN(vm.SN))
                    {
                        MessageBox.Show("请输入正确的序号!");
                        return;
                    }
                }
            }
            foreach (var listener in vm.listenerlist)
            {
                if (!listener.OnTestPlanRunStart())
                {
                    return;
                }
            }
            StartTestForSuccess();
            documentPanel2.Visibility= Visibility.Visible;
            documentPanel1.Visibility= Visibility.Collapsed;   
            if(!string.IsNullOrWhiteSpace(vm.SN) && vm.IsMultiDut == false)
            {
                documentPanel2.Caption = vm.SN;
            }

            DispatcherTimer dtimer = new System.Windows.Threading.DispatcherTimer();
            dtimer.Interval = TimeSpan.FromSeconds(0.001);
            dtimer.Tick += dtimer_Tick2;
            dtimer.Start();

            if (listBoxStep2.Items.Count >= 1)
            {
                //DispatcherTimer dtimer2 = new System.Windows.Threading.DispatcherTimer();
                dtimer2 = new System.Windows.Threading.DispatcherTimer();
                dtimer2.Interval = TimeSpan.FromSeconds(0.5);
                dtimer2.Tick += Rules_Tick;
                dtimer2.Start();
            }
        }

        private void Rules_Tick(object sender, EventArgs e)
        {
            if (vm.TestPlan.ManualConnectionList.Where((x) => x.PassFail != null).Count() == vm.TestPlan.ManualConnectionList.Count)
            {
                if (vm != null && !vm.TestPlan.IsRuning)
                {
                    RulesJudge();
                    //(sender as DispatcherTimer).Stop();
                }
            }
        }

        private void StartTestForSuccess()
        {
             IsTestSuccess = GeneTestSetup.Instance.IsTestSuccess;
             vm.InitialData();
             if(GeneTestSetup.Instance.IsTestSuccess)
             {
                 vm.StartTestSeq(0);
             }
        }

        private void dtimer_Tick2(object sender, EventArgs e)//EventArgs
        {
            (sender as DispatcherTimer).Stop();
            if(listBoxStep2.Items.Count > 0)
            {
               listBoxStep2.Focus();
               listBoxStep2.SelectedIndex = 0;
               listBoxStep2_KeyDown(null, null);
            }
        }

        private void txtInputBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                TextBox txt = sender as TextBox;
                if (txt != null)
                {
                    if (GeneTestSetup.Instance.IsEnable)
                    {
                        string result = snRule.Check_Rule(txt.Text.Trim(), GeneTestSetup.Instance.SNRule);
                        if (!string.IsNullOrEmpty(result))
                        {
                            DXMessageBox.Show(result);
                            return;
                        }
                    }
                    vm.SN = txt.Text.Trim();
                }
                Start_Click(null,null);
            }
        }

        private Random ran = new Random();
        public double GetRand(double low, double up)
        {
            return low + (up - low) * ran.NextDouble();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            DevExpress.Xpf.Core.DXGridDataController.DisableThreadingProblemsDetection = true;
            if(vm.ManualConnList.Count==0)
            {
                DXMessageBox.Show("没有加载状态文件！");
                return;
            }
            ManualConnStart();
            var item = (sender as FrameworkElement).DataContext;
            int index = vm.ManualConnList.IndexOf(item as TreeNodeVM);
            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            DXSplashScreen.Show<TestProgress>();
            IsTestSuccess = GeneTestSetup.Instance.IsTestSuccess;
            if (GeneTestSetup.Instance.IsTestSuccess)
            {
                GeneTestSetup.Instance.IsTestSuccess = false;
            }           
            vm.StartTestSeq(index);
        }

        private void TestPlan_SeqFish()
        {
            GeneTestSetup.Instance.IsTestSuccess = IsTestSuccess;
        }

        private void RulesJudge()
        {
            switch (GeneTestSetup.Instance.Rules)
            {
                case UploadRules.按键上传:
                    break;
                case UploadRules.测完即上传:
                    SaveTestData();
                    break;
                case UploadRules.合格即上传:
                   
                    if (vm != null && vm.TestPlan != null)// && vm.TestPlan.ManualConnectionList.Count == 1
                    {
                        if (vm.TestPlan.ManualConnectionList.Where((x) => x.PassFail == true).Count() == vm.TestPlan.ManualConnectionList.Count)
                        {
                            SaveTestData();
                        }
                        //if (vm.TestPlan.ManualConnectionList[0].PassFail != null && vm.TestPlan.ManualConnectionList[0].PassFail != false)
                        //{
                        //    SaveTestData();
                        //}
                    }
                    break;
                default:
                    break;
            }
        }

        private void listBoxStep2_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender == null && listBoxStep2.Items.Count == 1)
            {
                TestSeq();
            }
            else if(e!=null)
            {
                if (e.Key==Key.Enter)
                {
                    TestSeq();
                }
            }
        }

        private void TestSeq()
        {
            DXSplashScreen.Show<TestProgress>();
            var item = listBoxStep2.SelectedItem;
            int index = vm.ManualConnList.IndexOf(item as TreeNodeVM);
            if (listBoxStep2.Items.Count > index + 1)
            {
                vm.StartTestSeq(index);
                listBoxStep2.SelectedIndex = index + 1;
            }
            else if (listBoxStep2.Items.Count == index + 1)
            {
                vm.StartTestSeq(index);
                btnFinish.Focus();
            }

            //if (DXSplashScreen.IsActive) DXSplashScreen.Close();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = DXMessageBox.Show("是否停止测试？", "提示", MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No);
            
            if(dialogResult==MessageBoxResult.Yes)
            {
                if(dtimer2!=null)
                {
                    dtimer2.Stop();
                }
                vm.StopCurrentTestSeq();
                GetFocus();
                if (GeneTestSetup.Instance.IsEnable)
                {
                    vm.SN = snRule.SN_Rule(vm.SN, GeneTestSetup.Instance.SNRule);
                }
            }  
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GetFocus();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {   
            Button btnPause=sender as Button;
            if (IsPause)
            {
                IsPause = false;
                btnPause.Content = new Image() {Width=33,Height=33,Source = new BitmapImage(new Uri(@"/MeasurementUI;component/Images/开始 (1).png", UriKind.Relative)) };
            }
            else
            {
                IsPause = true;
                btnPause.Content = new Image() {Width=33,Height=33, Source = new BitmapImage(new Uri(@"/MeasurementUI;component/Images/暂停.png", UriKind.Relative)) };
            }        
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            SaveTestData();
            if (GeneTestSetup.Instance.IsEnable)
            {
                vm.SN = snRule.SN_Rule(vm.SN, GeneTestSetup.Instance.SNRule);
            }
        }

        private void SaveTestData()
        {
            if (dtimer2!=null)
            {
                dtimer2.Stop();
            }
            GetFocus();
            foreach (var listener in vm.listenerlist)
            {
                listener.OnTestPlanRunCompleted();
            }
            vm.FinishCurrentTestSeq(vm.SN);
        }

        List<SymtChartLib.XAxis> lstXAxis = new List<SymtChartLib.XAxis>();
        private void ShowChart(TestTrace tr,string str)
        {
            SymtChartLib.Trace trace = new SymtChartLib.Trace();
            if (lstXAxis.Where(x => x.Start == tr.XAxisInfo.Start && x.Stop == tr.XAxisInfo.Stop && x.Center == tr.XAxisInfo.Center).Count() == 0)
            {
                lstXAxis.Add(new SymtChartLib.XAxis() { Start = tr.XAxisInfo.Start, Stop = tr.XAxisInfo.Stop, Center = tr.XAxisInfo.Center });
            }
            int k = InterfaceChart2.Traces.Count;
            trace.Title = str;
            trace.AxisIndex = lstXAxis.FindIndex(x => x.Start == tr.XAxisInfo.Start && x.Stop == tr.XAxisInfo.Stop && x.Center == tr.XAxisInfo.Center);
            if (tr.ResultData  != null)
            {
                List<XYData> xyData = tr.ResultData;
                if (xyData.Count > 0)
                {
                    Point[] point = new Point[xyData.Count];
                    for (int i = 0; i < xyData.Count; i++)
                    {
                        Point p = new Point() { X = xyData[i].X, Y = xyData[i].Y };
                        point[i] = p;
                    }

                    trace.TraceData = point;
                    InterfaceChart2.Traces.Add(trace);
                }
            }
            if (tr.TestSpecList.Count > 0 && vm.SelectedSpecIndex >= 0)
            {
                XYTestLimit testLimit = tr.TestSpecList[vm.SelectedSpecIndex].TestLimit;
                if (testLimit != null)
                {
                    foreach (var limit in testLimit.LimitLine)
                    {
                        SymtChartLib.LimitLineTypeEnum type = new SymtChartLib.LimitLineTypeEnum();
                        if (limit.Type == LimitLineTypeEnum.Max)
                        {
                            type = SymtChartLib.LimitLineTypeEnum.Max;
                        }
                        else if (limit.Type == LimitLineTypeEnum.Min)
                        {
                            type = SymtChartLib.LimitLineTypeEnum.Min;
                        }
                        else
                        {
                            type = SymtChartLib.LimitLineTypeEnum.None;
                        }
                        trace.LimitLineList.Add(new SymtChartLib.LimitLine() { X1 = limit.X1, Y1 = limit.Y1, X2 = limit.X2, Y2 = limit.Y2, Type = type });
                    }
                }
            }
            trace.Scale = tr.Scale;
            trace.DivisionCount = tr.DivCount;
            trace.ReferenceLevel = tr.RefValue;
            trace.Unit = tr.Unit;
            trace.ReferencePosition = tr.RefPosition;
            InterfaceChart2.XAxisList = lstXAxis;
            InterfaceChart2.UpdateData();
        }
        private void IsAddToChart_Click(object sender, RoutedEventArgs e)
        {
            if(vm.SelectedItem as TreeNodeVM　!= null && (vm.SelectedItem.NodeObj as TestItem !=null))
            {
               // string traceName = (vm.SelectedItem.ParentNode.NodeObj as TestStep).Name + (vm.SelectedItem.NodeObj as TestItem).TypeName + vm.SelectedItem.ParentNode.SubTreeNodeList.IndexOf(vm.SelectedItem);
                string traceName = (vm.SelectedItem.ParentNode.NodeObj as TestStep).Name + "_" + (vm.SelectedItem.NodeObj as TestItem).TypeName;
                if (vm.SelectedItem.ParentNode.ParentNode.NodeObj as LoopTestStep == null)
                {
                    TestTrace tr = (treeView.SelectedItem as TreeNodeVM).NodeObj as TestTrace;
                    if (tr != null && tr.ResultData != null)
                    {
                        ShowChart(tr,traceName);
                    }
                }
                else
                {
                    DataTable dt = loopGridControl.ItemsSource as DataTable;
                    TraceEditValuelist.Clear();
                    if(dt !=null)
                    {
                        TraceSetWindow window = new TraceSetWindow(dt,TraceEditValuelist,traceName);
                        window.updateTraceData += UpdateTraceData;
                        window.Show();
                    }
                }
            }
        }
        int[] EditValueCountList;
        int[] CurrEditValueList;
        private void Loop(int i, List<TraceEditValue> TraceValuelist,string str,string traceName)
        {
            for (int j = 0; j < EditValueCountList[i]; j++)
            {
                CurrEditValueList[i] = j;
                if (i == 0)
                {
                    exeLoop(TraceValuelist,str,traceName);
                }
                else
                {
                    Loop(i - 1,TraceValuelist,str,traceName);
                }
            }

        }
        private void exeLoop(List<TraceEditValue> TraceValuelist,string str,string traceName)
        {
            DataTable dt = loopGridControl.ItemsSource as DataTable;
            string filterStr = null;
            for (int i = 0; i < TraceValuelist.Count; i++)
            {
                string columnName=TraceValuelist[i].DisplayName;
                var tev = TraceValuelist[i].EditValuelist[CurrEditValueList[i]];
                filterStr += columnName;
                filterStr += "=";
                filterStr += "'" + tev + "'";
                filterStr += " and ";
            }
            filterStr = filterStr.Substring(0, filterStr.Length - 4);
            Type type = dt.Columns[str].DataType;
            DataRow[] rows = dt.Select(filterStr);

            PlotChart(type, rows, str, traceName);
        }
        private void PlotChart(Type xAxisType,DataRow[] rows,string str, string traceName)
        {
            SymtChartLib.Trace trace = new SymtChartLib.Trace();
            if (xAxisType == typeof(double))
            {
                string start = rows.Min(x => x.Field<double>(str)).ToString();
                string stop = rows.Max(x => x.Field<double>(str)).ToString();
                if (lstXAxis.Where(x => x.Start == start && x.Stop == stop).Count() == 0)
                {
                    lstXAxis.Add(new SymtChartLib.XAxis() { Start = start, Stop = stop });
                }
                trace.AxisIndex = lstXAxis.FindIndex(x => x.Start == start && x.Stop == stop);
            }
            int k = InterfaceChart2.Traces.Count;
            trace.Title = traceName;
            if (rows.Count() > 0)
            {
                Point[] point = new Point[rows.Count()];
                for (int i = 0; i < rows.Count(); i++)
                {
                    Point p = new Point() { X = Convert.ToDouble(rows[i][str]), Y = Convert.ToDouble(rows[i]["Y"]) };
                    point[i] = p;
                }
                trace.TraceData = point;
                InterfaceChart2.Traces.Add(trace);
                InterfaceChart2.XAxisList = lstXAxis;
                InterfaceChart2.UpdateData();
            }
        }
        private void UpdateTraceData(List<TraceEditValue> TraceValuelist, string str,string traceName)
        {
            if (TraceValuelist == null || TraceValuelist.Count == 0)
            {
                DataTable dt = loopGridControl.ItemsSource as DataTable;
                Type type = dt.Columns[str].DataType;
                DataRow[] rows = dt.Select();
                PlotChart(type, rows, str, traceName);
            }
            else
            {
                EditValueCountList = TraceValuelist.Select(x => x.EditValuelist.Count).ToArray();
                CurrEditValueList = new int[EditValueCountList.Count()];
                Loop(EditValueCountList.Count() - 1, TraceValuelist, str, traceName);
            }
        }

        private void UpdateChildTestStepData(string stepName)
        {
            this.Dispatcher.Invoke(new Action(() =>
                {
                    foreach(var listener in vm.listenerlist)    
                    {    
                        listener.OnChildTestStepRunCompleted(stepName);
                    }
                }));
        }

        private void UpdateTestData(int i)
        {

            this.Dispatcher.Invoke(new Action(() =>
            {
                foreach (var listener in vm.listenerlist)
                {
                    listener.OnTestStepRunCompleted(i);
                }
                vm.Refresh(i);
            }));
            DataUtils.LOGINFO.WriteLog("step " + i + "update test data");
        }

        private void ManualConnStart()
        {
            this.Dispatcher.Invoke(new Action(() => { foreach(var listener in vm.listenerlist)
            {
                listener.OnTestManualConnRunStart();
            }}));
        }
        
        private void StepStart(int i)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                foreach (var listener in vm.listenerlist)
                {
                    listener.OnTestStepRunStart(i);
                }
            }));
        }

        private void PointFinish(int stepIndex, int traceIndex, int markerIndex)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                foreach (var listener in vm.listenerlist)
                {
                    listener.OnPointFinish(stepIndex, traceIndex, markerIndex);
                }
            }));
        }

        private void txtBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void expandAllItem_Click(object sender, RoutedEventArgs e)
        {
            SetNodeExpandedState(treeView,true);
        }

        private void UnexpandAllItem_Click(object sender, RoutedEventArgs e)
        {
            SetNodeExpandedState(treeView,false);
        }
        private void SetNodeExpandedState(ItemsControl control, bool expandNode)
        {
            try
            {
                if (control != null)
                {
                    foreach (object item in control.Items)
                    {
                        TreeViewItem treeItem = control.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                        if (treeItem != null && treeItem.HasItems)
                        {
                            treeItem.IsExpanded = expandNode;
                            if (treeItem.ItemContainerGenerator.Status != System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                            {
                                treeItem.UpdateLayout();
                            }
                            SetNodeExpandedState(treeItem as ItemsControl, expandNode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message.ToString());
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {          
            loadLayoutFromXml();
            SaveLayoutToDB();
        }
        private void saveLayoutToXml()
        {
            string currFilePath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = currFilePath + "/configfiles/ResultDisplay.xml";
            if(!System.IO.File.Exists(fileName))
            {
                dockLayoutManager1.SaveLayoutToXml(fileName);
            }
          //  SaveLayoutToDB();
        }
        private void loadLayoutFromXml()
        { 
             string currFilePath = AppDomain.CurrentDomain.BaseDirectory;
             string fileName = currFilePath + "/configfiles/ResultDisplay.xml"; 
             if(System.IO.File.Exists(fileName))
             {
                 dockLayoutManager1.RestoreLayoutFromXml(fileName);
             }
        }
        private void GetFocus()
        {
            this.documentPanel1.Dispatcher.Invoke(new Action(() => { this.documentPanel1.Visibility = Visibility.Visible; }));
            this.documentPanel2.Dispatcher.Invoke(new Action(() => { this.documentPanel2.Visibility = Visibility.Collapsed; }));
            DispatcherTimer dtimer = new System.Windows.Threading.DispatcherTimer();
            dtimer.Interval = TimeSpan.FromSeconds(0.001);
            dtimer.Tick += dtimer_Tick;
            dtimer.Start();
        }

        void dtimer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            this.documentGroup4.Dispatcher.Invoke(new Action(() => { this.documentGroup4.Focus(); }));
            if (snInputUserControl!=null)
            {
                this.snInputUserControl.txtInputBarCode.Dispatcher.Invoke(new Action(() => { this.snInputUserControl.txtInputBarCode.Focus(); }));
                this.snInputUserControl.txtInputBarCode.Dispatcher.Invoke(new Action(() => { this.snInputUserControl.txtInputBarCode.SelectAll(); }));
            }
        }

        private void DockingDemoModule_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveLayoutToDB();
        }

        private void treeView_Selected(object sender, RoutedEventArgs e)
        {
            vm.SelectedItem = treeView.SelectedItem as TreeNodeVM;
           // resultVM.SelectedlistViewIndex = resultVM.UpdateTestResultIndex(vm.SelectedItem,resultVM.XYMarkerDisplist[resultVM.SelectedSpecIndex]);
            loopGridControl.ItemsSource = vm.getSelectedTraceResult(vm.SelectedItem);
            GetSelectedDocumentPanel(vm.SelectedItem,"Table2");
        }

        private void GetSelectedDocumentPanel(TreeNodeVM node,string str)
        {   
            if(node !=null && node.ParentNode !=null && node.ParentNode.ParentNode !=null && node.ParentNode.ParentNode.NodeObj as LoopTestStep !=null)
            {
                for (int i = 0; i < 3; i++)
                {
                    DevExpress.Xpf.Docking.DocumentPanel dp = documentGroup1.SelectedItem as DevExpress.Xpf.Docking.DocumentPanel;
                    if (dp == null) return;
                    if (dp.Caption.ToString() == str)
                    {
                        return;
                    }

                    else
                    {
                        documentGroup1.SelectedTabIndex = i;
                    }

                }
            }
        }

        private void loopGridControl_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {
              if(e.Value is double)
              {
                  e.DisplayText = Convert.ToString((new DataUtils.FreqStringConverter()).Convert(e.Value,null,null,null));
              }
        }

        private void combTestSpec_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
             foreach(var listener in vm.listenerlist)
             {
                 listener.SpecIndex = vm.SelectedSpecIndex;
             }
        }

        #region 连接数据库

        /// <summary>
        /// 根据SN自动加载状态文件
        /// </summary>
        /// <param name="sn"></param>
        private bool LoadStateFileBySN(string sn)
        {
            bool IsLoad = false;
            string sql = @"select B.MATERIAL from G_SN_STATUS A
                           left join G_SHOPORDER_STATUS B ON A.SHOPORDER=B.SHOPORDER
                           WHERE A.SN='{0}'";
            string material = "";
            string process = "";
            DataTable dt = DataUtils.DB.GetDataSetFromSQL(string.Format(sql,sn)).Tables[0];
            if(dt!=null&&dt.Rows.Count>0)
            {
                material = Convert.ToString(dt.Rows[0]["MATERIAL"]);
                process = (new ViewModelLocator()).MainWindow.StatusInfo.Process;
            }

            if (DataUtils.StaticInfo.ATEStatusFile.ToUpper() == "TRUE")
            {
                if ((new ViewModelLocator()).MainWindow.StatusInfo.IsAdmin)
                {
                    IsLoad = GetStatusFile(material, process);
                }
                else
                {
                    DataTable dtFile = DataUtils.Interface.GetFile("", process, material);
                    if (dtFile.Rows.Count >= 1)
                    {
                        IsLoad = GetStatusFile(material, process);
                    }
                }
            }
            else
            {
                IsLoad = GetStatusFile(material, process);
            }
          
            return IsLoad;
        }

        private bool GetStatusFile(string material, string process)
        {
            bool IsLoad = false;
            DataSet ds = DataUtils.DB.GetDataSetFromSQL(string.Format("SELECT StateData,Material FROM SYS_TEST_PLAN WHERE Material='{0}' and Process='{1}'", material, process));
            if (ds.Tables[0].Rows.Count > 0)
            {
                TestPlanVM MTSM = new ViewModelLocator().CurrentTestPlanVm;
                if (ds.Tables[0].Rows[0]["StateData"] != System.DBNull.Value)
                {
                    byte[] data = (byte[])ds.Tables[0].Rows[0]["StateData"];
                    TestPlan testPlan = new TestPlan();
                    object obj = Interface.DeSerializerStateModel(data, testPlan);
                    testPlan = obj as TestPlan;
                    testPlan.SN = vm.SN;
                    if (testPlan != null)
                    {
                        MTSM.TestPlan = testPlan;
                    }
                }
                else
                {
                    MTSM.TestPlan = new TestPlan();
                }
                MTSM.DisplayName = ds.Tables[0].Rows[0]["Material"].ToString();
                MTSM.ApplyTestPlan();
                (new ViewModelLocator()).MainWindow.StatusInfo.OpenProject = ds.Tables[0].Rows[0]["Material"].ToString();
                (new ViewModelLocator()).MainWindow.StatusInfo.Material = MTSM.DisplayName;

                foreach (var item in vm.listenerlist)
                {
                    item.TestPlan = MTSM.TestPlan;
                }
                IsLoad = true;
            }
            else
            {
                TestPlanVM MTSM = new ViewModelLocator().CurrentTestPlanVm;
                MTSM.TestPlan = new TestPlan();
                MTSM.DisplayName = null;
                (new ViewModelLocator()).MainWindow.StatusInfo.OpenProject = null;
                (new ViewModelLocator()).MainWindow.StatusInfo.Material = MTSM.DisplayName;
            }
            return IsLoad;
        }

        #endregion

    }
    public class TraceEditValue
    {
        public TraceEditValue()
        {
            EditValuelist = new List<object>();
        }
        public string DisplayName { get; set; }
        public List<object> EditValuelist { get; set; }
    }
}
