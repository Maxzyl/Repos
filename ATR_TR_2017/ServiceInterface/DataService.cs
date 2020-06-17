using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;

namespace ServiceInterface
{
    public class DataService
    {
        public static myService.SymtIServiceClient TClient = new myService.SymtIServiceClient();

        //public DataService()
        //{
        //    TClient = new myService.SymtIServiceClient();
        //}

        private static void openTClient()
        {
            if (TClient.State != CommunicationState.Opened)
            {
                TClient = new myService.SymtIServiceClient();
                TClient.Open();
            }
        }

        private static void closeTClient()
        {
            if (TClient.State != CommunicationState.Closed)
            {
                TClient.Close();
            }
        }

        /// <summary>
        /// 用户验证，获取通讯Token
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pwd"></param>
        /// <param name="ATEKIND"></param>
        /// <param name="terminal"></param>
        /// <returns></returns>
        public static UserInfo CheckUser(string userid, string pwd, string ATEKIND, string terminal)
        {
            openTClient();
            string res = TClient.CheckUser(userid, pwd, ATEKIND, terminal);
            closeTClient();

            UserInfo user = new UserInfo();
            if (!res.StartsWith("ER"))
            {
                //解析数据
                user = LitJson.JsonMapper.ToObject<UserInfo>(res);
            }
            return user;
        }

        /// <summary>
        /// 获取产品物料清单
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="strSeach"></param>
        /// <returns></returns>
        public static List<string> GetMaterialList(string Token, string strSeach)
        {
            openTClient();
            string res = TClient.GetMaterialList(Token, strSeach);
            closeTClient();

            List<string> lstMaterial = new List<string>();
            if (!res.StartsWith("ER"))
            {
                //解析数据
                lstMaterial = LitJson.JsonMapper.ToObject<List<string>>(res);
            }
            return lstMaterial;
        }

        /// <summary>
        /// 获取生产工序清单
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static List<ProcessInfo> GetProcessList(string Token)
        {
            openTClient();
            string res = TClient.GetProcessList(Token);
            closeTClient();

            List<ProcessInfo> lstProcess = new List<ProcessInfo>();
            if (!res.StartsWith("ER"))
            {
                //解析数据
                lstProcess = LitJson.JsonMapper.ToObject<List<ProcessInfo>>(res);
            }
            return lstProcess;
        }

        /// <summary>
        /// 测试状态文件提交
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="data"></param>
        /// <param name="MATERIALHANDLE"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string UpLoadStatFile(string Token, string data, string MATERIALHANDLE, string FileName)
        {
            openTClient();
            string res = TClient.UpLoadStatFile(Token, data, MATERIALHANDLE, FileName);
            closeTClient();

            return res;
        }

        /// <summary>
        /// 状态文件检查
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="SN"></param>
        /// <param name="StateFileHasCode"></param>
        /// <returns></returns>
        public static bool CheckStatFileBySn(string Token, string SN, string StateFileHasCode)
        {
            openTClient();
            string res = TClient.CheckStatFileBySn(Token, SN, StateFileHasCode);
            closeTClient();

            return res.StartsWith("OK");
        }

        /// <summary>
        /// 状态文件更新
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="SN"></param>
        /// <returns></returns>
        public static byte[] DownLoadStatFile(string Token, string SN)
        {
            openTClient();
            string res = TClient.DownLoadStatFile(Token, SN);
            closeTClient();

            byte[] FileInfo = new byte[] { };
            if (!res.StartsWith("ER"))
            {
                //解析数据
                FileInfo = Convert.FromBase64String(res);
            }
            return FileInfo;
        }

        /// <summary>
        /// 产品进站
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="SN"></param>
        /// <returns></returns>
        public static string CheckINBYSN(string Token, string SN)
        {
            openTClient();
            string res = TClient.CheckINBYSN(Token, SN);
            closeTClient();
            return res;
        }

        /// <summary>
        /// 产品出站(提交测试数据)
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="SN"></param>
        /// <param name="TestData"></param>
        /// <returns></returns>
        public static string CheckOUTBYSN(string Token, string SN, string TestData)
        {
            openTClient();
            string res = TClient.CheckOUTBYSN(Token, SN, TestData);
            closeTClient();
            return res;
        }
    }

    public class UserInfo
    {
        public string TOKEN { get; set; }
        public string ATEKIND { get; set; }
        public string PROCESSID { get; set; }
        public string TERMINALID { get; set; }
        public string USERID { get; set; }
    }

    public class ProcessInfo
    {
        public string ProcessID { get; set; }
        public string ProcessNAME { get; set; }
    }
}
