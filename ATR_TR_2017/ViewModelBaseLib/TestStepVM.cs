using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Collections.ObjectModel;

using DevExpress.Mvvm;
namespace ViewModelBaseLib
{
    public class TestStepVM:NotifyBase
    {

        private string _ConnName;
        private const string ConnNamePropertyName = "ConnName";
        public string ConnName
        {
            get
            {
                return _ConnName;
            }
            set
            {
                _ConnName = value;
                NotifyPropertyChanged(ConnNamePropertyName);
            }
        }
        private TestStep _TestStep;
        private const string TestStepPropertyName = "TestStep";
        public TestStep TestStep
        {
            get
            {
                return _TestStep;
            }
            set
            {
                _TestStep = value;
                NotifyPropertyChanged(TestStepPropertyName);
            }
        }

        public string[] MeasInfoDisplayNameList
        {
            get
            {
                return TestStep.MeasInfoDisplayNameList;
            }
        }

        //private int _SelectedMeasInfoIndex;
        //private const string SelectedMeasInfoIndexPropertyName = "SelectedMeasInfoIndex";
        //public int SelectedMeasInfoIndex
        //{
        //    get
        //    {
        //        return TestStep.SelectedMeasInfoIndex;
        //    }
        //    set
        //    {
        //        TestStep.SelectedMeasInfoIndex = value;
        //        NotifyPropertyChanged(SelectedMeasInfoIndexPropertyName);
        //        NotifyPropertyChanged(MeasInfoDisplayNamePropertyName);
        //    }
        //}
        private string _MeasInfoDisplayName;
        private const string MeasInfoDisplayNamePropertyName = "MeasInfoDisplayName";
        public string MeasInfoDisplayName
        {
            get
            {
             
                return TestStep.MeasInfoDisplayName;
            }     

            set
            {
                TestStep.MeasInfoDisplayName = value;
                NotifyPropertyChanged(MeasInfoDisplayNamePropertyName);
            }
        }
        private string _DisplayName;
        private const string DisplayNamePropertyName = "DisplayName";
        public string DisplayName
        {
            get
            {
                return TestStep.Name;
            }
            set
            {
                TestStep.Name = value;
                NotifyPropertyChanged(DisplayNamePropertyName);
            }
        }
        private string _ConfigName;
        private const string ConfigNamePropertyName = "ConfigName";
        public string ConfigName
        {
            get
            {
                if (TestStep != null)
                {
                    return TestStep.PathConfigName;
                }
                else
                {
                    return "";
                }

            }
            set
            {
                if (TestStep != null)
                {
                    TestStep.PathConfigName = value;
                    NotifyPropertyChanged(ConfigNamePropertyName);
                }
            }
        }
        public string[] ConfigNameList
        {
            get
            {
                if (TestStep == null)
                {
                    return null;
                }
                else
                {
                    return TestStep.PathConfigNameList;
                }
            }
        }
    }
}
