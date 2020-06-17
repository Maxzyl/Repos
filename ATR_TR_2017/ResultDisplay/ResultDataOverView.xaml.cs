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
using Symtant.GeneFunLib;
using DataUtils;
using System.IO;
using System.Xml;
using System.Windows.Markup;
using System.Data;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Core;

namespace TestResultMarkerDip
{
    /// <summary>
    /// Interaction logic for ResultDataOverView.xaml
    /// </summary>
    [UIDisplayPara("数据总览")]
    public partial class ResultDataOverView : UserControl,IResultListerner
    {   
        public ResultDataOverView()
        {
            InitializeComponent();
            this.DataContext = vm;

        }
        TestResultDataOverView vm = new TestResultDataOverView();
        public TestPlan TestPlan { get; set; }
        public int SpecIndex { get; set; }
        DataTable dt;
        bool isTRTestItem;
        public bool OnTestPlanRunStart()
        {
            vm.TestResultTable.Clear();
            return true;
        }

        public bool OnTestPlanRunCompleted()
        {
            vm.TestResultTable.Clear();
            return true;
        }

        public void OnTestManualConnRunStart()
        {
           // throw new NotImplementedException();
        }

        public void OnTestManualConnRunCompleted()
        {
           // throw new NotImplementedException();
        }
        private void updateResultData(int stepIndex)
        { 
             TestStep step = TestPlan.ManualConnectionList[TestPlan.currentConnIndex].TestStepList[stepIndex];
             if (step is LoopTestStep) return;
             for (int i = 0; i < step.ItemList.Count();i++ )
             {
                 var item = step.ItemList[i];
                 string sn = TestPlan.ManualConnectionList[TestPlan.currentConnIndex].SN;
                 if(item is TestTrace)
                 {
                     TestTrace trace = item as TestTrace;
                     if (trace.ResultData.Count()==0)
                     {
                         continue;
                     }
                     double ymaxValue = trace.ResultData.Select(x => x.Y).Max();
                     double yminValue = trace.ResultData.Select(x => x.Y).Min();
                     double maxValue = Convert.ToDouble(ymaxValue.ToDigits(GeneTestSetup.Instance.DataDisplayDigits));
                     double minValue = Convert.ToDouble(yminValue.ToDigits(GeneTestSetup.Instance.DataDisplayDigits));
                     string xmaxValue = (new FreqStringConverter()).Convert(trace.ResultData.Find(x => x.Y == ymaxValue).X, null, null, null).ToString();
                     string xminValue = (new FreqStringConverter()).Convert(trace.ResultData.Find(x => x.Y == yminValue).X, null, null, null).ToString();
                     string tracePassFail = trace.PassFail == false ? "Fail" : "Pass";
                     for (int j = 0; j < TestPlan.TestSpecs.Count();j++ )
                     {
                         vm.TestResultTable.Rows.Add(new object[] {false,TestPlan.currentConnIndex + "," + stepIndex + "," + i,sn, trace.TypeName, TestPlan.TestSpecs[j].SpecName, trace.UserDefName,
                             trace.TestConfigDesciption, trace.XDescription, trace.TestSpecList[j].LimitDescription,xmaxValue + "," + xminValue, maxValue + "," + minValue, tracePassFail
                         });
                         foreach(XYTestMarker marker in trace.TestSpecList[j].TestMarkerList)
                         {
                             string markerPassFail = marker.PassFail == false ? "Fail" : "Pass";
                             double valueY = Convert.ToDouble(marker.MarkerResult[0].Y.ToDigits(GeneTestSetup.Instance.DataDisplayDigits));
                             string valueX = (new FreqStringConverter()).Convert(marker.MarkerResult[0].X, null, null, null).ToString();
                             vm.TestResultTable.Rows.Add(new object[] {false,"",sn,"Marker", TestPlan.TestSpecs[j].SpecName, marker.UserDefName,
                             marker.TestConfigDesciption, marker.XDescription,trace.TestSpecList[j].LimitDescription,valueX ,valueY, markerPassFail
                             });
                         }
                     }
                 }
                 else if(item is PointTestItem)
                 {
                     PointTestItem pointItem = item as PointTestItem;
                     double valueY = Convert.ToDouble(pointItem.Y.ToDigits(GeneTestSetup.Instance.DataDisplayDigits));
                     string valueX = (new FreqStringConverter()).Convert(pointItem.X, null, null, null).ToString();
                     string pointItemPassFail = pointItem.PassFail == false ? "Fail" : "Pass";
                     for (int j = 0; j < TestPlan.TestSpecs.Count(); j++)
                     {                         
                         vm.TestResultTable.Rows.Add(new object[]{false,"",sn, pointItem.TypeName,TestPlan.TestSpecs[j].SpecName, pointItem.UserDefName,
                             pointItem.TestConfigDesciption,pointItem.XDescription,pointItem.TestSpecList[j].LimitDescription, valueX, valueY, pointItemPassFail
                             });
                     }
                 }
                 else if(item is TRTestItem)
                 {   
                     TRTestItem trTestItem = item as TRTestItem;
                     if(trTestItem.Data == null)return;
                     if (trTestItem.FreqList == null||trTestItem.FreqList.Count()==0) return;
                     if (trTestItem.StateList == null||trTestItem.StateList.Count()==0) return;
                     double? maxValue;
                     double? minValue;
                     int maxRowIndex;
                     int maxColumnIndex;
                     int minRowIndex;
                     int minColumnIndex;
                     GeneFun.GetArrayMax(out maxRowIndex, out maxColumnIndex,out maxValue, trTestItem.Data);
                     GeneFun.GetArrayMin(out minRowIndex,out minColumnIndex,out minValue, trTestItem.Data);
                     maxValue = Convert.ToDouble(Convert.ToDouble(maxValue).ToDigits(GeneTestSetup.Instance.DataDisplayDigits));
                     minValue = Convert.ToDouble(Convert.ToDouble(minValue).ToDigits(GeneTestSetup.Instance.DataDisplayDigits));
                     int maxAtt = trTestItem.StateList[maxRowIndex].Att;
                     int maxPhase = trTestItem.StateList[maxRowIndex].Phase;
                     double maxFreq = trTestItem.FreqList[maxColumnIndex];
                     int minAtt = trTestItem.StateList[minRowIndex].Att;
                     int minPhase = trTestItem.StateList[minRowIndex].Phase;
                     double minFreq = trTestItem.FreqList[minColumnIndex];
                     for (int j = 0; j < TestPlan.TestSpecs.Count(); j++)
                     {   
                         string maxValueStr = "最大值：att =" + maxAtt + "phase = " + maxPhase + "freq = " +  maxFreq;
                         string minValueStr = "最小值：att =" + minAtt + "phase = " + minPhase + "freq = " + minFreq;
                         string trTestItemPassFail = trTestItem.TestSpecList[j].PassFail == false ? "Fail" : "Pass";
                         vm.TestResultTable.Rows.Add(new object[]{false,TestPlan.currentConnIndex + "," + stepIndex + "," + i,sn,trTestItem.TypeName,TestPlan.TestSpecs[j].SpecName, trTestItem.UserDefName,
                            trTestItem.TestConfigDesciption,trTestItem.XDescription,trTestItem.TestSpecList[j].LimitDescription,maxValueStr + "\r\n" + minValueStr,maxValue + "," + minValue, trTestItemPassFail
                         });
                     }
                 }
             }
             this.Dispatcher.Invoke(new Action(() => { bindGridControl(); }));
        }

