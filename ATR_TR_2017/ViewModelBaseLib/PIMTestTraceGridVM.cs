using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelBaseLib
{
    public class PIMTestTraceGridVM : NotifyBase
    {
        public PIMTestTraceGridVM()
        {
            PimTestTracelist = new ObservableCollection<PIMTestTraceVM>();
        }
        private string _DisplayName1;
        private const string DisplayNamePropertyName = "MyProperty";
        public string DisplayName1
        {
            get
            {
                return _DisplayName1;
            }
            set
            {
                _DisplayName1 = value;
                NotifyPropertyChanged(DisplayNamePropertyName);
            }
        }
        
        public ObservableCollection<PIMTestTraceVM> PimTestTracelist { get; set; }
    }
}
