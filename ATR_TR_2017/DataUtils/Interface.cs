using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataUtils
{
    public class Interface
    {
        /// <summary>
        /// Save the State File
        /// </summary>
        /// <param name="ManualConnectionList"></param>
        /// <returns></returns>
        public static string SaveStateModel(byte[] bytes, string id)
        {
            string result = "";
            //byte[] data = Encoding.Unicode.GetBytes(JasonStr);
            string strMD5 = CommSecurity.GetMD5HashFromByte(bytes);
            result = DB.ExecProc("UPDATE_SYS_TEST_PLAN",
                              new DBParameters(System.Data.SqlDbType.NChar,"@Action","UPDATE"),
                              new DBParameters(System.Data.SqlDbType.NVarChar, "@FileID", id),
                              new DBParameters(System.Data.SqlDbType.NVarChar, "@SYS_USER_ID", DataUtils.StaticInfo.LoginUser),
                              new DBParameters(System.Data.SqlDbType.NVarChar, "@FileDisp", strMD5),
                              new DBParameters(System.Data.SqlDbType.Image, "@StateData", bytes)
                              );
            return result;
        }
        /// <summary>
        /// 绑定工位
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTerminalInfo()
        {
            string SQLStr = @"SELECT A.TERMINALName TerminalName, A.TERMINALID TerminalID FROM SYS_TERMINAL A"
                         + " LEFT JOIN SYS_ASSIGN_PROCESS_TERMINAL B ON A.TERMINALID=B.TERMINALID"
                         + " LEFT JOIN SYS_PROCESS C ON B.PROCESSID=C.PROCESSID"
                         + " WHERE  C.PROCESSKIND = '测试工位' AND C.ENABLE = 'Y' AND C.PROCESSNAME LIKE '%{0}%'";
            DataSet ds = DataUtils.DB.GetDataSetFromSQL(string.Format(SQLStr, DataUtils.StaticInfo.ProcessName));
            DataTable dt = ds.Tables[0];
            return dt;
        }

        /// <summary>
        /// 获取PROCESSID
        /// </summary>
        /// <param name="terminalID"></param>
        /// <returns></returns>
        public static DataTable GetProcessFromTerminal(string terminalID)
        {
            string SQLStr = @"select PROCESSID from SYS_ASSIGN_PROCESS_TERMINAL WHERE TERMINALID='{0}' ";
            DataTable dt = DataUtils.DB.GetDataSetFromSQL(string.Format(SQLStr, terminalID)).Tables[0];
            return dt;
        }

        public static string UpLoadActivity(byte[] data, string ATEKIND, string MATERIALHANDLE, string PROCESS, string sLogonUserId, string FileName = "")
        {
            try
            {
                FileName = (FileName == "") ? (ATEKIND + ".xml") : FileName;
                string str = CommSecurity.GetMD5HashFromByte(data);
                SqlConnection connection = new SqlConnection(CommSecurity.GetConnectionString());
                connection.Open();
                SqlCommand cmd = new SqlCommand("UPLOADATESTATUS", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                CommSecurity.SetCommandParam(ref cmd, SqlDbType.VarChar, "ATEKIND", ATEKIND);
                CommSecurity.SetCommandParam(ref cmd, SqlDbType.VarChar, "MATERIALHANDLE", MATERIALHANDLE);
                CommSecurity.SetCommandParam(ref cmd, SqlDbType.VarChar, "PROCESS", PROCESS);
                CommSecurity.SetCommandParam(ref cmd, SqlDbType.VarChar, "FILENAME", FileName);
                CommSecurity.SetCommandParam(ref cmd, SqlDbType.VarChar, "FileTimeTag", str);
                CommSecurity.SetCommandParam(ref cmd, SqlDbType.Image, "FILES", data);
                CommSecurity.SetCommandParam(ref cmd, SqlDbType.VarChar, "UpdateUser", sLogonUserId);
                //新加的
                cmd.Parameters.Add("@TRES", SqlDbType.VarChar, 100);
                cmd.Parameters["@TRES"].Direction = ParameterDirection.Output;   //要设置长度

                cmd.ExecuteNonQuery();
                connection.Close();
                string result = Convert.ToString(cmd.Parameters["@TRES"].Value);
                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static DataTable GetFile(string id, string processID, string material)
        {
            DataTable dt = null;
            string getMaterial = "";
            if (!string.IsNullOrEmpty(id))
            {
                string materialSql = @"select Material from SYS_TEST_PLAN where FileID='{0}' and  PROCESS='{1}'";
                getMaterial = Convert.ToString(DataUtils.DB.GetSingleValueFromSQL(string.Format(materialSql, id, processID)));
            }
            if(!string.IsNullOrEmpty(material))
            {
                getMaterial = material;
            }
            string strSql = @"select PROCESSName from SYS_PROCESS where PROCESSID='{0}'";
            string processName = Convert.ToString(DataUtils.DB.GetSingleValueFromSQL(string.Format(strSql, processID)));

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
                                   where A.类型='{0}' and A.工序='{1}' and A.产品='{2}' and A.是否审核='Y'   ORDER BY 审核时间 DESC,更新时间 DESC";
            dt = DataUtils.DB.GetDataSetFromSQL(string.Format(sql, DataUtils.StaticInfo.ATEKind, processName, material)).Tables[0];
            return dt;
        }

        public static string ConnectDB(string sql, SqlParameter[] par=null)
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(CommSecurity.GetConnectionString()))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sql, con);
                    if (par!=null)
                    {
                        cmd.Parameters.AddRange(par);
                    }
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    return result = "ok";
                }
                catch (Exception ex)
                {
                    DataUtils.LOGINFO.WriteError(ex.Message+Environment.NewLine+ex.Source+Environment.NewLine+ex.StackTrace);
                    return result = ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace;
                }
            }
        }

        public static string InsertFileDB(string FileParentID, string IsFolder, string FileName, string FileVersion, string FileDisp, byte[] data, string Sys_User_ID, DateTime updateTime, string Enable)
        {
            string sql="insert into ATE_Test_FILE values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";
            string sqlStr = string.Format(sql, FileParentID, IsFolder, FileName, FileVersion, FileDisp, data, Sys_User_ID, updateTime, Enable);
            return ConnectDB(sqlStr);
        }

        public static string DeleteFileDB(string id)
        {
            string sql = string.Format("delete from ATE_Test_FILE where FileID='{0}'", id);
            return ConnectDB(sql);
        }

        public static string UpdateFileDB(byte[] data,string FileID)
        {
            string FileDisp = CommSecurity.GetMD5HashFromByte(data);
            SqlParameter[] par = { new SqlParameter("@TestFile", data) };
            string JarContent = Convert.ToBase64String(data);
            string sql = @"update ATE_Test_FILE 
                        set SYS_USER_ID='{0}',
                        FileDisp='{1}',
                        StateData= @TestFile,
                        UpdateTime='{2}' 
                        where FileID='{3}'";
            string sqlStr = string.Format(sql, DataUtils.StaticInfo.LoginUser, FileDisp, DateTime.Now, FileID);
            return ConnectDB(sqlStr, par);
        }
    }
}
