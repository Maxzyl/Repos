using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace ViewModelBaseLib
{
    public class LoopTestStepVM:NotifyBase
    {
        public LoopTestStep LoopTestStep { get; set; }
        public ObservableCollection<LoopParamInfo> ParamInfolist
        {
            get
            {    
                return LoopTestStep.ParamInfolist;
            }
        }
        public DataTable Table
        {
            get
            {
                return LoopTestStep.Table;
            }
        }
        public List<TableColumnInfo> ColumnInfolist
        {
            get
            {
                return LoopTestStep.ColumnInfolist;
            }
        }
        public Action<LoopTestStep> UpdateTable
        {
            get
            {
                return LoopTestStep.UpdateTable;
            }
            set
            {
                LoopTestStep.UpdateTable = value;
            }
        }
        private double _StartData;
        private const string StartDataPropertyName = "StartFreq";
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
        private const string StopDataPropertyName = "StopFreq";
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
      //  public ObservableCollection<LoopParamInfo> SelectedParaInfolist;
    }
}
