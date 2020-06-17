using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;

namespace ViewModelBaseLib
{
    public class InstruMgrVM : NotifyBase
    {   
        public InstruMgrVM()
        {
            Initial();        
        }
        #region
        private bool? _IsConnect=null;
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
            }
        }
        
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
        private ObservableCollection<InstruInfoVM> _InstruInfoList = new ObservableCollection<InstruInfoVM>();
        private const string InstruMgrListPropertyName = "InstruMgrList";
        public ObservableCollection<InstruInfoVM> InstruInfoList
        {
            get
            {
                return _InstruInfoList;
            }
            set
            {
                _InstruInfoList = value;
                NotifyPropertyChanged(InstruMgrListPropertyName);
            }
        }
        #endregion
        public void Initial()
        {
           
        }
        
        
        //void UpdateConnect()
        //{
        //    _IsConnect = _InstruMgrList.Where(x => x.InstruMgr.IsConnect == false).Count() < 0;
        //}
    }
}
