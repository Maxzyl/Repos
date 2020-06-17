using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModelBaseLib;
using TestModelLib;
using Symtant.InstruDriver.AISG;

namespace ViewModelBaseLib
{
    public class DUTTestStepVM : TestStepVM
    {
        public DUTTestStep testStep
        {
            get
            {
                return TestStep as DUTTestStep;
            }
        }

        public string[] MeasInfoDisplayNameList
        {
            get
            {
                return testStep.MeasInfoDisplayNameList;
            }
        }

        private const string SubNumPropertyName = "SubNum";
        public int SubNum
        {
            get
            {
                return testStep.SubNum;
            }
            set
            {
                testStep.SubNum = value;
                NotifyPropertyChanged(SubNumPropertyName);
            }
        }

        private const string ModelPropertyName = "Model";
        public AISGTMAModeType Model
        {
            get
            {
                return testStep.Mode;
            }
            set
            {
                testStep.Mode = value;
                NotifyPropertyChanged(ModelPropertyName);
            }
        }

        private const string GainPropertyName = "Gain";
        public double Gain
        {
            get
            {
                return testStep.Gain;
            }
            set
            {
                testStep.Gain = value;
                NotifyPropertyChanged(GainPropertyName);
            }
        }

        private const string VendorFlagPropertyName = "VendorFlag";
        public VendorFlagEnum VendorFlag
        {
            get
            {
                return testStep.VendorFlag;
            }
            set
            {
                testStep.VendorFlag = value;
                NotifyPropertyChanged(VendorFlagPropertyName);
            }
        }

        private const string AISGVersionPropertyName = "AISGVersion";
        public string AISGVersion
        {
            get
            {
                return testStep.AISGVersion;
            }
            set
            {
                testStep.AISGVersion = value;
                NotifyPropertyChanged(AISGVersionPropertyName);
            }
        }

        private const string WarningCurrentPropertyName = "WarningCurrent";
        public string WarningCurrent
        {
            get
            {
                return testStep.WarningCurrent;
            }
            set
            {
                testStep.WarningCurrent = value;
                NotifyPropertyChanged(WarningCurrentPropertyName);
            }
        }

        public int[] SubNumList
        {
            get
            {
                return new int[] { 1, 2};
            }
        }

        public AISGTMAModeType[] ModelList
        {
            get
            {
                return new AISGTMAModeType[] { AISGTMAModeType.Normal, AISGTMAModeType.Bypass };
            }
        }

        public double[] GainList
        {
            get
            {
                return new double[] { 8.0, 8.5, 9.0, 9.5, 10.0, 10.5, 11.0, 11.5, 12.0 };
            }
        }

        public VendorFlagEnum[] VendorFlagList
        {
            get
            {
                return new VendorFlagEnum[] { VendorFlagEnum.HW};
            }
        }

        public string[] AISGVersionList
        {
            get
            {
                return new string[] { "AISG1.1", "AISG2.0" };
            }
        }

        public string[] WarningCurrentList
        {
            get
            {
                return new string[] { "180", "250" };
            }
        }



    }
}
