using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using Newtonsoft.Json;

namespace DataUtils
{
    public class SymtantService
    {
        private myService.SymtIServiceClient TClient;

        public SymtantService()
        {
            TClient = new myService.SymtIServiceClient();
        }

        private void openTClient()
        {
            if (TClient.State != CommunicationState.Opened)
            {
                TClient.Open();
            }
        }

        private void closeTClient()
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
        public UserInfo CheckUser(string userid, string pwd, string ATEKIND, string terminal)
        {
            openTClient();
            string res = TClient.CheckUser(userid, pwd, ATEKIND, terminal);
            closeTClient();
            return LitJson.JsonMapper.ToObject<UserInfo>(res);
        }

        /// <summary>
        /// 获取产品物料清单
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="strSeach"></param>
        /// <returns></returns>
        public List<string> GetMaterialList(string Token, string strSeach)
        {
            List<string> lstMaterial = new List<string>();
            openTClient();
            string MaterialList = TClient.GetMaterialList(Token, strSeach);
            closeTClient();
            if (!MaterialList.StartsWith("ER"))
            {
                //解析数据

                string aa = "";
                lstMaterial.Add(aa);
            }
            return lstMaterial;
        }

        /// <summary>
        /// 获取生产工序清单
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public List<ProcessInfo> GetProcessList(string Token)
        {
            List<ProcessInfo> lstProcess = new List<ProcessInfo>();
            openTClient();
            string MaterialList = TClient.GetProcessList(Token);
            closeTClient();
            if (!MaterialList.StartsWith("ER"))
            {
                //解析数据
                ProcessInfo pro = new ProcessInfo();

                lstProcess.Add(pro);
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
        public string UpLoadStatFile(string Token, string data, string MATERIALHANDLE, string FileName)
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
        public string CheckStatFileBySn(string Token, string SN, string StateFileHasCode)
        {
            openTClient();
            string res = TClient.CheckStatFileBySn(Token, SN, StateFileHasCode);
            closeTClient();
            return res;
        }

        /// <summary>
        /// 状态文件更新
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="SN"></param>
        /// <returns></returns>
        public byte[] DownLoadStatFile(string Token, string SN)
        {
            openTClient();
            string res = TClient.DownLoadStatFile(Token, SN);
            closeTClient();
            return Convert.FromBase64String(res);
        }

        /// <summary>
        /// 产品进站
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="SN"></param>
        /// <returns></returns>
        public string CheckINBYSN(string Token, string SN)
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
        public string CheckOUTBYSN(string Token, string SN, string TestData)
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
