using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestResultMarkerDip
{
    public class XYMarkerDisp:NotifyBase
    {
        private string _PortName;
        private const string PortNamePropertyName = "PortName";
        public string PortName
        {
            get
            {
                return _PortName;
            }
            set
            {
                _PortName = value;
                NotifyPropertyChanged(PortNamePropertyName);
            }
        }
        private string _UserDefName;
        private const string UserDefNamePropertyName = "UserDefName";
        public string UserDefName
        {
            get
            {
                return _UserDefName;
            }
            set
            {
                _UserDefName = value;
                NotifyPropertyChanged(UserDefNamePropertyName);
            }
        }

        private double _Freq;
        private const string FreqPropertyName = "Freq";
        public double Freq
        {
            get
            {
                return _Freq;
            }
            set
            {
                _Freq = value;
                NotifyPropertyChanged(FreqPropertyName);
            }
        }
        private string _SpecDesc;
        private const string SpecDescPropertyName = "SpecDesc";
        public string SpecDesc
        {
            get
            {
                return _SpecDesc;
            }
            set
            {
                _SpecDesc = value;
                NotifyPropertyChanged(SpecDescPropertyName);
            }
        }
        private string _XDescription;
        private const string XDescriptionPropertyName = "XDescription";
        public string XDescription
        {
            get
            {
                return _XDescription;
            }
            set
            {
                _XDescription = value;
                NotifyPropertyChanged(XDescriptionPropertyName);
            }
        }
        private string _LimitDescription;
        private const string LimitDescriptionPropertyName = "LimitDescription";
        public string LimitDescription
        {
            get
            {
                return _LimitDescription;
            }
            set
            {
                _LimitDescription = value;
                NotifyPropertyChanged(LimitDescriptionPropertyName);
            }
        }
        
        private double _TestResult;
        private const string TestResultPropertyName = "TestResult";
        public double TestResult
        {
            get
            {
                return _TestResult;
            }
            set
            {
                _TestResult = value;
                NotifyPropertyChanged(TestResultPropertyName);
            }
        }
        private bool? _PassFail;
        private const string PassFailPropertyName = "PassFail";
        public bool? PassFail
        {
            get
            {
                return _PassFail;
            }
            set
            {
                _PassFail = value;
                NotifyPropertyChanged(PassFailPropertyName);
            }
        }
        public int ConnIndex { get; set; }
        public int StepIndex { get; set; }
        public int SpecIndex { get; set; }
        public int TraceIndex { get; set; }
        public int MarkerIndex { get; set; }
    }
}
