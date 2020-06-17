using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace DS_PA_Update
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                DownLoadActivity();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public static void DownLoadActivity()
        {
            try
            {
                System.Diagnostics.Process[] pc = System.Diagnostics.Process.GetProcessesByName("DS_PA");
                for (int i = 0; i < pc.Length; i++)
                {
                    pc[i].Kill();
                }

                UpdateProgressBar.EditValue = null;
                UpdateProgressBar.Maximum = 0;
                UpdateProgressBar.Content = null;
                UpdateProgressBar.info = null;
                UpdateProgressBar.msg = null;

                string str = "本次更新内容:";
                string strInfo = "";
                int count = 0;
                int index = 0;
                String SQLCmd = @"select ID,Name,Path,FileTimeTag ,ModifyTime FROM ActivityFile_DS_PA";
                DataSet dsActivity = DB.GetDataSetFromSQL(SQLCmd);
                if (dsActivity.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow reader in dsActivity.Tables[0].Rows)
                    {
                        String FilePath = "";
                        if (string.IsNullOrEmpty(Convert.ToString(reader["Path"])))
                        {
                            FilePath = Environment.CurrentDirectory + "\\" + reader["Name"];
                        }
                        else
                        {
                            FilePath = Environment.CurrentDirectory + "\\" + reader["Path"] + "\\" + reader["Name"];
                        }


                        //String FilePath = Environment.CurrentDirectory + "\\" + reader["Path"] + "\\" + reader["ActivityName"];

                        if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                        }

                        if (Convert.ToString(reader["Name"]).ToUpper().Equals("DS_PA_UPDATE.EXE"))
                        {
                            continue;
                        }

                        string fileMD5str = "";
                        if (File.Exists(FilePath))
                        {
                            fileMD5str = GetMD5HashFromFile(FilePath);
                        }
                        string fileMD5db = Convert.ToString(reader["FileTimeTag"]);
                        if (fileMD5db != fileMD5str)
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        DXSplashScreen.Show<UpdateProgressBar>(WindowStartupLocation.Manual);

                        foreach (DataRow reader in dsActivity.Tables[0].Rows)
                        {
                            

                            String FilePath = "";
                            if (string.IsNullOrEmpty(Convert.ToString(reader["Path"])))
                            {
                                FilePath = Environment.CurrentDirectory + "\\" + reader["Name"];
                            }
                            else
                            {
                                FilePath = Environment.CurrentDirectory + "\\" + reader["Path"] + "\\" + reader["Name"];
                            }

                            //String FilePath = Environment.CurrentDirectory + "\\" + reader["Path"] + "\\" + reader["Name"];

                            if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                            }

                            if (Convert.ToString(reader["Name"]).ToUpper().Equals("DS_PA_UPDATE.EXE"))
                            {
                                continue;
                            }

                            string fileMD5str = "";
                            if (File.Exists(FilePath))
                            {
                                fileMD5str = GetMD5HashFromFile(FilePath);
                            }

                            string fileMD5db = Convert.ToString(reader["FileTimeTag"]);
                            if (fileMD5db != fileMD5str)
                            {
                                index++;
                                UpdateProgressBar.EditValue = index;
                                UpdateProgressBar.Maximum = count;
                                UpdateProgressBar.Content = index + "/" + count;
                                UpdateProgressBar.info = Path.GetFileName(FilePath);

                                DataSet ds = DB.GetDataSetFromSQL(string.Format("select [Filestream] from ActivityFile_DS_PA where ID='{0}'", reader["ID"]));
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["Filestream"]);
                                    Stream localFile = new FileStream(FilePath, FileMode.Create);
                                    localFile.Write(ms.ToArray(), 0, (int)ms.Length);
                                    localFile.Close();
                                    strInfo += Path.GetFileName(FilePath) + "\r\n";
                                    UpdateProgressBar.msg = str + "\r\n" + strInfo;
                                }
                            }
                        }

                        MessageBoxResult response = DXMessageBox.Show("程序需要重新启动才能应用更新,请点击 'OK' 重新启动程序!", "更新完成", System.Windows.MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        if (response == MessageBoxResult.OK)
                        {
                            System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\" + "DS_PA.exe");
                            Environment.Exit(0);
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                    }
                    if (count == 0)
                    {
                        System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\" + "DS_PA.exe");
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("更新失败" + ex.Message + Environment.NewLine + ex.StackTrace);
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

    }

    public class DB
    {
        public static DataSet GetDataSetFromSQL(string SQL)
        {
            string SQLStr = CommSecurity.GetConnectionString();
            SqlConnection conn = new SqlConnection(SQLStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(SQL, conn);
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = cmd;
            sqlDA.SelectCommand.CommandTimeout = 0;
            DataSet ds = new DataSet();
            sqlDA.Fill(ds);
            conn.Close();
            return ds;
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
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Environment.CurrentDirectory + "\\DS_PA.exe");
            return DecryptDES(config.ConnectionStrings.ConnectionStrings["ConnectionStringName"].ConnectionString, "RMES2014");
        }
    }
}
