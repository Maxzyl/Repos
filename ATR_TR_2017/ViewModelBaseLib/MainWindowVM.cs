using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
namespace ViewModelBaseLib
{
    public class MainWindowVM
    {
        public MainWindowVM()
        {
            StatusInfo = new MainWindowStatusInfo() { OpenProject = "", CalStatus = "", EqpStatus = "", UserInfo = "" };
            TestStepFactory.InitTestStepInfo();
        }
        private MainWindowStatusInfo _StatusInfo;
        private const string StatusInfoPropertyName = "StatusInfo";
        public MainWindowStatusInfo StatusInfo
        {
            get
            {
                return _StatusInfo;
            }
            set
            {
                _StatusInfo = value;
            }
        }
        
    }
    public class MainWindowStatusInfo : NotifyBase
    {
        #region
        private string _OpenProject;
        private const string OpenProjectPropertyName = "OpenProject";
        public string OpenProject
        {
            get
            {
                return _OpenProject;
            }
            set
            {
                _OpenProject = value;
                NotifyPropertyChanged(OpenProjectPropertyName);
            }
        }
        private string _EqpStatus;
        private const string EqpStatusPropertyName = "EqpStatus";
        public string EqpStatus
        {
            get
            {
                return _EqpStatus;
            }
            set
            {
                _EqpStatus = value;
                NotifyPropertyChanged(EqpStatusPropertyName);
            }
        }
        private string _CalStatus;
        private const string CalStatusPropertyName = "CalStatus";
        public string CalStatus
        {
            get
            {
                return _CalStatus;
            }
            set
            {
                _CalStatus = value;
                NotifyPropertyChanged(CalStatusPropertyName);
            }
        }
        private string _UserInfo;
        private const string UserInfoPropertyName = "UserInfo";
        public string UserInfo
        {
            get
            {
                return _UserInfo;
            }
            set
            {
                _UserInfo = value;
                NotifyPropertyChanged(UserInfoPropertyName);
            }
        }
        private bool _IsAdmin;
        private const string IsAdminPropertyName = "IsAdmin";
        public bool IsAdmin
        {
            get
            {
                return _IsAdmin;
            }
            set
            {
                _IsAdmin = value;
                NotifyPropertyChanged(IsAdminPropertyName);
            }
        }
        public bool IsLoad { get; set; }
        public bool IsLocal { get; set; }
        public string Terminal { get; set; }
        public string Process { get; set; }
        public string Material { get; set; }
        public string Token { get; set; }
        #endregion

    }
}
