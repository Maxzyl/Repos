using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows.Media;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.Reporting.WinForms;
using System.Xml.Serialization;
using System.Reflection;
using System.Windows;
using DevExpress.Xpf.Core;
using NPOI.HSSF.UserModel;

namespace DataUtils
{   
    public class StaticInfo
    {
        public static string LoginUser { get; set; }
        public static string ExecutePath { get; set; }
        public static string LocalModel { get; set; }
        public static string AutoConnect { get; set; }
        public static string Process { get; set; }
        public static string Terminal { get; set; }
        public static string ProcessName { get; set; }
        public static string ATEKind { get; set; }
        public static string ATEStatusFile { get; set; }
        public static int intMilliseconds { get; set; }
        public static string AutoLoadStateBySN { get; set; }
        public static string MesMode { get; set; }
        public static string ResultString { get; set; }
        public static string AutoLoadStateFile { get; set; }
        public static string DBTime
        {
            get
            {
                return System.DateTime.Now.AddMilliseconds(intMilliseconds).ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
        }
    }
    public class LastUserInfo
    {
        public  string LastUserName { get; set; }
        public  string LastUserPassword { get; set; }
        public  bool IsSave { get; set; }
        public  string Process { get; set; }
        public string ProcessName { get; set; }
        public  string Terminal { get; set; }
        public string TerminalName { get; set; }
        public bool IsLocal { get; set; }
    }
    public class Test_Data
    {
        public int Index { get; set; }
        public string Item { get; set; }
        public string Ports { get; set; }
        public string Phase { get; set; }
        public string Att { get; set; }
        public string Frequency { get; set; }
        public string Test_Result { get; set; }
        public string UCL { get; set; }
        public string LCL { get; set; }
        public string Fail_Sign { get; set; }
        public string Kind { get; set; }
        public Test_Data()
        {
            Index = 0; Item = ""; Ports = ""; Phase = ""; Att = ""; Frequency = "";
            Test_Result = ""; UCL = ""; LCL = ""; Fail_Sign = ""; Kind = "";
        }
    }

    public class Test_Result
    {
        public string Test_ID { get; set; }
        public string SN { get; set; }
        public string UserID { get; set; }
        public string companyname { get; set; }
        public string customername { get; set; }
        public string partnumber { get; set; }
        public string otherlab { get; set; }
        public string test_envir { get; set; }
        public string Result { get; set; }
        public string Machine_Desc { get; set; }
        public DateTime Test_Time { get; set; }
        public List<Test_Data> TestDataList = new List<Test_Data>();
        public Test_Result()
        {
            Test_ID = ""; SN = ""; UserID = ""; companyname = ""; customername = "";
            partnumber = ""; otherlab = ""; test_envir = ""; Result = ""; Machine_Desc = "";
            Test_Time = DateTime.Now;
        }
    }


    public class UIPackage
    {
        public delegate void SetTestDataID(string ID);

        public static string CheckUserLogin(string userid, string pwd)
        {
            string bRes = "";
            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("USER_CHECKLOGIN", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar);
            cmd.Parameters["@UserID"].Value = userid;
            cmd.Parameters.Add("@PWD", SqlDbType.NVarChar);
            cmd.Parameters["@PWD"].Value = pwd;
            cmd.Parameters.Add("@TRES", SqlDbType.NVarChar, 100);
            cmd.Parameters["@TRES"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            bRes = Convert.ToString(cmd.Parameters["@TRES"].Value);
            conn.Close();
            return bRes;
        }
        ///<summary>
        /// 将DataSet写成CSV文件
        /// </summary>
        ///<summary>

        public static void setRDLCParams(Microsoft.Reporting.WinForms.LocalReport lr, string param, string value)
        {
            Microsoft.Reporting.WinForms.ReportParameter Param = new Microsoft.Reporting.WinForms.ReportParameter(param, value);
            lr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { Param });
        }

        private static string GetSheetName(List<ExcelSheet> list, string Testid)
        {
            string result = "";
            foreach (ExcelSheet es in list)
            {
                if (es.TestID == Testid)
                {
                    result = es.Name;
                    break;
                }
            }
            if (result == "")
            {
                ExcelSheet es = new ExcelSheet();
                es.TestID = Testid;
                es.Name = "第" + Convert.ToString(list.Count + 1) + "次测试结果";
                list.Add(es);
                result = es.Name;
            }
            return result;
        }

