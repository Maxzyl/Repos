using DevExpress.Mvvm;
using ModelBaseLib;
using Symtant.InstruDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace ViewModelBaseLib
{
    public class InstruInfoVM:NotifyBase
    {
        public InstruInfoVM()
        {
            
        }

        private InstruInfo _InstruInfo;
        private const string InstruMgrPropertyName = "InstruMgr";
        public InstruInfo InstruInfo
        {
            get
            {
                return _InstruInfo;
            }
            set
            {
                _InstruInfo = value;
                NotifyPropertyChanged(InstruMgrPropertyName);
            }
        }

        private List<InstruInfo> _InstruInfoList;
        private const string InstruInfoListPropertyName = "InstruInfoList";
        public List<InstruInfo> InstruInfoList
        {
            get
            {
                return _InstruInfoList;
            }
            set
            {
                _InstruInfoList = value;
                NotifyPropertyChanged(InstruInfoListPropertyName);
            }
        }

        public string Address
        {
            get
            {
                return InstruInfo.Address;
            }
            set
            {
                InstruInfo.Address = value;
                if (InstruInfoList!=null)
                {
                    foreach (var item in InstruInfoList)
                    {
                        item.Address = InstruInfo.Address;
                    }
                }
            }
        }

        public string FirmwareVersion
        {
            get
            {
                return InstruInfo.FirmwareVersion;
            }
            set
            {
                InstruInfo.FirmwareVersion = value;
                if (InstruInfoList != null)
                {
                    foreach (var item in InstruInfoList)
                    {
                        item.FirmwareVersion = InstruInfo.FirmwareVersion;
                    }
                }
            }
        }

        public string Model
        {
            get
            {
                return InstruInfo.Model;
            }
            set
            {
                InstruInfo.Model = value;
                if (InstruInfoList != null)
                {
                    foreach (var item in InstruInfoList)
                    {
                        item.Model = InstruInfo.Model;
                    }
                }
            }
        }

        public string RemoteInterface
        {
            get
            {
                return InstruInfo.RemoteInterface;
            }
            set
            {
                InstruInfo.RemoteInterface = value;
                if (InstruInfoList != null)
                {
                    foreach (var item in InstruInfoList)
                    {
                        item.RemoteInterface = InstruInfo.RemoteInterface;
                    }
                }
            }
        }

        public string SerialNum
        {
            get
            {
                return InstruInfo.SerialNum;
            }
            set
            {
                InstruInfo.SerialNum = value;
                if (InstruInfoList != null)
                {
                    foreach (var item in InstruInfoList)
                    {
                        item.SerialNum = InstruInfo.SerialNum;
                    }
                }
            }
        }

        public string Vendor
        {
            get
            {
                return InstruInfo.Vendor;
            }
            set
            {
                InstruInfo.Vendor = value;
                if (InstruInfoList != null)
                {
                    foreach (var item in InstruInfoList)
                    {
                        item.Vendor = InstruInfo.Vendor;
                    }
                }
            }
        }

        public string DisplayName
        {
            get
            {
                return InstruInfo.DisplayName;
            }
            set
            {
                InstruInfo.DisplayName = value;
                if (InstruInfoList != null)
                {
                    foreach (var item in InstruInfoList)
                    {
                        item.DisplayName = InstruInfo.DisplayName;
                    }
                }
            }
        }

        public string Name
        {
            get
            {
                return InstruInfo.Name;
            }
            set
            {
                InstruInfo.Name = value;
                if (InstruInfoList != null)
                {
                    foreach (var item in InstruInfoList)
                    {
                        item.Name = InstruInfo.Name;
                    }
                }
            }
        }
        public bool LogEnable
        {
            get
            {
                return InstruInfo.LogEnable;
            }
            set
            {
                InstruInfo.LogEnable = value;
                if (InstruInfoList != null)
                {
                    foreach (var item in InstruInfoList)
                    {
                        item.LogEnable = InstruInfo.LogEnable;
                    }
                }
            }
        }
        
        private bool _IsSelectedInstruMgr;
        private const string IsSelectedInstruMgrPropertyName = "IsSelectedInstruMgr";
        public bool IsSelectedInstruMgr
        {
            get
            {
                return _IsSelectedInstruMgr;
            }
            set
            {
                _IsSelectedInstruMgr = value;
                NotifyPropertyChanged(IsSelectedInstruMgrPropertyName);
            }
        }
        
        private DelegateCommand _ConnectCmd;
        public ICommand ConnectCommand
        {
            get
            {
                if (_ConnectCmd == null)
                {
                    _ConnectCmd = new DelegateCommand(()=>Connect());
                }
                return _ConnectCmd;
            }
        }
        private DelegateCommand _SetupCmd;
        public ICommand SetupCmd
        {
            get
            {
                if (_SetupCmd == null)
                {
                    _SetupCmd = new DelegateCommand(() => Setup());
                }
                return _SetupCmd;
            }
        }

        private void Setup()
        {
            
        }
        private string _IDN;
        private const string IDNPropertyName = "IDN";
        public string IDN
        {
            get
            {
                return _IDN;
            }
            set
            {
                _IDN = value;
                NotifyPropertyChanged(IDNPropertyName);
            }
        }
        public void Connect()
        {
            try
            {
                //var driver = InstruDriverFactory.CreateDriverFromInfo(InstruInfo);
                InstruInfo.InitDriver();
                var driver = InstruInfo.InstruDriver;
                if (driver != null)
                {
                    try
                    {
                        driver.Open();
                        IDN= driver.IdnString;
                        IsConnect = driver.IsOpen;
                        driver.Close();
                        return;
                    }
                    catch
                    {
                        IsConnect = false;
                    }
                }
            }
            catch
            {
                IsConnect = false;
            }
            
            IsConnect = false;

        }
        private bool? _IsConnect = null;
        private const string IsConnectPropertyName = "IsConnect";
        public bool? IsConnect
        {
            get
            {
                return _IsConnect;
            }
            set
            {
                _IsConnect = value;
                NotifyPropertyChanged(IsConnectPropertyName);
                NotifyPropertyChanged(ImagePropertyName);
                if (UpdateConnnect != null)
                {
                    UpdateConnnect.Invoke();
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Action UpdateConnnect { get; set; }
        private string _Image;
        private const string ImagePropertyName = "Image";
        public string Image
        {
            get
            {
                if (IsConnect == true)
                {
                    _Image = "/MeasurementUI;component/Images/副本" + InstruInfo.DisplayName + ".png";
                }
                else
                {
                    _Image = "/MeasurementUI;component/Images/" + InstruInfo.DisplayName + ".png";
                }
                return _Image;
            }
            set
            {
                _Image = value;
                NotifyPropertyChanged(ImagePropertyName);
            }
        }
    }
}
