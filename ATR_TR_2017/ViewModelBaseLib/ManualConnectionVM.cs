using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using System.Windows;
namespace ViewModelBaseLib
{
    public class ManualConnectionVM:NotifyBase
    {
        public ManualConnectionVM()
        {
        }
        private ManualConnection _ManualConn;
        private const string ManualConnPropertyName = "ManualConn";
        public ManualConnection ManualConn
        {
            get
            {
                return _ManualConn;
            }
            set
            {
                _ManualConn = value;
                NotifyPropertyChanged(ManualConnPropertyName);
            }           
        }
        //#endregion
        //private ManualConnection _ManualConn;
        //private const string ManualConnPropertyName = "ManualConn";
        //public ManualConnection ManualConn
        //{
        //    get
        //    {
        //        return _ManualConn;
        //    }
        //    set
        //    {
        //        _ManualConn = value;
        //        NotifyPropertyChanged(ManualConnPropertyName);
        //    }
        //}

        //private string _DisplayName;
        //private const string DisplayNamePropertyName = "DisplayName";
        //public string DisplayName
        //{
        //    get
        //    {
        //        return ManualConn.Name;
        //    }
        //    set
        //    {
        //        int count = TestPlan.ManualConnectionList.Where(x => x.Name == value).Count();
        //        if (count == 0)
        //        {
        //            ManualConn.Name = value;
        //            NotifyPropertyChanged(DisplayNamePropertyName);
        //            //if (Action != null)
        //            //{
        //            //    Action.Invoke();
        //            //}
        //        }
        //        else
        //        {
        //            DXMessageBox.Show("已经存在相同的名称，请重新命名", "提示", MessageBoxButton.YesNo);
        //        }
        //    }
        //}
        public TestPlan TestPlan { get; set; }
        private bool _IsTest;
        private const string IsTestPropertyName = "IsTest";
        public bool IsTest
        {
            get
            {
                return ManualConn.IsTest;
            }
            set
            {  
                ManualConn.IsTest = value;
                NotifyPropertyChanged(IsTestPropertyName);
            }
        }
        private const string NamePropertyName = "Name";
        public string Name
        {
            get
            {
                return ManualConn.Name;
            }
            set
            {
                int count = TestPlan.ManualConnectionList.Where(x => x.Name == value).Count();
                if (count == 0)
                {
                    ManualConn.Name = value;
                    NotifyPropertyChanged(NamePropertyName);
                    if (ManualConn.UpdateName != null)
                    {
                        ManualConn.UpdateName.Invoke();
                    }
                }
                else
                {
                    DXMessageBox.Show("已经存在相同的名称，请重新命名", "提示", MessageBoxButton.YesNo);
                }

            }
        }
        //前置延时
        private const string PreDelayPropertyName = "PreDelay";
        public double PreDelay
        {
            get
            {
                return ManualConn.PreDelay;
            }
            set
            {
                ManualConn.PreDelay = value;
                NotifyPropertyChanged(PreDelayPropertyName);
            }
        }

        //前置延时使能
        private const string IsPreDelayEnablePropertyName = "IsPreDelayEnable";
        public bool IsPreDelayEnable
        {
            get
            {
                return ManualConn.IsPreDelayEnable;
            }
            set
            {
                ManualConn.IsPreDelayEnable = value;
                NotifyPropertyChanged(IsPreDelayEnablePropertyName);
            }
        }

        //后置延时
        private const string PostDelayPropertyName = "PostDelay";
        public double PostDelay
        {
            get
            {
                return ManualConn.PostDelay;
            }
            set
            {
                ManualConn.PostDelay = value;
                NotifyPropertyChanged(PostDelayPropertyName);
            }
        }

        //后置延时使能
        private const string IsPostDelayEnablePropertyName = "IsPostDelayEnable";
        public bool IsPostDelayEnable
        {
            get
            {
                return ManualConn.IsPostDelayEnable;
            }
            set
            {
                ManualConn.IsPostDelayEnable = value;
                NotifyPropertyChanged(IsPostDelayEnablePropertyName);
            }
        }


        //步骤向导图片
        private const string ImageFileNamePropertyName = "ImageFileName";
        public string ImageFileName
        {
            get
            {
                return ManualConn.ImageFileName;
            }
            set
            {
                ManualConn.ImageFileName = value;
                NotifyPropertyChanged(ImageFileNamePropertyName);
            }
        }


        //向导图片描述
        private const string ImageDescriptionPropertyName = "ImageDescription";
        public string ImageDescription
        {
            get
            {
                return ManualConn.ImageDescription;
            }
            set
            {
                ManualConn.ImageDescription = value;
                NotifyPropertyChanged(ImageDescriptionPropertyName);
            }
        }


        //接续上步
        private const string IsFollowEnablePropertyName = "IsFollowEnable";
        public bool IsFollowEnable
        {
            get
            {
                return ManualConn.IsFollowEnable;
            }
            set
            {
                ManualConn.IsFollowEnable = value;
                NotifyPropertyChanged(IsFollowEnablePropertyName);
            }
        }

        //接续时长最小值
        private const string FollowTimeMinPropertyName = "FollowTimeMin";
        public double FollowTimeMin
        {
            get
            {
                return ManualConn.FollowTimeMin;
            }
            set
            {
                ManualConn.FollowTimeMin = value;
                NotifyPropertyChanged(FollowTimeMinPropertyName);
            }
        }

        //接续时长最大值
        private const string FollowTimeMaxPropertyName = "FollowTimeMax";
        public double FollowTimeMax
        {
            get
            {
                return ManualConn.FollowTimeMax;
            }
            set
            {
                ManualConn.FollowTimeMax = value;
                NotifyPropertyChanged(FollowTimeMaxPropertyName);
            }
        }

        //接续规则
        private const string FollowRulePropertyName = "FollowRule";
        public FollowRuleEnum FollowRule
        {
            get
            {
                return ManualConn.FollowRule;
            }
            set
            {
                ManualConn.FollowRule = value;
                NotifyPropertyChanged(FollowRulePropertyName);
            }
        }
        public FollowRuleEnum[] FollowRuleEnumList
        {
            get
            {
                return new FollowRuleEnum[] { FollowRuleEnum.超时自动测试, FollowRuleEnum.超时禁止测试 };
            }
        }
    }
}
