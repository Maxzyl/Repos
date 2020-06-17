using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;


using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.ObjectModel;

namespace ViewModelBaseLib
{
    public class TestTraceVM:TestItemVM
    {

        private TestTrace _TestTrace;
        private const string TestTracePropertyName = "TestTrace";
        public TestTrace TestTrace
        {
            get
            {
                return TestItem as TestTrace;
            }
            
        }
        private int _TestSpecIndex;
        private const string TestSpecIndexPropertyName = "TestSpecIndex";
        public int TestSpecIndex
        {
            get
            {
                return _TestSpecIndex;
            }
            set
            {
                _TestSpecIndex = value;
                NotifyPropertyChanged(TestLimitPropertyName);
                NotifyPropertyChanged(TestSpecIndexPropertyName);
                NotifyPropertyChanged(UpLimitPropertyName);
                NotifyPropertyChanged(LowLimitPropertyName);
                NotifyPropertyChanged(LimitDescriptionPropertyName);
            }
        }

        private XYTestLimit _TestLimit;
        private const string TestLimitPropertyName = "TestLimit";
        public XYTestLimit TestLimit
        {
            get
            {
                if (TestSpecIndex < 0)
                {
                    return TestTrace.TestSpecList[0].TestLimit;
                }
                else
                {
                    return TestTrace.TestSpecList[TestSpecIndex].TestLimit;
                }
            }
            set
            {
                if (TestSpecIndex >= 0)
                {
                    TestTrace.TestSpecList[TestSpecIndex].TestLimit = value;
                    NotifyPropertyChanged(TestLimitPropertyName);
                }
            }
        }



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
        //        NotifyPropertyChanged(DisplayName);
        //    }
        //}

        //private const string CompensationPropertyName = "Compensation";
        //[UIDisplay("补偿点")]
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

        private const string IsAutoScalePropertyName = "IsAutoScale";
        public bool IsAutoScale
        {
            get
            {
                return TestTrace.IsAutoScale;
            }
            set
            {
                TestTrace.IsAutoScale = value;
                NotifyPropertyChanged(IsAutoScalePropertyName);
            }
        }

        private const string RefValuePropertyName = "RefValue";
        public double RefValue
        {
            get
            {
                return TestTrace.RefValue;
            }
            set
            {
                TestTrace.RefValue = value;
                NotifyPropertyChanged(RefValuePropertyName);
            }
        }
        private int _RefPosition;
        private const string RefPositionPropertyName = "RefPosition";
        public int RefPosition
        {
            get
            {
                return _RefPosition;
            }
            set
            {
                _RefPosition = value;
                NotifyPropertyChanged(RefPositionPropertyName);
            }
        }
        
        private const string ScalePropertyName = "Scale";
        public double Scale
        {
            get
            {
                return TestTrace.Scale;
            }
            set
            {
                TestTrace.Scale = value;
                NotifyPropertyChanged(ScalePropertyName);
            }
        }

        private const string DivCountPropertyName = "DivCount";
        public int DivCount
        {
            get
            {
                return TestTrace.DivCount;
            }
            set
            {
                TestTrace.DivCount = value;
                NotifyPropertyChanged(DivCountPropertyName);
            }
        }

        private const string IsSaveImagePropertyName = "IsSaveImage";
        public bool IsSaveImage
        {
            get
            {
                return TestTrace.IsSaveImage;
            }
            set
            {
                TestTrace.IsSaveImage = value;
                NotifyPropertyChanged(IsSaveImagePropertyName);
            }
        }

        private const string UpLimitPropertyName = "UpLimit";
        public double? UpLimit
        {
            get
            {
                if (TestSpecIndex < 0)
                {
                    return TestTrace.TestSpecList[0].UpLimit;
                }
                else
                {
                    return TestTrace.TestSpecList[TestSpecIndex].UpLimit;
                }
            }
            set
            {
                 if(TestSpecIndex >= 0)
                 {
                     TestTrace.TestSpecList[TestSpecIndex].UpLimit = value;
                     NotifyPropertyChanged(UpLimitPropertyName);
                 }
            }
        }

