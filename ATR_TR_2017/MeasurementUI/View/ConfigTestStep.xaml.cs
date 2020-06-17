using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using ModelBaseLib;
using Newtonsoft.Json;
using SymtantPropertyGrid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for ConfigTestStep.xaml
    /// </summary>
    public partial class ConfigTestStep : UserControl
    {
        TestPlanVM vm = new TestPlanVM();
        TestPlan origTestPlan = new TestPlan();
        TreeViewItem item;
        TextBox tempBox;
        string filterstr;
        string[] filters=new string[3];
        string id;
        string tag;
        ObservableCollection<StateFileModel> stateFilelist = new ObservableCollection<StateFileModel>();
        StateFileModel sf = new StateFileModel();
        bool IsCopy;
        object selectedObject = null;
        object LastRunobj = null;
        public ConfigTestStep()
        {
            InitializeComponent();
            grid.DataContext = vm;
                    
        }
        
        //连接数据库
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                string processID = (new ViewModelLocator()).MainWindow.StatusInfo.Process;
                filterstr = Convert.ToString(this.Tag);
                string[] filters = filterstr.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (filters.Count() == 3)
                {
                    id = filters[1];
                    tag = filters[2];
                    vm.DisplayName = tag;
                    if ((new ViewModelLocator()).MainWindow.StatusInfo.IsLocal == true)
                    {
                        if (System.IO.File.Exists(id))
                        {
                            byte[] statefile = System.IO.File.ReadAllBytes(id);
                            TestPlan testPlan = new TestPlan();
                            if (statefile != null && statefile.Length > 0)
                            {
                                object obj = Interface.DeSerializerStateModel(statefile, testPlan);
                                vm.TestPlan = obj as TestPlan;
                                
                                //Interface.LoadLocalSettings(vm.TestPlan);
                            }
                            else
                            {
                                vm.TestPlan = testPlan;                              
                            }
                            vm.DisplayName = tag;
                            //Interface.LoadLocalSettings(vm.TestPlan);
                        }
                    }
                    else if (DataUtils.StaticInfo.MesMode.ToLower() == "true")
                    {
                        if (DataUtils.StaticInfo.ATEStatusFile.ToUpper() == "TRUE")
                        {
                            if ((new ViewModelLocator()).MainWindow.StatusInfo.IsAdmin)
                            {
                                GetStatusFile(id);
                            }
                            else
                            {
                                DataTable dt = DataUtils.Interface.GetFile(id, processID, "");
                                if (dt != null && dt.Rows.Count >= 1)
                                {
                                    GetStatusFile(id);
                                }
                            }
                        }
                        else
                        {
                            GetStatusFile(id);
                        }
                    }
                    else
                    {
                        GetStatusFile(id);
                    }
                    origTestPlan = Interface.CopyTestPlanVM(vm.TestPlan) as TestPlan;
                }
            }
            vm.AddTestSpecs("生产指标"); 
        }

        private void GetStatusFile(string id)
        {
            DataSet ds = null;
            if(DataUtils.StaticInfo.MesMode.ToLower()=="true")
            {
                 ds = DataUtils.DB.GetDataSetFromSQL(string.Format("SELECT StateData FROM SYS_TEST_PLAN WHERE FILEID='{0}'", id));
            }
            else
            {
                ds = DataUtils.DB.GetDataSetFromSQL(string.Format("SELECT StateData FROM ATE_TEST_FILE WHERE FILEID='{0}'", id));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["StateData"] != System.DBNull.Value)
                {
                    byte[] data = (byte[])ds.Tables[0].Rows[0]["StateData"];
                    if(data.Length!=0)
                    {
                        TestPlan testPlan = new TestPlan();
                        object obj = Interface.DeSerializerStateModel(data, testPlan);
                        if (obj != null)
                        {
                            vm.TestPlan = obj as TestPlan;
                            //Interface.LoadLocalSettings(vm.TestPlan);
                        }
                    }
                }
            }
        }

        private void barButtonAdd_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            vm.AddNode();
            AdvSettingPanel.Visibility = Visibility.Collapsed;
            if (vm.SelectedItem != null)
            {   
                if (vm.SelectedItem.Type == TreeNodeTypeEnum.ManualConnection || vm.SelectedItem.Type==TreeNodeTypeEnum.ParentTestStep)
                {
                    addItemList.ItemsSource = vm.AddTestStepCmdList;
                    dockLayoutManager1.DockController.Restore(addTestStepPanel);
                    int index = documentGroup2.Items.IndexOf(addTestStepPanel);
                    documentGroup2.SelectedTabIndex = index;
                    addTestStepPanel.Caption = "添加测试步骤";
                }
                if (vm.SelectedItem.Type == TreeNodeTypeEnum.TestStep)
                {
                    addItemList.ItemsSource = vm.AddTestItemCmdList;
                    dockLayoutManager1.DockController.Restore(addTestStepPanel);
                    int index = documentGroup2.Items.IndexOf(addTestStepPanel);
                    documentGroup2.SelectedTabIndex = index;
                    addTestStepPanel.Caption = "添加测试项";
                }
            }
            else
            {
                addItemList.ItemsSource = null;
            }
        }

        private void barButtonAddSub_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }
        private void barButtonDelete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
             vm.RemoveNode();
        }

        private void barButtonCopy_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            vm.CopyNode();
        }

        private void barButtonPaste_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            vm.PastNode();
        }
        private void barButtonPaste1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if ((vm.CopyItem as TreeNodeVM).NodeObj as TestStep != null && ((vm.SelectedItem as TreeNodeVM).NodeObj as LoopTestStep != null || (vm.SelectedItem as TreeNodeVM).NodeObj as ManualConnection != null))
            {
                if (((vm.CopyItem as TreeNodeVM).NodeObj as TestStep).PathConfigNameList != null)
                 {
                    SelectConfigPath pathWindow = new SelectConfigPath(vm);
                    pathWindow.ShowDialog();
                    vm.PastePathNode();
                 }
            }
        }
        private void barButtonSave_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Save();
        }

        private void barButtondownload_ItemClick(object sender,DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {         
      
        }

        private void barButtoUpload_ItemClick(object sender,DevExpress.Xpf.Bars.ItemClickEventArgs e)
        { 
        }
        private void barButtonUp_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            vm.UpNode();
        }
        private void barButtonDown_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            vm.DownNode();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string str = txtBoxSearch.Text;
            vm.Search(str);
        }

        private void txtBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {      
               if(e.Key==Key.Enter)
               {
                   string str = txtBoxSearch.Text;
                   vm.Search(str);
               }
        }
        private PropertyDisplayInfo[] GetAllSettingProperty(Type t)
        {
            List<PropertyDisplayInfo> res = new List<PropertyDisplayInfo>();
            var props = t.GetProperties();
            foreach (var prop in props)
            {
                var att = prop.GetCustomAttributes(typeof(ModelBaseLib.UIDisplayAttribute), false).FirstOrDefault() as UIDisplayAttribute;
                if (att != null)
                {
                    PropertyDisplayInfo info = new PropertyDisplayInfo();
                    info.Attr = att;
                    info.Info = prop;
                    res.Add(info);
                }                                            
            }
            return res.ToArray();
        }

        private GridColumn GetPropColumn(PropertyDisplayInfo prop, string bindingPath,object obj)
        {
            var binding = new Binding(bindingPath);
            binding.Mode = BindingMode.TwoWay;
            binding.Converter = prop.Attr.Converter;
            var column = new DevExpress.Xpf.Grid.GridColumn() { Header = prop.Attr.DisplayName, Binding = binding };

            if (prop.Attr.IsComb)
            {
                var editSettings = new ComboBoxEditSettings();
                if (prop.Attr.ItemSource == null)
                {
                    var itemSource = obj.GetType().GetProperty(prop.Attr.ItemSourceName).GetValue(obj);
                    editSettings.ItemsSource = itemSource;
                }
                else
                {
                    editSettings.ItemsSource = prop.Attr.ItemSource;
                }

                column.EditSettings = editSettings;

            }
            if (prop.Info.PropertyType == typeof(bool))
            {
                var ckSettings = new CheckEditSettings();
                column.EditSettings = ckSettings;
            }
            return column;
        }
        private void UpdateTestStepSettingTable(GridControl gridControl, TestStep step)
        {
            var stepVM = Interface.GetTestStepVM(step);
            var allProps = GetAllSettingProperty(stepVM.GetType());
            //首先把所有TestStepVM中的标记属性添加到列里面
            foreach (var prop in allProps)
            {
                var column = GetPropColumn(prop, prop.Info.Name, stepVM);
                gridControl.Columns.Add(column);
            }
            //然后把TestStep中的标记属性显示在表格中
            var allStepProps = GetAllSettingProperty(step.GetType());
            foreach (var prop in allStepProps)
            {
                var column = GetPropColumn(prop, "TestStep." + prop.Info.Name, step);
                gridControl.Columns.Add(column);
            }
        
        }
        private void UpdateTestTraceSettingTable(GridControl gridControl, TestTrace tr)
        {
            var itemVM = Interface.GetTestItemVM(tr);
            var allProps = GetAllSettingProperty(itemVM.GetType());
            foreach (var prop in allProps)
            {
                var column = GetPropColumn(prop, prop.Info.Name, itemVM);
                gridControl.Columns.Add(column);
            }
            var allItemProps = GetAllSettingProperty(tr.GetType());
            foreach (var prop in allItemProps)
            {
                var column = GetPropColumn(prop, "TestTrace." + prop.Info.Name, tr);
                gridControl.Columns.Add(column);
            }
        }
        private void UpdatePointTestItemSettingTable(GridControl gridControl, PointTestItem pItem)
        {
            var itemVM = Interface.GetTestItemVM(pItem);                                                                                               
            var allProps = GetAllSettingProperty(itemVM.GetType());
            foreach (var prop in allProps)
            {
                var column = GetPropColumn(prop, prop.Info.Name, itemVM);
                gridControl.Columns.Add(column);
            }
            var allItemProps = GetAllSettingProperty(pItem.GetType());
            foreach (var prop in allItemProps)
            {
                var column = GetPropColumn(prop, "PointTestItem." + prop.Info.Name, pItem);
                gridControl.Columns.Add(column);
            }
        }
        private void UpdateTRTestItemSettingTable(GridControl gridControl, TRTestItem pItem)
        {
            var itemVM = Interface.GetTestItemVM(pItem);
            var allProps = GetAllSettingProperty(itemVM.GetType());
            foreach (var prop in allProps)
            {
                var column = GetPropColumn(prop, prop.Info.Name, itemVM);
                gridControl.Columns.Add(column);
            }
            var allItemProps = GetAllSettingProperty(pItem.GetType());
            foreach (var prop in allItemProps)
            {
                var column = GetPropColumn(prop, "TRTestItem." + prop.Info.Name, pItem);
                gridControl.Columns.Add(column);
            }
        }
        //标记
        private void tvDepartment_Selected(object sender, RoutedEventArgs e)
        {
            DataUtils.LOGINFO.WriteLine(DateTime.Now.ToString() + "Start");

            vm.SelectedItem = treeView.SelectedItem as TreeNodeVM;
           // treeView.MouseMove -= treeView_MouseMove;
            if (vm.SelectedItem == null)
            {
                getTestSpecContent();
            }
            else
            {
                GetSelectedDocumentPanel("设置总览");
                switch(vm.SelectedItem.Type)
                {
                    case TreeNodeTypeEnum.ManualConnection:
                        getManualConnectionContent();
                        break;
                    case TreeNodeTypeEnum.ParentTestStep:
                        getParentTestStepContent();
                        break;
                    case TreeNodeTypeEnum.TestStep:
                        getStepContent();
                        break;
                    case TreeNodeTypeEnum.PointTestItem:
                        getPointItemContent();
                        break;
                    case TreeNodeTypeEnum.TestTrace:
                        getTestTraceContent();
                        break;
                    case TreeNodeTypeEnum.TestMarker:
                        getTestMarkerContent();
                        break;
                    case TreeNodeTypeEnum.BoolTestItem:
                        break;
                    case TreeNodeTypeEnum.TRTestItem:
                        GetTRTestItemContent();
                        break;
                    default:
                        break;
                }
                selectedObject = vm.SelectedItem.NodeObj;
            }
           DataUtils.LOGINFO.WriteLine(DateTime.Now.ToString() + "Stop");
        }

        private void combTestSpec_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(vm.SelectedItem !=null && vm.SelectedItem.Type==TreeNodeTypeEnum.PointTestItem)
            {
                getPointItemContent();
            }
            else if(vm.SelectedItem != null && vm.SelectedItem.Type == TreeNodeTypeEnum.TestTrace)
            {
                getTestTraceContent();
            }
        }
        private void getTestSpecContent()
        {
            TestSpecUC specUC = new TestSpecUC(vm);
            contentControl1.Content = specUC;
           // contentControl2.Content = null;
            AdvSettingPanel.Visibility = Visibility.Collapsed;
        }
        private void getManualConnectionContent()
        {   
            GridControl gridControl = new GridControl();
            gridControl.SelectedItem = null;
            gridControl.SelectedItemChanged -= gridControl_SelectedItemChanged;
            if((selectedObject != null && selectedObject.GetType() != vm.SelectedItem.NodeObj.GetType()) || selectedObject == null)
            {
                getGridForManualConn();
            }
            string mcmodelStr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_ManualConnection).Name;
            Type mctype = Type.GetType(mcmodelStr);
            object mcuc = Activator.CreateInstance(mctype);
            gridControl = (mcuc as UserControl).FindName("gridControl") as GridControl;
            (mcuc as UserControl).DataContext = vm;
            contentControl1.Content = mcuc;
            foreach (var gridVM in vm.GridVMlist)
            {
                if (gridVM is ManualConnectionVM)
                {
                    if ((gridVM as ManualConnectionVM).ManualConn.Equals(vm.SelectedItem.NodeObj))
                    {
                        gridControl.SelectedItem = gridVM;
                    }
                }
            }
            var mcVM = Interface.GetManualConntionVM(vm.SelectedItem.NodeObj as ManualConnection) as ManualConnectionVM;
            contentControl2.Content = mcVM;
            mcVM.TestPlan = vm.TestPlan;
            gridControl.SelectedItemChanged += gridControl_SelectedItemChanged;

        }
        private void getGridForManualConn()
        {
            vm.GridVMlist.Clear();
            vm.GridVMlist = new ObservableCollection<object>();
            foreach (var conn in vm.TestPlan.ManualConnectionList)
            {
                ManualConnectionVM mconVM = Interface.GetManualConntionVM(conn);
                vm.GridVMlist.Add(mconVM);
            }
        }
        private void getParentTestStepContent()
        {
            if (vm.SelectedItem.NodeObj as ManualLoopTestStep != null)
            {
                ManualLoopTestStepVM manualVM = new ManualLoopTestStepVM();
                manualVM.ManualLoopTestStep = (vm.SelectedItem as TreeNodeVM).NodeObj as ManualLoopTestStep;
                ManualStepUC mc = new ManualStepUC(manualVM);
                contentControl1.Content = mc;
            }
            else
            {
                LoopTestStepVM loopVM = new LoopTestStepVM();
                loopVM.LoopTestStep = (vm.SelectedItem as TreeNodeVM).NodeObj as LoopTestStep;
                LoopTestStepUC lc = new LoopTestStepUC(loopVM);
                contentControl1.Content = lc;
            }
        }
        private void getStepContent()
        {
            GridControl gridControl = new GridControl();
            gridControl.SelectedItem = null;
            gridControl.SelectedItemChanged -= gridControl_SelectedItemChanged;
            Type itemType = vm.SelectedItem.NodeObj.GetType();
            if (vm.SelectedItem.NodeObj as FormulaCalcTestStep != null)
            {
                vm.GridVMlist.Clear();
                vm.GridVMlist = new ObservableCollection<object>();
                contentControl1.Content = null;
                string stepmodelStr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_Adv_FormulaCalcTestStep).Name;
                Type steptype = Type.GetType(stepmodelStr);
                object stepuc = Activator.CreateInstance(steptype);
                (stepuc as UserControl).DataContext = vm;
                contentControl2.Content = stepuc;
            }
            else
            {  
                if ((selectedObject != null && selectedObject.GetType() != vm.SelectedItem.NodeObj.GetType()) || selectedObject == null)
                {
                    getGridForStep(itemType);
                    vm.RefreshConfigPath(vm.SelectedItem.NodeObj as TestStep);
                }
                //DataUtils.LOGINFO.WriteLine(DateTime.Now.ToString() + "Start1");
                string stepmodelStr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_TestStep).Name;
                Type steptype = Type.GetType(stepmodelStr);
                object stepuc = Activator.CreateInstance(steptype);
                gridControl = (stepuc as UserControl).FindName("gridControl") as GridControl;
                UpdateTestStepSettingTable(gridControl, vm.SelectedItem.NodeObj as TestStep);
                (stepuc as UserControl).DataContext = vm;
                contentControl1.Content = stepuc;
               // DataUtils.LOGINFO.WriteLine(DateTime.Now.ToString() + "Start2");
                foreach (var gridVM in vm.GridVMlist)
                {
                    if (gridVM is TestStepVM)
                    {
                        if ((gridVM as TestStepVM).TestStep.Equals(vm.SelectedItem.NodeObj))
                        {
                            gridControl.SelectedItem = gridVM;
                        }
                    }
                }
                //DataUtils.LOGINFO.WriteLine(DateTime.Now.ToString() + "Start3");
                if(vm.SelectedItem.NodeObj is LinkTestStep)
                {
                    LinkTestStepVM stepVM = Interface.GetTestStepVM(vm.SelectedItem.NodeObj as TestStep) as LinkTestStepVM;
                    if(stepVM !=null)
                    {
                        contentControl2.Content = stepVM;
                    }
                   
                }
                else
                {
                    //DataUtils.LOGINFO.WriteLine(DateTime.Now.ToString() + "Stop1");
                    var stepVM = Interface.GetTestStepVM(vm.SelectedItem.NodeObj as TestStep);
                    var testStepVM = Interface.GetStepViewModelFromModel((vm.SelectedItem.NodeObj as TestStep).GetType()) as TestStepVM;
                    UserControl stepVMControl = Interface.GetStepUserControlFromModel(vm.SelectedItem.NodeObj as TestStep) as UserControl;
                   // DataUtils.LOGINFO.WriteLine(DateTime.Now.ToString() + "Stop2");
                    if (stepVMControl != null && testStepVM !=null)
                    {
                        stepVMControl.DataContext = stepVM;
                        contentControl2.Content = stepVMControl;
                    }
                    else if(stepVMControl !=null && testStepVM==null)
                    {
                        stepVMControl.DataContext=vm.SelectedItem.NodeObj as TestStep;
                        contentControl2.Content = stepVMControl;
                    }
                    else
                    {
                        contentControl2.Content = null;
                    }
                }
                gridControl.SelectedItemChanged += gridControl_SelectedItemChanged;
            }

            #region 属性查看器
            if (contentControl2.Content == null)
            {
              
                MeasurementUI.View.PropertyGridVM pgVM = new View.PropertyGridVM();
                pgVM.Property = vm.SelectedItem.NodeObj;
                MeasurementUI.View.PropertyGridUC pc = new MeasurementUI.View.PropertyGridUC(pgVM);
                contentControl2.Content = pc;

            }
            #endregion
        }
        private void getGridForStep(Type itemType)
        {
            vm.GridVMlist.Clear();
            vm.GridVMlist = new ObservableCollection<object>();
            foreach (var conn in vm.TestPlan.ManualConnectionList)
            {
                foreach (var step in conn.TestStepList)
                {
                    foreach (var realStep in Interface.GetAllSubTestStep(step))
                    {
                        if (realStep.GetType().Name == itemType.Name)
                        {
                            TestStepVM newstepVM = Interface.GetTestStepVM(realStep);
                            if (realStep is LinkTestStep)
                            {
                                LinkTestStepVM linkVM = newstepVM as LinkTestStepVM;
                                linkVM.TestPlanVM = vm;
                                (realStep as LinkTestStep).TestPlan = vm.TestPlan;
                            }
                            newstepVM.ConnName = conn.Name;
                            vm.GridVMlist.Add(newstepVM);
                        }
                    }
                }
            }
        }
        private void getPointItemContent()
        {
            GridControl gridControl = new GridControl();
            gridControl.SelectedItem = null;
            gridControl.SelectedItemChanged -= gridControl_SelectedItemChanged;
            Type itemType = vm.SelectedItem.NodeObj.GetType();
            if(vm.SelectedItem.NodeObj is CalcPointTestItem)
            {
                vm.GridVMlist.Clear();
                vm.GridVMlist = new ObservableCollection<object>();
                contentControl1.Content = null;
                CalcPointTestItem pointItem = vm.SelectedItem.NodeObj as CalcPointTestItem;
                contentControl2.Content = pointItem;
                return;
            }
            if ((selectedObject != null && selectedObject.GetType() != vm.SelectedItem.NodeObj.GetType()) || selectedObject == null)
            {
                getGridForPointItem(itemType);
            }
            UC_PointTestItem ucPointTestItem = new UC_PointTestItem();
            gridControl = (ucPointTestItem as UserControl).FindName("gridControl") as GridControl;
            UpdatePointTestItemSettingTable(gridControl, vm.SelectedItem.NodeObj as PointTestItem);
            ucPointTestItem.DataContext = vm;
            contentControl1.Content = ucPointTestItem;
            foreach (var gridVM in vm.GridVMlist)
            {
                if ((gridVM as PointTestItemVM).PointTestItem.Equals(vm.SelectedItem.NodeObj))
                {
                    gridControl.SelectedItem = gridVM;
                }
            }
            var pItemVM = Interface.GetTestItemVM(vm.SelectedItem.NodeObj as PointTestItem) as PointTestItemVM;
            var testItemVM=Interface.GetItemViewModelFromModel((vm.SelectedItem.NodeObj as TestItem).GetType()) as TestItemVM;
            pItemVM.SelectedSpecIndex = vm.SelectedSpecIndex;
            UserControl itemVMControl = Interface.GetItemUserControlFromModel((vm.SelectedItem.NodeObj as TestItem).GetType()) as UserControl;
            if(pItemVM !=null && pItemVM is PointTestItemVM)
            {
                PointTestItemVM pointVM = pItemVM as PointTestItemVM;
                pointVM.SelectedSpecIndex = vm.SelectedSpecIndex;
            }
            if(itemVMControl !=null && testItemVM !=null)
            {
                itemVMControl.DataContext = pItemVM;
                contentControl2.Content = itemVMControl;
            }
            else if (itemVMControl != null && testItemVM == null)
            {
                itemVMControl.DataContext = vm.SelectedItem.NodeObj as TestItem;
                contentControl2.Content = itemVMControl;
            }
            else
            {
                contentControl2.Content = pItemVM;
            }
            gridControl.SelectedItemChanged += gridControl_SelectedItemChanged;

            #region 属性查看器
            if (contentControl2.Content==null)
            {
               
                MeasurementUI.View.PropertyGridVM pgVM = new View.PropertyGridVM();
                pgVM.Property = vm.SelectedItem.NodeObj;
                MeasurementUI.View.PropertyGridUC pc = new MeasurementUI.View.PropertyGridUC(pgVM);
                contentControl2.Content = pc;

            }
            #endregion

        }
        private void getGridForPointItem(Type itemType)
        {
            vm.GridVMlist.Clear();
            vm.GridVMlist = new ObservableCollection<object>();
            foreach (var conn in vm.TestPlan.ManualConnectionList)
            {
                foreach (var step in conn.TestStepList)
                {
                    foreach (var realStep in Interface.GetAllSubTestStep(step))
                    {
                        foreach (var testitem in realStep.ItemList)
                        {
                            if (testitem as PointTestItem != null && testitem.GetType().Name == itemType.Name && (vm.SelectedItem.ParentNode.NodeObj.GetType() == realStep.GetType()))
                            {
                                TestItemVM newPointItemVM = Interface.GetTestItemVM(testitem as TestItem);
                                newPointItemVM.ConnName = conn.Name + "-" + realStep.Name+"-" + realStep.PathConfigName;
                                vm.GridVMlist.Add(newPointItemVM);
                            }
                        }
                    }
                }
            }
        }

        private void getTestTraceContent()
        {
            GridControl gridControl = new GridControl();
            gridControl.SelectedItem = null;
            gridControl.SelectedItemChanged -= gridControl_SelectedItemChanged;
            Type itemType = vm.SelectedItem.NodeObj.GetType();
            if (vm.SelectedItem.NodeObj is CalcPointTestTrace)
            {
                vm.GridVMlist.Clear();
                vm.GridVMlist = new ObservableCollection<object>();
                contentControl1.Content = null;
                CalcPointTestTrace pointTrace = vm.SelectedItem.NodeObj as CalcPointTestTrace;
                contentControl2.Content = pointTrace;
                return;
            }
            if ((selectedObject != null && selectedObject.GetType() != vm.SelectedItem.NodeObj.GetType()) || selectedObject == null)
            {
                getGridForTrace(itemType);
            }
            string modelTraceVM = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_TestTrace).Name;
            Type typeTrace = Type.GetType(modelTraceVM);
            object ucTrace = Activator.CreateInstance(typeTrace);
            gridControl = (ucTrace as UserControl).FindName("gridControl") as GridControl;
            UpdateTestTraceSettingTable(gridControl, vm.SelectedItem.NodeObj as TestTrace);
            (ucTrace as UserControl).DataContext = vm;
            contentControl1.Content = ucTrace;
            foreach (var gridVM in vm.GridVMlist)
            {
                if ((gridVM as TestTraceVM).TestTrace.Equals(vm.SelectedItem.NodeObj))
                {
                    gridControl.SelectedItem = gridVM;
                }
            }
            //获取ViewModel，默认的或者是用户指定的
            var trVM = Interface.GetTestItemVM(vm.SelectedItem.NodeObj as TestTrace);
            //获得用户自定义的ViewModel
            var testTrVM = Interface.GetItemViewModelFromModel((vm.SelectedItem.NodeObj as TestItem).GetType()) as TestItemVM;
            //获得用户自定义的界面
            UserControl trVMControl = Interface.GetItemUserControlFromModel((vm.SelectedItem.NodeObj as TestItem).GetType()) as UserControl;
            if (trVM != null && trVM is TestTraceVM)
            {
                TestTraceVM traceVM = trVM as TestTraceVM;
                traceVM.TestSpecIndex = vm.SelectedSpecIndex;
            }
            //如果指定了界面和ViewModel，使用指定的界面和ViewModel
            if (trVMControl != null && testTrVM != null)
            {
                trVMControl.DataContext = trVM;
                contentControl2.Content = null;
                contentControl2.Content = trVMControl;
            }
            //如果指定了界面，但是没有指定ViewModel，那么把界面绑定到Model上
            else if (trVMControl != null && testTrVM == null)
            {
                trVMControl.DataContext = vm.SelectedItem.NodeObj as TestItem;
                contentControl2.Content = null;
                contentControl2.Content = trVMControl;
            }
            else
            {
                contentControl2.Content = trVM;
            }
            gridControl.SelectedItemChanged += gridControl_SelectedItemChanged;

            #region 属性查看器
            if (contentControl2.Content == null)
            {
               
                MeasurementUI.View.PropertyGridVM pgVM = new View.PropertyGridVM();
                pgVM.Property = vm.SelectedItem.NodeObj;
                MeasurementUI.View.PropertyGridUC pc = new MeasurementUI.View.PropertyGridUC(pgVM);
                contentControl2.Content = pc;

            }
            #endregion
        }

        private void getGridForTRTestItem(Type itemType)
        {
            vm.GridVMlist.Clear();
            vm.GridVMlist = new ObservableCollection<object>();
            foreach (var conn in vm.TestPlan.ManualConnectionList)
            {
                foreach (var step in conn.TestStepList)
                {
                    foreach (var realStep in Interface.GetAllSubTestStep(step))
                    {
                        foreach (var testitem in realStep.ItemList)
                        {
                            if (testitem as TRTestItem != null && testitem.GetType().Name == itemType.Name && (vm.SelectedItem.ParentNode.NodeObj.GetType() == realStep.GetType()))
                            {
                                TestItemVM newTRTestItemVM = Interface.GetTestItemVM(testitem as TestItem);
                                newTRTestItemVM.ConnName = conn.Name + "-" + realStep.Name + "-" + realStep.PathConfigName;
                                vm.GridVMlist.Add(newTRTestItemVM);
                            }
                        }
                    }
                }
            }
        }

        private void GetTRTestItemContent()
        {
            #region
            GridControl gridControl = new GridControl();
            gridControl.SelectedItem = null;
            gridControl.SelectedItemChanged -= gridControl_SelectedItemChanged;
            Type itemType = vm.SelectedItem.NodeObj.GetType();
            //if (vm.SelectedItem.NodeObj is CalcPointTestItem)
            //{
            //    vm.GridVMlist.Clear();
            //    contentControl1.Content = null;
            //    CalcPointTestItem pointItem = vm.SelectedItem.NodeObj as CalcPointTestItem;
            //    contentControl2.Content = pointItem;
            //    return;
            //}
            if ((selectedObject != null && selectedObject.GetType() != vm.SelectedItem.NodeObj.GetType()) || selectedObject == null)
            {
                getGridForTRTestItem(itemType);
            }
            TRTestItemView ucTRTestItem = new TRTestItemView();
            gridControl = (ucTRTestItem as UserControl).FindName("gridControl") as GridControl;
            UpdateTRTestItemSettingTable(gridControl, vm.SelectedItem.NodeObj as TRTestItem);
            ucTRTestItem.DataContext = vm;
            contentControl1.Content = ucTRTestItem;
            foreach (var gridVM in vm.GridVMlist)
            {
                if ((gridVM as TRTestItemVM).TRTestItem.Equals(vm.SelectedItem.NodeObj))
                {
                    gridControl.SelectedItem = gridVM;
                }
            }
            #endregion

            //获取ViewModel，默认的或者是用户指定的
            var trVM = Interface.GetTestItemVM(vm.SelectedItem.NodeObj as TestItem);
            //获取用户自定义的ViewModel
            var testTrVM = Interface.GetItemViewModelFromModel((vm.SelectedItem.NodeObj as TestItem).GetType()) as TestItemVM;

            //获得用户自定义的界面
            UserControl trVMControl = Interface.GetItemUserControlFromModel((vm.SelectedItem.NodeObj as TestItem).GetType()) as UserControl;
            //如果指定了界面和ViewModel，使用指定的界面和ViewModel
            if (trVMControl != null && testTrVM != null)
            {
                trVMControl.DataContext = trVM;
                contentControl2.Content = trVMControl;
            }
            //如果指定了界面，但是没有指定ViewModel，那么把界面绑定到Model上
            else if (trVMControl != null && testTrVM == null)
            {
                trVMControl.DataContext = vm.SelectedItem.NodeObj as TestItem;
                contentControl2.Content = null;
                contentControl2.Content = trVMControl;
            }
            else
            {
                contentControl2.Content = trVM;
            }
            gridControl.SelectedItemChanged += gridControl_SelectedItemChanged;
           
        }

        private void getGridForTrace(Type itemType)
        {
            vm.GridVMlist.Clear();
            vm.GridVMlist = new ObservableCollection<object>();
            bool rowColor = false;
            string currentConnName = null;
            foreach (var conn in vm.TestPlan.ManualConnectionList)
            {
                foreach (var step in conn.TestStepList)
                {
                    foreach (var realStep in Interface.GetAllSubTestStep(step))
                    {
                        foreach (var trace in realStep.ItemList)
                        {
                            if (trace as TestTrace != null && trace.GetType().Name == itemType.Name && (vm.SelectedItem.ParentNode.NodeObj.GetType() == realStep.GetType()))
                            {
                                TestItemVM newtraceVM = Interface.GetTestItemVM(trace as TestItem);
                                newtraceVM.ConnName = conn.Name + "-" + realStep.Name + "-" + realStep.PathConfigName;
                                if (newtraceVM.ConnName != currentConnName)
                                {
                                    currentConnName = newtraceVM.ConnName;
                                    rowColor = !rowColor;
                                }
                                
                                newtraceVM.RowColorIndicator = rowColor;
                                
                                
                                vm.GridVMlist.Add(newtraceVM);
                            }
                        }
                    }
                }
            }
        }
        private void getTestMarkerContent()
        {
            GridControl gridControl = new GridControl();
            gridControl.SelectedItem = null;
            gridControl.SelectedItemChanged -= gridControl_SelectedItemChanged;
            if ((selectedObject != null && selectedObject.GetType() != vm.SelectedItem.NodeObj.GetType()) || selectedObject == null)
            {
                getGridForMarker();
            }
            string modelMarkerVM = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_XYTestMarker).Name;
            Type typeMarker = Type.GetType(modelMarkerVM);
            object ucMarker = Activator.CreateInstance(typeMarker);
            gridControl = (ucMarker as UserControl).FindName("gridControl") as GridControl;
            (ucMarker as UserControl).DataContext = vm;
            contentControl1.Content = ucMarker;
            foreach (var gridVM in vm.GridVMlist)
            {
                if ((gridVM as TestMarkerVM).Marker.Equals(vm.SelectedItem.NodeObj))
                {
                    gridControl.SelectedItem = gridVM;
                }
            }
            var mkVM = Interface.GetTestMarkerVM(vm.SelectedItem.NodeObj as XYTestMarker);
            contentControl2.Content = mkVM;
            gridControl.SelectedItemChanged += gridControl_SelectedItemChanged;
        }
        private void gridControl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {   
            
            //GridControl treelistControl = sender as GridControl;
            //if (LastRunobj == e.NewItem || e.NewItem == null)
            //{
            //    return;
            //}
            //else
            //{
            //    if (treelistControl.SelectedItem as TestStepVM != null)
            //    {
            //        getAllTreeNode(vm.ManualConnList, (treelistControl.SelectedItem as TestStepVM).TestStep);
            //    }
            //    else if (treelistControl.SelectedItem as TestTraceVM != null)
            //    {
            //        getAllTreeNode(vm.ManualConnList, (treelistControl.SelectedItem as TestTraceVM).TestTrace);
            //    }
            //    else if (treelistControl.SelectedItem as TestMarkerVM != null)
            //    {
            //        getAllTreeNode(vm.ManualConnList, (treelistControl.SelectedItem as TestMarkerVM).Marker);
            //    }
            //    LastRunobj = e.NewItem;
            //}
        }

        private void getGridForMarker()
        {
            vm.GridVMlist.Clear();
            vm.GridVMlist = new ObservableCollection<object>();
            bool rowColor = false;
            string currentConnName = null;
            foreach (var conn in vm.TestPlan.ManualConnectionList)
            {
                foreach (var step in conn.TestStepList)
                {
                    foreach (var realStep in Interface.GetAllSubTestStep(step))
                    {
                        foreach (var item in realStep.ItemList)
                        {
                            if (item as TestTrace != null && (vm.SelectedItem.ParentNode.ParentNode.NodeObj.GetType() == realStep.GetType()))
                            {
                                foreach (var marker in (item as TestTrace).TestSpecList[vm.SelectedSpecIndex].TestMarkerList)
                                {
                                    TestMarkerVM newMarkerVM = Interface.GetTestMarkerVM(marker);
                                    newMarkerVM.ConnName = conn.Name + "-" + realStep.Name + "-" + realStep.PathConfigName + "-" + item.TypeName;
                                    if (newMarkerVM.ConnName != currentConnName)
                                    {
                                        currentConnName = newMarkerVM.ConnName;
                                        rowColor = !rowColor;
                                    }

                                    newMarkerVM.RowColorIndicator = rowColor;
                                    vm.GridVMlist.Add(newMarkerVM);
                                }
                            }
                            if (item as TRTestItem != null && (vm.SelectedItem.ParentNode.ParentNode.NodeObj.GetType() == realStep.GetType()))
                            {
                                foreach (var marker in (item as TRTestItem).TestSpecList[vm.SelectedSpecIndex].TestMarkerList)
                                {
                                    TestMarkerVM newMarkerVM = Interface.GetTestMarkerVM(marker);
                                    newMarkerVM.ConnName = conn.Name + "-" + realStep.Name + "-" + realStep.PathConfigName + "-" + item.TypeName;
                                    if (newMarkerVM.ConnName != currentConnName)
                                    {
                                        currentConnName = newMarkerVM.ConnName;
                                        rowColor = !rowColor;
                                    }

                                    newMarkerVM.RowColorIndicator = rowColor;
                                    vm.GridVMlist.Add(newMarkerVM);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void barButtonSetup_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GetSelectedDocumentPanel("设置总览");
            getTestSpecContent();

        }
        private void addManuConnItem_Click(object sender, RoutedEventArgs e)
        {
            vm.AddManualConnection();
        }
        private void GetSelectedDocumentPanel(string str)
        {
            for (int i = 0; i < 2; i++)
            {
                DevExpress.Xpf.Docking.DocumentPanel dp = documentGroup2.SelectedItem as DevExpress.Xpf.Docking.DocumentPanel;
                if (dp == null) return;
                if (dp.Caption.ToString() == str)
                {
                    AdvSettingPanel.Visibility = Visibility.Visible;
                    return;
                }

                else
                { 
                    documentGroup2.SelectedTabIndex = i;
                }

            }
        }
        private void reNameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            reName();
        }

        private void reNameText_LostFocus(object sender, RoutedEventArgs e)
        {
            var mv = vm.SelectedItem;
            (mv.NodeObj as ManualConnection).Name = (sender as TextBox).Text.Trim();
            TextBlock textBlock =DataUtils.CommUtils.FindVisualChild<TextBlock>(item as DependencyObject);
            textBlock.Text = (sender as TextBox).Text.Trim();
            textBlock.Visibility = Visibility.Visible;
            tempBox.Visibility = Visibility.Collapsed;
        }

        private void reNameText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                reNameText_LostFocus(sender, null);

            }
        }
        //保存
        private void Save()
        {
            string terminal = (new ViewModelLocator()).MainWindow.StatusInfo.Terminal;
            string processid = (new ViewModelLocator()).MainWindow.StatusInfo.Process;
            if (!Interface.IsTestPlanEqual(vm.TestPlan, origTestPlan))
            {
                vm.TestPlan.GuidStr = Guid.NewGuid().ToString("D");
                byte[] bytes = ViewModelBaseLib.Interface.SerializerStateModel(vm.TestPlan);
                if ((new ViewModelLocator()).MainWindow.StatusInfo.IsLocal == true)
                {
                    System.IO.File.WriteAllBytes(id, bytes);
                    origTestPlan = Interface.CopyTestPlanVM(vm.TestPlan) as TestPlan;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        if (DataUtils.StaticInfo.MesMode.ToLower()=="true")
                        {
                            if (DataUtils.StaticInfo.ATEStatusFile.ToUpper() == "TRUE")
                            {
                                string material = vm.TestPlan.DisplayName;
                                string str = DataUtils.Interface.UpLoadActivity(bytes, DataUtils.StaticInfo.ATEKind, material, processid, DataUtils.StaticInfo.LoginUser, material);
                                if (!str.ToUpper().Equals("OK"))
                                {
                                    DXMessageBox.Show(str, "提示");
                                    return;
                                }
                            }
                            string result = DataUtils.Interface.SaveStateModel(bytes, id);
                            if (!result.StartsWith("OK"))
                            {
                                DXMessageBox.Show(result.ToString());
                            }
                        }
                        else
                        {
                            string material = vm.TestPlan.DisplayName;
                            DataUtils.Interface.UpdateFileDB(bytes, id);
                          
                        }
                        origTestPlan = Interface.CopyTestPlanVM(vm.TestPlan) as TestPlan;
                    }
                }
            }
            //Interface.SaveAllLocalSettings(vm.TestPlan);
        }
        //重命名
        private void reName()
        {
            if (treeView.SelectedItem as ManualConnectionVM != null)
            {
                tempBox =DataUtils.CommUtils.FindVisualChild<TextBox>(item as DependencyObject);
                tempBox.Visibility = Visibility.Visible;
                TextBlock t =DataUtils.CommUtils.FindVisualChild<TextBlock>(item as DependencyObject);
                t.Visibility = Visibility.Collapsed;
                tempBox.Focus();
                tempBox.SelectAll();
            }
        }

        private void treeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            item = GetParentObjectEx<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (item != null)
            {
                item.Focus();
                e.Handled = true;
            }
        }
        public TreeViewItem GetParentObjectEx<TreeViewItem>(DependencyObject obj) where TreeViewItem : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is TreeViewItem)
                {
                    return (TreeViewItem)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
       
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Interface.IsTestPlanEqual(vm.TestPlan,origTestPlan))
            { 
                return;
            }
            else
            {
                var dialogResult = DXMessageBox.Show("是否保存状态文件？", "提示", MessageBoxButton.OKCancel);
                if (dialogResult == MessageBoxResult.OK)
                {
                    Save();
                }
            }
        }

        private void expandAllItem_Click(object sender, RoutedEventArgs e)
        {
            SetNodeExpandedState(treeItem,true);
        }
        private void UnexpandAllItem_Click(object sender, RoutedEventArgs e)
        {
            SetNodeExpandedState(treeItem,false);
        }
        private void SetNodeExpandedState(ItemsControl control,bool expandNode)
        {
            try
            { 
               if(control !=null)
               {
                   foreach(object item in control.Items)
                   {
                       TreeViewItem treeItem = control.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                       if(treeItem !=null && treeItem.HasItems)
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
            catch(Exception ex)
            {
                DXMessageBox.Show(ex.Message.ToString());
            }
        }

        private void LoadFile(byte[] statefile)
        {
            TestPlanVM MTSM = new ViewModelLocator().CurrentTestPlanVm;
            TestPlan testPlan = new TestPlan();
            if (statefile != null && statefile.Length > 0)
            {
                object obj = Interface.DeSerializerStateModel(statefile, testPlan);
                testPlan = obj as TestPlan;
                if (testPlan != null)
                {
                    MTSM.TestPlan = testPlan;
                }
            }
            else
            {
                MTSM.TestPlan = new TestPlan();
            }
            MTSM.ApplyTestPlan();
        }
        private void treeView_Drop(object sender, DragEventArgs e)
        {
            //TreeViewItem container = GetNearContainer(e.OriginalSource as UIElement);
            //if (container == null || container.Header.Equals(treeView.SelectedItem)) return;
            //vm.UpdateTreeViewAfterDrop(container.Header,treeView.SelectedItem,IsCopy);
        }

        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            //TreeViewItem dragItem = new TreeViewItem();
            //if(vm.SelectedItem ==null || (vm.SelectedItem as TreeNodeVM).NodeObj as TestTrace !=null || (vm.SelectedItem as TreeNodeVM).NodeObj as TestMarker !=null)return;
            //if (e.LeftButton == MouseButtonState.Pressed && System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control)
            //{
            //     IsCopy = true;
            //     DragDrop.DoDragDrop(dragItem, sender, DragDropEffects.Copy);                
            //}
            //if (e.LeftButton == MouseButtonState.Pressed && System.Windows.Forms.Control.ModifierKeys != System.Windows.Forms.Keys.Control)
            //{
            //    IsCopy = false;
            //    DragDrop.DoDragDrop(dragItem, sender, DragDropEffects.Move);
            //}    
        }
        private TreeViewItem GetNearContainer(UIElement element)
        {
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }
        private void getAllTreeNode(ObservableCollection<TreeNodeVM> nodes,object obj)
        {
            foreach (TreeNodeVM node in nodes)
            {
                if (node.NodeObj.Equals(obj))
                {
                    node.IsSelected = true;
                    ParentNodeExpand(node);
                }
                else
                {
                    node.IsSelected = false;
                }
                if (node.SubTreeNodeList != null)
                {
                    getAllTreeNode(node.SubTreeNodeList, obj);
                }
            }
        }
        private void ParentNodeExpand(TreeNodeVM node)
        {
            if (node.ParentNode != null)
            {
                node.ParentNode.IsExpanded = true;
                ParentNodeExpand(node.ParentNode);
            }
        }

        private void AddFromTemplateClick(object sender, ItemClickEventArgs e)
        {
            string tempClsStr = System.Configuration.ConfigurationManager.AppSettings["TestPlanTemplate"];
            if (!string.IsNullOrWhiteSpace(tempClsStr))
            {
                ITestPlanTemplate testPlanTmpt = Symtant.GeneFunLib.GeneFun.GetObjectFromClsPath(tempClsStr) as ITestPlanTemplate;
                if (testPlanTmpt != null)
                {
                    TestPlan testPlan= testPlanTmpt.GetTestPlan();
                    vm.TestPlan = testPlan;
                }
            }
        }
        
    }
    public class PropertyDisplayInfo
    {
        public System.Reflection.PropertyInfo Info { get; set; }
        public UIDisplayAttribute Attr { get; set; }
    }
}
