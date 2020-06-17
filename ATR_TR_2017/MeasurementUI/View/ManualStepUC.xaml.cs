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
using System.Data;
using System.Data.SqlClient;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ModelBaseLib;
using Symtant.GeneFunLib;

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for ManualStepUC.xaml
    /// </summary>
    public partial class ManualStepUC : UserControl
    {
        ManualLoopTestStepVM vm;
        public ManualStepUC(ManualLoopTestStepVM vm)
        {
            InitializeComponent();
            this.vm = vm;
            this.DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            manualgridControl.ItemsSource = null;
            this.Dispatcher.BeginInvoke(new Action(UpdateGridControl));
        }
        private void UpdateGridControl()
        {
            manualgridControl.ItemsSource = vm.ColumnTable;
            foreach(DataColumn column in vm.ColumnTable.Columns)
            {
                 if(manualgridControl.Columns.Where(x=>x.Header.ToString()==column.ColumnName).Count()==0)
                 {
                     if (column.DataType == typeof(double))
                     {
                         var binding = new Binding(column.ColumnName);
                         binding.Mode = BindingMode.TwoWay;
                         binding.Converter = Activator.CreateInstance(typeof(DataUtils.SIPrefixConverter)) as IValueConverter;
                         var gridColumn = new DevExpress.Xpf.Grid.GridColumn() { Header = column.ColumnName, Binding = binding };
                         manualgridControl.Columns.Add(gridColumn);
                     }
                     else
                     {
                         var binding = new Binding(column.ColumnName);
                         binding.Mode = BindingMode.TwoWay;
                         var gridColumn = new DevExpress.Xpf.Grid.GridColumn() { Header = column.ColumnName,Binding=binding};
                         manualgridControl.Columns.Add(gridColumn);
                     }
                 }
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            vm.ColumnTable.Columns.Remove(manualgridControl.CurrentColumn.HeaderCaption.ToString());
            manualgridControl.Columns.Remove(manualgridView.FocusedColumn);          
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as MenuItem).Header.ToString() == "删除行")
            {
                var items = manualgridControl.SelectedItems;
                if (items.Count != 0)
                {
                    List<int> selectList = new List<int>(manualgridView.GetSelectedRowHandles());
                    for (int row = selectList.Count - 1; row >= 0; row--)
                    {
                        manualgridView.DeleteRow(selectList[row]);
                    }
                }
            }
            else if((sender as MenuItem).Header.ToString() == "快速填充")
            {
                if (manualgridControl.CurrentColumn != null)
                 {
                     ManualLoopTestStep loopStep = vm.ManualLoopTestStep;
                     int columnIndex = manualgridControl.Columns.IndexOf(manualgridView.FocusedColumn);
                     DataColumn column = loopStep.ColumnTable.Columns[columnIndex];
                     FillDataToTable(vm.ManualLoopTestStep.ColumnTable, column.ColumnName, columnIndex, column.DataType);
                 }
            }
        }

        private void FillDataToTable(DataTable table, string columnName, int columnIndex, Type type)
        {
            if (type != typeof(double)) return;          
            double[] templist = QuickType.GenerateIndexedArray(Convert.ToDouble(vm.StartData), Convert.ToDouble(vm.StopData), Convert.ToInt32(vm.Points));
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
                for (int i = templist.Count(); i < tableRowCount; i++)
                {
                    table.Rows[i][columnIndex] = DBNull.Value;
                }
            }
            for (int i = 0; i < templist.Count(); i++)
            {
                table.Rows[i][columnIndex] = templist[i];
            }
            this.Dispatcher.Invoke(new Action(() => { manualgridControl.ItemsSource = table; }));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(vm.ColumnTable==null)return;
            foreach (DataColumn column in vm.ColumnTable.Columns)
            {
                if (manualgridControl.Columns.Where(x => x.Header.ToString() == column.ColumnName).Count() == 0)
                {
                    if (column.DataType == typeof(double))
                    {
                        var binding = new Binding(column.ColumnName);
                        binding.Mode = BindingMode.TwoWay;
                        binding.Converter = Activator.CreateInstance(typeof(DataUtils.SIPrefixConverter)) as IValueConverter;
                        var gridColumn = new DevExpress.Xpf.Grid.GridColumn() { Header = column.ColumnName, Binding = binding };
                        manualgridControl.Columns.Add(gridColumn);
                    }
                    else 
                    {
                        var binding = new Binding(column.ColumnName);
                        binding.Mode = BindingMode.TwoWay;
                        var gridColumn = new DevExpress.Xpf.Grid.GridColumn() { Header = column.ColumnName,Binding=binding};
                        manualgridControl.Columns.Add(gridColumn);
                    }
                }
            }
        }
    }
}