        private const string LowLimitPropertyName = "LowLimit";
        public double? LowLimit
        {
            get
            {
                if (TestSpecIndex < 0)
                {
                    return TestTrace.TestSpecList[0].LowLimit;
                }
                else
                {
                    return TestTrace.TestSpecList[TestSpecIndex].LowLimit;
                }
            }
            set
            {
                 if(TestSpecIndex >= 0)
                 {
                     TestTrace.TestSpecList[TestSpecIndex].LowLimit = value;
                     NotifyPropertyChanged(LowLimitPropertyName);
                 }
            }
        }

        private const string LimitDescriptionPropertyName = "LimitDescription";
        public string LimitDescription
        {
            get
            {
                if (TestSpecIndex < 0)
                {
                    return TestTrace.TestSpecList[0].LimitDescription;
                }
                else
                {
                    return TestTrace.TestSpecList[TestSpecIndex].LimitDescription;
                }
            }
            set
            {
                if (TestSpecIndex >= 0)
                {
                    TestTrace.TestSpecList[TestSpecIndex].LimitDescription = value;
                    NotifyPropertyChanged(LimitDescriptionPropertyName);
                }
            }
        }
        public LimitLineTypeEnum[] LimitLineTypeEnumList
        {
            get
            {
                return new LimitLineTypeEnum[] { LimitLineTypeEnum.None, LimitLineTypeEnum.Max, LimitLineTypeEnum.Min };
            }
        }

        public static void TableToExcel(XYTestLimit dt, string saveFileName)
        {
            if (dt == null) return;

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
            ISheet sheetLimitLine = workbook.CreateSheet("LimitLine");

            //表头  
            IRow rowHeadLimitLine = sheetLimitLine.CreateRow(0);
            ICell cellHeadType = rowHeadLimitLine.CreateCell(0);
            cellHeadType.SetCellValue("类型");
            ICell cellHeadX1 = rowHeadLimitLine.CreateCell(1);
            cellHeadX1.SetCellValue("起始频率");
            ICell cellHeadX2 = rowHeadLimitLine.CreateCell(2);
            cellHeadX2.SetCellValue("截止频率");
            ICell cellHeadY1 = rowHeadLimitLine.CreateCell(3);
            cellHeadY1.SetCellValue("起始值");
            ICell cellHeadY2 = rowHeadLimitLine.CreateCell(4);
            cellHeadY2.SetCellValue("截止值");

            //数据  
            for (int i = 1; i <= dt.LimitLine.Count; i++)
            {
                LimitLine LL = dt.LimitLine[i - 1];

                IRow rowLimitLine = sheetLimitLine.CreateRow(i);

                ICell cellType = rowLimitLine.CreateCell(0);
                cellType.SetCellValue(LL.Type.ToString());
                ICell cellX1 = rowLimitLine.CreateCell(1);
                cellX1.SetCellValue(LL.X1.ToString());
                ICell cellX2 = rowLimitLine.CreateCell(2);
                cellX2.SetCellValue(LL.X2.ToString());
                ICell cellY1 = rowLimitLine.CreateCell(3);
                cellY1.SetCellValue(LL.Y1.ToString());
                ICell cellY2 = rowLimitLine.CreateCell(4);
                cellY2.SetCellValue(LL.Y2.ToString());
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

        public static XYTestLimit ExcelToTable(string file)
        {
            if (!File.Exists(file)) return null;
            XYTestLimit dt = new XYTestLimit();
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

                ISheet sheetLimitLine = workbook.GetSheet("LimitLine");

                for (int i = 1; i <= sheetLimitLine.LastRowNum - sheetLimitLine.FirstRowNum; i++)
                {
                    LimitLine LL = new LimitLine();

                    LL.Type = (LimitLineTypeEnum)Enum.Parse(typeof(LimitLineTypeEnum), GetValueType(sheetLimitLine.GetRow(i).GetCell(0)).ToString());
                    LL.X1 = Convert.ToDouble(GetValueType(sheetLimitLine.GetRow(i).GetCell(1)));
                    LL.X2 = Convert.ToDouble(GetValueType(sheetLimitLine.GetRow(i).GetCell(2)));
                    LL.Y1 = Convert.ToDouble(GetValueType(sheetLimitLine.GetRow(i).GetCell(3)));
                    LL.Y2 = Convert.ToDouble(GetValueType(sheetLimitLine.GetRow(i).GetCell(4)));

                    dt.LimitLine.Add(LL);
                }

                if(dt.LimitLine.Count >=0)
                {
                    dt.Enable = true;
                }
                else
                {
                    dt.Enable = false;
                }
            }
            return dt;
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
