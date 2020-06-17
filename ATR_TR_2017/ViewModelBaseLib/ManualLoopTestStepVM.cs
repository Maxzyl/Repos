using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Mvvm;
using System.Windows.Input;

namespace ViewModelBaseLib
{
    public class ManualLoopTestStepVM : NotifyBase
    {
        public ManualLoopTestStep ManualLoopTestStep { get; set; }
        public DataTable ColumnTable
        {
            get
            {
                return ManualLoopTestStep.ColumnTable;
            }
            set
            {
                ManualLoopTestStep.ColumnTable = value;
            }
        }
        public List<string> Typelist
        {
            get
            {
                return ManualLoopTestStep.Typelist;
            }
        }
        public Action<ManualLoopTestStep> UpdateTable
        {
            get
            {
                return ManualLoopTestStep.UpdateTable;
            }
            set
            {
                ManualLoopTestStep.UpdateTable = value;
            }
        }
        public ColumnInfo ColumnInfo
        {
            get
            {
                return ManualLoopTestStep.ColumnInfo;
            }
            set
            {
                ManualLoopTestStep.ColumnInfo = value;
            }
        }
        public ICommand AddColumn
        {
            get
            {
                return ManualLoopTestStep.AddColumn;
            }
        }

        private double _StartData;
        private const string StartDataPropertyName = "StartData";
        public double StartData
        {
            get
            {
                return _StartData;
            }
            set
            {
                _StartData = value;
                NotifyPropertyChanged(StartDataPropertyName);
            }
        }

        private double _StopData;
        private const string StopDataPropertyName = "StopData";
        public double StopData
        {
            get
            {
                return _StopData;
            }
            set
            {
                _StopData = value;
                NotifyPropertyChanged(StopDataPropertyName);
            }
        }

        private int _Points;
        private const string PointsPropertyName = "Points";
        public int Points
        {
            get
            {
                return _Points;
            }
            set
            {
                _Points = value;
                NotifyPropertyChanged(PointsPropertyName);
            }
        }

        public void AddColumnToTable(ColumnInfo info)
        {   
            if(ColumnTable==null)
            {
                ColumnTable = new DataTable() { TableName = "ColumnTable" };
            }
            if (!string.IsNullOrWhiteSpace(info.ColumnName) && !ColumnTable.Columns.Contains(info.ColumnName) && info.ColumnType != null)
            {
                ColumnTable.Columns.Add(info.ColumnName, info.ColumnType);
            }
        }
    }
}
