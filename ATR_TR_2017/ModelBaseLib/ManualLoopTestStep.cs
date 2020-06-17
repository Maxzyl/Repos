using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;
using DevExpress.Mvvm;
using System.Windows;

namespace ModelBaseLib
{
    public class ManualLoopTestStep:LoopTestStep
    {
        public ManualLoopTestStep()
        {
            Typelist = new List<string>() { "Double", "String" };
            _ColumnTable = new DataTable() { TableName = "ColumnTable" };
            ColumnInfo = new ColumnInfo();
        }
        [System.Xml.Serialization.XmlIgnore]
        public Action<ManualLoopTestStep> UpdateTable { get; set; }
        private DataTable _ColumnTable;
        private const string ColumnTablePropertyName = "ColumnTable";
        public DataTable ColumnTable
        {
            get
            {
                if (_ColumnTable.Columns.Count == 0)
                {
                    return null;
                }
                else
                {
                    return _ColumnTable;
                }
            }
            set
            {
                if (value != null)
                {
                    _ColumnTable = value;
                    if (UpdateTable != null)
                    {
                        UpdateTable.Invoke(this);
                    }
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public int SelectedIndex { get; set; }
        private DelegateCommand<ColumnInfo> _AddColumn;
        public ICommand AddColumn
        {
            get
            {
                if (_AddColumn == null)
                {
                    _AddColumn = new DelegateCommand<ColumnInfo>(AddColumnToDatable);
                }
                return _AddColumn;
            }
        }
        public ColumnInfo ColumnInfo { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public List<string> Typelist { get; set; }
        public override void Single()
        {   
            if (ColumnTable == null) return;
            foreach(LoopTestItem item in ItemList)
            {
                item.ItemResult.Rows.Clear();
            }
            for (int i = 0; i < ColumnTable.Rows.Count; i++)
            {
                SelectedIndex = i;
                System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(ShowWindow));
                if (SelectedIndex > i) break;
                i = SelectedIndex;
                List<object> RowValuelist = new List<object>();
                for (int k = 0; k < ColumnTable.Columns.Count; k++)
                {
                    RowValuelist.Add(ColumnTable.Rows[SelectedIndex][k]);
                }
                foreach (var childStep in ChildTestStepList)
                {
                    if (GetRunState() == false) return;
                    childStep.Single();
                    if (childStep is ManualLoopTestStep)
                    {
                        for (int j = 0; j < childStep.ItemList.Count; j++)
                        {
                            LoopTestItem childStepItem = childStep.ItemList[j] as LoopTestItem;
                            LoopTestItem item = GetItem(childStepItem.TestStepName, childStepItem.ItemIndex);
                            foreach (DataRow itemrow in childStepItem.ItemResult.Rows)
                            {
                                List<object> resList = new List<object>();
                                resList.AddRange(RowValuelist);
                                resList.AddRange(itemrow.ItemArray);
                                item.ItemResult.Rows.Add(resList.ToArray());
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < childStep.ItemList.Count; j++)
                        {
                            LoopTestItem item = GetItem(childStep.Name, j);
                            TestItem childStepItem = childStep.ItemList[j];
                            if (childStepItem is PointTestItem)
                            {
                                List<object> reslist = new List<object>();
                                reslist.AddRange(RowValuelist);
                                reslist.Add((childStepItem as PointTestItem).Y);
                                item.ItemResult.Rows.Add(reslist.ToArray());
                            }
                            if (childStepItem is TestTrace)
                            {
                                foreach (var pRes in (childStepItem as TestTrace).ResultData)
                                {
                                    List<object> reslist = new List<object>();
                                    reslist.AddRange(RowValuelist);
                                    reslist.Add(pRes.Y);
                                    item.ItemResult.Rows.Add(reslist.ToArray());
                                }
                            }
                        }
                    }
                    if (UpdateChildTestStep != null && childStep as LoopTestStep != null && childStep.ItemList.Count() > 0)
                    {
                        UpdateChildTestStep.Invoke(childStep.Name);
                    }
                }
            }
            if (UpdateChildTestStep != null)
            {
                UpdateChildTestStep.Invoke("");
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Action<string> UpdateChildTestStep;
        private void AddColumnToDatable(ColumnInfo info)
        {
            if (!_ColumnTable.Columns.Contains(info.ColumnName) && info.ColumnType != null)
            {
                _ColumnTable.Columns.Add(info.ColumnName, info.ColumnType);
            }
        }
        //public void AddColumnToTable(ColumnInfo info)
        //{ 
        //     if(!string.IsNullOrWhiteSpace(info.ColumnName) && !_ColumnTable.Columns.Contains(info.ColumnName) && info.ColumnType !=null)
        //     {
        //         _ColumnTable.Columns.Add(info.ColumnName,info.ColumnType);
        //     }
        //}

        public void InitManulTestItem()
        {
            foreach (TestStep step in ChildTestStepList)
            {
                if (step is ManualLoopTestStep)
                {
                    (step as ManualLoopTestStep).InitManulTestItem();
                    foreach (LoopTestItem item in step.ItemList)
                    {
                        DataTable tb = new DataTable();
                        if (Table != null)
                        {
                            foreach (DataColumn column in ColumnTable.Columns)
                            {
                                tb.Columns.Add(column.ColumnName, column.DataType);
                            }
                        }
                        foreach (DataColumn column in (item as LoopTestItem).ItemResult.Columns)
                        {
                            tb.Columns.Add(column.ColumnName, column.DataType);
                        }
                        ItemList.Add(new LoopTestItem()
                        {
                            TestStepName = (item as LoopTestItem).TestStepName,
                            ItemIndex = (item as LoopTestItem).ItemIndex,
                            ItemResult = tb

                        });
                    }
                }
                else
                {
                    foreach (var item in step.ItemList)
                    {
                        DataTable tb = new DataTable();
                        if (ColumnTable != null)
                        {
                            foreach (DataColumn column in ColumnTable.Columns)
                            {
                                tb.Columns.Add(column.ColumnName, column.DataType);
                            }
                        }
                        tb.Columns.Add("Y", typeof(double));
                        ItemList.Add(new LoopTestItem()
                        {
                            TestStepName = step.Name,
                            ItemIndex = step.ItemList.IndexOf(item),
                            ItemResult = tb
                        });
                    }
                }
            }
        }
        private void ShowWindow()
        {
            ManualLoopWindow manualWindow = new ManualLoopWindow(this.ColumnTable, SelectedIndex);
            manualWindow.PassIndex += getIndex;
            manualWindow.ShowDialog();
        }
        private void getIndex(int index)
        {
            SelectedIndex = index;
        }
    }
    public class ColumnInfo
    {
        public string ColumnName { get; set; }
        public string ColumnTypeStr { get; set; }
        public Type ColumnType
        {
            get
            {
                switch (ColumnTypeStr)
                {
                    case "Double":
                        return typeof(double);
                    case "String":
                        return typeof(string);
                    default:
                        return null;
                }
            }
        }
    }
}