        private void bindGridControl()
        {
            DataTemplate temp = DataOverViewGridControl.Columns["全选1"].HeaderTemplate;
            DataOverViewGridControl.ItemsSource = vm.TestResultTable;
            DataOverViewGridControl.Columns["IndexStr"].Visible = false;

            DataOverViewGridControl.Columns["SN"].Visible = TestPlan.IsMultiDut;

            // DataOverViewGridControl.Columns["PassFail"].Visible = false;
            DataOverViewGridControl.Columns["全选"].HeaderTemplate = temp;
        }

        private void SelectAll_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < DataOverViewGridControl.VisibleRowCount; i++)
            {
                ((DataRowView)DataOverViewGridControl.GetRow(i))["全选"] = true;
            }
        }

        private void SelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < DataOverViewGridControl.VisibleRowCount; i++)
            {
                ((DataRowView)DataOverViewGridControl.GetRow(i))["全选"] = false;
            }
        }
   
        public void OnTestStepRunStart(int stepIndex)
        {
           // throw new NotImplementedException();
        }

        public void OnTestStepRunCompleted(int stepIndex)
        {
            updateResultData(stepIndex);
        }

        public void OnChildTestStepRunStart()
        {
           // throw new NotImplementedException();
        }

        public void OnChildTestStepRunCompleted(string stepName)
        {
           // throw new NotImplementedException();
        }

        public void OnPointFinish(int stepIndex, int traceIndex, int markerIndex)
        {
           // throw new NotImplementedException();
        }

        private void queryBarBtn_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            getTableFromSelect();
            DocumentGroup dpGroup = DevExpress.Xpf.Core.Native.LayoutHelper.FindLayoutOrVisualParentObject(this as FrameworkElement, (el) => { return el is DocumentGroup; }) as DocumentGroup;
            DocumentPanel panel = new DocumentPanel() { ShowCloseButton = true, Caption = "详细数据" };
            DetailResultData uc = new DetailResultData(dt);
            panel.Content = uc;
            dpGroup.Add(panel);
            int index = dpGroup.Items.IndexOf(panel);
            dpGroup.SelectedTabIndex = index;
        }

        private void consistenBarBtn_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            getTableFromSelect();
            if(dt.Rows.Count == 0)
            {
                DXMessageBox.Show("请选中要查询的行！");
                return;
            }
            DataTable dtResult = RowConvert(dt);
            DocumentGroup dpGroup = DevExpress.Xpf.Core.Native.LayoutHelper.FindLayoutOrVisualParentObject(this as FrameworkElement, (el) => { return el is DocumentGroup; }) as DocumentGroup;
            DocumentPanel panel = new DocumentPanel() { ShowCloseButton = true, AllowClose = true, Caption = "一致性数据" };
            DetailResultData uc = new DetailResultData(dtResult);
            panel.Content = uc;
            dpGroup.Add(panel);
            int index = dpGroup.Items.IndexOf(panel);
            dpGroup.SelectedTabIndex = index;
        }

        private DataTable RowConvert(DataTable dt)
        {
            if (isTRTestItem)
            {
                return TRTableConvert(dt);
            }
            else
            {
                return GeneralTableConvert(dt);
            }
        }
        private DataTable TRTableConvert(DataTable dt)
        {
            string where = "全选='{0}'";
            DataRow[] drs = vm.TestResultTable.Select(string.Format(where,true));
            DataTable dtResult = ToDataTable(drs);           
            List<string> AllDynamicColumn = new List<string>();
            DataTable dtResultCopy = dtResult.Clone();
            dtResultCopy.Columns.Add("Att");
            dtResultCopy.Columns.Add("Phase");
            foreach(DataRow dr in dt.DefaultView.ToTable(true,"freq").Rows)
            {
                if(dr["freq"] !=null && !string.IsNullOrWhiteSpace(dr["freq"].ToString()))
                {
                    dtResultCopy.Columns.Add((new FreqStringConverter()).Convert(dr["freq"].ToString(), null, null, null).ToString());
                }
            }
            foreach(DataRow row in dtResult.Rows)
            {
                string[] strs = row["IndexStr"].ToString().Split(',');
                if (!string.IsNullOrWhiteSpace(row["IndexStr"].ToString()))
                {
                    TRTestItem trTestItem = TestPlan.ManualConnectionList[Convert.ToInt32(strs[0])].TestStepList[Convert.ToInt32(strs[1])].ItemList[Convert.ToInt32(strs[2])] as TRTestItem;
                    if(trTestItem != null)
                    {
                        for (int i = 0; i < trTestItem.Data.GetLength(0); i++)
                        {
                            DataRow datarow = dtResultCopy.NewRow();
                            int att = trTestItem.StateList[i].Att;
                            int phase = trTestItem.StateList[i].Phase;
                            for (int j = 0; j < trTestItem.Data.GetLength(1); j++)
                            {
                                string freq = (new FreqStringConverter()).Convert(trTestItem.FreqList[j], null, null, null).ToString();
                                string value = trTestItem.Data[i, j].ToDigits(GeneTestSetup.Instance.DataDisplayDigits);
                                datarow[freq] = value;
                            }
                            datarow["SN"] = row["SN"];
                            datarow["类型名"] = row["类型名"];
                            datarow["门限类型"] = row["门限类型"];
                            datarow["测试项名称"] = row["测试项名称"];
                            datarow["端口"] = row["端口"];
                            datarow["测试条件(XDesc)"] = row["测试条件(XDesc)"];
                            datarow["门限描述"] = row["门限描述"];
                            datarow["PassFail"] = row["PassFail"];
                            datarow["Att"] = att;
                            datarow["Phase"] = phase;
                            dtResultCopy.Rows.Add(datarow);
                        }
                    }
                }
            }
            dtResultCopy.Columns.Remove("X");
            dtResultCopy.Columns.Remove("IndexStr");
            dtResultCopy.Columns.Remove("测试数据");
            dtResultCopy.Columns.Remove("全选");
            return dtResultCopy;
        }
        private DataTable GeneralTableConvert(DataTable dt)
        {
            string where = "全选='{0}'";
            DataRow[] drs = vm.TestResultTable.Select(string.Format(where, true));
            DataTable dtResult = ToDataTable(drs);
            List<string> AllDynamicColumn = new List<string>();
            foreach (DataRow dr in dt.DefaultView.ToTable(true, "X").Rows)
            {
                if (dr["X"] != null && !string.IsNullOrEmpty(dr["X"].ToString()))
                {
                    dtResult.Columns.Add(dr["X"].ToString());
                }
            }
            foreach (DataRow row in dtResult.Rows)
            {
                string[] strs = row["IndexStr"].ToString().Split(',');
                if (!string.IsNullOrWhiteSpace(row["IndexStr"].ToString()))
                {
                    TestTrace trace = TestPlan.ManualConnectionList[Convert.ToInt32(strs[0])].TestStepList[Convert.ToInt32(strs[1])].ItemList[Convert.ToInt32(strs[2])] as TestTrace;
                    if(trace != null)
                    {
                        for (int i = 0; i < trace.ResultData.Count(); i++)
                        {
                            string xValue = (new FreqStringConverter()).Convert(trace.ResultData[i].X, null, null, null).ToString();
                            string yValue = trace.ResultData[i].Y.ToDigits(GeneTestSetup.Instance.DataDisplayDigits);
                            row[xValue] = yValue;
                        }
                    }
                }
                else
                {
                    string xValue = row["X"].ToString();
                    string yValue = row["测试数据"].ToString();
                    row[xValue] = yValue;
                }
            }
            dtResult.Columns.Remove("X");
            dtResult.Columns.Remove("IndexStr");
            dtResult.Columns.Remove("测试数据");
            dtResult.Columns.Remove("全选");
            return dtResult;
        }
     
        private DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone(); // 复制DataRow的表结构
            foreach (DataRow row in rows)
            {
                tmp.ImportRow(row); // 将DataRow添加到DataTable中
            }
            return tmp;
        }

        private void getTableFromSelect()
        {
            DataOverViewGridControl.View.FocusedRowHandle = -1;
            DataRow[] drs = vm.TestResultTable.Select("全选 = true");
            if (drs.Count() == 0)return;            
            DataRow dataRow = drs[0];
            if (!string.IsNullOrWhiteSpace(dataRow["IndexStr"].ToString()))
            {
                string[] indexStrs = dataRow["IndexStr"].ToString().Split(',');
                var testItem = TestPlan.ManualConnectionList[Convert.ToInt32(indexStrs[0])].TestStepList[Convert.ToInt32(indexStrs[1])].ItemList[Convert.ToInt32(indexStrs[2])];
                if(testItem is TestTrace)
                {
                    isTRTestItem = false;
                    dt = vm.DetailTestResultTable.Clone();
                }
                else if(testItem is TRTestItem)
                {
                    isTRTestItem = true;
                    dt = vm.TRDetailTestResultTable.Clone();
                }
            }
            else
            {
                isTRTestItem = false;
                dt = vm.DetailTestResultTable.Clone();
            }
            if (isTRTestItem == false)
            {
                getGeneralTable(drs);
            }
            else
            {
                getTRTestItemTable(drs);
            }
        }

        private void getGeneralTable(DataRow[] drs)
        {
            foreach (DataRow row in drs)
            {
                if (!string.IsNullOrWhiteSpace(row["IndexStr"].ToString()))
                {
                    string[] strs = row["IndexStr"].ToString().Split(',');
                    TestTrace trace = TestPlan.ManualConnectionList[Convert.ToInt32(strs[0])].TestStepList[Convert.ToInt32(strs[1])].ItemList[Convert.ToInt32(strs[2])] as TestTrace;
                    if (trace != null)
                    {
                        for (int i = 0; i < trace.ResultData.Count(); i++)
                        {
                            string xValue = (new FreqStringConverter()).Convert(trace.ResultData[i].X, null, null, null).ToString();
                            string yValue = trace.ResultData[i].Y.ToDigits(GeneTestSetup.Instance.DataDisplayDigits);
                            dt.Rows.Add(new object[] { row["类型名"].ToString(), row["门限类型"].ToString(), row["测试项名称"].ToString(), row["端口"].ToString(),
                                                   row["测试条件(XDesc)"].ToString(),row["门限描述"].ToString(),xValue, yValue,row["PassFail"].ToString()});
                        }
                    }
                }
                else
                {
                    dt.Rows.Add(new object[] { row["类型名"].ToString(), row["门限类型"].ToString(), row["测试项名称"].ToString(), row["端口"].ToString(),
                                               row["测试条件(XDesc)"].ToString(),row["门限描述"].ToString(),row["X"].ToString(), row["测试数据"].ToString(),row["PassFail"].ToString()});
                }
            }
        }

        private void getTRTestItemTable(DataRow[] drs)
        { 
           foreach(DataRow row in drs)
           {
               if (!string.IsNullOrWhiteSpace(row["IndexStr"].ToString()))
               {
                  string[] strs = row["IndexStr"].ToString().Split(',');
                  TRTestItem trTestItem = TestPlan.ManualConnectionList[Convert.ToInt32(strs[0])].TestStepList[Convert.ToInt32(strs[1])].ItemList[Convert.ToInt32(strs[2])] as TRTestItem;
                  if(trTestItem != null)
                  {
                      for (int i = 0; i < trTestItem.Data.GetLength(0); i++)
                      {
                          for (int j = 0; j < trTestItem.Data.GetLength(1);j++)
                          {
                              int att = trTestItem.StateList[i].Att;
                              int phase = trTestItem.StateList[i].Phase;
                              string freq = (new FreqStringConverter()).Convert(trTestItem.FreqList[j], null, null, null).ToString(); 
                              string value = trTestItem.Data[i, j].ToDigits(GeneTestSetup.Instance.DataDisplayDigits);
                              dt.Rows.Add(new object[]{row["类型名"].ToString(), row["门限类型"].ToString(), row["测试项名称"].ToString(), row["端口"].ToString(),
                                                       row["测试条件(XDesc)"].ToString(),row["门限描述"].ToString(),att,phase,freq,value,row["PassFail"].ToString()                           
                              });
                          }
                      }
                  }
               }
           }
        }
        
    }
}
