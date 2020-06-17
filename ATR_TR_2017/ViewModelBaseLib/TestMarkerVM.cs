using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
namespace ViewModelBaseLib
{
    public class TestMarkerVM:NotifyBase
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
        private bool _IsSelected;
        private const string IsSelectedPropertyName = "IsSelected";
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                NotifyPropertyChanged(IsSelectedPropertyName);
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
        private string _VarName;
        private const string VarNamePropertyName = "VarName";
        public string VarName
        {
            get
            {
                return Marker.VarName;
            }
            set
            {
                Marker.VarName = value;
                NotifyPropertyChanged(VarNamePropertyName);
            }
        }
        private bool? _PassFail;
        private const string PassFailPropertyName = "PassFail";
        public bool? PassFail
        {
            get
            {
                return _PassFail;
            }
            set
            {
                _PassFail = value;
                NotifyPropertyChanged(PassFailPropertyName);
            }
        }

        private bool _IsTest;
        private const string IsTestPropertyName = "IsTest";
        public bool IsTest
        {
            get
            {
                return Marker.IsTest;
            }
            set
            {
                Marker.IsTest = value;
                NotifyPropertyChanged(IsTestPropertyName);
            }
        }



        private TestMarker _Marker;
        private const string MarkerPropertyName = "Marker";
        public TestMarker Marker
        {
            get
            {
                return _Marker;
            }
            set
            {
                _Marker = value;
                NotifyPropertyChanged(MarkerPropertyName);
            }
        }
        /// <summary>
        /// true在表格中是一种颜色，false在表格中是另外一种颜色
        /// </summary>
        public bool RowColorIndicator { get; set; }
        
    }
}
