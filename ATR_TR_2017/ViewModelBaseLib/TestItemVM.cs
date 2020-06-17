using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Text.RegularExpressions;
using DevExpress.Xpf.Core;
namespace ViewModelBaseLib
{
    public class TestItemVM:ModelBaseLib.NotifyBase
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
        public TestItem TestItem { get; set; }
        private string _Unit;
        private const string UnitPropertyName = "Unit";
        public string Unit
        {
            get
            {
                return TestItem.Unit ;
            }
            set
            {
                TestItem.Unit = value;
                NotifyPropertyChanged(UnitPropertyName);
            }
        }
        private string _TypeName;
        private const string TypeNamePropertyName = "TypeName";
        
        public string TypeName
        {
            get { return TestItem.TypeName; }
            set
            {
                TestItem.TypeName = value;
                NotifyPropertyChanged(TypeNamePropertyName);
            }
        }
        private string _VarName;
        private const string VarNamePropertyName = "VarName";
        public string VarName
        {
            get
            { 
                return TestItem.VarName;
            }
            set  
            {
                Regex regNum = new Regex("^[0-9]");
                if (!regNum.IsMatch(value))
                {
                    TestItem.VarName = value;
                    NotifyPropertyChanged(VarNamePropertyName);
                }
                else
                {
                    DXMessageBox.Show("变量名不能以数字开始！","提示！");
                }
            }
        }
        private double _Compensation;
        private const string CompensationPropertyName = "Compensation";
        public double Compensation
        {
            get
            {
                return TestItem.Compensation;
            }
            set
            {
                TestItem.Compensation = value;
                NotifyPropertyChanged(CompensationPropertyName);
            }
        }
        private string _UserDefName;
        private const string UserDefNamePropertyName = "UserDefName";
        public string UserDefName
        {
            get
            {
                return TestItem.UserDefName;
            }
            set
            {
                TestItem.UserDefName = value;
                NotifyPropertyChanged(UserDefNamePropertyName);
            }
        }
        private string _Info1;
        private const string Info1PropertyName = "Info1";
        public string Info1
        {
            get
            {
                return TestItem.Info1;
            }
            set
            {
                TestItem.Info1 = value;
                NotifyPropertyChanged(Info1PropertyName);
            }
        }

        private string _testConfigDesciption;
        private const string TestConfigDesciptionPropertyName = "TestConfigDesciption";
        public string TestConfigDesciption
        {
            get
            {
                return TestItem.TestConfigDesciption;
            }
            set
            {
                TestItem.TestConfigDesciption = value;
                NotifyPropertyChanged(TestConfigDesciptionPropertyName);
            }
        }


        private string _XDescription;
        private const string XDescriptionPropertyName = "XDescription";
        public string XDescription
        {
            get
            {
                return TestItem.XDescription;
            }
            set
            {
                TestItem.XDescription = value;
                NotifyPropertyChanged(XDescriptionPropertyName);
            }
        }
        /// <summary>
        /// true在表格中是一种颜色，false在表格中是另外一种颜色
        /// </summary>
        public bool RowColorIndicator { get; set; }
    }
}
