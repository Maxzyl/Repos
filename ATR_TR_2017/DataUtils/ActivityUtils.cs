using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataUtils
{
    public class ActivityUtils
    {
        public delegate void delegateNoticeStatus(string message, double process, double imin = 0, double imax = 100);

        public static string UpdataUserLayoutByActivity(string ActivityFile, string ActivityName, string UserID, List<DockLayoutManager> dlmList)
        {
            string result = "OK";
            if (StaticInfo.LocalModel == "LOCAL")
            {

            }
            else
            {
                foreach (DockLayoutManager dlm in dlmList)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    dlm.SaveLayoutToStream(ms);
                    ms.Position = 0;
                    int len1 = Convert.ToInt32(ms.Length);
                    byte[] data = new byte[len1];
                    ms.Read(data, 0, len1);
                    ms.Close();
                    //string SQLStr = CommSecurity.GetConnectionString();
                    string SQLStr = "";

                    SQLStr = CommSecurity.GetConnectionString();

                    SqlConnection conn = new SqlConnection(SQLStr);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE_SYS_LAYOUT", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ACTIVITYFILE", SqlDbType.VarChar);
                    cmd.Parameters["@ACTIVITYFILE"].Value = ActivityFile;
                    cmd.Parameters.Add("@ACTIVITYNAME", SqlDbType.VarChar);
                    cmd.Parameters["@ACTIVITYNAME"].Value = ActivityName;
                    cmd.Parameters.Add("@LAYOUTID", SqlDbType.VarChar);
                    cmd.Parameters["@LAYOUTID"].Value = dlm.Name;
                    cmd.Parameters.Add("@FILESTREAM", SqlDbType.Image);
                    cmd.Parameters["@FILESTREAM"].Value = data;
                    cmd.Parameters.Add("@UPDATEUSER", SqlDbType.VarChar);
                    cmd.Parameters["@UPDATEUSER"].Value = UserID;
                    cmd.Parameters.Add("@TRES", SqlDbType.VarChar, 100);
                    cmd.Parameters["@TRES"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    if (!Convert.ToString(cmd.Parameters["@TRES"].Value).StartsWith("OK"))
                    {
                        result = Convert.ToString(cmd.Parameters["@TRES"].Value);
                        break;
                    }
                }
            }
            return result;
        }

        public static string LoadUserLayoutByActivity(string ActivityFile, string ActivityName, string UserID, List<DockLayoutManager> dlmList)
        {
            string result = "OK";
            foreach (DockLayoutManager dlm in dlmList)
            {
                string strSQL = "SELECT * FROM SYS_LAYOUT WHERE ACTIVITYFILE='{0}' AND SYS_USER_ID='{1}' AND ACTIVITYNAME='{2}' AND LAYOUTID='{3}'";
                DataSet ds = DB.GetDataSetFromSQL(string.Format(strSQL, ActivityFile, UserID, ActivityName, dlm.Name));
                if (ds.Tables[0].Rows.Count > 0)
                {  
                    MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["LAYOUTSTREAM"]);
                    dlm.RestoreLayoutFromStream(ms);
                    ms.Close();
                }
            }
            return result;
        }

        public static Boolean UploadActivity(string FullPath)
        {
           
            string FullName = FullPath;
            string ActivityName = "";
            string ActivityPath = "";
            string SupportLocal = "";

            string FileName = System.IO.Path.GetFileNameWithoutExtension(FullName);
            if (".DLL".Equals(System.IO.Path.GetExtension(FullName).ToUpper()))
            {
                try
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(FullName);
                    if (fileName.ToUpper().StartsWith("DEVEXPRESS."))
                    {
                        ActivityPath = "DevLibs";
                        ActivityName = System.IO.Path.GetFileName(FullName);
                        SupportLocal = "Y";
                    }
                    else if (fileName.ToUpper().StartsWith("SYSTEM."))
                    {
                        ActivityPath = "System";
                        ActivityName = System.IO.Path.GetFileName(FullName);
                        SupportLocal = "Y";
                    }
                    else
                    {
                        ActivityPath = "";
                        ActivityName = System.IO.Path.GetFileName(FullName);
                        SupportLocal = "Y";
                    }
                }
                catch (Exception)
                {
                    ActivityPath = "";
                    ActivityName = System.IO.Path.GetFileName(FullName);
                    SupportLocal = "Y";
                }
            }
            else if (".ZIP".Equals(System.IO.Path.GetExtension(FullName).ToUpper()))
            {

            }
            else
            {
                ActivityPath = System.IO.Path.GetExtension(FullName).ToUpper().Substring(1);
                ActivityName = System.IO.Path.GetFileName(FullName);
                if ("XML.".Equals(ActivityPath) && FileName.ToUpper().StartsWith("SYSTEM."))
                {
                    ActivityPath = "SYSTEM";
                }
                else if ("XML".Equals(ActivityPath) && FileName.ToUpper().StartsWith("DEVEXPRESS."))
                {
                    ActivityPath = "DevLibs";
                }
                else if ("XML".Equals(ActivityPath)) ActivityPath = "";
                else if ("JPG".Equals(ActivityPath)) ActivityPath = "IMAGE";
                else if ("JPEG".Equals(ActivityPath)) ActivityPath = "IMAGE";
                else if ("PNG".Equals(ActivityPath)) ActivityPath = "IMAGE";
                else if ("BMP".Equals(ActivityPath)) ActivityPath = "IMAGE";
                else if ("EXE".Equals(ActivityPath)) ActivityPath = "";
                else if ("CONFIG".Equals(ActivityPath)) ActivityPath = "";
                else if ("XAML".Equals(ActivityPath)) ActivityPath = "RESOURCE";
            }
            System.IO.FileStream fs = new System.IO.FileStream(FullName, FileMode.Open, FileAccess.Read);
            string fileMD5str = GetMD5HashFromFile(fs);
            int len1 = Convert.ToInt32(fs.Length);
            byte[] data = new byte[len1];
            fs.Read(data, 0, len1);
            fs.Close();

            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPLOADACTIVITYFILE_DS_PA", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FILENAME", SqlDbType.VarChar);
            cmd.Parameters["@FILENAME"].Value =ActivityName ;
            cmd.Parameters.Add("@QCode", SqlDbType.VarChar);
            cmd.Parameters["@QCode"].Value = "";
            cmd.Parameters.Add("@ACTIVITYNAME", SqlDbType.VarChar);
            cmd.Parameters["@ACTIVITYNAME"].Value = FileName;
            cmd.Parameters.Add("@ACTIVITYGROUP", SqlDbType.VarChar);
            cmd.Parameters["@ACTIVITYGROUP"].Value = ActivityPath;
            cmd.Parameters.Add("@ACTIVITYNAMEDESC", SqlDbType.VarChar);
            cmd.Parameters["@ACTIVITYNAMEDESC"].Value = "";
            cmd.Parameters.Add("@ACTIVITYGROUPDESC", SqlDbType.VarChar);
            cmd.Parameters["@ACTIVITYGROUPDESC"].Value = "";
            cmd.Parameters.Add("@PATH", SqlDbType.VarChar);
            cmd.Parameters["@PATH"].Value = ActivityPath;
            cmd.Parameters.Add("@IsActivity", SqlDbType.VarChar);
            cmd.Parameters["@IsActivity"].Value = SupportLocal;
            cmd.Parameters.Add("@FILE", SqlDbType.Image);
            cmd.Parameters["@FILE"].Value = data;
            cmd.Parameters.Add("@FileTimeTag", SqlDbType.VarChar);
            cmd.Parameters["@FileTimeTag"].Value = fileMD5str;
            cmd.Parameters.Add("@TRES", SqlDbType.VarChar, 100);
            cmd.Parameters["@TRES"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            conn.Close();
            return Convert.ToString(cmd.Parameters["@TRES"].Value).ToUpper().StartsWith("OK");

        }

        public static Boolean DownLoadActivity(double beforeValue)//, delegateNoticeStatus NoticeStatus
        {
            int UpdateFileCount = 0;
            float step = 0;
            double stepcount = beforeValue;

            System.Diagnostics.Process[] pc = System.Diagnostics.Process.GetProcessesByName("DS_PA_Update");
            for (int i = 0; i < pc.Length; i++)
            {
                pc[i].Kill();
            }

            String SQLCmdc = "SELECT COUNT(1) COUNT FROM ActivityFile_DS_PA ";
            DataSet dstotal = DataUtils.DB.GetDataSetFromSQL(SQLCmdc);

            if (dstotal.Tables[0].Rows.Count > 0)
            {
                UpdateFileCount = Convert.ToInt32(dstotal.Tables[0].Rows[0]["COUNT"]);
            }
            if (UpdateFileCount > 0)
            {
                step = (float)30 / UpdateFileCount;
            }

            string sql = @"select ID,Name,Path,FileTimeTag ,Filestream FROM ActivityFile_DS_PA where Name='{0}'";
            DataSet ds = DataUtils.DB.GetDataSetFromSQL(string.Format(sql, "DS_PA_Update.exe"));

            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                String FilePath = "";
                if (string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Path"])))
                {
                    FilePath = Environment.CurrentDirectory + "\\" + ds.Tables[0].Rows[0]["Name"];
                }
                else
                {
                    FilePath = Environment.CurrentDirectory + "\\" + ds.Tables[0].Rows[0]["Path"] + "\\" + ds.Tables[0].Rows[0]["Name"];
                }
                if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                }
                string fileMD5str = "";
                if (File.Exists(FilePath))
                {
                    fileMD5str = GetMD5HashFromFile(FilePath);
                }
                string fileMD5db = Convert.ToString(ds.Tables[0].Rows[0]["FileTimeTag"]);
                if (fileMD5db != fileMD5str)
                {
                    MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["Filestream"]);
                    Stream localFile = new FileStream(FilePath, FileMode.Create);
                    localFile.Write(ms.ToArray(), 0, (int)ms.Length);
                    localFile.Close();
                }
            }

            string sqlcmd = @"select ID,Name,Path,FileTimeTag ,ModifyTime FROM ActivityFile_DS_PA";
            DataSet dsActivity = DataUtils.DB.GetDataSetFromSQL(sqlcmd);

            if (dsActivity.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow reader in dsActivity.Tables[0].Rows)
                {
                    //String FilePath = Environment.CurrentDirectory + "\\" + reader["Path"] + "\\" + reader["Name"];
                    String FilePath = "";
                    if (string.IsNullOrEmpty(Convert.ToString(reader["Path"])))
                    {
                        FilePath = Environment.CurrentDirectory + "\\" + reader["Name"];
                    }
                    else
                    {
                        FilePath = Environment.CurrentDirectory + "\\" + reader["Path"] + "\\" + reader["Name"];
                    }

                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(FilePath)))
                    {
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(FilePath));
                    }

                    string fileMD5str = "";
                    if (File.Exists(FilePath))
                    {
                        fileMD5str = GetMD5HashFromFile(FilePath);
                    }

                    string fileMD5db = Convert.ToString(reader["FileTimeTag"]);
                    if (fileMD5db != fileMD5str)
                    {
                        MessageBoxResult response = DXMessageBox.Show("有新的版本可以更新,是否更新为最新版本!", "系统提示", System.Windows.MessageBoxButton.OKCancel, MessageBoxImage.Question);
                        if (response == MessageBoxResult.OK)
                        {
                            System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\" + "DS_PA_Update.exe");
                            Environment.Exit(0);
                        }
                        else
                        {
                            break;
                        }
                    }
                    stepcount += step;
                }
            }
            return true;
        }

        public static string GetMD5HashFromFile(Stream fileStream)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fileStream);
                fileStream.Position = 0;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("X2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("X2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
    }
}
