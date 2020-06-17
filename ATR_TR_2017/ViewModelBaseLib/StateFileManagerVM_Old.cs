using ModelBaseLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelBaseLib
{
    public class StateFileManagerVM_Old:NotifyBase
    {
        public StateFileManagerVM_Old()
        {
            CurrentFileList = new ObservableCollection<StateFileModel>();
            FileterFileList = new ObservableCollection<StateFileModel>();
            getCurrentFileData();
        }
        public ObservableCollection<StateFileModel> CurrentFileList { get; set; }
        public ObservableCollection<StateFileModel> FileterFileList { get; set; }
        public StateFileModel SelectedFileModel { get; set; }
        public void Search(string str)
        {   
            FileterFileList.Clear();
            if (!string.IsNullOrWhiteSpace(str))
            {
                foreach (var sf in CurrentFileList)
                {
                    if (sf.Name.ToLower().Contains(str.ToLower()))
                    {
                        FileterFileList.Add(sf);
                    }
                }
            }
            else
            { 
                foreach(var sf in CurrentFileList)
                {
                    FileterFileList.Add(sf);
                }
            }
        }
        public void Save()
        { 
            if((new ViewModelLocator()).MainWindow.StatusInfo.IsLocal==true)
            {
                string str = JsonConvert.SerializeObject(CurrentFileList);
                string currFilePath = AppDomain.CurrentDomain.BaseDirectory;
                string fileName = currFilePath + "configfiles/StateFile.sta";
                System.IO.File.WriteAllText(fileName,str);
            }
        }

        //连接数据库
        public void  getCurrentFileData()
        {
            if (CurrentFileList!=null)
            {
                CurrentFileList.Clear();
                FileterFileList.Clear();
            }
            else
            {
                CurrentFileList = new ObservableCollection<StateFileModel>();
                FileterFileList = new ObservableCollection<StateFileModel>();
            }
            if (DataUtils.StaticInfo.MesMode.ToLower() == "true")
            {
                string ateKind = DataUtils.StaticInfo.ATEKind;
                string terminal = (new ViewModelLocator()).MainWindow.StatusInfo.Terminal;
                string material = (new ViewModelLocator()).MainWindow.StatusInfo.Material;
                string processid = (new ViewModelLocator()).MainWindow.StatusInfo.Process;
                if (DataUtils.StaticInfo.ATEStatusFile.ToUpper() == "TRUE")
                {
                    if ((new ViewModelLocator()).MainWindow.StatusInfo.IsAdmin)
                    {
                        GetAllFileData();
                    }
                    else
                    {
                        string strSql = @"select PROCESSName from SYS_PROCESS where PROCESSID='{0}'";
                        string processName = DataUtils.DB.GetSingleValueFromSQL(string.Format(strSql, processid)).ToString();

                        string sql = @"select * from (SELECT 类型,产品,ISNULL(B.PROCESSNAME,'') 工序,文件名,更新时间,更新人员,审核人员,审核时间,是否审核 FROM (
                                   SELECT A.ATEKIND 类型,A.MATERIALHANDLE 产品,A.PROCESS,A.MATERIALHANDLE+'-'+A.PROCESS 文件名,CONVERT(VARCHAR(100), A.UPDATETIME, 120) 更新时间,A.UPDATEUSER 更新人员,A.CONFIRMUSER 审核人员,
                                   CONVERT(VARCHAR(100), A.CONFIRMTIME, 120) 审核时间,ISNULL(A.CONFIRMSTATUS,'N') 是否审核 
                                   FROM ATE_STATUS_FILE A 
                                   UNION
                                   SELECT 'ATES' 类型,
                                   CASE WHEN CHARINDEX(C.PROCESSID,B.FILENAME)>0 THEN LEFT(B.FILENAME,LEN(B.FILENAME)-LEN(C.PROCESSID )-1) ELSE B.FILENAME END 产品, 
                                   C.PROCESSID ,B.FILENAME 文件名,CONVERT(VARCHAR(100), B.UPDATETIME, 120) 更新时间,B.UPDATEUSER 更新人员,B.CONFIRMUSER 审核人员,
                                   CONVERT(VARCHAR(100), B.CONFIRMTIME, 120) 审核时间,ISNULL(B.CONFIRMSTATUS,'N') 是否审核 
                                   FROM  G_TEST_STATUS B,(SELECT DISTINCT SHOPORDER,PROCESSID,STATUSID FROM G_ASSIGN_SHOPORDER_TEST_STATUS WHERE PROCESSID IS NOT NULL AND STATUSID<>'') C WHERE B.FILENAME=C.STATUSID
                                   ) A LEFT JOIN SYS_PROCESS B ON A.PROCESS=B.PROCESSID) A
                                   where A.类型='{0}' and A.工序='{1}' and A.是否审核='Y'   ORDER BY 审核时间 DESC,更新时间 DESC";
                        DataTable dtFile = DataUtils.DB.GetDataSetFromSQL(string.Format(sql, ateKind, processName)).Tables[0];
                        if (dtFile.Rows.Count <= 0) { CurrentFileList = null; FileterFileList = null; return; }
                        else
                        {
                            foreach (DataRow dr in dtFile.Rows)
                            {
                                string SQLFile = @"select A.FileID, A.Material,A.IsFolder, A.PROCESS, datalength(StateData)/1024 SIZE,A.UpDateTime from SYS_TEST_PLAN A WHERE a.Material='{0}'  and  PROCESS='{1}'";
                                DataTable dtData = DataUtils.DB.GetDataSetFromSQL(string.Format(SQLFile, Convert.ToString(dr["产品"]), processid)).Tables[0];
                                foreach (DataRow dRow in dtData.Rows)
                                {
                                    if (Convert.IsDBNull(dRow["SIZE"]))
                                    {
                                        dRow["SIZE"] = "0";
                                    }
                                    StateFileModel sf = new StateFileModel()
                                    {
                                        ID = Convert.ToInt32(dRow["FileID"]),
                                        IsFolder = Convert.ToChar(dRow["IsFolder"]) == 'Y' ? true : false,
                                        Name = Convert.ToString(dRow["Material"]),
                                        UpdateDateTime = Convert.ToString(dRow["UpdateTime"]),
                                        FileProcess = Convert.ToString(dRow["Process"]),
                                        FileSize = Convert.ToString(dRow["SIZE"]) + "KB"
                                    };
                                    CurrentFileList.Add(sf);
                                    FileterFileList.Add(sf);
                                }
                            }
                        }
                    }
                }
                else
                {
                    GetAllFileData();
                }
            }
            else
            {
                GetATEModeFileData();
            }
         }

        private void GetAllFileData()
        {
            string process = (new ViewModelLocator()).MainWindow.StatusInfo.Process;
            string TRES = "";
            string userSQL = @"SELECT UserName FROM SYS_USER WHERE UserID='{0}'";
            object obj = DataUtils.StaticInfo.LoginUser;
            string userID = Convert.ToString(DataUtils.StaticInfo.LoginUser);
            string userInfor = DataUtils.DB.GetSingleValueFromSQL(string.Format(userSQL, userID)).ToString();

            DataTable dt = DataUtils.DB.QueryProc("GET_MATERIAL", ref TRES, "@MATERIAL=" + "").Tables[0];
            if (TRES.Substring(0, 2) == "OK" && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string materialStr = row["MATERIAL"].ToString();
                    string result = DataUtils.DB.ExecProc
                        ("UPDATE_SYS_TEST_PLAN",
                        "@ACTION=" + "UPDATEDATA",
                        "@Material=" + materialStr,
                        "@Process=" + process,
                        "@Sys_User_ID=" + userID
                        );
                }
            }
            string SQLFile = @"select A.FileID, A.Material,A.IsFolder, A.PROCESS, datalength(StateData)/1024 SIZE,A.UpDateTime from SYS_TEST_PLAN A WHERE a.Material in (select distinct MATERIAL from G_SHOPORDER_STATUS) and  PROCESS='{0}'";
            DataTable dtFile = DataUtils.DB.GetDataSetFromSQL(string.Format(SQLFile, process)).Tables[0];
            foreach (DataRow dr in dtFile.Rows)
            {
                if (Convert.IsDBNull(dr["SIZE"]))
                {
                    dr["SIZE"] = "0";
                }
                StateFileModel sf = new StateFileModel()
                {
                    ID = Convert.ToInt32(dr["FileID"]),
                    IsFolder = Convert.ToChar(dr["IsFolder"]) == 'Y' ? true : false,
                    Name = Convert.ToString(dr["Material"]),
                    UpdateDateTime = Convert.ToString(dr["UpdateTime"]),
                    FileProcess = Convert.ToString(dr["Process"]),
                    FileSize = Convert.ToString(dr["SIZE"]) + "KB"
                };
                CurrentFileList.Add(sf);
                FileterFileList.Add(sf);
            }

        }
        private void GetATEModeFileData()
        {
            string SQLFile = @"select A.FileID, A.FileName ,A.IsFolder, datalength(StateData)/1024 SIZE,A.UpDateTime from ATE_Test_FILE A";
            DataTable dtFile = DataUtils.DB.GetDataSetFromSQL(string.Format(SQLFile)).Tables[0];
            foreach (DataRow dr in dtFile.Rows)
            {
                if (Convert.IsDBNull(dr["SIZE"]))
                {
                    dr["SIZE"] = "0";
                }
                StateFileModel sf = new StateFileModel()
                {
                    ID = Convert.ToInt32(dr["FileID"]),
                    IsFolder = Convert.ToChar(dr["IsFolder"]) == 'Y' ? true : false,
                    Name = Convert.ToString(dr["FileName"]),
                    UpdateDateTime = Convert.ToString(dr["UpdateTime"]),
                    FileSize = Convert.ToString(dr["SIZE"]) + "KB"
                };
                CurrentFileList.Add(sf);
                FileterFileList.Add(sf);
            }
        }
    }
}
