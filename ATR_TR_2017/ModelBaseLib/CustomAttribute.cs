using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;


namespace ModelBaseLib
{
    /// <summary>
    /// 标记ViewModel或者Model中的属性是否在总览表格中显示
    /// </summary>
    public class UIDisplayAttribute:System.Attribute
    {
        public UIDisplayAttribute(string displayName, Type converterType)
        {
            DisplayName = displayName;
            if (converterType != null)
            {
                Converter = Activator.CreateInstance(converterType) as IValueConverter;
            }
        }
        public UIDisplayAttribute(string displayName):this(displayName,null)
        {
        }
        public UIDisplayAttribute(string displayName, Type converterType, string itemSourceName):this(displayName,converterType)
        {
            //if (itemSourceProviderType != null)
            //{
            //    IDataProvider provider = Activator.CreateInstance(itemSourceProviderType) as IDataProvider;
            //    if (provider != null)
            //    {
            //        ItemSource = provider.GetValue(displayName);
            //    }
            //}
            ItemSourceName = itemSourceName;
            IsComb = true;
        }
        public UIDisplayAttribute(string displayName, Type converterType, params object[] itemSource)
            : this(displayName, converterType)
        {
            ItemSource = itemSource;
            IsComb = true;
        }
        /// <summary>
        /// 在表格中的列名
        /// </summary>
        public string DisplayName { get; set; }
        public string UIPath { get; set; }
        public IValueConverter Converter { get; set; }
        public string ItemSourceName { get; set; }
        /// <summary>
        /// 如果是复选框，首先查看并使用ItemSource，如果ItemSource是Null，
        /// 从类中查找名字为ItemSourceName属性作为复选框ItemSource
        /// </summary>
        public object ItemSource { get; set; }
        public bool IsComb { get; set; }
    }
    public class UIDisplayParaAttribute : System.Attribute
    {
        public UIDisplayParaAttribute(string displayName)
        {
             this.DisplayName=displayName;
        }
        public UIDisplayParaAttribute(string displayName, Type converterType)
            : this(displayName)
        {
            if (converterType != null)
            {
                Converter = Activator.CreateInstance(converterType) as IValueConverter;
            }
        }
        public IValueConverter Converter { get; set; }
        public string DisplayName{get;set;}
    }
    public class UITestStepParaAttribute : System.Attribute
    {
        public UITestStepParaAttribute(string displayName,string stepType,string userControlType)
        {
            this.DisplayName = displayName;
            this.StepTypeStr = stepType;
            this.UserControlType = userControlType;
        }
        public UITestStepParaAttribute(string displayName,string stepType,string userControlType,bool isFixedItem):this( displayName, stepType, userControlType)
        {
            this.IsFixedItem = isFixedItem;
        }
        public UITestStepParaAttribute(string displayName, string stepType, string userControlType, bool isFixedItem, bool isNeedCal)
            : this(displayName, stepType, userControlType)
        {
            this.IsNeedCal = isNeedCal;
        }
        public string DisplayName { get; set; }
        public string StepTypeStr { get; set; }
        public string UserControlType { get; set; }
        public bool IsFixedItem { get; set; }
        public bool IsNeedCal { get; set; }
    }
    public class UITestItemParaAttribute : System.Attribute
    {
        public UITestItemParaAttribute(string itemViewModelTypeStr,string userControlType)
        {
            this.ItemViewModelTypeStr = itemViewModelTypeStr;
            this.UserControlType = userControlType;
        }
        public string ItemViewModelTypeStr { get; set; }
        public string UserControlType { get; set; }
    }
    public class UIUserControlParaAttribute : System.Attribute
    {
        public UIUserControlParaAttribute(string displayName,string userControlTypeStr)
        {
            this.DisplayName = displayName;
            this.UserControlTypeStr = userControlTypeStr;
        }
        public UIUserControlParaAttribute(string displayName):this(displayName,null)
        { 
        }
        public string DisplayName { get; set; }
        public string UserControlTypeStr { get; set; }
    }
    public class UICalibrationParaAttribute : System.Attribute
    {
        public UICalibrationParaAttribute(string displayName,string typeStr)
        {
            this.DisplayName = displayName;
            this.TypeStr = typeStr;
        }
        public UICalibrationParaAttribute(string displayName):this(displayName,null)
        { 
        
        }
        public UICalibrationParaAttribute():this("",null)
        { 
        }
        public string DisplayName { get; set; }
        public string TypeStr { get; set; }
    }
}
