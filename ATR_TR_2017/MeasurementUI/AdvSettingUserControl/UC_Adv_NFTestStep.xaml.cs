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
using TestModelLib;
using DataUtils;

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for UC_Adv_NFTestStep.xaml
    /// </summary>
    public partial class UC_Adv_NFTestStep : UserControl
    {
        public UC_Adv_NFTestStep()
        {
            InitializeComponent();
        }

        private void gcBLimit_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "X")
            {
                e.DisplayText = Convert.ToString((new DataUtils.FreqStringConverter()).Convert(e.Value, null, null, null));
            }
            else if (e.Column.FieldName == "Y")
            {
                e.DisplayText = Convert.ToString((new DataUtils.FreqStringConverter()).Convert(e.Value, null, null, null));
            }
        }

        private void gcALimit_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "X")
            {
                e.DisplayText = Convert.ToString((new DataUtils.FreqStringConverter()).Convert(e.Value, null, null, null));
            }
            else if (e.Column.FieldName == "Y")
            {
                e.DisplayText = Convert.ToString((new DataUtils.FreqStringConverter()).Convert(e.Value, null, null, null));
            }
        }

        private void rdoBConst_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
            if (this.rdoBConst.IsChecked == true)
            {
                this.spBConst.Visibility = Visibility.Visible;
                this.spBTable.Visibility = Visibility.Collapsed;
            }
            }
            catch (Exception EX)
            {

            }
        }

        private void rdoBTable_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.rdoBTable.IsChecked == true)
                {
                    
                    this.spBConst.Visibility = Visibility.Collapsed;
                    this.spBTable.Visibility = Visibility.Visible;
                }
            }
            catch (Exception EX)
            {

            }

        }

        private void rdoAConst_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.rdoAConst.IsChecked == true)
                {
                    this.spAConst.Visibility = Visibility.Visible;
                    this.spATable.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception EX)
            {

            }
        }

        private void rdoATable_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.rdoATable.IsChecked == true)
                {
                    this.spAConst.Visibility = Visibility.Collapsed;
                    this.spATable.Visibility = Visibility.Visible;
                }
            }
            catch (Exception EX)
            {

            }
            
        }

        private void btnBAdd_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            (this.DataContext as NFTestStepVM).LossTableBeforeDut.lstXYData.Add(new xyData());
        }

        private void btnBDel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (this.gcBLimit.SelectedItem== null) { return; }
            (this.DataContext as NFTestStepVM).LossTableBeforeDut.lstXYData.Remove((xyData)this.gcBLimit.SelectedItem);
        }

        private void btnBExport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog SFD = new System.Windows.Forms.SaveFileDialog();
                SFD.Filter = "XML files (*.xml)|*.xml|STA files (*.sta)|*.sta|XLS files (*.xls)|*.xls|XLSX files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    XYDataList lstXYData = (this.DataContext as NFTestStepVM).LossTableBeforeDut;
                    if (SFD.FilterIndex == 3 || SFD.FilterIndex == 4)
                    {
                        NFTestStepVM.TableToExcel(lstXYData, SFD.FileName);
                    }
                    else
                    {
                        CommUtils.SerializeData(lstXYData.GetType(), lstXYData, SFD.FileName);
                    }
                    MessageBox.Show("导出成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnBImport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog OFD = new System.Windows.Forms.OpenFileDialog();
                OFD.Filter = "XML files (*.xml)|*.xml|STA files (*.sta)|*.sta|XLS files (*.xls)|*.xls|XLSX files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    XYDataList lstXYData = new XYDataList();
                    if (OFD.FilterIndex == 3 || OFD.FilterIndex == 4)
                    {
                        lstXYData = NFTestStepVM.ExcelToTable(OFD.FileName);
                    }
                    else
                    {
                        lstXYData = (XYDataList)CommUtils.DeserializerData(typeof(XYDataArr), OFD.FileName);
                    }

                    (this.DataContext as NFTestStepVM).LossTableBeforeDut = lstXYData;
                    //MessageBox.Show("导入成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        

        private void btnAAdd_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            (this.DataContext as NFTestStepVM).LossTableAfterDut.lstXYData.Add(new xyData());
        }

        private void btnADel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (this.gcALimit.SelectedItem == null) { return; }
            (this.DataContext as NFTestStepVM).LossTableAfterDut.lstXYData.Remove((xyData)this.gcALimit.SelectedItem);
        }

        private void btnAExport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog SFD = new System.Windows.Forms.SaveFileDialog();
                SFD.Filter = "XML files (*.xml)|*.xml|STA files (*.sta)|*.sta|XLS files (*.xls)|*.xls|XLSX files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    XYDataList lstXYData = (this.DataContext as NFTestStepVM).LossTableAfterDut;
                    if (SFD.FilterIndex == 3 || SFD.FilterIndex == 4)
                    {
                        NFTestStepVM.TableToExcel(lstXYData, SFD.FileName);
                    }
                    else
                    {
                        CommUtils.SerializeData(lstXYData.GetType(), lstXYData, SFD.FileName);
                    }
                    MessageBox.Show("导出成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAImport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog OFD = new System.Windows.Forms.OpenFileDialog();
                OFD.Filter = "XML files (*.xml)|*.xml|STA files (*.sta)|*.sta|XLS files (*.xls)|*.xls|XLSX files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    XYDataList lstXYData = new XYDataList();
                    if (OFD.FilterIndex == 3 || OFD.FilterIndex == 4)
                    {
                        lstXYData = NFTestStepVM.ExcelToTable(OFD.FileName);
                    }
                    else
                    {
                        lstXYData = (XYDataList)CommUtils.DeserializerData(typeof(XYDataArr), OFD.FileName);
                    }

                    (this.DataContext as NFTestStepVM).LossTableAfterDut = lstXYData;
                    //MessageBox.Show("导入成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void gcALimit_GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            ((DevExpress.Xpf.Editors.TextEdit)gcALimit.View.ActiveEditor).EditValue = new FreqStringConverter().ConvertBack(e.Value, null, null, null);
        }

        private void gcBLimit_GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            ((DevExpress.Xpf.Editors.TextEdit)gcBLimit.View.ActiveEditor).EditValue = new FreqStringConverter().ConvertBack(e.Value, null, null, null);
        }

        

        

        

        
    }
}
