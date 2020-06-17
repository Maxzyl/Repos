using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ViewModelBaseLib;

using System.Windows.Controls;
using System.Collections.ObjectModel;
using Symtant.GeneFunLib;
using System.Windows;
using DevExpress.Xpf.Grid;

using DataUtils;
using DevExpress.Xpf.Docking;

namespace MeasurementUI
{

    public class IsSelectedConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isConnect = (bool)value;
            if (isConnect == true)
            {
              //    return new SolidColorBrush(Color.FromRgb(255,242,197));
                 return new SolidColorBrush(Colors.AliceBlue);
            }
            else
            {
                return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IsConnectConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isConnect=(bool)value;
            if(isConnect==true)
            {
                return new SolidColorBrush(Colors.LightGreen); 
            }
            else
            {
                return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ImageConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {  
            if(value !=null)
            {
                string str = value.ToString();
                return new BitmapImage(new Uri(str,UriKind.Relative));
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AbsoluteImageConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null && value != "")
                {
                    string str = value.ToString();
                    BitmapImage img = new BitmapImage(new Uri(str, UriKind.Absolute));
                    return img;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConnectImageConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value !=null)
            {
                bool isConnnect = (bool)value;
                if (isConnnect==true)
                {
                    return new BitmapImage(new Uri("/MeasurementUI;component/Images/Connnect.png", UriKind.Relative));
                }
                else
                {
                    return new BitmapImage(new Uri("/MeasurementUI;component/Images/UnConnect.png", UriKind.Relative));
                }
            }
            else
            {
                return new BitmapImage(new Uri("/MeasurementUI;component/Images/null1.png", UriKind.Relative));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class FileImageConverter : MarkupExtension, IValueConverter
    {

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = value.ToString();
            return new BitmapImage(new Uri(str, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ManuconnConverter : MarkupExtension, IValueConverter
    {

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value as ViewModelBaseLib.ManualConnectionVM != null)
            {
                ManualConnectionVM mn = value as ViewModelBaseLib.ManualConnectionVM;
                return null;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class CombConverter : MarkupExtension, IValueConverter
    {

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ContentControlConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
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
    public class TreeViewSelectedConverter : MarkupExtension, IValueConverter
    {

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {  
            //if(value as ViewModelBaseLib.ManualConnectionVM !=null)
            //{
            //    ManualConnectionVM mcNode = value as ViewModelBaseLib.ManualConnectionVM;
            //    if(mcNode.ManualConn as ManualConnection !=null)
            //    {
            //        ManualConnection mc = mcNode.ManualConn as ManualConnection;
            //        ManualConnectionVM mcVM = mcNode;
            //        mcVM.ManualConn = mc;
            //        string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_Adv_ManualConnection).Name;
            //        Type type = Type.GetType(modelstr);
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
            //        if (type != null)
            //        {
            //            uc = Activator.CreateInstance(type);
            //            (uc as UserControl).DataContext = mcVM;
            //        }
            //        return uc;
            //    }
            //    else
            //    {
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" };
            //        return uc;
            //    }
            //}
            //else if(value as ViewModelBaseLib.TestStepVM !=null)
            //{
            //    TestStepVM tn = value as TestStepVM;
            //    if (tn.TestStep as PIMTestStep != null)
            //    {
            //        PIMTestStep ps = tn.TestStep as PIMTestStep;
            //        PIMTestStepVM psVM = tn as PIMTestStepVM;
            //        psVM.TestStep = ps;
            //        string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_Adv_PIMTestStep).Name;
            //        Type type = Type.GetType(modelstr);
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
            //        if (type != null)
            //        {
            //            uc = Activator.CreateInstance(type);
            //            (uc as UserControl).DataContext = psVM;
            //        }
            //        return uc;
            //    }
            //    else if(tn.TestStep as NFTestStep !=null)
            //    {
            //        NFTestStep nf = tn.TestStep as NFTestStep;
            //        NFTestStepVM nfVM = tn as NFTestStepVM;
            //        nfVM.TestStep = nf;
            //        string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_Adv_NFTestStep).Name;
            //        Type type = Type.GetType(modelstr);
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
            //        if (type != null)
            //        {
            //            uc = Activator.CreateInstance(type);
            //            (uc as UserControl).DataContext = nfVM;
            //        }
            //        return uc;
            //    }
            //    else if(tn.TestStep as IMDTestStep !=null)
            //    {
            //        IMDTestStep imd = tn.TestStep as IMDTestStep;
            //        IMDTestStepVM imdVM = tn as IMDTestStepVM;
            //        imdVM.TestStep = imd;
            //        string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_Adv_IP3TestStep).Name;
            //        Type type = Type.GetType(modelstr);
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
            //        if (type != null)
            //        {
            //            uc = Activator.CreateInstance(type);
            //            (uc as UserControl).DataContext = imdVM;
            //        }
            //        return uc;
            //    }
            //    else if(tn.TestStep as DCPSTestStep !=null)
            //    {
            //        DCPSTestStep dcps = tn.TestStep as DCPSTestStep;
            //        DCPSTestStepVM dcpsVM = tn as DCPSTestStepVM;
            //        dcpsVM.TestStep = dcps;
            //        string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_Adv_DCPSTestStep).Name;
            //        Type type = Type.GetType(modelstr);
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
            //        if (type != null)
            //        {
            //            uc = Activator.CreateInstance(type);
            //            (uc as UserControl).DataContext = dcpsVM;
            //        }
            //        return uc;
            //    }
            //    else if(tn.TestStep as DUTTestStep !=null)
            //    {
            //        DUTTestStep dut = tn.TestStep as DUTTestStep;
            //        DUTTestStepVM dutVM = tn as DUTTestStepVM;
            //        dutVM.TestStep = dut;
            //        string modelstr=System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_Adv_DUTTestStep).Name;
            //        Type type = Type.GetType(modelstr);
            //        object uc = new Label() {Content="找不到对应类型的模板文件:" + modelstr + "！"};
            //        if(type !=null)
            //        {
            //            uc = Activator.CreateInstance(type);
            //            (uc as UserControl).DataContext = dutVM;
            //        }
            //        return uc;
            //    }
            //    else
            //    {
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" };
            //        return uc;
            //    }
            //}
            
            //else if(value as ViewModelBaseLib.TestTraceVM !=null)
            //{
            //    TestTraceVM tn = value as TestTraceVM;
            //    if(tn.TestTrace as PIMTestTrace !=null)
            //    {
            //        PIMTestTrace tc = tn.TestTrace as PIMTestTrace;
            //        PIMTestTraceVM tcVM = tn as PIMTestTraceVM;
            //        tcVM.TestTrace = tc;
            //        string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_Adv_PIMTestTrace).Name;
            //        Type type = Type.GetType(modelstr);
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
            //        if (type != null)
            //        {
            //            uc = Activator.CreateInstance(type);
            //            (uc as UserControl).DataContext = tcVM;
            //        }
            //        return uc;
            //    }
            //    else if (tn.TestTrace as NFTestTrace != null)
            //    {
            //        NFTestTrace tc = tn.TestTrace as NFTestTrace;
            //        NFTestTraceVM tcVM = tn as NFTestTraceVM;
            //        tcVM.TestTrace = tc;
            //        string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_Adv_NFTestTrace).Name;
            //        Type type = Type.GetType(modelstr);
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
            //        if (type != null)
            //        {
            //            uc = Activator.CreateInstance(type);
            //            (uc as UserControl).DataContext = tcVM;
            //        }
            //        return uc;
            //    }
            //    else if(tn.TestTrace as IMDTestTrace !=null)
            //    {
            //        IMDTestTrace tc = tn.TestTrace as IMDTestTrace;
            //        IMDTestTraceVM imdVM = tn as IMDTestTraceVM;
            //        imdVM.TestTrace = tc;
            //        string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_Adv_IP3TestTrace).Name;
            //        Type type = Type.GetType(modelstr);
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
            //        if (type != null)
            //        {
            //            uc = Activator.CreateInstance(type);
            //            (uc as UserControl).DataContext = imdVM;
            //        }
            //        return uc;
            //    }
            //    else
            //    {
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" };
            //        return uc;
            //    }
            //}
            //else if(value as ViewModelBaseLib.TestMarkerVM !=null)
            //{
            //    TestMarkerVM tn = value as TestMarkerVM;
            //    if(tn.Marker as XYTestMarker !=null)
            //    {
            //        XYTestMarker tm = tn.Marker as XYTestMarker;
            //        XYTestMarkerVM tmVM = tn as XYTestMarkerVM;
            //        tmVM.Marker = tm;
            //        string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_Adv_XYTestMarker).Name;
            //        Type type = Type.GetType(modelstr);
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
            //        if (type != null)
            //        {
            //            uc = Activator.CreateInstance(type);
            //            (uc as UserControl).DataContext = tmVM;
            //        }
            //        return uc;
            //    }
            //    else
            //    {
            //        object uc = new Label() { Content = "找不到对应类型的模板文件:" };
            //        return uc;
            //    }
            //}
            //else
            //{
            //    object uc = new Label() { Content = "找不到对应类型的模板文件:" };
            //    return uc;
            //}
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TreeViewSelectedConverter2 : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
              //if(values[0] as TestPlanVM !=null && values[1] !=null)
              //{
              //    TestPlanVM vm = values[0] as TestPlanVM;
              //    if(values[1] as ManualConnectionVM !=null)
              //    {
              //        ManualConnectionVM mcNode = values[1] as ManualConnectionVM;
              //        if(mcNode.ManualConn as ManualConnection !=null)
              //        {
              //            vm.ManualConnectionGridVmlist.Clear();
              //            ManualConnectionGridVM mcGrid = new ManualConnectionGridVM();
              //            foreach(ManualConnectionVM mc in vm.ManualConnList)
              //            {                            
              //                var item = mc as ManualConnectionVM;
              //                item.ManualConn = mc.ManualConn as ManualConnection;
              //                item.TestPlan = vm.TestPlan;
              //                mcGrid.DisplayName1 = vm.DisplayName;
              //                mcGrid.maualConnVMlist.Add(item);                         
              //            }
              //            if(mcGrid.maualConnVMlist.Count > 0)
              //            {
              //                vm.ManualConnectionGridVmlist.Add(mcGrid);
              //            }
              //            string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_ManualConnection).Name;
              //            Type type = Type.GetType(modelstr);
              //            object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
              //            if (type != null)
              //            {
              //                uc = Activator.CreateInstance(type);
              //                (uc as UserControl).DataContext = vm;
              //            }
              //            return uc;
              //        }
              //        else
              //        {
              //            return null;
              //        }
              //    }
              //    else if (values[1] as TestStepVM != null)
              //    {
              //        TestStepVM stepNode = values[1] as TestStepVM;
              //        if (stepNode.TestStep as PIMTestStep != null)
              //        {
              //            vm.PimTestStepGridVmlist.Clear();
              //            foreach (ManualConnectionVM mc in vm.ManualConnList)
              //            {
              //                PIMTestStepGridVM pimTestStepGrid = new PIMTestStepGridVM();
              //                foreach (TestStepVM step in mc.TestStepList)
              //                {
              //                    if (step.TestStep as PIMTestStep != null)
              //                    {   
              //                        var item = step as PIMTestStepVM;
              //                        item.TestStep = step.TestStep as PIMTestStep;
              //                        pimTestStepGrid.DisplayName = mc.DisplayName;
              //                        pimTestStepGrid.PimTestSteplist.Add(item);                                     
              //                    }
              //                }
              //                if(pimTestStepGrid.PimTestSteplist.Count > 0)
              //                {
              //                    vm.PimTestStepGridVmlist.Add(pimTestStepGrid);
              //                }
              //            }
              //            string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_PimTestStep).Name;
              //            Type type = Type.GetType(modelstr);
              //            object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
              //            if (type != null)
              //            {
              //                uc = Activator.CreateInstance(type);
              //               (uc as UserControl).DataContext = vm;
              //            }
              //            return uc;
              //        }
              //        else if(stepNode.TestStep as NFTestStep !=null)
              //        {
              //            vm.NFTestStepGridVmlist.Clear();
              //            foreach(ManualConnectionVM mc in vm.ManualConnList)
              //            {
              //                NFTestStepGridVM nfTestStepGrid = new NFTestStepGridVM();
              //                foreach(TestStepVM step in mc.TestStepList)
              //                {
              //                     if(step.TestStep as NFTestStep !=null)
              //                     {
              //                         var item = step as NFTestStepVM;
              //                         item.TestStep = step.TestStep as NFTestStep;
              //                         nfTestStepGrid.DisplayName = mc.DisplayName;
              //                         nfTestStepGrid.NFTestSteplist.Add(item);
              //                     }
              //                }
              //                if(nfTestStepGrid.NFTestSteplist.Count > 0)
              //                {
              //                    vm.NFTestStepGridVmlist.Add(nfTestStepGrid);
              //                }
              //            }
              //            string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_NFTestStep).Name;
              //            Type type = Type.GetType(modelstr);
              //            object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
              //            if (type != null)
              //            {
              //                uc = Activator.CreateInstance(type);
              //                (uc as UserControl).DataContext = vm;
              //            }
              //            return uc;
              //        }
              //        else if(stepNode.TestStep as IMDTestStep !=null)
              //        {
              //            vm.IMDTestStepGridVmlist.Clear();
              //            foreach(ManualConnectionVM mc in vm.ManualConnList)
              //            {
              //                IMDTestStepGridVM imdTestStepGrid = new IMDTestStepGridVM();
              //                foreach(TestStepVM step in mc.TestStepList)
              //                {
              //                    if(step.TestStep as IMDTestStep !=null)
              //                    {
              //                        var item = step as IMDTestStepVM;
              //                        item.TestStep = step.TestStep as IMDTestStep;
              //                        imdTestStepGrid.DisplayName = mc.DisplayName;
              //                        imdTestStepGrid.IMDTestSteplist.Add(item);
              //                    }
              //                }
              //                if(imdTestStepGrid.IMDTestSteplist.Count > 0)
              //                {
              //                     vm.IMDTestStepGridVmlist.Add(imdTestStepGrid);
              //                }
              //            }
              //            string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_IMDTestStep).Name;
              //            Type type = Type.GetType(modelstr);
              //            object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
              //            if (type != null)
              //            {
              //                uc = Activator.CreateInstance(type);
              //                (uc as UserControl).DataContext = vm;
              //            }
              //            return uc;
              //        }
              //        else if(stepNode.TestStep as DCPSTestStep !=null)
              //        {
              //            vm.DCPSTestStepGridVmlist.Clear();
              //            foreach(ManualConnectionVM mc in vm.ManualConnList)
              //            {
              //                DCPSTestStepGridVM dcpsTestStepGrid = new DCPSTestStepGridVM();
              //                foreach(TestStepVM step in mc.TestStepList)
              //                {
              //                   if(step.TestStep as DCPSTestStep !=null)
              //                   {
              //                       var item = step as DCPSTestStepVM;
              //                       item.TestStep = step.TestStep as DCPSTestStep;
              //                       dcpsTestStepGrid.DisplayName = mc.DisplayName;
              //                       dcpsTestStepGrid.DCPSTestSteplist.Add(item);
              //                   }
              //                }
              //                if(dcpsTestStepGrid.DCPSTestSteplist.Count > 0)
              //                {
              //                    vm.DCPSTestStepGridVmlist.Add(dcpsTestStepGrid);
              //                }
              //            }
              //            string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_DCPSTestStep).Name;
              //            Type type = Type.GetType(modelstr);
              //            object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
              //            if (type != null)
              //            {
              //                uc = Activator.CreateInstance(type);
              //                (uc as UserControl).DataContext = vm;
              //            }
              //            return uc;
              //        }
              //        else if (stepNode.TestStep as DUTTestStep != null)
              //        {
              //            vm.DUTTestStepGridVmlist.Clear();
              //            foreach (ManualConnectionVM mc in vm.ManualConnList)
              //            {
              //                DUTTestStepGridVM dutTestStepGrid = new DUTTestStepGridVM();
              //                foreach (TestStepVM step in mc.TestStepList)
              //                {
              //                    if (step.TestStep as DUTTestStep != null)
              //                    {
              //                        var item = step as DUTTestStepVM;
              //                        item.TestStep = step.TestStep as DUTTestStep;
              //                        dutTestStepGrid.DisplayName = mc.DisplayName;
              //                        dutTestStepGrid.DUTTestSteplist.Add(item);
              //                    }
              //                }
              //                if (dutTestStepGrid.DUTTestSteplist.Count > 0)
              //                {
              //                    vm.DUTTestStepGridVmlist.Add(dutTestStepGrid);
              //                }
              //            }
              //            string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_DUTTestStep).Name;
              //            Type type = Type.GetType(modelstr);
              //            object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
              //            if (type != null)
              //            {
              //                uc = Activator.CreateInstance(type);
              //                (uc as UserControl).DataContext = vm;
              //            }
              //            return uc;
              //        }
              //        else
              //        {
              //            return null;
              //        }
              //    }
              //    else if (values[1] as TestTraceVM != null)
              //    {
              //        TestTraceVM traceNode = values[1] as TestTraceVM;
              //        if (traceNode.TestTrace as PIMTestTrace != null)
              //        {
              //            vm.PimTestTraceGridVmlist.Clear();
              //            foreach (ManualConnectionVM mc in vm.ManualConnList)
              //            {
              //                foreach (TestStepVM step in mc.TestStepList)
              //                {
              //                    PIMTestTraceGridVM pimTestTraceGrid = new PIMTestTraceGridVM();
              //                    foreach (TestTraceVM trace in step.TestTraceList)
              //                    {
              //                        if (trace.TestTrace as PIMTestTrace != null)
              //                        {
              //                            var item = trace as PIMTestTraceVM;
              //                            item.TestTrace = trace.TestTrace as PIMTestTrace;
              //                            pimTestTraceGrid.DisplayName1 = mc.DisplayName + "-" + step.DisplayName;
              //                            pimTestTraceGrid.PimTestTracelist.Add(item);                                          
              //                        }
              //                    }
              //                   if(pimTestTraceGrid.PimTestTracelist.Count > 0)
              //                   {
              //                       vm.PimTestTraceGridVmlist.Add(pimTestTraceGrid);
              //                   }
              //                }
              //            }
              //            string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_PimTestTrace).Name;
              //            Type type = Type.GetType(modelstr);
              //            object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
              //            if (type != null)
              //            {
              //                uc = Activator.CreateInstance(type);
              //                (uc as UserControl).DataContext = vm;
              //            }
              //            return uc;
              //        }
              //        else if(traceNode.TestTrace as NFTestTrace !=null)
              //        {
              //            vm.NFTestTraceGridVmlist.Clear();
              //            foreach(ManualConnectionVM mc in vm.ManualConnList)
              //            {
              //                foreach(TestStepVM step in mc.TestStepList)
              //                {
              //                    NFTestTraceGridVM nfTestTraceGrid = new NFTestTraceGridVM();
              //                    foreach(TestTraceVM trace in step.TestTraceList)
              //                    {
              //                         if(trace.TestTrace as NFTestTrace !=null)
              //                         {
              //                             var item = trace as NFTestTraceVM;
              //                             item.TestTrace = trace.TestTrace as NFTestTrace;
              //                             nfTestTraceGrid.DisplayName1 = mc.DisplayName + "-" + step.DisplayName;
              //                             nfTestTraceGrid.NFTestTracelist.Add(item);
              //                         }
              //                    }
              //                    if(nfTestTraceGrid.NFTestTracelist.Count > 0)
              //                    {
              //                        vm.NFTestTraceGridVmlist.Add(nfTestTraceGrid);
              //                    }
              //                }
              //            }
              //            string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_NFTestTrace).Name;
              //            Type type = Type.GetType(modelstr);
              //            object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
              //            if (type != null)
              //            {
              //                uc = Activator.CreateInstance(type);
              //                (uc as UserControl).DataContext = vm;
              //            }
              //            return uc;
              //        }
              //        else if(traceNode.TestTrace as IMDTestTrace !=null)
              //        {
              //            vm.IMDTestTraceGridVmlist.Clear();
              //            foreach(ManualConnectionVM mc in vm.ManualConnList)
              //            {
              //                foreach(TestStepVM step in mc.TestStepList)
              //                {
              //                    IMDTestTraceGridVM imdTestTraceGrid = new IMDTestTraceGridVM();
              //                    foreach(TestTraceVM trace in step.TestTraceList)
              //                    {
              //                        if(trace.TestTrace as IMDTestTrace !=null)
              //                        {
              //                            var item = trace as IMDTestTraceVM;
              //                            item.TestTrace = trace.TestTrace as IMDTestTrace;
              //                            imdTestTraceGrid.DisplayName1 = mc.DisplayName + "-" + step.DisplayName;
              //                            imdTestTraceGrid.IMDTestTracelist.Add(item);
              //                        }
              //                    }
              //                    if(imdTestTraceGrid.IMDTestTracelist.Count > 0)
              //                    {
              //                        vm.IMDTestTraceGridVmlist.Add(imdTestTraceGrid);
              //                    }
              //                }
              //            }
              //            string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_IMDTestTrace).Name;
              //            Type type = Type.GetType(modelstr);
              //            object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
              //            if (type != null)
              //            {
              //                uc = Activator.CreateInstance(type);
              //                (uc as UserControl).DataContext = vm;
              //            }
              //            return uc;
              //        }
              //        else
              //        {
              //            return null;
              //        }
              //    }
              //    else if (values[1] as TestMarkerVM != null)
              //    {
              //        TestMarkerVM markerNode = values[1] as TestMarkerVM;
              //        if (markerNode.Marker as XYTestMarker != null)
              //        {
              //            vm.XYTestMarkerGridVmlist.Clear();
              //            foreach (ManualConnectionVM mc in vm.ManualConnList)
              //            {
              //                foreach (TestStepVM step in mc.TestStepList)
              //                {
              //                    foreach (TestTraceVM trace in step.TestTraceList)
              //                    {
              //                        XYTestMarkerGridVM xyTestMarkerGridVM = new XYTestMarkerGridVM();
              //                        foreach (TestMarkerVM marker in trace.MarkerList)
              //                        {
              //                            if (marker.Marker as XYTestMarker != null)
              //                            {
              //                                var item = marker as XYTestMarkerVM;
              //                                item.Marker = marker.Marker as XYTestMarker;
              //                                xyTestMarkerGridVM.DisplayName1 = mc.DisplayName + "-" + step.DisplayName + "-" + trace.DisplayName;
              //                                xyTestMarkerGridVM.XYTestMarkerlist.Add(item);                                             
              //                            }
              //                        }
              //                        if(xyTestMarkerGridVM.XYTestMarkerlist.Count > 0)
              //                        {
              //                            vm.XYTestMarkerGridVmlist.Add(xyTestMarkerGridVM);
              //                        }
              //                    }
              //                }
              //            }
              //            string modelstr = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + typeof(UC_XYTestMarker).Name;
              //            Type type = Type.GetType(modelstr);
              //            object uc = new Label() { Content = "找不到对应类型的模板文件:" + modelstr + "！" };
              //            if (type != null)
              //            {
              //                uc = Activator.CreateInstance(type);
              //                (uc as UserControl).DataContext = vm;
              //            }
              //            return uc;
              //        }
              //        else
              //        {
              //            return null;
              //        }
              //    }
              //    else
              //    {
              //        return null;
              //    }
              //}
              //else
              //{
              //    return null;
              //}
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //public class ListViewItemSourceConverter : MarkupExtension, IMultiValueConverter
    //{
    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        return this;
    //    }
    //    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        if (values[0] as ObservableCollection<ObservableCollection<XYMarkerDisp>> != null)
    //        {
    //             ObservableCollection<ObservableCollection<XYMarkerDisp>> XYMarkerDispList=values[0] as ObservableCollection<ObservableCollection<XYMarkerDisp>>;
    //             int index=System.Convert.ToInt32(values[1]);
    //             if(index==-1)
    //             {
    //                 index = 0;
    //             }
    //             return XYMarkerDispList[index];
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    public class  PassFailConverter: MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = System.Convert.ToString(value);
            if(str=="True")
            {
                return new SolidColorBrush(Colors.Green);
            }
            else if (str == "False")
            {
                return new SolidColorBrush(Colors.Red);
            }
            else
            {
                return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TestResultConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {   
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {   
            string str=value.ToString();
            double ? values = str.ToNullDouble();
            if (values != null)
            {   
                double j=(double)values;
                int i = GeneTestSetup.Instance.DataDisplayDigits;        
                return j.ToDigits(i);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TestResultConverter2 : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                bool v = (bool)value;
                return v ? "PASS" : "FAIL";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TreeViewHeaderConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string str = value.ToString();
                return str;
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class RowHandleConverter : MarkupExtension, IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
            {
                if (System.Convert.ToInt32(value) == -1 || System.Convert.ToInt32(value) == -2 || System.Convert.ToInt32(value) == -3)
                {
                    return null;
                }
                else
                {
                    int i = System.Convert.ToInt32(value);
                    return i + 1;
                }
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
    public class ManualConnColorConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return new SolidColorBrush(Color.FromRgb(230,231,232));
            }
            else
            {
                bool passFail = (bool)value;
                if (passFail)
                {
                    return new SolidColorBrush(Color.FromRgb(21, 194, 60));
                }
                else
                {
                    return new SolidColorBrush(Colors.Red);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ManualConnColorConverter2 : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
               // return new SolidColorBrush(Color.FromRgb(230, 231, 232));
                return new SolidColorBrush(Colors.LightYellow);
            }
            else
            {
                bool passFail = (bool)value;
                if (passFail)
                {
                    return new SolidColorBrush(Color.FromRgb(21, 194, 60));
                }
                else
                {
                    return new SolidColorBrush(Colors.Red);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class CheckBoxIsVisibleConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((new ViewModelLocator()).MainWindow.StatusInfo.IsAdmin == true)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ManualConnForColorConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return new SolidColorBrush(Colors.Black);
            }
            else
            {
                 return new SolidColorBrush(Colors.White);
                //bool passFail = (bool)value;
                //if (passFail)
                //{
                //    
                //}
                //else
                //{
                //    return new SolidColorBrush(Colors);
                //}
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class LabelPassFailConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                bool passFail = (bool)value;
                if (passFail)
                {
                    return "Pass";
                }
                else
                {
                    return "Fail";
                }
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class RangeValueVisibleConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class InstruConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
             if (value as TestStepInfo != null)
             {
                 var testStep = value as TestStepInfo;
                 return testStep.MeasClsInfoList;
             }
             else
             {
                 return null;
             }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TreelistControlCheckBoxVisible : MarkupExtension, IValueConverter
    {

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = value.ToString();
            if (string.IsNullOrWhiteSpace(str))
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TreelistControlFreqConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string[] strs = value.ToString().Split('-');
            string str1="";
            string str2="";
            if(strs.Count()==1)
            {
                str1 = System.Convert.ToString((new FreqStringConverter()).Convert(strs[0], null, null, null));
                return str1;
            }
            else if(strs.Count()==2)
            {
                if (!string.IsNullOrWhiteSpace(strs[0]) || !string.IsNullOrWhiteSpace(strs[1]))
                {
                    if (!string.IsNullOrWhiteSpace(strs[0]))
                    {
                        str1 = System.Convert.ToString((new FreqStringConverter()).Convert(strs[0], null, null, null));
                    }
                    if (!string.IsNullOrWhiteSpace(strs[1]))
                    {
                        str2 = System.Convert.ToString((new FreqStringConverter()).Convert(strs[1], null, null, null));
                    }
                    return str1 + "-" + str2;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //public class DocumentgroupSelectedConverter : MarkupExtension, IMultiValueConverter
    //{
    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        return this;
    //    }

    //    //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    //{
    //    //    DocumentPanel panel=value as DocumentPanel;
    //    //    if (panel != null)
    //    //    {
    //    //        string str = panel.Caption.ToString();
    //    //        if (str == "NFA Cal")
    //    //        {
    //    //        }
    //    //        if (str == "IP3 Cal")
    //    //        {
    //    //        }
    //    //    }
    //    //    else
    //    //    {
    //    //        return null;
    //    //    }

    //    //}

    //    //public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}

    //    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        if (values[0] != null && values[1] != null)
    //        {
    //            MainCalVM vm = values[0] as MainCalVM;
    //            DocumentPanel panel = values[1] as DocumentPanel;
    //            if (panel.Caption.ToString() == "NFA Cal")
    //            {
    //                return vm.NFTestStepCal.ManualConnList;                
    //            }
    //            if (panel.Caption.ToString() == "IP3 Cal")
    //            {
    //                return vm.IMDTestStepCal.ManualConnList;
    //            }
    //            else
    //            {
    //                return null;
    //            }
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    public class ShowExportInfo : MarkupExtension, IValueConverter
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
                result = "正在加载状态文件...";
            }
            else
            {
                result = "加载完成！";
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ComboBoxEditEnabledConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class CalibrationContentConverter : MarkupExtension, IValueConverter
    {

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int index = System.Convert.ToInt32(value);
            if (TestStepFactory.CalibUClist.Count > 0)
            {
                return TestStepFactory.CalibUClist[index];
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class NullValueConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }
            else if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return null;
            }
            else
            {
                return value;
            }
        }
    }
    public class TestConverter : MarkupExtension, IValueConverter
    {

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var item = value;
            return item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
