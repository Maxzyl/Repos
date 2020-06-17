using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using ModelBaseLib;
using PIMModelLib;
namespace ViewModelBaseLib
{
    public class PIMTestStepVM:TestStepVM
    {
        const string PIMParamInfoFileName = "./configfiles/PIMParamInfo.xml";

        public PIMTestStep testStep
        {
            get
            {
                return TestStep as PIMTestStep; 
            }
        }

        public string[] MeasInfoDisplayNameList
        {
            get
            {
                return testStep.MeasInfoDisplayNameList;
            }
        }
        private int _SelectedMeasInfoIndex;
        private const string SelectedMeasInfoIndexPropertyName = "SelectedMeasInfoIndex";
        public int SelectedMeasInfoIndex
        {
            get
            {
                return testStep.SelectedMeasInfoIndex;
            }
            set
            {
                testStep.SelectedMeasInfoIndex = value;
                NotifyPropertyChanged(SelectedMeasInfoIndexPropertyName);
            }
        }

        public void setFreqRefresh()
        {
            if (CWFreq1 < TXFreq1 || CWFreq1 > TXFreq2
                            || CWFreq2 < TXFreq1 || CWFreq2 > TXFreq2
                            || StartFreq1 < TXFreq1 || StartFreq1 > TXFreq2
                            || StartFreq2 < TXFreq1 || StartFreq2 > TXFreq2
                            || StopFreq1 < TXFreq1 || StopFreq1 > TXFreq2
                            || StopFreq2 < TXFreq1 || StopFreq2 > TXFreq2)
            {
                int num = PIMOrder / 2;

                double dblIMF = (((num + 1) * CWFreq1) - (num * CWFreq2));

                CWFreq1 = TXFreq1;
                CWFreq2 = TXFreq2;
                StartFreq1 = TXFreq1;
                StopFreq1 = (num * TXFreq2 + RXFreq2) / (num + 1);
                StartFreq2 = ((num + 1) * TXFreq1 - RXFreq2) / num;
                StopFreq2 = TXFreq2;
            }
        }

        private string _FreqRange;
        private const string FreqRangePropertyName = "FreqRange";
        public string FreqRange
        {
            get
            {
                return testStep.FreqRange;
            }
            set
            {
                testStep.FreqRange = value;
                NotifyPropertyChanged(FreqRangePropertyName);
                if (value != "")
                {
                    List<double> lstFreq = LoadFreqRangeInfoFromXml(value);
                    if (lstFreq.Count == 4)
                    {
                        TXFreq1 = lstFreq[0] * 1000000;
                        TXFreq2 = lstFreq[1] * 1000000;
                        RXFreq1 = lstFreq[2] * 1000000;
                        RXFreq2 = lstFreq[3] * 1000000;

                        if (!IsFreqEnable)
                        {
                            return;
                        }

                        setFreqRefresh();

                    }
                }

            }
        }

        private double _TXFreq1;
        private const string TXFreq1PropertyName = "TXFreq1";
        public double TXFreq1
        {
            get
            {
                return testStep.TXFreq1;
            }
            set
            {
                testStep.TXFreq1 = value;
                NotifyPropertyChanged(TXFreq1PropertyName);
            }
        }

        private double _TXFreq2;
        private const string TXFreq2PropertyName = "TXFreq2";
        public double TXFreq2
        {
            get
            {
                return testStep.TXFreq2;
            }
            set
            {
                testStep.TXFreq2 = value;
                NotifyPropertyChanged(TXFreq2PropertyName);
            }
        }

        private double _RXFreq1;
        private const string RXFreq1PropertyName = "RXFreq1";
        public double RXFreq1
        {
            get
            {
                return testStep.RXFreq1;
            }
            set
            {
                testStep.RXFreq1 = value;
                NotifyPropertyChanged(RXFreq1PropertyName);
            }
        }

        private double _RXFreq2;
        private const string RXFreq2PropertyName = "RXFreq2";
        public double RXFreq2
        {
            get
            {
                return testStep.RXFreq2;
            }
            set
            {
                testStep.RXFreq2 = value;
                NotifyPropertyChanged(RXFreq2PropertyName);
            }
        }


