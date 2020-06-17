using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ModelBaseLib
{
    public class StateFileModel:NotifyBase
    {
        public StateFileModel()
        {
            StateFileList = new ObservableCollection<StateFileModel>();
        }
        private bool _IsLoad;
        public bool IsLoad
        {
            get
            {
                return _IsLoad;
            }
            set
            {
                _IsLoad = value;
                NotifyPropertyChanged("IsLoad");
                NotifyPropertyChanged("Image");
            }
        }
        public string Name { get; set; }
        public int ID { get; set; }
        public string Version { get; set; }
        private bool _IsFolder;
        public bool IsFolder
        {
            get { return _IsFolder; }
            set
            {
                _IsFolder = value;
                NotifyPropertyChanged("IsFolder");
            }
        }
        private string _FileSize;
        public string FileSize
        {
            get
            {
                return _FileSize;
            }
            set
            {
                _FileSize = value;
                NotifyPropertyChanged("FileSize");
            }
        }
        public ObservableCollection<StateFileModel> StateFileList{get;set;}
        public StateFileModel ParentFile { get; set; }
        private string _Image="";
        public string Image
        {
            get
            {
                if (IsFolder)
                {
                    _Image = "/MeasurementUI;component/Images/文件夹.png";
                    return _Image;
                }
                else if (IsLoad)
                {
                    _Image = "/MeasurementUI;component/Images/添加文件 (1).png";
                    return _Image;
                }
                else
                {
                    _Image = "/MeasurementUI;component/Images/添加文件.png";
                    return _Image;
                }
            }
            set
            {
                _Image = value;
                NotifyPropertyChanged("Image");
            }

        }
        public string UpdateUser { get; set; }
        public string UpdateDateTime { get; set; }
        public string FileDisp { get; set; }
        public string FileProcess { get; set; }
        public int ParentFileId { get; set; }
        public void CreateFile(bool isFolder) { }
        public void CopyFile(StateFileModel file) { }
        public void PasteFile() { }
        public void RemoveFile(StateFileModel file) { }
    }
    public class StateFileUtil
    {
        //
        /// <summary>
        /// get file from data base and convert to tree 
        /// </summary>
        public static StateFileModel GetFileList() { return null; }
        //
        /// <summary>
        /// send file to data base
        /// </summary>
        public static void UpdateFileList(StateFileModel rootFile) { }

    }
}
