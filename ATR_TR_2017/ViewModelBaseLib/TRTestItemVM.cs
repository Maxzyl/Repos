using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
namespace ViewModelBaseLib
{
    public class TRTestItemVM:TestItemVM
    {
        public TRTestItem TRTestItem
        {
            get
            {
                return (TestItem as TRTestItem);
            }
        }
        
        private const string HighLimitPropertyName = "HighLimit";
        public string HighLimit
        {
            get
            {
                if (SelectedSpecIndex < 0)
                {
                    return TRTestItem.TestSpecList[0].HighLimit;
                }
                else
                {
                    return TRTestItem.TestSpecList[SelectedSpecIndex].HighLimit;
                }
            }
            set
            {
                if (SelectedSpecIndex >= 0)
                {
                    TRTestItem.TestSpecList[SelectedSpecIndex].HighLimit = value;
                    NotifyPropertyChanged(HighLimitPropertyName);
                }
            }
        }
        
        private const string LowLimitPropertyName = "LowLimit";
        public string LowLimit
        {
            get
            {
                if (SelectedSpecIndex < 0)
                {
                    return TRTestItem.TestSpecList[0].LowLimit;
                }
                else
                {
                    return TRTestItem.TestSpecList[SelectedSpecIndex].LowLimit;
                }
            }
            set
            {
                if (SelectedSpecIndex >= 0)
                {
                    TRTestItem.TestSpecList[SelectedSpecIndex].LowLimit = value;
                    NotifyPropertyChanged(LowLimitPropertyName);
                }
            }
        }


        private int _SelectedSpecIndex;
        private const string SelectedSpecIndexPropertyName = "SelectedSpecIndex";
        public int SelectedSpecIndex
        {
            get
            {
                return _SelectedSpecIndex;
            }
            set
            {
                _SelectedSpecIndex = value;
                NotifyPropertyChanged(SelectedSpecIndexPropertyName);
                NotifyPropertyChanged(HighLimitPropertyName);
                NotifyPropertyChanged(LowLimitPropertyName);
            }
        }
        private const string LimitDescriptionPropertyName = "LimitDescription";
        public string LimitDescription
        {
            get
            {
                if (SelectedSpecIndex < 0)
                {
                    return TRTestItem.TestSpecList[0].LimitDescription;
                }
                else
                {
                    return TRTestItem.TestSpecList[SelectedSpecIndex].LimitDescription;
                }
            }
            set
            {
                if (SelectedSpecIndex >= 0)
                {
                    TRTestItem.TestSpecList[SelectedSpecIndex].LimitDescription = value;
                    NotifyPropertyChanged(LimitDescriptionPropertyName);
                }
            }
        }
    }
}