        private double _CWFreq1;
        private const string CWFreq1PropertyName = "CWFreq1";
        public double CWFreq1
        {
            get
            {
                return testStep.CWFreq1;
            }
            set
            {
                testStep.CWFreq1 = value;
                NotifyPropertyChanged(CWFreq1PropertyName);
            }
        }

        private double _CWFreq2;
        private const string CWFreq2PropertyName = "CWFreq2";
        public double CWFreq2
        {
            get
            {
                return testStep.CWFreq2;
            }
            set
            {
                testStep.CWFreq2 = value;
                NotifyPropertyChanged(CWFreq2PropertyName);
            }
        }

        private double _CWPOW1;
        private const string CWPOW1PropertyName = "CWPOW1";
        public double CWPOW1
        {
            get
            {
                return testStep.CWPOW1;
            }
            set
            {
                testStep.CWPOW1 = value;
                NotifyPropertyChanged(CWPOW1PropertyName);
            }
        }

        private double _CWPOW2;
        private const string CWPOW2PropertyName = "CWPOW2";
        public double CWPOW2
        {
            get
            {
                return testStep.CWPOW2;
            }
            set
            {
                testStep.CWPOW2 = value;
                NotifyPropertyChanged(CWPOW2PropertyName);
            }
        }

        private double _TestStepper;
        private const string TestStepperPropertyName = "TestStepper";
        public double TestStepper
        {
            get
            {
                return testStep.TestStepper;
            }
            set
            {
                testStep.TestStepper = value;
                NotifyPropertyChanged(TestStepperPropertyName);
            }
        }

        private double _TestTime;
        private const string TestTimePropertyName = "TestTime";
        public double TestTime
        {
            get
            {
                return testStep.TestTime;
            }
            set
            {
                testStep.TestTime = value;
                NotifyPropertyChanged(TestTimePropertyName);
            }
        }

        private double _StartFreq1;
        private const string StartFreq1PropertyName = "StartFreq1";
        public double StartFreq1
        {
            get
            {
                return testStep.StartFreq1;
            }
            set
            {
                testStep.StartFreq1 = value;
                NotifyPropertyChanged(StartFreq1PropertyName);
            }
        }

        private double _StopFreq1;
        private const string StopFreq1PropertyName = "StopFreq1";
        public double StopFreq1
        {
            get
            {
                return testStep.StopFreq1;
            }
            set
            {
                testStep.StopFreq1 = value;
                NotifyPropertyChanged(StopFreq1PropertyName);
            }
        }

        private double _StartFreq2;
        private const string StartFreq2PropertyName = "StartFreq2";
        public double StartFreq2
        {
            get
            {
                return testStep.StartFreq2;
            }
            set
            {
                testStep.StartFreq2 = value;
                NotifyPropertyChanged(StartFreq2PropertyName);
            }
        }

        private double _StopFreq2;
        private const string StopFreq2PropertyName = "StopFreq2";
        public double StopFreq2
        {
            get
            {
                return testStep.StopFreq2;
            }
            set
            {
                testStep.StopFreq2 = value;
                NotifyPropertyChanged(StopFreq2PropertyName);
            }
        }

        private PIMMeasMode _MeasMode;
        private const string MeasModePropertyName = "MeasMode";
        public PIMMeasMode MeasMode
        {
            get
            {
                return testStep.MeasMode;
            }
            set
            {
                testStep.MeasMode = value;
                NotifyPropertyChanged(MeasModePropertyName);
            }
        }

        private int _PIMOrder;
        private const string PIMOrderPropertyName = "PIMOrder";
        public int PIMOrder
        {
            get
            {
                return testStep.PIMOrder;
            }
            set
            {
                testStep.PIMOrder = value;
                NotifyPropertyChanged(PIMOrderPropertyName);
            }
        }

        private PIMSidebandTypeEnum _SideBand;
        private const string SideBandPropertyName = "SideBand";
        public PIMSidebandTypeEnum SideBand
        {
            get
            {
                return testStep.SideBand;
            }
            set
            {
                testStep.SideBand = value;
                NotifyPropertyChanged(SideBandPropertyName);
            }
        }

