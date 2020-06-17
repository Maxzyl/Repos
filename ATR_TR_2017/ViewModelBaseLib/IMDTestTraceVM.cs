using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModelBaseLib;
using TestModelLib;

namespace ViewModelBaseLib
{
    public class IMDTestTraceVM : TestTraceVM
    {
        public IMDTestTraceVM()
        {
            TypeNameList = new string[] { IMDTestTraceType.IMD, IMDTestTraceType.IIP3 };
        }

        public string[] TypeNameList { get; set; }

        //private string _DisplayName;
        //private const string DisplayNamePropertyName = "DisplayName";
        //public string DisplayName
        //{
        //    get
        //    {
        //        return TestTrace.UserDefName;
        //    }
        //    set
        //    {
        //        TestTrace.UserDefName = value;
        //        NotifyPropertyChanged(DisplayNamePropertyName);
        //    }
        //}

        //private string _TypeName;
        //private const string TypeNamePropertyName = "TypeName";
        //public string TypeName
        //{
        //    get
        //    {
        //        return TestTrace.TypeName;
        //    }
        //    set
        //    {
        //        TestTrace.TypeName = value;
        //        NotifyPropertyChanged(TypeNamePropertyName);
        //    }
        //}

        //private const string CompensationPropertyName = "Compensation";
        //public double Compensation
        //{
        //    get
        //    {
        //        return TestTrace.Compensation;
        //    }
        //    set
        //    {
        //        TestTrace.Compensation = value;
        //        NotifyPropertyChanged(CompensationPropertyName);
        //    }
        //}

        //private const string IsAutoScalePropertyName = "IsAutoScale";
        //public bool IsAutoScale
        //{
        //    get
        //    {
        //        return TestTrace.IsAutoScale;
        //    }
        //    set
        //    {
        //        TestTrace.IsAutoScale = value;
        //        NotifyPropertyChanged(IsAutoScalePropertyName);
        //    }
        //}

        //private const string RefValuePropertyName = "RefValue";
        //public double RefValue
        //{
        //    get
        //    {
        //        return TestTrace.RefValue;
        //    }
        //    set
        //    {
        //        TestTrace.RefValue = value;
        //        NotifyPropertyChanged(RefValuePropertyName);
        //    }
        //}

        //private const string ScalePropertyName = "Scale";
        //public double Scale
        //{
        //    get
        //    {
        //        return TestTrace.Scale;
        //    }
        //    set
        //    {
        //        TestTrace.Scale = value;
        //        NotifyPropertyChanged(ScalePropertyName);
        //    }
        //}

        //private const string DivCountPropertyName = "DivCount";
        //public double DivCount
        //{
        //    get
        //    {
        //        return TestTrace.DivCount;
        //    }
        //    set
        //    {
        //        TestTrace.DivCount = value;
        //        NotifyPropertyChanged(DivCountPropertyName);
        //    }
        //}

        //private const string IsSaveImagePropertyName = "IsSaveImage";
        //public bool IsSaveImage
        //{
        //    get
        //    {
        //        return TestTrace.IsSaveImage;
        //    }
        //    set
        //    {
        //        TestTrace.IsSaveImage = value;
        //        NotifyPropertyChanged(IsSaveImagePropertyName);
        //    }
        //}

        //private const string IsTestPropertyName = "IsTest";
        //public bool IsTest
        //{
        //    get
        //    {
        //        return TestTrace.IsTest;
        //    }
        //    set
        //    {
        //        TestTrace.IsTest = value;
        //        NotifyPropertyChanged(IsTestPropertyName);
        //    }
        //}

    }
}

