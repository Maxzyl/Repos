using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Data;
using DevExpress.Xpf.Core;
using Symtant.GeneFunLib;
namespace ModelBaseLib
{   
    //ZYL add this class 
    public class LoopTestStep:ParentTestStep
    {
        public LoopTestStep()
        {
            CacheParamInfoList = new List<LoopParamInfo>();
           // _Table = new DataTable() { TableName = "ParamInfoTable" };
        }
        [System.Xml.Serialization.XmlIgnore]
        public List<TestStep> Steplist = new List<TestStep>();
        [System.Xml.Serialization.XmlIgnore]
        public List<TableColumnInfo> ColumnInfolist = new List<TableColumnInfo>();
        public List<LoopParamInfo> CacheParamInfoList { get; set; }
        ObservableCollection<LoopParamInfo> Params = new ObservableCollection<LoopParamInfo>();
        [System.Xml.Serialization.XmlIgnore]
        public ObservableCollection<LoopParamInfo> ParamInfolist
        {
            get
            {   
                Steplist.Clear();
                Params.Clear();
                ColumnInfolist.Clear();
                GetValidTestStep(this);        
                foreach(TestStep step in Steplist)
                {
                    var propeties = GetStepParams(step.GetType());
                    foreach (var prop in propeties)
                    {
                        LoopParamInfo paramInfo = new LoopParamInfo() {TestStepName=step.Name,PropertyName=prop.Info.Name,DisplayName=step.Name+ "_" + prop.Attr.DisplayName,PropertyType=prop.Info.PropertyType};
                        var v = CacheParamInfoList.Where(x => x.TestStepName == paramInfo.TestStepName && x.PropertyName == paramInfo.PropertyName).FirstOrDefault();
                        if (v != null)
                        {
                            paramInfo.IsChecked = v.IsChecked;
                            paramInfo.Params = v.Params;
                            paramInfo.SelectedLoopParamInfo = v.SelectedLoopParamInfo;
                            paramInfo.Index = v.Index;
                        }
                        Params.Add(paramInfo);
                        ColumnInfolist.Add(new TableColumnInfo() {ColumnName=step.Name+ "_" + prop.Attr.DisplayName,Converter=prop.Attr.Converter});
                    }
                }
                CacheParamInfoList = Params.ToList();
                DataTable CloneTable = _Table.Clone();
                for (int i = 0; i < _Table.Rows.Count;i++ )
                {
                    CloneTable.Rows.Add(_Table.Rows[i].ItemArray);
                }
                foreach(DataColumn column in _Table.Columns)
                {
                    if(Params.Where(x=>x.DisplayName==column.ColumnName).Count()==0)
                    {
                        CloneTable.Columns.Remove(column.ColumnName);
                    }
                }
                _Table = CloneTable;
                if (UpdateTable != null)
                {
                    UpdateTable.Invoke(this);
                }
                foreach (var item in Params)
                {
                    item.PropertyChanged += item_PropertyChanged;      
                }

                return Params;
            }

        }
        
        private DataTable _Table = new DataTable() { TableName = "ParamInfoTable" };
        private const string TablePropertyName = "Table";
        