        private PIMCalType _CalType;
        private const string CalTypePropertyName = "CalType";
        public PIMCalType CalType
        {
            get
            {
                return testStep.CalType;
            }
            set
            {
                testStep.CalType = value;
                NotifyPropertyChanged(CalTypePropertyName);
            }
        }

        private bool _isEnable;
        private const string isEnablePropertyName = "isEnable";
        public bool isEnable
        {
            get
            {
                return testStep.isEnable;
            }
            set
            {
                testStep.isEnable = value;
                NotifyPropertyChanged(isEnablePropertyName);
            }
        }

        private bool _isImageSave;
        private const string isImageSavePropertyName = "isImageSave";
        public bool isImageSave
        {
            get
            {
                return testStep.isImageSave;
            }
            set
            {
                testStep.isImageSave = value;
                NotifyPropertyChanged(isImageSavePropertyName);
            }
        }

        private bool _IsFreqEnable;
        private const string IsFreqEnablePropertyName = "IsFreqEnable";
        public bool IsFreqEnable
        {
            get
            {
                return _IsFreqEnable;
            }
            set
            {
                _IsFreqEnable = value;
                NotifyPropertyChanged(IsFreqEnablePropertyName);
            }
        }

        private Port _PortName;
        private const string PortNamePropertyName = "PortName";
        public Port PortName
        {
            get
            {
                return testStep.PortName;
            }
            set
            {
                testStep.PortName = value;
                NotifyPropertyChanged(PortNamePropertyName);
            }
        }

        private List<double> LoadFreqRangeInfoFromXml(string strRootNode)
        {
            List<double> lstFreqRange = new List<double>();
            if (strRootNode == null || strRootNode=="")
            {
                return lstFreqRange;
            }
            
            if (File.Exists(PIMParamInfoFileName))
            {
                XElement rootEle = XElement.Load(PIMParamInfoFileName);
                if (rootEle != null)
                {
                    foreach (XElement ele in rootEle.Element(strRootNode).Elements())
                    {
                        string instruName = ele.Value;
                        lstFreqRange.Add(Convert.ToDouble(instruName));
                    }
                }
            }
            return lstFreqRange;
        }

        private List<string> LoadFreqRangeInfoListFromXml()
        {
            List<string> lstFreqRange = new List<string>();
            if (File.Exists(PIMParamInfoFileName))
            {
                XElement rootEle = XElement.Load(PIMParamInfoFileName);
                if (rootEle != null)
                {
                    //foreach (XElement ele in rootEle.Elements("FreqRange"))
                    //{
                    //    string instruName = ele.Element("Value").Value;
                    //    lstFreqRange.Add(instruName);
                    //}

                    foreach (XElement ele in rootEle.Element("BANDS").Elements("Band"))
                    {
                        string instruName = ele.Value;
                        lstFreqRange.Add(instruName);
                    }
                }
            }
            return lstFreqRange;
        }

        public string[] FreqRangeList
        {
            get
            {
                return LoadFreqRangeInfoListFromXml().ToArray();
            }
        }

        public PIMMeasMode[] PIMMeasModeList
        {
            get
            {
                return new PIMMeasMode[] { PIMMeasMode.REFL, PIMMeasMode.TRAN};
            }
        }

        public int[] PIMOrderList
        {
            get
            {
                return new int[] { 3, 5, 7, 9, 11, 13, 15 };
            }
        }

        public PIMSidebandTypeEnum[] PIMSidebandTypeEnumList
        {
            get
            {
                return new PIMSidebandTypeEnum[] { PIMSidebandTypeEnum.Low, PIMSidebandTypeEnum.High, PIMSidebandTypeEnum.Dual };
            }
        }

        public PIMCalType[] PIMCalTypeList
        {
            get
            {
                return new PIMCalType[] { PIMCalType.Point, PIMCalType.Sweep, PIMCalType.TimeDomain };
            }
        }

        public ResultUnit[] ResultUnitList
        {
            get
            {
                return new ResultUnit[] { ResultUnit.dBm, ResultUnit.dBc };
            }
        }

        public Port[] PortList
        {
            get
            {
                return new Port[] { Port.Port1, Port.Port2 };
            }
        }
    }
}
