﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceInterface.myService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="myService.SymtIService")]
    public interface SymtIService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SymtIService/CheckUser", ReplyAction="http://tempuri.org/SymtIService/CheckUserResponse")]
        string CheckUser(string userid, string pwd, string ATEKIND, string ternimal);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SymtIService/GetMaterialList", ReplyAction="http://tempuri.org/SymtIService/GetMaterialListResponse")]
        string GetMaterialList(string Token, string strSeach);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SymtIService/GetProcessList", ReplyAction="http://tempuri.org/SymtIService/GetProcessListResponse")]
        string GetProcessList(string Token);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SymtIService/UpLoadStatFile", ReplyAction="http://tempuri.org/SymtIService/UpLoadStatFileResponse")]
        string UpLoadStatFile(string Token, string data, string MATERIALHANDLE, string FileName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SymtIService/CheckStatFileBySn", ReplyAction="http://tempuri.org/SymtIService/CheckStatFileBySnResponse")]
        string CheckStatFileBySn(string Token, string SN, string StateFileHasCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SymtIService/DownLoadStatFile", ReplyAction="http://tempuri.org/SymtIService/DownLoadStatFileResponse")]
        string DownLoadStatFile(string Token, string SN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SymtIService/CheckINBYSN", ReplyAction="http://tempuri.org/SymtIService/CheckINBYSNResponse")]
        string CheckINBYSN(string Token, string SN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SymtIService/CheckOUTBYSN", ReplyAction="http://tempuri.org/SymtIService/CheckOUTBYSNResponse")]
        string CheckOUTBYSN(string Token, string SN, string TestData);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SymtIServiceChannel : ServiceInterface.myService.SymtIService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SymtIServiceClient : System.ServiceModel.ClientBase<ServiceInterface.myService.SymtIService>, ServiceInterface.myService.SymtIService {
        
        public SymtIServiceClient() {
        }
        
        public SymtIServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SymtIServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SymtIServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SymtIServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string CheckUser(string userid, string pwd, string ATEKIND, string ternimal) {
            return base.Channel.CheckUser(userid, pwd, ATEKIND, ternimal);
        }
        
        public string GetMaterialList(string Token, string strSeach) {
            return base.Channel.GetMaterialList(Token, strSeach);
        }
        
        public string GetProcessList(string Token) {
            return base.Channel.GetProcessList(Token);
        }
        
        public string UpLoadStatFile(string Token, string data, string MATERIALHANDLE, string FileName) {
            return base.Channel.UpLoadStatFile(Token, data, MATERIALHANDLE, FileName);
        }
        
        public string CheckStatFileBySn(string Token, string SN, string StateFileHasCode) {
            return base.Channel.CheckStatFileBySn(Token, SN, StateFileHasCode);
        }
        
        public string DownLoadStatFile(string Token, string SN) {
            return base.Channel.DownLoadStatFile(Token, SN);
        }
        
        public string CheckINBYSN(string Token, string SN) {
            return base.Channel.CheckINBYSN(Token, SN);
        }
        
        public string CheckOUTBYSN(string Token, string SN, string TestData) {
            return base.Channel.CheckOUTBYSN(Token, SN, TestData);
        }
    }
}