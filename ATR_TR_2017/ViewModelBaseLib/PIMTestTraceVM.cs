using ModelBaseLib;
using PIMModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelBaseLib
{
    public class PIMTestTraceVM:TestTraceVM
    {
        public PIMTestTraceVM()
        {
            TypeNameList=new string[]{ PIMTestTraceType.PIMTrace1, PIMTestTraceType.PIMTrace2 };
        }
        public PIMTestTrace TestTrace = new PIMTestTrace();
        private const string UnitPropertyName = "Unit";
        public string Unit
        {
            get
            {
                return TestTrace.Unit;
            }
            set
            {
                TestTrace.Unit = value;
                NotifyPropertyChanged(UnitPropertyName);
            }
        }

        private const string CompensationPropertyName = "Compensation";
        public double Compensation
        {
            get
            {
                return TestTrace.Compensation;
            }
            set
            {
                TestTrace.Compensation = value;
                NotifyPropertyChanged(CompensationPropertyName);
            }
        }

        public ResultUnit[] ResultUnitList
        {
            get
            {
                return new ResultUnit[] { ResultUnit.dBm, ResultUnit.dBc };
            }
        }
        private string _DisplayName;
        private const string DisplayNamePropertyName = "DisplayName";
        public string DisplayName
        {
            get
            {
                return TestTrace.UserDefName;
            }
            set
            {
                TestTrace.UserDefName = value;
                NotifyPropertyChanged(DisplayNamePropertyName);
            }
        }
        public string[] TypeNameList{get;set;}
        private string _TypeName;
        private const string TypeNamePropertyName = "TypeName";
        public string TypeName
        {
            get
            {
                return TestTrace.TypeName;
            }
            set
            {
                TestTrace.TypeName = value;
                NotifyPropertyChanged(TypeNamePropertyName);
            }
        }
        
    }
}
