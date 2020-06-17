using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelBaseLib
{
    public  class ResultDisplayListenerVM:NotifyBase
    {
        public ResultDisplayListenerVM()
        {
            ResultDisplayInfos = new ObservableCollection<ResultDisplayInfo>();
        }
        private ObservableCollection<ResultDisplayInfo> _ResultDisplayInfos;
        private const string ResultDisplayInfosPropertyName = "ResultDisplayInfos";
        public ObservableCollection<ResultDisplayInfo> ResultDisplayInfos
        {
            get
            {
                return _ResultDisplayInfos;
            }
            set
            {
                _ResultDisplayInfos = value;
                NotifyPropertyChanged(ResultDisplayInfosPropertyName);
            }
        }
        public void AddResultDisplayListener(string name)
        {
            int i = 0;
            var items = ResultDisplayInfos.Where(x => x.Name == name);
            ResultDisplayInfo displayInfo = new ResultDisplayInfo() { Name = name };
            if (items.Count() != 0)
            {
                i = ResultDisplayInfos.Select(x => x.Index).Max();
                displayInfo.Index = i + 1;
                displayInfo.DisplayName = name + displayInfo.Index;
            }
            else
            {
                displayInfo.DisplayName = name;
                displayInfo.Index = 0;
            }
            var resultDisplayType = TestStepFactory.ResultDisplaylist.Where(x => x.Att.DisplayName == name).FirstOrDefault();
            if (resultDisplayType is ResultDisplayType)
            {
                ResultDisplayType type = resultDisplayType as ResultDisplayType;
                Assembly assembly = Assembly.LoadFile(type.AssemblyPath);
                string str = type.ResultType.FullName;
                Type modelType = assembly.GetType(str);
                object obj = Activator.CreateInstance(modelType) as object;
                if (obj is IResultListerner)
                {
                    displayInfo.ResultListener = obj as IResultListerner;
                }
                ResultDisplayInfos.Add(displayInfo);
            }
        }

        public void DeleteResultDisplayListener(ResultDisplayInfo info)
        {
            ResultDisplayInfos.Remove(info);
        }
    }

    public class ResultDisplayInfo
    {
        public int Index { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public IResultListerner ResultListener { get; set; }
    }
}