        private static void setDataSetByID(DataSet dsto,DataSet dsfrom, string testid)
        {
            if (dsto.Tables[testid] == null)
            {
                DataTable dt=dsfrom.Tables[0].Copy();
                dsto.Tables.Add(dt);
                dsto.Tables[dsto.Tables.Count - 1].TableName = testid;
            }
            foreach (DataColumn dc in dsfrom.Tables[0].Columns)
            {
                if (dsto.Tables[testid].Columns.IndexOf(dc) < 0)
                {
                    dsto.Tables[testid].Columns.Add(dc);
                }
            }

            foreach (DataRow dr in dsfrom.Tables[0].Rows)
            {
                dsto.Tables[testid].Rows.Add(dr.ItemArray);
            }
        }




        /// <summary>
        ///调用一下存储过程，执行数据上传
        ///p_Symtant_Move_Out
        ///p_Symtant_Test_Data
        /// </summary>
        /// <param name="TestResult">测试结果</param>
        /// <returns>返回执行结果，OK开头标识正常，ER开头标识有出现异常情况，需要查看具体的错误信息。</returns>
        public static string Product_Test_Finished(Test_Result TestResult)
        {
            string result = "";
            TestResult.Test_ID = "";
            string strselect = @"
                                SELECT * FROM G_TEST_SN WHERE 1=1 
                                AND SERIAL_NUMBER='{0}' 
                                AND MODEL='{1}' 
                                AND MACHINE_DESC='{2}' 
                                AND OTHERLAB='{3}' 
                                AND TEST_ENVIR='{4}' 
                                ORDER BY UPDATE_TIME DESC
            ";
            DataSet ds = DB.GetDataSetFromSQL(string.Format(strselect,
                TestResult.SN,
                TestResult.partnumber,
                TestResult.Machine_Desc,
                TestResult.otherlab,
                TestResult.test_envir));
            if (ds.Tables[0].Rows.Count > 0)
            {
                TestResult.Test_ID = Convert.ToString(ds.Tables[0].Rows[0]["TEST_ID"]);
            }
            result = DB.ExecProc("p_Symtant_Move_Out",
                                   "@SN=" + TestResult.SN,
                                   "@UserID=" + TestResult.UserID,
                                   "@companyname=" + TestResult.companyname,
                                   "@customername=" + TestResult.customername,
                                   "@partnumber=" + TestResult.partnumber,
                                   "@otherlab=" + TestResult.otherlab,
                                   "@test_envir=" + TestResult.test_envir,
                                   "@Result=" + TestResult.Result,
                                   "@Machine_Desc=" + TestResult.Machine_Desc,
                                   "@Test_Time=" + TestResult.Test_Time.ToString("yyyy-MM-dd HH:mm:ss"),
                                   "@Test_ID=" + TestResult.Test_ID
                                );
            if (result.Split(new char[] { ':' })[0] != "OK")
            { return result; }
            TestResult.Test_ID = result.Split(new char[] { ':' })[1];
            SqlBulkCopy blockInsert = new SqlBulkCopy(CommSecurity.GetConnectionString());
            blockInsert.DestinationTableName = "G_TEST_DATA";
            DataTable dataTable = DB.GetDataSetFromSQL("SELECT TOP 1 * FROM G_TEST_DATA").Tables[0].Clone();
            foreach (Test_Data td in TestResult.TestDataList)
            {
                DataRow dr=dataTable.NewRow();
                dr["Test_ID"] = Convert.ToString(TestResult.Test_ID);
                dr["Index"] = Convert.ToString( td.Index);
                dr["Item"] =  Convert.ToString(td.Item);
                dr["Ports"] = Convert.ToString( td.Ports);
                dr["Att"] =  Convert.ToString(td.Att);
                dr["Phase"] =  Convert.ToString(td.Phase);
                dr["Frequency"] = Convert.ToString( td.Frequency);
                dr["Test_Result"] =  Convert.ToString(td.Test_Result);
                dr["UCL"] =  Convert.ToString(td.UCL);
                dr["LCL"] =  Convert.ToString(td.LCL);
                dr["Fail_Sign"] =  Convert.ToString(td.Fail_Sign);
                dr["Kind"] = Convert.ToString( td.Kind);
                dataTable.Rows.Add(dr);
            }
            if (dataTable != null && dataTable.Rows.Count != 0)
            {
                blockInsert.WriteToServer(dataTable);
            }
            blockInsert.Close(); 

            //foreach (Test_Data td in TestResult.TestDataList)
            //{
            //    result = DB.ExecProc("p_Symtant_Test_Data",
            //                       "@Test_ID=" + TestResult.Test_ID,
            //                       "@Item=" + td.Item,
            //                       "@Index=" + td.Index,
            //                       "@Ports=" + td.Ports,
            //                       "@Phase=" + td.Phase,
            //                       "@Att=" + td.Att,
            //                       "@Frequency=" + td.Frequency,
            //                       "@Test_Result=" + td.Test_Result,
            //                       "@UCL=" + td.UCL,
            //                       "@LCL=" + td.LCL,
            //                       "@Fail_Sign=" + td.Fail_Sign,
            //                       "@Kind=" + td.Kind
            //                    );
            //    if (result.Split(new char[] { ':' })[0] != "OK")
            //    { return result; }
            //}
            return result;
        }


