using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModelBaseLib;
using TestModelLib;

namespace ViewModelBaseLib
{
    public class DCPSTestStepVM : TestStepVM
    {
        public DCPSTestStep testStep
        {
            get
            {
                return TestStep as DCPSTestStep;
            }
        }

        private OutTypeEnum _Info2OutType;
        private const string Info2OutTypePropertyName = "Info2OutType";
        public OutTypeEnum Info2OutType
        {
            get
            {
                return DCPSInfo2.OutType;
            }
            set
            {
                DCPSInfo2.OutType = value;
                NotifyPropertyChanged(Info2OutTypePropertyName);
            }
        }
        private double _Info2Voltage;
        private const string Info2VoltagePropertyName = "Info2Voltage";
        public double Info2Voltage
        {
            get
            {
                return DCPSInfo2.Voltage;
            }
            set
            {
                DCPSInfo2.Voltage = value;
                NotifyPropertyChanged(Info2VoltagePropertyName);
            }
        }
        private double _Info2Current;
        private const string Info2CurrentPropertyName = "Info2Current";
        public double Info2Current
        {
            get
            {
                return DCPSInfo2.Current;
            }
            set
            {
                DCPSInfo2.Current = value;
                NotifyPropertyChanged(Info2CurrentPropertyName);
            }
        }
        private bool _Info2isAutoOff;
        private const string Info2isAutoOffPropertyName = "Info2isAutoOff";
        public bool Info2isAutoOff
        {
            get
            {
                return DCPSInfo2.IsAutoOff;
            } 
            set
            {
                DCPSInfo2.IsAutoOff = value;
                NotifyPropertyChanged(Info2isAutoOffPropertyName);
            }
        }

        private bool _Info2isTest;
        private const string Info2isTestPropertyName = "Info2isTest";
        public bool Info2isTest
        {
            get
            {
                return DCPSInfo2.IsTest;
            }
            set
            {
                testStep.DCPSInfo2.IsTest = value;
                NotifyPropertyChanged(Info2isTestPropertyName);
            }
        }
        private OutTypeEnum _Info3OutType;
        private const string Info3OutTypePropertyName = "Info3OutType";
        public OutTypeEnum Info3OutType
        {
            get
            {
                return DCPSInfo3.OutType;
            }
            set
            {
                DCPSInfo3.OutType = value;
                NotifyPropertyChanged(Info3OutTypePropertyName);
            }
        }
        private double _Info3Voltage;
        private const string Info3VoltagePropertyName = "Info3Voltage";
        public double Info3Voltage
        {
            get
            {
                return DCPSInfo3.Voltage;
            }
            set
            {
                DCPSInfo3.Voltage = value;
                NotifyPropertyChanged(Info3VoltagePropertyName);
            }
        }
        private double _Info3Current;
        private const string Info3CurrentPropertyName = "Info3Current";
        public double Info3Current
        {
            get
            {
                return DCPSInfo3.Current;
            }
            set
            {
                DCPSInfo3.Current = value;
                NotifyPropertyChanged(Info3CurrentPropertyName);
            }
        }
        private bool _Info3isAutoOff;
        private const string Info3isAutoOffPropertyName = "Info1isAutoOff";
        public bool Info3isAutoOff
        {
            get
            {
                return DCPSInfo3.IsAutoOff;
            }
            set
            {
                DCPSInfo3.IsAutoOff = value;
                NotifyPropertyChanged(Info3isAutoOffPropertyName);
            }
        }

        private bool _Info3isTest;
        private const string Info3isTestPropertyName = "Info1isTest";
        public bool Info3isTest
        {
            get
            {
                return DCPSInfo3.IsTest;
            }
            set
            {
                DCPSInfo3.IsTest = value;
                NotifyPropertyChanged(Info3isTestPropertyName);
            }
        }
        private OutTypeEnum _Info4OutType;
        private const string Info4OutTypePropertyName = "Info4OutType";
        public OutTypeEnum Info4OutType
        {
            get
            {
                return DCPSInfo4.OutType;
            }
            set
            {
                DCPSInfo4.OutType = value;
                NotifyPropertyChanged(Info4OutTypePropertyName);
            }
        }
        private double _Info4Voltage;
        private const string Info4VoltagePropertyName = "Info4Voltage";
        public double Info4Voltage
        {
            get
            {
                return DCPSInfo4.Voltage;
            }
            set
            {
                DCPSInfo4.Voltage = value;
                NotifyPropertyChanged(Info4VoltagePropertyName);
            }
        }
        private double _Info4Current;
        private const string Info4CurrentPropertyName = "Info4Current";
        public double Info4Current
        {
            get
            {
                return DCPSInfo4.Current;
            }
            set
            {
                DCPSInfo4.Current = value;
                NotifyPropertyChanged(Info4CurrentPropertyName);
            }
        }
        private bool _Info4isAutoOff;
        private const string Info4isAutoOffPropertyName = "Info4isAutoOff";
        public bool Info4isAutoOff
        {
            get
            {
                return DCPSInfo4.IsAutoOff;
            }
            set
            {
                DCPSInfo4.IsAutoOff = value;
                NotifyPropertyChanged(Info4isAutoOffPropertyName);
            }
        }

