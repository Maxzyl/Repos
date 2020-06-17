using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Data;
using System.Data.SqlClient;
using DataUtils;
using System.Collections.ObjectModel;
using DevExpress.Xpf.Core;

namespace ViewModelBaseLib
{
    public class StateFileManagerVM:NotifyBase
    {
        public StateFileManagerVM()
        {
            CurrentFatherId = 0;
            CurrentFileList = new ObservableCollection<StateFileModel>();
            SelectedFileModels = new ObservableCollection<StateFileModel>();
            CopyFileList = new ObservableCollection<StateFileModel>();
            CutFileList = new ObservableCollection<StateFileModel>();
            InitialData();          
        }
        //current father file
        public int CurrentFatherId { get; set; }
        private string _PathStr="状态文件：";
        public string PathStr
        {
            get { return _PathStr; }
            set
            {
                _PathStr = value;
                NotifyPropertyChanged("PathStr");
            }
        }

        public ObservableCollection<StateFileModel> CurrentFileList { get; set; }
        public ObservableCollection<StateFileModel> SelectedFileModels { get; set; }
        public ObservableCollection<StateFileModel> CopyFileList { get; set; }
        public ObservableCollection<StateFileModel> CutFileList { get; set; }
        public StateFileModel SelectedFileModel { get; set; }

        #region 目前没有用
        //move to next level, current father File is file
        public void NextLevel(StateFileModel sf)
        {
            this.CurrentFatherId = sf.ID;
            getCurrentFileData(CurrentFileList,CurrentFatherId);
            PathStr = "状态文件：";
            getCurrentPath(CurrentFatherId);
        }
        //move to prev level, current father file is file.parent
        public void PrevLevel()
        {
            int fatherOfParentID;
            string SQL = @"SELECT TOP 1  * FROM SYS_TEST_PLAN WHERE FileID={0} AND ENABLE='Y'";
            DataTable dt=DataUtils.DB.GetDataSetFromSQL(string.Format(SQL,CurrentFatherId)).Tables[0];
            if(dt.Rows.Count > 0)
            {
                fatherOfParentID = Convert.ToInt32(dt.Rows[0]["FileParentID"]);
                CurrentFatherId = fatherOfParentID;
                getCurrentFileData(CurrentFileList,CurrentFatherId);
                PathStr = "状态文件：";
                getCurrentPath(CurrentFatherId);
            }
        }

        //create new file or new folder
        public void Create(string str)
        {
             StateFileModel sf = new StateFileModel();
             if(str=="新建文件夹")
             {
                 string outstr = DataUtils.DB.ExecProc(
                     "UPDATE_SYS_TEST_PLAN",
                     "@ACTION=" + "UPDATE",
                     "@FileParentID=" + CurrentFatherId,
                     "@IsFolder=" + "Y", 
                     "@FileName=" + "新建文件夹",
                     "@FileVersion=" + "",
                     "@FileDisp=" + "",
                     "@FileProcess=" + "",
                     "@UpdateTime=" + DateTime.Now,
                     "@Sys_User_ID=" + DataUtils.StaticInfo.LoginUser
                     );
             }
             else if(str=="新建文件")
             {
                 string outstr = DataUtils.DB.ExecProc(
                    "UPDATE_SYS_TEST_PLAN",
                    "@ACTION=" + "UPDATE",
                    "@FileParentID=" + CurrentFatherId,
                    "@IsFolder=" + "N",
                    "@FileName=" + "新建文件",
                    "@FileVersion=" + "",
                    "@FileDisp=" + "",
                    "@FileProcess=" + "",
                    "@UpdateTime=" + DateTime.Now,
                    "@Sys_User_ID=" + DataUtils.StaticInfo.LoginUser
                    );
             }
            getCurrentFileData(CurrentFileList,CurrentFatherId);
            
        }

        //delete file or folder
        public void Remove()
        {  
           foreach(StateFileModel sf in SelectedFileModels)
           {
               string outstr = DataUtils.DB.ExecProc(
                "UPDATE_SYS_TEST_PLAN",
                "@ACTION=" + "DELETE",
                "@FileID=" + sf.ID,
                "@FileParentID=" + CurrentFatherId,
                "@IsFolder=" + (sf.IsFolder==true ? "Y" : "N"),
                "@UpdateTime=" + DateTime.Now,
                "@Sys_User_ID=" + DataUtils.StaticInfo.LoginUser
                );
           }
            getCurrentFileData(CurrentFileList,CurrentFatherId);
        }

        public string ReName(StateFileModel sf,string str)
        {
            string outstr = DataUtils.DB.ExecProc(
                   "UPDATE_SYS_TEST_PLAN",
                   "@ACTION=" + "MODIFY",
                   "@FileID=" + sf.ID,
                   "@FileParentID=" + sf.ParentFileId,
                   "@IsFolder=" + (sf.IsFolder==true ? "Y": "N"),
                   "@FileName=" + str,
                   "@FileVersion=" + sf.Version,
                   "@FileDisp=" + "",
                   "@FileProcess=" + "",
                   "@UpdateTime=" + DateTime.Now,
                   "@Sys_User_ID=" + DataUtils.StaticInfo.LoginUser
                   );
            return outstr;
      
        }