        /// <summary>
        /// 从DataSet导出数据到Excel
        /// </summary>
        /// <param name="sourceDs">DataSet数据源</param>
        /// <param name="sheetName">Excel标签页名称</param>
        /// <returns>0:成功;9999:异常错误</returns>
        public static int ExportExcelByNPOI(DataSet sourceDs, string[] sheetNames)
        {
            int result = 999;
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xlsx)|*.xlsx|Execl files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            //saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "Export Excel File";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!"".Equals(saveFileDialog.FileName))
                {

                    IWorkbook workbook = new XSSFWorkbook();
                    //string[] sheetNames = sheetName.Split(';');
                    for (int i = 0; i < sheetNames.Length; i++)
                    {
                        ISheet sheet = workbook.CreateSheet(sheetNames[i]);
                        IRow headerRow = sheet.CreateRow(0);
                        // handling header.
                        foreach (DataColumn column in sourceDs.Tables[i].Columns)
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                        // handling value.
                        int rowIndex = 1;
                        foreach (DataRow row in sourceDs.Tables[i].Rows)
                        {
                            IRow dataRow = sheet.CreateRow(rowIndex);
                            foreach (DataColumn column in sourceDs.Tables[i].Columns)
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                            }
                            rowIndex++;
                        }
                    }

