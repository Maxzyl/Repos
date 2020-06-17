using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Data;
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
using Symtant.GeneFunLib;

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for LoopTestStepUC.xaml
    /// </summary>
    public partial class LoopTestStepUC : UserControl
    {
        LoopTestStepVM vm;
        public LoopTestStepUC(LoopTestStepVM vm)
        {
            InitializeComponent();
            this.vm = vm;
            this.DataContext = vm;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as MenuItem).Header.ToString() == "导出")
            {
                if (loopgridControl.ItemsSource == null) return;
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.Filter = "CSV 文件|*.csv";
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                   // (loopgridControl.View as TableView).ExportToCsv(saveFileDialog.FileName);
                    DataTable dt =(DataTable)loopgridControl.ItemsSource;
                    Interface.ExportDataTableToCSV(saveFileDialog.FileName, dt);
                }
            }
            else if ((sender as MenuItem).Header.ToString() == "导入")
            {
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "CSV 文件|*.csv";
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(openFileDialog.FileName))
                    {   
                        LoopTestStep loopStep = vm.LoopTestStep;
                        DataTable dt = Interface.GetDataTableFromCSV(openFileDialog.FileName, loopStep.ParamInfolist);
                        if (dt == null) return;
                        bool Flag = true;
                        foreach (DataColumn column in dt.Columns)
                        {
                            column.ColumnName = column.ColumnName.Replace(" ", "");
                            if (loopStep.ParamInfolist.Where(x => x.DisplayName == column.ColumnName && x.IsChecked == true).Count() == 0)
                            {
                                Flag = false;
                                break;
                            }
                        }
                        if (Flag == false) return;
                        dt.TableName = loopStep.Table.TableName;
                        loopStep.Table = dt;
                        loopgridControl.ItemsSource = vm.Table;
                    }
                }
            }
            else if ((sender as MenuItem).Header.ToString() == "删除行")
            {
                var items = loopgridControl.SelectedItems;
                if (items.Count != 0)
                {
                    List<int> selectList = new List<int>(loopgridView.GetSelectedRowHandles());
                    for (int row = selectList.Count - 1; row >= 0; row--)
                    {
                        loopgridView.DeleteRow(selectList[row]);
                    }
                }
                LoopTestStep loopStep =vm.LoopTestStep;
                loopStep.Table = loopgridControl.ItemsSource as DataTable;
            }
            else if((sender as MenuItem).Header.ToString() == "快速填充")
            {
                 if(loopgridControl.CurrentColumn != null)
                 {
                     LoopTestStep loopStep = vm.LoopTestStep;
                     int columnIndex=loopgridControl.Columns.IndexOf(loopgridView.FocusedColumn);
                     DataColumn column = loopStep.Table.Columns[columnIndex];
                     FillDataToTable(vm.LoopTestStep.Table,column.ColumnName,columnIndex,column.DataType);
                 }
            }
        }

        private void FillDataToTable(DataTable table,string columnName,int columnIndex,Type type)
        {
            if (type != typeof(double)) return;
            double[] templist = QuickType.GenerateIndexedArray(vm.StartData, vm.StopData, vm.Points);
            int tableRowCount = table.Rows.Count;
            if (templist.Count() > tableRowCount)
            {
                for (int i = tableRowCount; i < templist.Count(); i++)
                {
                    DataRow newRow = table.NewRow();
                    table.Rows.Add(newRow);
                }
            }
            else
            {
                for (int i = templist.Count() ; i < tableRowCount;i++ )
                {
                    table.Rows[i][columnIndex] = DBNull.Value;
                }
            }
            for (int i = 0; i < templist.Count(); i++)
            {
                table.Rows[i][columnIndex] = templist[i];
            }
            this.Dispatcher.Invoke(new Action(() => { loopgridControl.ItemsSource = table; }));
        }


        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            string displayName = (sender as CheckBox).Tag.ToString();
            if(vm.Table !=null)
            {                 
                loopgridControl.ItemsSource = vm.Table;
                if ((sender as CheckBox).IsChecked == true)
                {
                    var item = vm.ColumnInfolist.Where(x => x.ColumnName == displayName).FirstOrDefault();
                    var binding = new Binding(displayName);
                    binding.Mode = BindingMode.TwoWay;
                    binding.Converter = item.Converter;
                    var gridColumn = new DevExpress.Xpf.Grid.GridColumn() { Header = item.ColumnName, Binding = binding };
                    loopgridControl.Columns.Add(gridColumn);
                }
                else
                {
                    var column = loopgridControl.Columns.Where(x => x.Header.ToString() == displayName).FirstOrDefault();
                    loopgridControl.Columns.Remove(column);
                }
            }
            else
            {   
                if((sender as CheckBox).IsChecked==false)
                {
                    var column = loopgridControl.Columns.Where(x => x.Header.ToString() == displayName).FirstOrDefault();
                    loopgridControl.Columns.Remove(column);
                }
                loopgridControl.ItemsSource = new DataTable();                
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(vm.Table==null)return;
            ConvertDataColumn();
        }
        private void ConvertDataColumn()
        {
            foreach (DataColumn column in vm.Table.Columns)
            {
                var item = vm.ColumnInfolist.Where(x => x.ColumnName == column.ColumnName).FirstOrDefault();
                var binding = new Binding(column.ColumnName);
                binding.Mode = BindingMode.TwoWay;
                binding.Converter = item.Converter;
                var gridColumn = new DevExpress.Xpf.Grid.GridColumn() { Header = item.ColumnName, Binding = binding };
                loopgridControl.Columns.Add(gridColumn);
            }
        }

    }
}
