using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using TestModelLib;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using DataUtils;

namespace ViewModelBaseLib
{
    public class NFTestStepVM : TestStepVM
    {
        private NFTestStep testStep
        {
            get
            {
                return TestStep as NFTestStep;
            }
        }
                

        private DUTTypeEnum _DUTType;
        private const string DUTTypePropertyName = "DUTType";
        [UIDisplay("被测件类型", null, DUTTypeEnum.放大器)]
        public DUTTypeEnum DUTType
        {
            get
            {
                return _DUTType;
            }
            set
            {
                _DUTType = value;
                NotifyPropertyChanged(DUTTypePropertyName);
            }
        }
        
        private double _StartFreq;
        private const string StartFreqPropertyName = "StartFreq";
        [UIDisplay("起始频率",typeof(FreqStringConverter))]
        public double StartFreq
        {
            get
            {
                return testStep.StartFreq;
            }
            set
            {
                testStep.StartFreq = value;
                NotifyPropertyChanged(StartFreqPropertyName);
            }
        }

        private double _StopFreq;
        private const string StopFreqPropertyName = "StopFreq";
        [UIDisplay("截止频率",typeof(FreqStringConverter))]
        public double StopFreq
        {
            get
            {
                return testStep.StopFreq;
            }
            set
            {
                testStep.StopFreq = value;
                NotifyPropertyChanged(StopFreqPropertyName);
            }
        }
        private string _FreqRange;
        private const string FreqRangePropertyName = "FreqRange";
        public string FreqRange
        {
            get
            {  
                return StartFreq.ToString() + "-" + StopFreq.ToString();
            }
        }
        
        private int _SweepPoints;
        private const string SweepPointsPropertyName = "SweepPoints";
        [UIDisplay("测量点数")]
        public int SweepPoints
        {
            get
            {
                return testStep.SweepPoints;
            }
            set
            {
                testStep.SweepPoints = value;
                NotifyPropertyChanged(SweepPointsPropertyName);
            }
        }

        private double _MeasBandwidth;
        private const string MeasBandwidthPropertyName = "MeasBandwidth";
        [UIDisplay("接收机带宽")]
        public double MeasBandwidth
        {
            get
            {
                return testStep.MeasBandwidth;
            }
            set
            {
                testStep.MeasBandwidth = value;
                NotifyPropertyChanged(MeasBandwidthPropertyName);
            }
        }

        private bool _AverageEnable;
        private const string AverageEnablePropertyName = "AverageEnable";
        [UIDisplay("是否平均")]
        public bool AverageEnable
        {
            get
            {
                return testStep.AverageEnable;
            }
            set
            {
                testStep.AverageEnable = value;
                NotifyPropertyChanged(AverageEnablePropertyName);
            }
        }

        private int _AverageCount;
        private const string AverageCountPropertyName = "AverageCount";
        [UIDisplay("平均次数")]
        public int AverageCount
        {
            get
            {
                return testStep.AverageCount;
            }
            set
            {
                testStep.AverageCount = value;
                NotifyPropertyChanged(AverageCountPropertyName);
            }
        }

        private bool _IsTest;
        private const string IsTestPropertyName = "IsTest";
        public bool IsTest
        {
            get
            {
                return testStep.IsTest;
            }
            set
            {
                testStep.IsTest = value;
                NotifyPropertyChanged(IsTestPropertyName);
            }
        }

        private bool _IsSavePic;
        private const string IsSavePicPropertyName = "IsSavePic";
        public bool IsSavePic
        {
            get
            {
                return testStep.IsSavePic;
            }
            set
            {
                testStep.IsSavePic = value;
                NotifyPropertyChanged(IsSavePicPropertyName);
            }
        }

        private double _LossBeforeDut;
        private const string LossBeforeDutPropertyName = "LossBeforeDut";
        public double LossBeforeDut
        {
            get
            {
                return testStep.CorrData.LossBeforeDut;
            }
            set
            {
                testStep.CorrData.LossBeforeDut = value;
                NotifyPropertyChanged(LossBeforeDutPropertyName);
            }
        }

        private double _LossAfterDut;
        private const string LossAfterDutPropertyName = "LossAfterDut";
        public double LossAfterDut
        {
            get
            {
                return testStep.CorrData.LossAfterDut;
            }
            set
            {
                testStep.CorrData.LossAfterDut = value;
                NotifyPropertyChanged(LossAfterDutPropertyName);
            }
        }

        private bool _LossTableBeforeDutEnable;
        private const string LossTableBeforeDutEnablePropertyName = "LossTableBeforeDutEnable";
        public bool LossTableBeforeDutEnable
        {
            get
            {
                return testStep.CorrData.LossTableBeforeDutEnable;
            }
            set
            {
                testStep.CorrData.LossTableBeforeDutEnable = value;
                NotifyPropertyChanged(LossTableBeforeDutEnablePropertyName);
            }
        }

        private bool _LossTableAfterDutEnable;
        private const string LossTableAfterDutEnablePropertyName = "LossTableAfterDutEnable";
        public bool LossTableAfterDutEnable
        {
            get
            {
                return testStep.CorrData.LossTableAfterDutEnable;
            }
            set
            {
                testStep.CorrData.LossTableAfterDutEnable = value;
                NotifyPropertyChanged(LossTableAfterDutEnablePropertyName);
            }
        }

