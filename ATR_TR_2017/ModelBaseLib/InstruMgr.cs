using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symtant.InstruDriver;
namespace ModelBaseLib
{
    public class InstruMgr:NotifyBase
    {  
        #region
        private string _DisplayName;
        private const string DisplayNamePropertyName = "DisplayName";
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
                NotifyPropertyChanged(DisplayNamePropertyName);
            }
        }
        private string _Address;
        private const string AddressPropertyName = "Address";
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
                NotifyPropertyChanged(AddressPropertyName);
            }
        }
        private string _Image;
        private const string ImagePropertyName = "Image";
        public string Image
        {
            get
            {
                if (IsConnect==true)
                {
                    _Image = "/MeasurementUI;component/Images/副本" + DisplayName + ".png";
                }
                else 
                {
                    _Image = "/MeasurementUI;component/Images/" + DisplayName + ".png";
                }
                return _Image;
            }
            set
            {
                _Image = value;
                NotifyPropertyChanged(ImagePropertyName);           
            }
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
        private bool ? _IsConnect=null;
        private const string IsConnectPropertyName = "IsConnect";
        public bool ? IsConnect
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
                if(UpdateConnnect !=null)
                {
                    UpdateConnnect.Invoke();
                } 
            }
        }
        public Action UpdateConnnect { get; set; }
        #endregion
        public void Connect(object obj)
        {
            InstruDriver driver = new InstruDriver();
            try
            {
                driver.Open(Address);
                string res = driver.IdnString;
                IDN = res;
                driver.Close();
                IsConnect = true;
            }
            catch (Exception ex)
            {
                IsConnect = false;
                throw new Exception(ex.ToString());
                
            }
        }
    }
}
