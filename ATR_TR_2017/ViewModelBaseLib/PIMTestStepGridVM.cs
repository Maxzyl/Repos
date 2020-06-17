using ModelBaseLib;
using PIMModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelBaseLib
{
    public class PIMTestStepGridVM:NotifyBase
    {
        public PIMTestStepGridVM()
        {
            PimTestSteplist = new ObservableCollection<PIMTestStepVM>();
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
        public ObservableCollection<PIMTestStepVM> PimTestSteplist { get; set; }
        
    }
}