        public DataTable Table
        {
            get 
            {
                if (_Table.Columns.Count == 0)
                {
                    return null;
                }
                else
                {
                    return _Table;
                }
            }
            set
            {
                if (value != null)
                {
                    _Table = value;
                    if(UpdateTable !=null)
                    {
                        UpdateTable.Invoke(this);
                    }
                }
                
                
            }
        }
        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChecked")
            {
                LoopParamInfo paramInfo = sender as LoopParamInfo;
                if (paramInfo.IsChecked == true)
                {
                    _Table.Columns.Add(paramInfo.DisplayName, paramInfo.PropertyType);
                    paramInfo.Params.Clear();
                }
                else
                {
                    _Table.Columns.Remove(paramInfo.DisplayName);
                }
                if (UpdateTable != null)
                {
                    UpdateTable.Invoke(this);
                }
                List<LoopParamInfo> paramInfos = new List<LoopParamInfo>();
                foreach (var param in ParamInfolist)
                {
                    if (param.IsChecked == true)
                    {
                        paramInfos.Add(param);
                    }
                }
                foreach(var param in ParamInfolist)
                {
                    param.Params.Clear();
                    if(param.IsChecked==false)
                    {
                        foreach(var item in paramInfos)
                        {                            
                            if(!item.Equals(param) && item.PropertyType.GetType()==param.PropertyType.GetType())
                            {
                                param.Params.Add(item);
                            }                           
                        }
                    }
                    param.Params.Insert(0, new LoopParamInfo());
                }
                
                
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Action<LoopTestStep> UpdateTable { get; set; }
        public override void Single()
        {
            //foreach row in datatable
            //set property of childTestStep from this row
            //childStep.Single
            //get childSteps itemvalue
            //set item of parent step
            if (Table == null) return;
            Steplist.Clear();
            GetValidTestStep(this);
            //InitTestItem();
            foreach (LoopTestItem item in ItemList)
            {
                item.ItemResult.Rows.Clear();
            }
            
            foreach(DataRow row in Table.Rows)
            {
                //set all desendent childStep property
                var paramInfoList = CacheParamInfoList.Where(x => x.IsChecked);
                List<object> paramValueList=new List<object>();
                if (paramInfoList != null)
                {
                    int aa = paramInfoList.Count();
                    foreach (var paramInfo in paramInfoList)
                    {
                        var allchildstep = Steplist.Where(x => x.Name == paramInfo.TestStepName).FirstOrDefault();
                        if (allchildstep != null)
                        {
                            var propInfo = allchildstep.GetType().GetProperty(paramInfo.PropertyName);
                            var paramValue=row[paramInfo.DisplayName];
                            if (string.IsNullOrWhiteSpace(paramValue.ToString()))
                            {
                                TestStep step = this;
                                if (UpdateChildTestStep != null)
                                {
                                    UpdateChildTestStep.Invoke("");
                                }
                                return;
                            }
                            propInfo.SetValue(allchildstep, paramValue);
                           // zyl add below
                            var bindParam = CacheParamInfoList.Where(x=>x.IsChecked==false && x.SelectedLoopParamInfo !=null && x.SelectedLoopParamInfo.DisplayName==paramInfo.DisplayName);
                            if(bindParam !=null)
                            {
                                foreach(var param in bindParam)
                                {
                                    var allChildstep2 = Steplist.Where(x => x.Name == param.TestStepName).FirstOrDefault();
                                    if(allChildstep2 !=null)
                                    {
                                        var propInfo2 = allChildstep2.GetType().GetProperty(param.PropertyName);
                                        propInfo2.SetValue(allChildstep2,paramValue);
                                    }
                                }
                            }
                        }
                        paramValueList.Add(row[paramInfo.DisplayName]);
                    }
                }
                //single all direct child steps
                foreach (var childstep in ChildTestStepList)
                {
                    if (GetRunState() == false) return;
                    childstep.Single();
                    DataUtils.LOGINFO.WriteLog(DateTime.Now.ToString() + childstep.DisplayName + " single finish");
                    if (paramValueList.Count == 0) continue;
                    if (childstep is LoopTestStep)
                    {

                        for (int i = 0; i < childstep.ItemList.Count; i++)
                        {
                            LoopTestItem childStepItem = childstep.ItemList[i] as LoopTestItem;
                            LoopTestItem item = GetItem(childStepItem.TestStepName, childStepItem.ItemIndex);
                            foreach (DataRow itemrow in childStepItem.ItemResult.Rows)
                            {
                                List<object> resList = new List<object>();
                                resList.AddRange(paramValueList);
                                resList.AddRange(itemrow.ItemArray);
                                item.ItemResult.Rows.Add(resList.ToArray());
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < childstep.ItemList.Count; i++)
                        {
                            LoopTestItem item = GetItem(childstep.Name, i);
                            TestItem childStepItem = childstep.ItemList[i];
                            if (childStepItem is PointTestItem)
                            {
                                List<object> resList = new List<object>();
                                resList.AddRange(paramValueList);
                                resList.Add((childStepItem as PointTestItem).Y);
                                item.ItemResult.Rows.Add(resList.ToArray());
                            }
                            if (childStepItem is TestTrace)
                            {
                                foreach (var pRes in (childStepItem as TestTrace).ResultData)
                                {
                                    List<object> resList = new List<object>();
                                    resList.AddRange(paramValueList);
                                    resList.Add(pRes.X);
                                    resList.Add(pRes.Y);
                                    item.ItemResult.Rows.Add(resList.ToArray());
                                }
                            }
                        }
                        
                    }
                     if (UpdateChildTestStep != null && childstep as LoopTestStep != null && childstep.ItemList.Count() > 0)
                    {
                        UpdateChildTestStep.Invoke(childstep.Name);
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
        public LoopTestItem GetItem(string stepName,int itemIndex)
        {
            var v = ItemList.Where(g => ((g as LoopTestItem).TestStepName == stepName && (g as LoopTestItem).ItemIndex == itemIndex)).FirstOrDefault();
            //if (v == null)
            //{
            //    ItemList.Add(new LoopTestItem() { TestStepName = stepName, ItemIndex = itemIndex });
            //}
            return v as LoopTestItem;
        }
        public void InitTestItem()
        {
            foreach (TestStep step in ChildTestStepList)
            {
                if (step is LoopTestStep)
                {
                    (step as LoopTestStep).InitTestItem();

                    foreach (LoopTestItem item in step.ItemList)
                    {
                        DataTable tb = new DataTable();
                        if (Table != null)
                        {
                            foreach (DataColumn column in Table.Columns)
                            {
                                tb.Columns.Add(column.ColumnName,column.DataType);
                            }
                        }
                        foreach (DataColumn column in (item as LoopTestItem).ItemResult.Columns)
                        {
                            DataColumn columnClone = new DataColumn();
                            columnClone.ColumnName = column.ColumnName;
                            columnClone.DataType = column.DataType;
                            columnClone.Caption = column.Caption;
                            tb.Columns.Add(columnClone);
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
                        if (Table != null)
                        {
                            foreach (DataColumn column in Table.Columns)
                            {
                                tb.Columns.Add(column.ColumnName,column.DataType);
                            }
                        }
                        if (item is TestTrace)
                        {
                            tb.Columns.Add("X", typeof(double));
                        }
                        DataColumn itemcolumn = new DataColumn();
                        itemcolumn.Caption = "Y";
                        itemcolumn.ColumnName = item.TypeName;
                        itemcolumn.DataType = typeof(double);
                        tb.Columns.Add(itemcolumn);
                        //tb.Columns.Add("Y",typeof(double));
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
        
        private void GetValidTestStep(LoopTestStep step)
        { 
             foreach(var item in step.ChildTestStepList)
             {
                 if (item.GetType() == typeof(LoopTestStep))
                 {
                     GetValidTestStep(item as LoopTestStep);
                 }
                 else
                 {
                     Steplist.Add(item);
                 }
             }
        }
        private PropertyDisplayParamInfo[] GetStepParams(Type type)
        {
            List<PropertyDisplayParamInfo> paramInfos = new List<PropertyDisplayParamInfo>();
            var props = type.GetProperties();
            foreach(var prop in props)
            {
                var att = prop.GetCustomAttributes(typeof(UIDisplayParaAttribute), false).FirstOrDefault() as UIDisplayParaAttribute;
                if(att!=null)
                {
                    PropertyDisplayParamInfo info = new PropertyDisplayParamInfo();
                    info.Attr = att;
                    info.Info = prop;
                    paramInfos.Add(info);
                }
            }
            return paramInfos.ToArray();
        }

    }
    [Serializable]
    public class LoopParamInfo:NotifyBase
    {
        public LoopParamInfo()
        {
            Params = new ObservableCollection<LoopParamInfo>();
        }
        [System.Xml.Serialization.XmlIgnore]
        public Type PropertyType { get; set; }
        public string TestStepName { get; set; }
        public string PropertyName { get; set; }
        public string DisplayName { get; set; }
        private LoopParamInfo _SelectedLoopParamInfo;
        private const string SelectedLoopParamInfoPropertyName = "SelectedLoopParamInfo";
        public LoopParamInfo SelectedLoopParamInfo
        {
            get
            {
                return _SelectedLoopParamInfo;
            }
            set
            {
                _SelectedLoopParamInfo = value;
                NotifyPropertyChanged(SelectedLoopParamInfoPropertyName);
            }
        }
        
        private bool _IsChecked;
        private const string IsCheckedPropertyName = "IsChecked";
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                _IsChecked = value;
                NotifyPropertyChanged(IsCheckedPropertyName);
            }
        }
        private int _Index;
        private const string IndexPropertyName = "Index";
        public int Index
        {
            get
            {
                return _Index;
            }
            set
            {
                _Index = value;
                if (Index == -1)
                {
                    SelectedLoopParamInfo = null;
                }
                else
                {
                    if (Params.Count > Index)
                    {
                        SelectedLoopParamInfo = Params[value];
                    }
                }
                NotifyPropertyChanged(IndexPropertyName);
            }
        }
        
        public ObservableCollection<LoopParamInfo> Params { get; set; } 

    }
    public class PropertyDisplayParamInfo
    {
        public System.Reflection.PropertyInfo Info { get; set; }
        public UIDisplayParaAttribute Attr { get; set; }
    }

    public class LoopTestItem:TestItem
    {
        public string TestStepName { get; set; }
        public int ItemIndex { get; set; }
        public DataTable ItemResult { get; set; }
        //spec
    }
    public class TableColumnInfo
    {
        public string ColumnName { get; set; }
        public IValueConverter Converter { get; set; }
    }

}
