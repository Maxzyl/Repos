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

using ViewModelBaseLib;
using ModelBaseLib;
using DataUtils;

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for UC_Adv_NFTestTrace.xaml
    /// </summary>
    public partial class UC_Adv_TestTrace : UserControl
    {
        public UC_Adv_TestTrace()
        {
            InitializeComponent();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            (this.DataContext as TestTraceVM).TestLimit.LimitLine.Add(new LimitLine());
        }

        private void btnDel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            (this.DataContext as TestTraceVM).TestLimit.LimitLine.Remove((LimitLine)this.gcLimit.SelectedItem);
        }

        private void btnExport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog SFD = new System.Windows.Forms.SaveFileDialog();
                SFD.Filter = "XML files (*.xml)|*.xml|STA files (*.sta)|*.sta|XLS files (*.xls)|*.xls|XLSX files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    XYTestLimit lstLimitLine = (this.DataContext as TestTraceVM).TestLimit;
                    if (SFD.FilterIndex == 3 || SFD.FilterIndex == 4)
                    {
                        TestTraceVM.TableToExcel(lstLimitLine, SFD.FileName);
                    }
                    else
                    {
                        CommUtils.SerializeData(lstLimitLine.GetType(), lstLimitLine, SFD.FileName);
                    }
                    MessageBox.Show("导出成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnImport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog OFD = new System.Windows.Forms.OpenFileDialog();
                OFD.Filter = "XML files (*.xml)|*.xml|STA files (*.sta)|*.sta|XLS files (*.xls)|*.xls|XLSX files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    XYTestLimit lstLimitLine = new XYTestLimit();
                    if (OFD.FilterIndex == 3 || OFD.FilterIndex == 4)
                    {
                        lstLimitLine = TestTraceVM.ExcelToTable(OFD.FileName);
                    }
                    else
                    {
                        lstLimitLine = (XYTestLimit)CommUtils.DeserializerData(typeof(XYTestLimit), OFD.FileName);
                    }

                    (this.DataContext as TestTraceVM).TestLimit = lstLimitLine;
                    //MessageBox.Show("导入成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void gcLimit_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "X1")
            {
                e.DisplayText = Convert.ToString((new DataUtils.FreqStringConverter()).Convert(e.Value, null, null, null));
            }
            else if (e.Column.FieldName == "X2")
            {
                e.DisplayText = Convert.ToString((new DataUtils.FreqStringConverter()).Convert(e.Value, null, null, null));
            }
            else if (e.Column.FieldName == "Y1")
            {
                e.DisplayText = Convert.ToString((new DataUtils.FreqStringConverter()).Convert(e.Value, null, null, null));
            }
            else if (e.Column.FieldName == "Y2")
            {
                e.DisplayText = Convert.ToString((new DataUtils.FreqStringConverter()).Convert(e.Value, null, null, null));
            }
        }

        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            ((DevExpress.Xpf.Editors.TextEdit)gcLimit.View.ActiveEditor).EditValue = new FreqStringConverter().ConvertBack(e.Value, null, null, null);
        }
    }
}