        private bool _Info4isTest;
        private const string Info4isTestPropertyName = "Info4isTest";
        public bool Info4isTest
        {
            get
            {
                return DCPSInfo4.IsTest;
            }
            set
            {
                DCPSInfo4.IsTest = value;
                NotifyPropertyChanged(Info4isTestPropertyName);
            }
        }
        private OutTypeEnum _Info1OutType;
        private const string Info1OutTypePropertyName = "Info1OutType";
        public OutTypeEnum Info1OutType
        {
            get
            {
                return DCPSInfo1.OutType;
            }
            set
            {
                DCPSInfo1.OutType= value;
                NotifyPropertyChanged(Info1OutTypePropertyName);
            }
        }
        private double _Info1Voltage;
        private const string Info1VoltagePropertyName = "Info1Voltage";
        public double Info1Voltage
        {
            get
            {
                return DCPSInfo1.Voltage;
            }
            set
            {
                DCPSInfo1.Voltage = value;
                NotifyPropertyChanged(Info1VoltagePropertyName);
            }
        }
        private double _Info1Current;
        private const string Info1CurrentPropertyName = "Info1Current";
        public double Info1Current
        {
            get
            {
                return DCPSInfo1.Current;
            }
            set
            {
                DCPSInfo1.Current = value;
                NotifyPropertyChanged(Info1CurrentPropertyName);
            }
        }
        private bool _Info1isAutoOff;
        private const string Info1isAutoOffPropertyName = "Info1isAutoOff";
        public bool Info1isAutoOff
        {
            get
            {
                return DCPSInfo1.IsAutoOff;
            }
            set
            {
                DCPSInfo1.IsAutoOff = value;
                NotifyPropertyChanged(Info1isAutoOffPropertyName);
            }
        }

        private bool _Info1isTest;
        private const string Info1isTestPropertyName = "Info1isTest";
        public bool Info1isTest
        {
            get
            {
                return DCPSInfo1.IsTest;
            }
            set
            {
                DCPSInfo1.IsTest = value;
                NotifyPropertyChanged(Info1isTestPropertyName);
            }
        }
        private DCPSInfo _DCPSInfo1=new DCPSInfo();
        private const string DCPSInfo1PropertyName = "DCPSInfo1";
        public DCPSInfo DCPSInfo1
        {
            get
            {
                return testStep.DCPSInfo1;
            }
            set
            {
                testStep.DCPSInfo1 = value;
                NotifyPropertyChanged(DCPSInfo1PropertyName);
            }
        }

        private DCPSInfo _DCPSInfo2=new DCPSInfo();
        private const string DCPSInfo2PropertyName = "DCPSInfo2";
        public DCPSInfo DCPSInfo2
        {
            get
            {
                return testStep.DCPSInfo2;
            }
            set
            {
                testStep.DCPSInfo2 = value;
                NotifyPropertyChanged(DCPSInfo2PropertyName);
            }
        }

        private DCPSInfo _DCPSInfo3=new DCPSInfo();
        private const string DCPSInfo3PropertyName = "DCPSInfo3";
        public DCPSInfo DCPSInfo3
        {
            get
            {
                return testStep.DCPSInfo3;
            }
            set
            {
                testStep.DCPSInfo3 = value;
                NotifyPropertyChanged(DCPSInfo3PropertyName);
            }
        }

        private DCPSInfo _DCPSInfo4=new DCPSInfo();
        private const string DCPSInfo4PropertyName = "DCPSInfo4";
        public DCPSInfo DCPSInfo4
        {
            get
            {
                return testStep.DCPSInfo4;
            }
            set
            {
                testStep.DCPSInfo4 = value;
                NotifyPropertyChanged(DCPSInfo4PropertyName);
            }
        }

        public OutTypeEnum[] OutTypeEnumList
        {
            get
            {
                return new OutTypeEnum[] { OutTypeEnum.电压, OutTypeEnum.电流 };
            }
        }
    }
}
