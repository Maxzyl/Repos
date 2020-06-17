using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelBaseLib
{
    public class PointTestItemVM:TestItemVM
    {
        public PointTestItem PointTestItem
        {
            get
            {
                return (TestItem as PointTestItem);
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
                NotifyPropertyChanged(XMinPropertyName);
                NotifyPropertyChanged(XMaxPropertyName);
                NotifyPropertyChanged(YMinPropertyName);
                NotifyPropertyChanged(YMaxPropertyName);
                NotifyPropertyChanged(XValueJudgeEnablePropertyName);
                NotifyPropertyChanged(YValueJudgeEnablePropertyName);
            }
        }
        private const string XMinPropertyName = "XMin";
        public double? XMin
        {
            get
            {
                return PointTestItem.TestSpecList[SelectedSpecIndex].Limit.XMin;
            }
            set
            {
                PointTestItem.TestSpecList[SelectedSpecIndex].Limit.XMin=value;
                NotifyPropertyChanged(XMinPropertyName);
            }
        }
        private const string XMaxPropertyName = "XMax";
        public double? XMax
        {
            get
            {
                return PointTestItem.TestSpecList[SelectedSpecIndex].Limit.XMax;
            }
            set
            {
                PointTestItem.TestSpecList[SelectedSpecIndex].Limit.XMax = value;
                NotifyPropertyChanged(XMaxPropertyName);
            }
        }
        private const string YMinPropertyName = "YMin";
        public double? YMin
        {
            get
            {
                return PointTestItem.TestSpecList[SelectedSpecIndex].Limit.YMin;
            }
            set
            {
                PointTestItem.TestSpecList[SelectedSpecIndex].Limit.YMin = value;
                NotifyPropertyChanged(YMinPropertyName);
            }
        }
        private const string YMaxPropertyName = "YMax";
        public double? YMax
        {
            get
            {
                return PointTestItem.TestSpecList[SelectedSpecIndex].Limit.YMax;
            }
            set
            {
                PointTestItem.TestSpecList[SelectedSpecIndex].Limit.YMax = value;
                NotifyPropertyChanged(YMaxPropertyName);
            }
        }

        private const string XValueJudgeEnablePropertyName = "XValueJudgeEnable";
        public bool XValueJudgeEnable
        {
            get
            {
                return PointTestItem.TestSpecList[SelectedSpecIndex].Limit.XValueJudgeEnable;
            }
            set
            {
                PointTestItem.TestSpecList[SelectedSpecIndex].Limit.XValueJudgeEnable = value;
                NotifyPropertyChanged(XValueJudgeEnablePropertyName);
            }
        }
        private const string YValueJudgeEnablePropertyName = "YValueJudgeEnable";
        public bool YValueJudgeEnable
        {
            get
            {
                return PointTestItem.TestSpecList[SelectedSpecIndex].Limit.YValueJudgeEnable;
            }
            set
            {
                PointTestItem.TestSpecList[SelectedSpecIndex].Limit.YValueJudgeEnable = value;
                NotifyPropertyChanged(YValueJudgeEnablePropertyName);
            }
        }

        private const string LimitDescriptionPropertyName = "LimitDescription";
        public string LimitDescription
        {
            get
            {
                if (SelectedSpecIndex < 0)
                {
                    return PointTestItem.TestSpecList[0].LimitDescription;
                }
                else
                {
                    return PointTestItem.TestSpecList[SelectedSpecIndex].LimitDescription;
                }
            }
            set
            {
                if (SelectedSpecIndex >= 0)
                {
                    PointTestItem.TestSpecList[SelectedSpecIndex].LimitDescription = value;
                    NotifyPropertyChanged(LimitDescriptionPropertyName);
                }
            }
        }
    }
}
