using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace ModelBaseLib
{
    public class GeneTestSetup:NotifyBase
    {
        private static GeneTestSetup instance = new GeneTestSetup();

        public static GeneTestSetup Instance
        {
            get
            {
                return instance;
            }
        }
        private GeneTestSetup()
        {
            string currFilePath = Environment.CurrentDirectory;
            string fileName = currFilePath + "/configfiles/generalSetup.xml";
            XmlDocument xmlDoc = new XmlDocument();
            if(File.Exists(fileName))
            {
                try
                {
                    XDocument xDoc = XDocument.Load(fileName);
                    var query = from item in xDoc.Element("GeneTestSetup").Elements()
                                select new
                                {
                                    TypeName=item.Name,
                                     Value=item.Value
                                };
                   foreach(var item in query)
                   {
                       if (item.TypeName == "IsSimulated")
                       {
                           IsSimulated = item.Value == "true" ? true : false;
                       }
                       else if (item.TypeName == "DataDisplayDigits")
                       {
                           DataDisplayDigits = Convert.ToInt32(item.Value);
                       }
                       //else if (item.TypeName == "IsMultiDut")
                       //{
                       //    IsMultiDut = item.Value == "true" ? true : false;
                       //}
                       else if (item.TypeName == "IsTestSuccess")
                       {
                           IsTestSuccess = item.Value == "true" ? true : false;
                       }
                       else if (item.TypeName == "Rules")
                       {
                           Rules = (UploadRules)Enum.Parse(typeof(UploadRules), item.Value);
                       }
                       else if (item.TypeName == "SNRule")
                       {
                           SNRule = Convert.ToString(item.Value);
                       }
                       else if (item.TypeName == "IsEnable")
                       {
                           IsEnable = item.Value == "true" ? true : false;
                       }
                   }
                }
                catch (Exception ex) { }
            }

        }
        private int _DataDisplayDigits;
        private const string DataDisplayDigitsPropertyName = "DataDisplayDigits";
        public int DataDisplayDigits
        {
            get
            {
                return _DataDisplayDigits;
            }
            set
            {
                _DataDisplayDigits = value;
                NotifyPropertyChanged(DataDisplayDigitsPropertyName);
            }
        }
        private bool _IsSimulated;
        private const string IsSimulatedPropertyName = "IsSimulated";
        public bool IsSimulated
        {
            get
            {
                return _IsSimulated;
            }
            set
            {
                _IsSimulated = value;
                NotifyPropertyChanged(IsSimulatedPropertyName);
            }
        }
        //private bool _IsMultiDut;
        //private const string IsMultiDutPropertyName = "IsMultiDut";
        //public bool IsMultiDut
        //{
        //    get
        //    {
        //        return _IsMultiDut;
        //    }
        //    set
        //    {
        //        _IsMultiDut = value;
        //        NotifyPropertyChanged(IsMultiDutPropertyName);
        //    }
        //}
        private bool _IsTestSuccess;
        private const string IsTestSuccessPropertyName = "IsTestSuccess";
        public bool IsTestSuccess
        {
            get
            {
                return _IsTestSuccess;
            }
            set
            {
                _IsTestSuccess = value;
                NotifyPropertyChanged(IsTestSuccessPropertyName);
            }
        }
        /// <summary>
        /// 判断联机或者本地；
        /// 本地：true
        /// 联机：false
        /// </summary>
        private bool _IsLocal;
        private const string IsLocalPropertyName = "IsLocal";
        public bool IsLocal
        {
            get
            {
                return _IsLocal;
            }
            set
            {
               // _IsLocal = true;
                _IsLocal = value;
                NotifyPropertyChanged(IsLocalPropertyName);
            }
        }
        /// <summary>
        /// 上传规则
        /// </summary>
        private UploadRules _Rules;
        private const string RulesPropertyName = "Rules";
        public UploadRules Rules
        {
            get
            {
                return _Rules;
            }
            set
            {
                _Rules = value;
                NotifyPropertyChanged(RulesPropertyName);
            }
        }
        public UploadRules[] UploadRulesList
        {
            get
            {
                return new UploadRules[] { UploadRules.按键上传, UploadRules.合格即上传, UploadRules.测完即上传 };
            }
        }

        /// <summary>
        /// 条码规则
        /// </summary>
        private string _SNRule;
        private const string SNRulePropertyName = "SNRule";
        public string SNRule
        {
            get
            {
                return _SNRule;
            }
            set
            {
                _SNRule = value;
                NotifyPropertyChanged(SNRulePropertyName);
            }
        }

        /// <summary>
        /// 启用规则
        /// </summary>
        private bool _IsEnable;
        private const string IsEnablePropertyName = "IsEnable";
        public bool IsEnable
        {
            get
            {
                return _IsEnable;
            }
            set
            {
                _IsEnable = value;
                NotifyPropertyChanged(IsEnablePropertyName);
            }
        }
    }

    public enum UploadRules
    {
        按键上传, 合格即上传, 测完即上传
    }
}