        public void Copy() 
        {
        
        }

        public void Paste()
        {
            if(CopyFileList.Count > 0)
            {
                 foreach(StateFileModel sf in CopyFileList)
                 {
                    string outstr = DataUtils.DB.ExecProc(
                      "UPDATE_SYS_TEST_PLAN",
                      "@ACTION=" + "PASTE",
                      "@FileID=" + sf.ID,
                      "@FileParentID=" + CurrentFatherId,
                      "@IsFolder=" + (sf.IsFolder == true ? "Y" : "N"),
                      "@FileName=" + sf.Name,
                      "@UpdateTime=" + DateTime.Now,
                      "@Sys_User_ID=" + DataUtils.StaticInfo.LoginUser
                      );   
                     if(!outstr.StartsWith("OK"))
                     {
                         System.Windows.MessageBox.Show(outstr);
                     }
                 }
            }
            else if(CutFileList.Count > 0)
            {
                foreach (StateFileModel sf in CutFileList)
                {
                    string outstr = DataUtils.DB.ExecProc(
                      "UPDATE_SYS_TEST_PLAN",
                      "@ACTION=" + "CUTPASTE",
                      "@FileID=" + sf.ID,
                      "@FileParentID=" + CurrentFatherId,
                      "@IsFolder=" + (sf.IsFolder == true ? "Y" : "N"),
                      "@FileName=" + sf.Name,
                      "@UpdateTime=" + DateTime.Now,
                      "@Sys_User_ID=" + DataUtils.StaticInfo.LoginUser
                      );
                }
            }
            getCurrentFileData(CurrentFileList,CurrentFatherId);
        }

        public void Cut()
        {
            if (CutFileList.Count > 0)
            {
                foreach (StateFileModel sf in CutFileList)
                 {
                     string outstr = DataUtils.DB.ExecProc(
                       "UPDATE_SYS_TEST_PLAN",
                       "@ACTION=" + "CUT",
                       "@FileID=" + sf.ID,
                       "@IsFolder=" + (sf.IsFolder == true ? "Y" : "N"),
                       "@UpdateTime=" + DateTime.Now,
                       "@Sys_User_ID=" + DataUtils.StaticInfo.LoginUser
                       );
                 }  
            }
            getCurrentFileData(CurrentFileList,CurrentFatherId);
        }

        public void getCurrentPath(int id)
        {
            string path = "";
            if (id != 0)
            {
                string sqlStr = @"select FileName  from SYS_TEST_PLAN where FileID='{0}' ";
                string sqlint = @"select FileParentID  from SYS_TEST_PLAN where FileID='{0}'";
                string str = DataUtils.DB.GetSingleValueFromSQL(string.Format(sqlStr, id)).ToString();
                int parentid = (Int32)DataUtils.DB.GetSingleValueFromSQL(string.Format(sqlint, id));
                if (!string.IsNullOrWhiteSpace(str))
                {
                    path = str + "/" + path;
                }
                if (parentid != 0)
                {
                    getCurrentPath(parentid);
                }
            }
            PathStr = PathStr + path;
        }
        #endregion

        public void InitialData()
        {
            getCurrentFileData(CurrentFileList,CurrentFatherId);
        }

        //连接数据库
        private void getCurrentFileData(ObservableCollection<StateFileModel> fileList,int id)
        {
            fileList.Clear();
            string SQL = @"SELECT * FROM SYS_TEST_PLAN WHERE FileParentID={0} AND ENABLE='Y' ";
            DataTable dt=DataUtils.DB.GetDataSetFromSQL(string.Format(SQL,id)).Tables[0];
            if(dt.Rows.Count > 0)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    string SizeSql = @"SELECT datalength(StateData)/1024 FROM dbo.SYS_TEST_PLAN  where  FileID='{0}' ";
                    string sizeStr = DataUtils.DB.GetSingleValueFromSQL(string.Format(SizeSql, Convert.ToInt32(dr["FileID"]))).ToString();
                    if(string.IsNullOrWhiteSpace(sizeStr))
                    {
                        sizeStr = "0";
                    }
                    string userSQL= @"SELECT UserName  FROM SYS_USER WHERE ID='{0}'";
                    int userID = Convert.ToInt32(DataUtils.StaticInfo.LoginUser);
                    string userInfor = DataUtils.DB.GetSingleValueFromSQL(string.Format(userSQL, userID)).ToString();
                    StateFileModel sf = new StateFileModel() { ID = Convert.ToInt32(dr["FileID"]), ParentFileId = Convert.ToInt32(dr["FileParentID"]), 
                    IsFolder = Convert.ToChar(dr["IsFolder"]) == 'Y' ? true : false, Name = Convert.ToString(dr["FileName"]), Version = Convert.ToString(dr["FileVersion"]), 
                    UpdateDateTime = Convert.ToString(dr["UpdateTime"]),UpdateUser=userInfor,
                    FileDisp=Convert.ToString(dr["FileDisp"])};
                    if(sf.IsFolder==false)
                    {
                        sf.FileSize = sizeStr + "KB";
                    }
                    fileList.Add(sf);
                }                
            }
        }
    }
}