        private XYDataList _LossTableBeforeDut;
        private const string LossTableBeforeDutPropertyName = "LossTableBeforeDut";
        public XYDataList LossTableBeforeDut
        {
            get
            {
                return testStep.CorrData.LossTableBeforeDut;
            }
            set
            {
                testStep.CorrData.LossTableBeforeDut = value;
                NotifyPropertyChanged(LossTableBeforeDutPropertyName);
            }
        }

        private XYDataList _LossTableAfterDut;
        private const string LossTableAfterDutPropertyName = "LossTableAfterDut";
        public XYDataList LossTableAfterDut
        {
            get
            {
                return testStep.CorrData.LossTableAfterDut;
            }
            set
            {
                testStep.CorrData.LossTableAfterDut = value;
                NotifyPropertyChanged(LossTableAfterDutPropertyName);
            }
        }

        public DUTTypeEnum[] DUTTypeList
        {
            get
            {
                return new DUTTypeEnum[] { DUTTypeEnum.放大器 };
            }
        }

        private bool _CorrectionEnable;
        private const string CorrectionEnablePropertyName = "CorrectionEnable";
        public bool CorrectionEnable
        {
            get
            {
                return testStep.CorrectionEnable;
            }
            set
            {
                testStep.CorrectionEnable = value;
                NotifyPropertyChanged(CorrectionEnablePropertyName);
            }
        }

        private int _CalInterval;
        private const string CalIntervalPropertyName = "CalInterval";
        public int CalInterval
        {
            get
            {
                return testStep.CalInterval;
            }
            set
            {
                testStep.CalInterval = value;
                NotifyPropertyChanged(CalIntervalPropertyName);
            }
        }

        private CalWarningTypeEnum _CalWarning;
        private const string CalWarningPropertyName = "CalWarning";
        public CalWarningTypeEnum CalWarning
        {
            get
            {
                return testStep.CalWarning;
            }
            set
            {
                testStep.CalWarning = value;
                NotifyPropertyChanged(CalWarningPropertyName);
            }
        }

        private bool _IsCalEachTest;
        private const string IsCalEachTestPropertyName = "IsCalEachTest";
        public bool IsCalEachTest
        {
            get
            {
                return testStep.IsCalEachTest;
            }
            set
            {
                testStep.IsCalEachTest = value;
                NotifyPropertyChanged(IsCalEachTestPropertyName);
            }
        }

        public CalWarningTypeEnum[] CalWarningTypeEnumList
        {
            get
            {
                return new CalWarningTypeEnum[] { CalWarningTypeEnum.不提醒, CalWarningTypeEnum.强制校准, CalWarningTypeEnum.提醒校准 };
            }
        }

        public static void TableToExcel(XYDataList lstXYData, string saveFileName)
        {
            if (lstXYData == null) return;

            if (File.Exists(saveFileName))
            {
                File.Delete(saveFileName);
            }

            IWorkbook workbook;
            string type = Path.GetExtension(saveFileName).ToLower();
            if (type == ".xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (type == ".xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                workbook = null;
            }
            if (workbook == null) { return; }

            //sheet名
            ISheet sheetXYData = workbook.CreateSheet("XYData");

            //表头  
            IRow rowHeadXYData = sheetXYData.CreateRow(0);

            ICell cellHeadX = rowHeadXYData.CreateCell(0);
            cellHeadX.SetCellValue("频率");
            ICell cellHeadY = rowHeadXYData.CreateCell(1);
            cellHeadY.SetCellValue("Resp");

            //数据  
            for (int i = 1; i <= lstXYData.lstXYData.Count; i++)
            {
                xyData XYD = lstXYData.lstXYData[i - 1];

                IRow rowXYData = sheetXYData.CreateRow(i);

                ICell cellX = rowXYData.CreateCell(0);
                cellX.SetCellValue(XYD.X.ToString());
                ICell cellY = rowXYData.CreateCell(1);
                cellY.SetCellValue(XYD.Y.ToString());
            }

            //转为字节数组  
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buf = stream.ToArray();

            //保存为Excel文件  
            using (FileStream fs = new FileStream(saveFileName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
        }

        public static XYDataList ExcelToTable(string file)
        {
            if (!File.Exists(file)) return null;
            XYDataList lstXYData = new XYDataList();
            IWorkbook workbook;
            string fileExt = Path.GetExtension(file).ToLower();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                //XSSFWorkbook 适用XLSX格式，HSSFWorkbook 适用XLS格式
                if (fileExt == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else if (fileExt == ".xls")
                {
                    workbook = new HSSFWorkbook(fs);
                }
                else
                {
                    workbook = null;
                }
                if (workbook == null) { return null; }

                ISheet sheetXYData = workbook.GetSheet("XYData");

                for (int i = 1; i <= sheetXYData.LastRowNum - sheetXYData.FirstRowNum; i++)
                {
                    xyData XYD = new xyData();

                    XYD.X = Convert.ToDouble(GetValueType(sheetXYData.GetRow(i).GetCell(0)));
                    XYD.Y = Convert.ToDouble(GetValueType(sheetXYData.GetRow(i).GetCell(1)));

                    lstXYData.lstXYData.Add(XYD);
                }

            }
            return lstXYData;
        }

        private static object GetValueType(ICell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:  
                    return null;
                case CellType.Boolean: //BOOLEAN:  
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:  
                    return cell.NumericCellValue;
                case CellType.String: //STRING:  
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:  
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:  
                default:
                    return "=" + cell.CellFormula;
            }
        }
        
    }
    
}
 
