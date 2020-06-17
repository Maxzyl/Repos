using DevExpress.Xpf.Editors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace DataUtils
{
    public class CommUtils
    {
        public static Window GetWindowFromUrl(string url)
        {
            string uri = "";
            string param = "";
            if (GetAssemblyInstanse(url, ref uri, ref param) == null) return null;
            Window window = (System.Windows.Window)System.Windows.Application.LoadComponent(new System.Uri(uri, System.UriKind.Relative));
            window.Tag = param;
            return window;
        }

        public static UserControl GetUserControlFromUrl(string url)
        {
            string uri = "";
            string param = "";
            if (GetAssemblyInstanse(url, ref uri, ref param) == null) return null;
            UserControl UC = (UserControl)System.Windows.Application.LoadComponent(new System.Uri(uri, System.UriKind.Relative));
            UC.Tag = param;
            return UC;
        }

        public static ImageSource GetImageFromUrl(string url)
        {
            string uri = "";
            string param = "";
            if (GetAssemblyInstanse(url, ref uri, ref param) == null) return null;
            ImageSource UC = new BitmapImage(new Uri("pack://application:,,," + uri, UriKind.Absolute));
            return UC;
        }

        //Dim appd As AppDomain
        //appd = AppDomain.CreateDomain("RDLC TEST")
        //Dim drp As ReportInterface.DynamicReport
        //drp = appd.CreateInstanceFromAndUnwrap("ReportInterface.Dll", "ReportInterface.DynamicReport")
        //drp.ShowReport(ReportPackage, customerLg, companyLg, reportFormat, FileName)
        //drp.Dispose()
        //drp = Nothing
        //AppDomain.Unload(appd)

        public static object GetPropertyFromUrl(string url)
        {
            string uri = "";
            string param = "";
            Assembly myasmby = GetAssemblyInstanse(url, ref uri, ref param);
            if (myasmby == null) return null;
            string[] uris = uri.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (uris.Count() < 2) return null;
            string[] uriss = uris[1].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (uriss.Count() < 2) return null;
            string fileName = (uriss[0]);
            object result = null;
            try
            {
                object obj = myasmby.CreateInstance(fileName);
                result = obj.GetType().GetProperty(uriss[1]).GetValue(obj, null);
            }
            catch { }
            return result;
        }

        public static object ExcuteMethodByUrl(string url, params object[] args)
        {
            try
            {
                object result = null;
                object objinstance = null;
                MethodInfo mi = GetMethodFromUrl(url, ref objinstance);
                result = mi.Invoke(objinstance, args);
                return result;
            }
            catch (Exception ex)
            { return ex.Message; }
        }

        public static MethodInfo GetMethodFromUrl(string url, ref object outobj)
        {
            //System.Type[] paramTypes = new System.Type[2]; 
            //paramTypes[0] = System.Type.GetType("System.String");
            //paramTypes[1] = System.Type.GetType("System.String");
            //System.Reflection.MethodInfo mi = t.GetMethod("aa", paramTypes);  
            MethodInfo result = null;
            string uri = "";
            string param = "";
            Assembly myasmby = GetAssemblyInstanse(url, ref uri, ref param);
            if (myasmby == null) return null;
            string[] uris = uri.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (uris.Count() < 2) return null;
            string[] uriss = uris[1].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (uriss.Count() < 2) return null;
            string fileName = (uriss[0]);
            try
            {
                object obj = myasmby.CreateInstance(fileName);
                result = obj.GetType().GetMethod(uriss[1]);
                outobj = obj;
            }
            catch { }
            return result;
        }

        public static Assembly GetAssemblyInstanse(string url, ref string uri, ref string param)
        {
            string[] urls = url.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (urls.Count() == 0) return null;
            System.Reflection.Assembly MyAssembly = System.Reflection.Assembly.LoadFrom(urls[0]);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(urls[0]);
            if (urls.Count() > 1)
            {
                urls[1] = "/" + fileName + ";" + urls[1];
                int index = urls[1].IndexOf('?');
                uri = index > -1 ? urls[1].Substring(0, index) : urls[1];
                param = index > -1 ? urls[1].Substring(index + 1) : "";
            }
            return MyAssembly;
        }
        //序列化
        public static string SerializeData(Type type, object obj, string filename)
        {
            string result = null;
            using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                serializer.Serialize(stream, obj);
            }
            result = "OK";
            return result;
        }
        //反序列化
        public static Object DeserializerData(Type type, string path)
        {
            if (File.Exists(path))
            {
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer xmlserializer = new XmlSerializer(type);
                    Object obj = (Object)xmlserializer.Deserialize(file);
                    return obj;
                }

            }
            else
            {
                return null;
            }
        }
        sealed class PremergeToMergedDeserializationBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                Type typeToDeserialize = null;
                String exeAssembly = Assembly.GetExecutingAssembly().FullName;

                typeToDeserialize = Type.GetType(string.Format("{0}", "{1}", typeName, exeAssembly));
                return typeToDeserialize;
            }

        }
        public static List<T> GetChildObjects<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child));
            }
            return childList;
        }
        public static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        public static TreeViewItem GetParentObjectEx<TreeViewItem>(DependencyObject obj) where TreeViewItem : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is TreeViewItem)
                {
                    return (TreeViewItem)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
    }
}