                    FileStream sw = File.Create(saveFileDialog.FileName);
                    workbook.Write(sw);
                    sw.Close();
                    result = 0;
                }
            }
            return result;
        }

        /// <summary>
        /// Excel文件导成Datatable
        /// </summary>
        /// <param name="strFilePath">Excel文件目录地址</param>
        /// <param name="strTableName">Datatable表名</param>
        /// <param name="iSheetIndex">Excel sheet index</param>
        /// <returns></returns>
        public static System.Data.DataTable XlSToDataTable(string strFilePath, string strTableName, string iSheetName)
        {

            string strExtName = Path.GetExtension(strFilePath);

            System.Data.DataTable dt = new System.Data.DataTable();
            if (!string.IsNullOrEmpty(strTableName))
            {
                dt.TableName = strTableName;
            }

            if (strExtName.Equals(".xls") || strExtName.Equals(".xlsx"))
            {
                using (FileStream file = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    IWorkbook workbook = null;
                    if (strExtName.Equals(".xls")) workbook = new HSSFWorkbook(file);
                    else workbook = new XSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheet(iSheetName);

                    //列头
                    foreach (ICell item in sheet.GetRow(sheet.FirstRowNum).Cells)
                    {
                        dt.Columns.Add(item.ToString(), typeof(string));
                    }

                    //写入内容
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                    while (rows.MoveNext())
                    {
                        IRow row = (IRow)rows.Current;
                        if (row.RowNum == sheet.FirstRowNum)
                        {
                            continue;
                        }

                        DataRow dr = dt.NewRow();
                        foreach (ICell item in row.Cells)
                        {
                            switch (item.CellType)
                            {
                                case CellType.Boolean:
                                    dr[item.ColumnIndex] = item.BooleanCellValue;
                                    break;
                                case CellType.Error:
                                    dr[item.ColumnIndex] = NPOI.SS.Formula.Eval.ErrorEval.GetText(item.ErrorCellValue);
                                    break;
                                case CellType.Formula:
                                    switch (item.CachedFormulaResultType)
                                    {
                                        case CellType.Boolean:
                                            dr[item.ColumnIndex] = item.BooleanCellValue;
                                            break;
                                        case CellType.Error:
                                            dr[item.ColumnIndex] = NPOI.SS.Formula.Eval.ErrorEval.GetText(item.ErrorCellValue);
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(item))
                                            {
                                                dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = item.NumericCellValue;
                                            }
                                            break;
                                        case CellType.String:
                                            string str = item.StringCellValue;
                                            if (!string.IsNullOrEmpty(str))
                                            {
                                                dr[item.ColumnIndex] = str.ToString();
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = null;
                                            }
                                            break;
                                        case CellType.Unknown:
                                        case CellType.Blank:
                                        default:
                                            dr[item.ColumnIndex] = string.Empty;
                                            break;
                                    }
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(item))
                                    {
                                        dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = item.NumericCellValue;
                                    }
                                    break;
                                case CellType.String:
                                    string strValue = item.StringCellValue;
                                    if (!string.IsNullOrEmpty(strValue))
                                    {
                                        dr[item.ColumnIndex] = strValue.ToString();
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = null;
                                    }
                                    break;
                                case CellType.Unknown:
                                case CellType.Blank:
                                default:
                                    dr[item.ColumnIndex] = string.Empty;
                                    break;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }

        public static DataTable FormatDataTableFromColIndex(DataTable dt, int index)
        {
            DataTable dtout = dt.Clone();
            for (int i = index; i < dt.Columns.Count; i++)
            {
                dtout.Columns[i].DataType = typeof(System.String);
                dtout.Columns[i].ColumnName = NumToStringWithUnit(System.Convert.ToDouble(dtout.Columns[i].ColumnName), "#0.000");
            }

            foreach (DataRow dr in dt.Rows)
            {
                DataRow drout = dtout.Rows.Add(dr.ItemArray);
                for (int i = index; i < dt.Columns.Count; i++)
                {
                    string strValue = Convert.ToString(drout[i]);
                    string svalue = NumToStringWithUnit(System.Convert.ToDouble(strValue), "#0.000");
                    drout[i] = svalue;
                }
            }
            return dtout;
        }
        public static string NumToStringWithUnit(double DataValue, string NumFormat = "")
        {
            if (DataValue == 0)
            {
                return "0";
            }
            else
            {
                int n = (int)Math.Floor(Math.Log10(Math.Abs(DataValue)) / 3);
                string preStr = System.Convert.ToString((DataValue / Math.Pow(10, 3 * n)));
                if ("".Equals(NumFormat))
                {
                    preStr = System.Convert.ToString(Convert.ToDouble(preStr));
                }
                else
                {
                    preStr = Math.Round(Convert.ToDouble(preStr), 5).ToString(NumFormat);
                }
                switch (n)
                {
                    case 4:
                        return preStr + "T";
                    case 3:
                        return preStr + "G";
                    case 2:
                        return preStr + "M";
                    case 1:
                        return preStr + "K";
                    case 0:
                        return preStr + "";
                    case -1:
                        return preStr + "m";
                    case -2:
                        return preStr + "u";
                    case -3:
                        return preStr + "n";
                    case -4:
                        return preStr + "p";
                    case -5:
                        return preStr + "f";
                    default:
                        return DataValue.ToString("e");
                }
            }
        }

        public static double StringToNumWithUnit(string str)
        {
            double result = 0;
            str = str.Trim();
            string mark = str.Substring(str.Length - 1, 1);
            try
            {
                Convert.ToDouble(mark);
                result = Convert.ToDouble(str);
            }
            catch
            {
                int n = 0;
                if (mark == "T") n = 4;
                if (mark == "G") n = 3;
                if (mark == "M") n = 2;
                if (mark == "K") n = 1;
                if (mark == " ") n = 0;
                if (mark == "m") n = -1;
                if (mark == "u") n = -2;
                if (mark == "n") n = -3;
                if (mark == "p") n = -4;
                if (mark == "f") n = -5;
                result = Convert.ToDouble(str.Substring(0, str.Length - 1));
                n = n * 3;
                result = result * Math.Pow(10, n);
            }

            return result;
        }
    }

    public class DB
    {
        public static DataSet GetDataSetFromSQL(string SQL)
        {
            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(SQL, conn);
            cmd.CommandTimeout = 600;
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = cmd;
            DataSet ds = new DataSet();
            sqlDA.Fill(ds);
            conn.Close();
            return ds;
        }
        public static object GetSingleValueFromSQL(string SQL)
        {
            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(SQL,conn);
            object value = (object)cmd.ExecuteScalar();
            conn.Close();
            return value;
        }

        public static byte[] GetImageDatatFromSQL(int id)
        {
            byte[] content;
            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT STATEDATA FROM SYS_TEST_PLAN WHERE TESTPLANID=" + id, conn);
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = cmd;
            DataSet ds = new DataSet();
            sqlDA.Fill(ds);
            DataRow dr=ds.Tables[0].Rows[0];
            if (dr["STATEDATA"] != DBNull.Value)
            {
                content = (byte[])dr["STATEDATA"];
                return content;
            }
            else
            {
                return null;
            }
        }
        public static string ExecSQL(string SQL, params string[] Parameters)
        {
            string result = "OK";
            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(SQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 600;
            foreach (string param in Parameters)
            {
                if (!string.IsNullOrWhiteSpace(param))
                {
                    string paramname = param.Split('=')[0].Trim();
                    string paramvalue = param.Split('=')[1].Trim();
                    cmd.Parameters.Add(paramname, DbType.String);
                    cmd.Parameters[paramname].Value = paramvalue;
                }
            }
            try
            {
                int i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { result = "ER:" + ex.Message; }
            conn.Close();
            return result;
        }

        public static string ExecSQL(string SQL, params DBParameters[] Parameters)
        {
            string result = "OK";
            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(SQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 600;
            foreach (DBParameters param in Parameters)
            {
                if (param != null)
                {
                    cmd.Parameters.Add(param.ColName, param.Type);
                    cmd.Parameters[param.ColName].Value = param.Value;
                }
            }
            try
            {
                int i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { result = "ER:" + ex.Message; }
            conn.Close();
            return result;
        }

        public static string ExecProc(string ProName, params DBParameters[] Parameters)
        {
            string result = "OK";
            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(ProName, conn);
            cmd.CommandTimeout = 600;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TRES", SqlDbType.NVarChar, 100);
            cmd.Parameters["@TRES"].Direction = ParameterDirection.Output;
            foreach (DBParameters param in Parameters)
            {
                if (param != null)
                {
                    cmd.Parameters.Add(param.ColName, param.Type);
                    cmd.Parameters[param.ColName].Value = param.Value;
                }
            }
            try
            {
                int i = cmd.ExecuteNonQuery();
                result = Convert.ToString(cmd.Parameters["@TRES"].Value);
            }
            catch (Exception ex)
            { result = "ER:" + ex.Message; }
            conn.Close();
            return result;
        }


        public static string ExecProc(string ProName, params string[] Parameters)
        {
            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(ProName, conn);
            cmd.CommandTimeout = 600;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TRES", SqlDbType.NVarChar, 100);
            cmd.Parameters["@TRES"].Direction = ParameterDirection.Output;
            foreach (string param in Parameters)
            {
                string paramname = param.Split('=')[0].Trim();
                string paramvalue = param.Split('=')[1].Trim().Replace("::", "=");
                cmd.Parameters.Add(paramname, SqlDbType.NVarChar);
                cmd.Parameters[paramname].Value = paramvalue;
            }
            cmd.ExecuteNonQuery();
            conn.Close();
            return Convert.ToString(cmd.Parameters["@TRES"].Value);
        }

        public static DataSet QueryProc(string ProName, ref string TRES, params string[] Parameters)
        {
            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(ProName, conn);
            cmd.CommandTimeout = 600;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TRES", SqlDbType.NVarChar, 100);
            cmd.Parameters["@TRES"].Direction = ParameterDirection.Output;
            foreach (string param in Parameters)
            {

                string paramname = param.Split('=')[0].Trim();
                string paramvalue = param.Split('=')[1].Trim().Replace("::", "="); ;
                cmd.Parameters.Add(paramname, SqlDbType.NVarChar);
                cmd.Parameters[paramname].Value = paramvalue;
            }
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = cmd;
            DataSet ds = new DataSet();
            sqlDA.Fill(ds);
            TRES = Convert.ToString(cmd.Parameters["@TRES"].Value); ;
            conn.Close();
            return ds;
        }
        public static DataSet GetDatatableProc(string ProName,string Parameter)
        {
            DataSet ds = new DataSet();
            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(ProName,conn);
            cmd.CommandTimeout = 600;
            cmd.CommandType = CommandType.StoredProcedure;
            if (Parameter !=null)
            {

                string paramname = "@strWhere";
                string paramvalue = Parameter.Substring(11);
                cmd.Parameters.Add(paramname, SqlDbType.NVarChar);
                cmd.Parameters[paramname].Value = paramvalue;            
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds,"ds");
            return ds;

        }
        public static DataSet GetDatatableProc2(string ProName, params string[] Parameters)
        {
            DataSet ds = new DataSet();
            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(ProName, conn);
            cmd.CommandTimeout = 600;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (string param in Parameters)
            {

                string paramname = param.Split('=')[0].Trim();
                string paramvalue = param.Split('=')[1].Trim().Replace("::", "="); ;
                cmd.Parameters.Add(paramname, SqlDbType.NVarChar);
                cmd.Parameters[paramname].Value = paramvalue;
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "ds");
            return ds;

        }
    }

    public class DBParameters
    {
        private SqlDbType type;

        public SqlDbType Type
        {
            get { return type; }
            set { type = value; }
        }
        private string colName;

        public string ColName
        {
            get { return colName; }
            set { colName = value; }
        }
        private object value;

        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        public DBParameters(SqlDbType type, string colName, object value)
        {
            this.type = type;
            this.colName = colName;
            this.value = value;
        }
    }

    public class CommSecurity
    {
        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return StrToHex(Convert.ToBase64String(mStream.ToArray()));
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                decryptString = HexToStr(decryptString);
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        public static string StrToHex(string str)
        {
            string res = "";
            for (int i = 0; i < str.Length; i++)
            {
                res += Convert.ToString(str[i], 16);
            }
            return res;
        }

        public static string HexToStr(string str)
        {
            string res = "";
            for (int i = 0; i < (str.Length / 2); i++)
            {
                res += Convert.ToChar(Convert.ToByte(str.Substring(i * 2, 2), 16));
            }
            return res;
        }

        public static string GetMD5HashFromByte(byte[] data)
        {
            string str;
            try
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(data, 0, data.Length);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    builder.Append(buffer[i].ToString("X2"));
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw new Exception("GetMD5HashFromByte() fail,error:" + exception.Message);
            }
            return str;
        }

        public static string GetConnectionString()
        {
            return DecryptDES(ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString, "RMES2014");
        }

        public static void SetCommandParam(ref SqlCommand cmd, SqlDbType dbType, string name, object value)
        {
            if (name.Substring(0, 1) != "@")
            {
                name = "@" + name;
            }
            cmd.Parameters.Add(name, dbType);
            cmd.Parameters[name].Value = value;
        }
    }

    public class NotBool : MarkupExtension, IValueConverter
    {
        public object Convert(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class IntoToColorConverter : MarkupExtension, IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            //if (value.GetType() == typeof(DataRowState))
            //{
            //    if ((DataRowState)value != DataRowState.Unchanged)
            //        return Brushes.Red;
            //    else
            //        return Brushes.Black;
            //}
            //else return Brushes.Black;
            //if ((string)value == "FAIL")
            //    return Brushes.Red;
            //else
            //    return Brushes.Black;
            //if ((string)value == "FAIL")
            //    return new LinearGradientBrush(
            //            Color.FromArgb(100, 255, 0, 0),
            //            Color.FromArgb(0, 255, 0, 0), 0);
            //else
            //    return Brushes.White;
            if (!string.IsNullOrWhiteSpace(value.ToString()))
            {
                if (value.ToString() == "FAIL")
                {
                    return Brushes.Red;
                }
                else
                {
                    return Brushes.Black;
                }
            }
            else
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class RowColorConverter : MarkupExtension, IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            SolidColorBrush scb1 = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            SolidColorBrush scb2 = new SolidColorBrush(Color.FromRgb(241, 252, 255));
            if ((bool)value)
                return scb1;
            else
                return scb2;
        }

        public object ConvertBack(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class SizeConvert : MarkupExtension, IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            object result = 0;
            try
            {
                result = System.Convert.ToDouble(value) + System.Convert.ToDouble(parameter);
            }
            catch { result = value; }
            return result;
        }

        public object ConvertBack(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class IsShowTempCond : MarkupExtension, IValueConverter
    {
        public object Convert(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            System.Windows.Visibility result = System.Windows.Visibility.Hidden;
            if ((string)value == "什么高低温")
            {
                result = System.Windows.Visibility.Visible;
            }

            return result;
        }

        public object ConvertBack(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class ShowProcessInfo : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            string result = "请稍等...";
            if (values.Count() == 2)
            {
                result = "请稍等...{0}/{1}";
                result = string.Format(result, System.Convert.ToString(values[0]), System.Convert.ToString(values[1]));
            }
            return result;
        }

        public object[] ConvertBack(object value, System.Type[] targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class ShowExportInfo : MarkupExtension,IValueConverter
    {

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = "";
            if (System.Convert.ToInt32(value) == 0)
            {
                result = "正在导出文件...";
            }
            else
            {
                result = "文件导出成功！";
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public struct ExcelSheet
    {
        public string Name;
        public string TestID;
    }

    public struct TableRangle
    {
        public int ColBegin;
        public int ColEnd;
        public int RowBegin;
        public int RowEnd;
    }

    public class ChartPoint
    {
        public double dx { get; set; }
        public string sx { get; set; }
        public double y { get; set; }
        public ChartPoint()
        { }
        public ChartPoint(string x, double y)
        {
            this.sx = x;
            this.y = y;
        }

        public ChartPoint(double x, double y)
        {
            this.dx = x;
            this.y = y;
        }
    }

    public class LoadAssembly
    {
        public static IValueConverter GetIValueConverter(string[] info)
        {
            IValueConverter iValue = null;
            if (info.Count() > 1)
            {
                Assembly assembly = Assembly.LoadFrom(info[0]);
                Type modelType = assembly.GetType(info[1]);
                iValue = Activator.CreateInstance(modelType) as IValueConverter;
            }
            return iValue;
        }
    }

    public class CSVFile
    {
        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fullPath">CSV的文件路径</param>
        public static void SaveCSV(DataTable dt, string fullPath)
        {
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";
            //写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);
            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                    if (str.Contains(',') || str.Contains('"')
                        || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                    {
                        str = string.Format("\"{0}\"", str);
                    }

                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
            MessageBoxResult mbResult = DXMessageBox.Show("CSV文件导出成功,是否打开该文件？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (mbResult == MessageBoxResult.Yes)
            {
                System.Diagnostics.Process.Start(fullPath);
            }
        }

        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable ReadCSV(string filePath)
        {
            Encoding encoding = GetType(filePath);
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, encoding);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }
            sr.Close();
            fs.Close();
            return dt;
        }

        /// <summary> 
        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型 
        /// </summary> 
        /// <param name=“fileName“>文件路径</param> 
        /// <returns>文件的编码类型</returns> 
        public static Encoding GetType(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            Encoding r = GetType(fs);
            fs.Close();
            return r;
        }

        /// <summary> 
        /// 通过给定的文件流，判断文件的编码类型 
        /// </summary> 
        /// <param name=“fs“>文件流</param> 
        /// <returns>文件的编码类型</returns> 
        public static Encoding GetType(FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
            Encoding reVal = Encoding.Default;

            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;

        }

        /// <summary> 
        /// 判断是否是不带 BOM 的 UTF8 格式 
        /// </summary> 
        /// <param name=“data“></param> 
        /// <returns></returns> 
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
            byte curByte; //当前分析的字节. 
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前 
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1 
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }

    }

    public class ToConvert<T> where T : new()
    {
        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public DataTable ToDataTable(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData(modelList[0]);

            foreach (T model in modelList)
            {
                Type t = model.GetType();
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in t.GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private DataTable CreateData(T model)
        {
            Type t = model.GetType();
            DataTable dataTable = new DataTable(t.Name);
            foreach (PropertyInfo propertyInfo in t.GetProperties())
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }

        /// <summary>
        /// 将DataRow行转换成EntityClass
        /// </summary>
        /// <param name="dr">行数据</param>
        /// <param name="model">实体类结构</param>
        /// <returns></returns>
        public T ToEntityClass(DataRow dr, T model)
        {
            Type info = model.GetType();
            object obj = Activator.CreateInstance(info);
            T tempModel = (T)obj;
            foreach (PropertyInfo propertyInfo in info.GetProperties())
            {
                propertyInfo.SetValue(tempModel, Convert.ChangeType(dr[propertyInfo.Name], propertyInfo.PropertyType), null);
            }
            return tempModel;
        }

        /// <summary>
        /// 将DataTable转换成EntityClass列表
        /// </summary>
        /// <param name="model">实体类结构</param>
        /// <param name="dt">数据DataTable</param>
        /// <returns></returns>
        public ObservableCollectionCore<T> ToList(T model, DataTable dt)
        {
            ObservableCollectionCore<T> list = new ObservableCollectionCore<T>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(ToEntityClass(dr, model));
            }
            return list;
        }

    }

    public class TableRowColorConverter : MarkupExtension, IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            SolidColorBrush scb1 = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            SolidColorBrush scb2 = new SolidColorBrush(Color.FromRgb(241, 252, 255));
            
            if ((bool)value)
                return scb1;
            else
                return scb2;
        }

        public object ConvertBack(object value, System.Type targetType,
                    object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }
    
    public class TableIntoToColorConverter : MarkupExtension, IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() == typeof(DataRowState))
            {
                if ((DataRowState)value != DataRowState.Unchanged)
                    return Brushes.Red;
                else
                    return Brushes.Black;
            }
            else return Brushes.Black;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class SNRule
    {
        int i = 0;
        private void Find(char[] ch)
        {
            bool isFlag = false;
            i++;
            string result = "";
            string data = "";
            if (ch[ch.Length - i] - '0' >= 0 && ch[ch.Length - i] - '0' <= 9)
            {
                data = Convert.ToString(Convert.ToInt32(ch[ch.Length - i].ToString()) + 1);
            }
            else if ((ch[ch.Length - i] >= 'A' && ch[ch.Length - i] <= 'Z') || (ch[ch.Length - i] >= 'a' && ch[ch.Length - i] <= 'z'))
            {
                data = Convert.ToString(Convert.ToChar(ch[ch.Length - i] + 1));
            }
            if ((ch.Length - i == 0 && ch[ch.Length - i] - '0' == 9) || (ch.Length - i == 0 && ch[ch.Length - i] == 'Z') || (ch.Length - i == 0 && ch[ch.Length - i] == 'z'))
            {
                sn = "ERR:序号超过条码规则长度!";
                isFlag = false;
            }
            else if (ch[ch.Length - i] - '0' == 9)
            {
                ch[ch.Length - i] = '0';
                Find(ch);
            }
            else if (ch[ch.Length - i] == 'Z')
            {
                ch[ch.Length - i] = 'A';
                Find(ch);
            }
            else if (ch[ch.Length - i] == 'z')
            {
                ch[ch.Length - i] = 'a';
                Find(ch);
            }
            else
            {
                ch[ch.Length - i] = Convert.ToChar(data);
                result = new string(ch);
                isFlag = true;
            }
            if (isFlag)
            {
                sn = result;
            }
        }
        private bool Compare(char[] ch, char[] ch2)
        {
            bool isFlag = false;
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i] != ch2[i])
                {
                    isFlag = true;
                    break;
                }
            }
            return isFlag;
        }
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
        string sn = "";
        public  string SN_Rule(string str, string str2)
        {
            i = 0;
            string[] data = str2.Split(new char[] { '[', ']' }, StringSplitOptions.None);
            sn = str.Substring(data[0].Length, data[1].Length);
            Find(sn.ToArray());
            if (sn.StartsWith("ERR"))
            {
                return sn;
            }
            else
            {
                sn = data[0] + sn + data[2];
            }
            return sn;
        }
        public  string Check_Rule(string str, string str2)
        {
            string result = "";
            if (str.Length + 2 == str2.Length)
            {
                string[] data = str2.Split(new char[] { '[', ']' }, StringSplitOptions.None);
                if (!reg.IsMatch(data[1]))
                {
                    result = "ERR:无效字符!";
                    return result;
                }
                else
                {
                    int start = data[0].Length + data[1].Length;
                    int count = data[2].Length;
                    if (Compare(data[0].ToArray(), str.ToArray()) || Compare(data[2].ToArray(), str.Substring(start, count).ToArray()))
                    {
                        result = "ERR:不符合条码规则!";
                        return result;
                    }
                }
            }
            else
            {
                result = "ERR:序号与条码规则长度不等!";
                return result;
            }
            return result;
        }
    }

}
