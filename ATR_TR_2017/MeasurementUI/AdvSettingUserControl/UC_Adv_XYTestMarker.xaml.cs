using DataUtils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModelBaseLib;

namespace MeasurementUI
{
    /// <summary>
    /// Interaction logic for UC_Adv_XYTestMarker.xaml
    /// </summary>
    public partial class UC_Adv_XYTestMarker : UserControl
    {
        private StringBuilder codeCopy = null;
        private StringBuilder sb = null;
        private StringBuilder tempSb = null;
        public List<Params> paramsList = null;
        private string S_FuncName_ID = "";
        public string expression { get; set; }


        string filterstr;
        string[] filters = new string[3];
        string id;
        string tag;
        TestPlanVM vm = null;

        public UC_Adv_XYTestMarker()
        {
            InitializeComponent();
          
            //vm.t

          

            //string s = vm.DisplayName;
            //treeView.DataContext = vm;
            //vm.AddTestSpecs("生产指标");
            //ss();
            
            //treeView.ItemsSource = vm.;
          
            //LoadFuncName();
            ////codeCopy = new StringBuilder();
            ////sb = new StringBuilder();
            ////tempSb = new StringBuilder();
            ////BindFuncList();
            //BindParamList();
            //Test();
            ////codeCopy.Append(WrapExpression());
            //foreach (var item in paramsList)
            //{
            //    string str = @"public Complex " + item.param.ToUpper() + ";\r\n";
            //    sb.Append(str);
            //}
            //codeCopy = codeCopy.Replace("{0}", sb.ToString());

            //if (sb==null)
            //{
            //    codeCopy = codeCopy.Replace("{0}", "");
            //}
            ////codeCopy = codeCopy.Replace("{2}", LoadFunc());

        }

      

        private void Test()
        {
            paramsList = new List<Params>();
            Params p = new Params();
            p.param = "a";
            p.value = -18.0;
            paramsList.Add(p);

            double[] bb = { -1, 2, -3, 4 };
            p.param = "b";
            p.value = bb;
            paramsList.Add(p);

            double[] cc = { 2, 8 };
            p.param = "c";
            p.value = cc;
            paramsList.Add(p);

            double dd = 8;
            p.param = "d";
            p.value = dd;
            paramsList.Add(p);

            double[] ee = { 1, 2, 3, 4 };
            p.param = "e";
            p.value = ee;
            paramsList.Add(p);
        }

        /// <summary>
        /// cs文件
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private string WrapExpression(string expression = null)
        {
            string code = @"
                using System;
                using System.Linq;
                class ExpressionCalculate
                {
                    
                    {0}
                    
                    public object Calculate()
                    {
                        object result=null;
                        object obj= {1};
                        System.Type t = obj.GetType();
                        if(t.Name.Equals(""Complex""))
                        {
                           Complex cc = (Complex)obj;
                           result=cc.Value;
                        }
                        else
                        {
                           result=obj;
                        }
                        return result;
                    }

                    {2} 

                }

                public struct Complex
                {
                    public object Value { get; set; }

                    public Complex(object Value)
                        : this()
                    {
                        this.Value = Value;
                    }

                    public static Complex operator +(Complex c1, object obj)
                    {
                         
                        System.Type type = c1.Value.GetType();
                        System.Type t = obj.GetType();
                        double num = 0.0;
                        object result = null;
                        if (t.Name.Equals(""Int32"") || t.Name.Equals(""Double""))
                        {
                            num = Convert.ToDouble(obj);
                        }
                        if (type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]""))
                        {
                            bool temp = type.Name == ""Int32[]"" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x+num).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x + num).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(c1.Value) + num;
                            result = temp;
                        }
                        return new Complex
                        {
                            Value = result
                        };
                    }

                    public static Complex operator +(object obj, Complex c2)
                    {
                        System.Type type = c2.Value.GetType();
                        System.Type t = obj.GetType();
                        double num = 0.0;
                        object result = null;
                        if (t.Name.Equals(""Int32"") || t.Name.Equals(""Double""))
                        {
                            num = Convert.ToDouble(obj);
                        }
                        if (type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]""))
                        {
                            bool temp = type.Name == ""Int32[]"" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])c2.Value;
                                result = aa.Zip(aa, (x, y) => x + num).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])c2.Value;
                                result = aa.Zip(aa, (x, y) => x + num).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(c2.Value) + num;
                            result = temp;
                        }
                        return new Complex
                        {
                            Value=result
                        };
                    }

                    public static Complex operator +(Complex c1, Complex c2)
                    {
                        System.Type type = c1.Value.GetType();
                        System.Type t = c2.Value.GetType();
                        object result = null;
                        if ((type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]"")) && (t.Name.Equals(""Int32[]"") || t.Name.Equals(""Double[]"")))
                        {
                            bool temp = type.Name == ""Int32[]"" ? true : false;
                            bool flag = t.Name == ""Int32[]"" ? true : false;
                            if (temp && flag)
                            {
                                int[] aa = (int[])c1.Value;
                                int[] bb = (int[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                int[] tempArray = aa.Zip(bb, (x, y) => x + y).ToArray();
                                result = Array.ConvertAll<int, double>(tempArray, s => Convert.ToDouble(s));
                            }
                            else if (temp && !flag)
                            {
                                int[] aa = (int[])c1.Value;
                                double[] bb = (double[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                result = aa.Zip(bb, (x, y) => x + y).ToArray();
                            }
                            else if (!temp && flag)
                            {
                                double[] aa = (double[])c1.Value;
                                int[] bb = (int[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                result = aa.Zip(bb, (x, y) => x + y).ToArray();
                            }
                            else if (!temp && !flag)
                            {
                                double[] aa = (double[])c1.Value;
                                double[] bb = (double[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                result = aa.Zip(bb, (x, y) => x + y).ToArray();
                            }
                        }
                        else if ((type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]"")))
                        {
                            bool tempa = type.Name == ""Int32[]"" ? true : false;
                            double num = Convert.ToDouble(c2.Value);
                            if (tempa)
                            {
                                int[] aa = (int[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x + num).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x * num).ToArray();
                            }
                        }
                        else if ((t.Name.Equals(""Int32[]"") || t.Name.Equals(""Double[]"")))
                        {
                            bool tempb = t.Name == ""Int32[]"" ? true : false;
                            double num = Convert.ToDouble(c1.Value);
                            if (tempb)
                            {
                                int[] bb = (int[])c2.Value;
                                result = bb.Zip(bb, (x, y) => x + num).ToArray();
                            }
                            else
                            {
                                double[] bb = (double[])c2.Value;
                                result = bb.Zip(bb, (x, y) => x + num).ToArray();
                            }
                        }
                        else
                        {
                            result = Convert.ToDouble(c1.Value) + Convert.ToDouble(c2.Value);
                        }
                        return new Complex
                        {
                            Value=result
                        };
                    }

                    public static Complex operator -(Complex c1, object obj)
                    {
                        System.Type type = c1.Value.GetType();
                        System.Type t = obj.GetType();
                        double num = 0.0;
                        object result = null;
                        if (t.Name.Equals(""Int32"") || t.Name.Equals(""Double""))
                        {
                            num = Convert.ToDouble(obj);
                        }
                        if (type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]""))
                        {
                            bool temp = type.Name == ""Int32[]"" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x - num).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x - num).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(c1.Value) - num;
                            result = temp;
                        }
                        return new Complex
                        {
                            Value = result
                        };
                    }

                    public static Complex operator -(object obj, Complex c2)
                    {
                        System.Type type = c2.Value.GetType();
                        System.Type t = obj.GetType();
                        double num = 0.0;
                        object result = null;
                        if (t.Name.Equals(""Int32"") || t.Name.Equals(""Double""))
                        {
                            num = Convert.ToDouble(obj);
                        }
                        if (type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]""))
                        {
                            bool temp = type.Name == ""Int32[]"" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])c2.Value;
                                result = aa.Zip(aa, (x, y) => x - num).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])c2.Value;
                                result = aa.Zip(aa, (x, y) => x - num).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(c2.Value) - num;
                            result = temp;
                        }
                        return new Complex
                        {
                            Value = result
                        };
                    }

                    public static Complex operator -(Complex c1, Complex c2)
                    {
                        System.Type type = c1.Value.GetType();
                        System.Type t = c2.Value.GetType();
                        object result = null;
                        if ((type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]"")) && (t.Name.Equals(""Int32[]"") || t.Name.Equals(""Double[]"")))
                        {
                            bool temp = type.Name == ""Int32[]"" ? true : false;
                            bool flag = t.Name == ""Int32[]"" ? true : false;
                            if (temp && flag)
                            {
                                int[] aa = (int[])c1.Value;
                                int[] bb = (int[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                int[] tempArray = aa.Zip(bb, (x, y) => x - y).ToArray();
                                result = Array.ConvertAll<int, double>(tempArray, s => Convert.ToDouble(s));
                            }
                            else if (temp && !flag)
                            {
                                int[] aa = (int[])c1.Value;
                                double[] bb = (double[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                result = aa.Zip(bb, (x, y) => x - y).ToArray();
                            }
                            else if (!temp && flag)
                            {
                                double[] aa = (double[])c1.Value;
                                int[] bb = (int[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                result = aa.Zip(bb, (x, y) => x - y).ToArray();
                            }
                            else if (!temp && !flag)
                            {
                                double[] aa = (double[])c1.Value;
                                double[] bb = (double[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                result = aa.Zip(bb, (x, y) => x - y).ToArray();
                            }
                        }
                        else if ((type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]"")))
                        {
                            bool tempa = type.Name == ""Int32[]"" ? true : false;
                            double num = Convert.ToDouble(c2.Value);
                            if (tempa)
                            {
                                int[] aa = (int[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x - num).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x - num).ToArray();
                            }
                        }
                        else if ((t.Name.Equals(""Int32[]"") || t.Name.Equals(""Double[]"")))
                        {
                            bool tempb = t.Name == ""Int32[]"" ? true : false;
                            double num = Convert.ToDouble(c1.Value);
                            if (tempb)
                            {
                                int[] bb = (int[])c2.Value;
                                result = bb.Zip(bb, (x, y) => x - num).ToArray();
                            }
                            else
                            {
                                double[] bb = (double[])c2.Value;
                                result = bb.Zip(bb, (x, y) => x - num).ToArray();
                            }
                        }
                        else
                        {
                            result = Convert.ToDouble(c1.Value) - Convert.ToDouble(c2.Value);
                        }
                        return new Complex
                        {
                            Value = result
                        };
                    }

                    public static Complex operator *(Complex c1, object obj)
                    {
                        System.Type type = c1.Value.GetType();
                        System.Type t = obj.GetType();
                        double num = 0.0;
                        object result = null;
                        if (t.Name.Equals(""Int32"") || t.Name.Equals(""Double""))
                        {
                            num = Convert.ToDouble(obj);
                        }
                        if (type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]""))
                        {
                            bool temp = type.Name == ""Int32[]"" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x * num).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x * num).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(c1.Value) * num;
                            result = temp;
                        }
                        return new Complex
                        {
                            Value = result
                        };
                    }

                    public static Complex operator *(object obj, Complex c2)
                    {
                        System.Type type = c2.Value.GetType();
                        System.Type t = obj.GetType();
                        double num = 0.0;
                        object result = null;
                        if (t.Name.Equals(""Int32"") || t.Name.Equals(""Double""))
                        {
                            num = Convert.ToDouble(obj);
                        }
                        if (type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]""))
                        {
                            bool temp = type.Name == ""Int32[]"" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])c2.Value;
                                result = aa.Zip(aa, (x, y) => x * num).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])c2.Value;
                                result = aa.Zip(aa, (x, y) => x * num).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(c2.Value) * num;
                            result = temp;
                        }
                        return new Complex
                        {
                            Value = result
                        };
                    }

                    public static Complex operator *(Complex c1, Complex c2)
                    {
                        System.Type type = c1.Value.GetType();
                        System.Type t = c2.Value.GetType();
                        object result = null;
                        if ((type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]"")) && (t.Name.Equals(""Int32[]"") || t.Name.Equals(""Double[]"")))
                        {
                            bool temp = type.Name == ""Int32[]"" ? true : false;
                            bool flag = t.Name == ""Int32[]"" ? true : false;
                            if (temp && flag)
                            {
                                int[] aa = (int[])c1.Value;
                                int[] bb = (int[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                int[] tempArray = aa.Zip(bb, (x, y) => x * y).ToArray();
                                result = Array.ConvertAll<int, double>(tempArray, s => Convert.ToDouble(s));
                            }
                            else if (temp && !flag)
                            {
                                int[] aa = (int[])c1.Value;
                                double[] bb = (double[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                result = aa.Zip(bb, (x, y) => x * y).ToArray();
                            }
                            else if (!temp && flag)
                            {
                                double[] aa = (double[])c1.Value;
                                int[] bb = (int[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                result = aa.Zip(bb, (x, y) => x * y).ToArray();
                            }
                            else if (!temp && !flag)
                            {
                                double[] aa = (double[])c1.Value;
                                double[] bb = (double[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                result = aa.Zip(bb, (x, y) => x * y).ToArray();
                            }
                        }
                        else if ((type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]"")))
                        {
                            bool tempa = type.Name == ""Int32[]"" ? true : false;
                            double num = Convert.ToDouble(c2.Value);
                            if (tempa)
                            {
                                int[] aa = (int[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x * num).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x * num).ToArray();
                            }
                        }
                        else if ((t.Name.Equals(""Int32[]"") || t.Name.Equals(""Double[]"")))
                        {
                            bool tempb = t.Name == ""Int32[]"" ? true : false;
                            double num = Convert.ToDouble(c1.Value);
                            if (tempb)
                            {
                                int[] bb = (int[])c2.Value;
                                result = bb.Zip(bb, (x, y) => x * num).ToArray();
                            }
                            else
                            {
                                double[] bb = (double[])c2.Value;
                                result = bb.Zip(bb, (x, y) => x * num).ToArray();
                            }
                        }
                        else
                        {
                            result = Convert.ToDouble(c1.Value) * Convert.ToDouble(c2.Value);
                        }
                        return new Complex
                        {
                            Value = result
                        };
                    }

                    public static Complex operator /(Complex c1, object obj)
                    {
                        System.Type type = c1.Value.GetType();
                        System.Type t = obj.GetType();
                        double num = 0.0;
                        object result = null;
                        if (t.Name.Equals(""Int32"") || t.Name.Equals(""Double""))
                        {
                            num = Convert.ToDouble(obj);
                        }
                        if (type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]""))
                        {
                            bool temp = type.Name == ""Int32[]"" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x / num).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x / num).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(c1.Value) / num;
                            result = temp;
                        }
                        return new Complex
                        {
                            Value = result
                        };
                    }

                    public static Complex operator /(object obj, Complex c2)
                    {
                        System.Type type = c2.Value.GetType();
                        System.Type t = obj.GetType();
                        double num = 0.0;
                        object result = null;
                        if (t.Name.Equals(""Int32"") || t.Name.Equals(""Double""))
                        {
                            num = Convert.ToDouble(obj);
                        }
                        if (type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]""))
                        {
                            bool temp = type.Name == ""Int32[]"" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])c2.Value;
                                result = aa.Zip(aa, (x, y) => x / num).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])c2.Value;
                                result = aa.Zip(aa, (x, y) => x / num).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(c2.Value) / num;
                            result = temp;
                        }
                        return new Complex
                        {
                            Value = result
                        };
                    }

                    public static Complex operator /(Complex c1, Complex c2)
                    {
                        System.Type type = c1.Value.GetType();
                        System.Type t = c2.Value.GetType();
                        object result = null;
                        if ((type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]"")) && (t.Name.Equals(""Int32[]"") || t.Name.Equals(""Double[]"")))
                        {
                            bool temp = type.Name == ""Int32[]"" ? true : false;
                            bool flag = t.Name == ""Int32[]"" ? true : false;
                            if (temp && flag)
                            {
                                int[] aa = (int[])c1.Value;
                                int[] bb = (int[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                int[] tempArray = aa.Zip(bb, (x, y) => x / y).ToArray();
                                result = Array.ConvertAll<int, double>(tempArray, s => Convert.ToDouble(s));
                            }
                            else if (temp && !flag)
                            {
                                int[] aa = (int[])c1.Value;
                                double[] bb = (double[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                result = aa.Zip(bb, (x, y) => x / y).ToArray();
                            }
                            else if (!temp && flag)
                            {
                                double[] aa = (double[])c1.Value;
                                int[] bb = (int[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                result = aa.Zip(bb, (x, y) => x / y).ToArray();
                            }
                            else if (!temp && !flag)
                            {
                                double[] aa = (double[])c1.Value;
                                double[] bb = (double[])c2.Value;
                                if (aa.Length != bb.Length)
                                {
                                    throw new Exception(""两数组长度不等,不能进行运算!"");
                                }
                                result = aa.Zip(bb, (x, y) => x / y).ToArray();
                            }
                        }
                        else if ((type.Name.Equals(""Int32[]"") || type.Name.Equals(""Double[]"")))
                        {
                            bool tempa = type.Name == ""Int32[]"" ? true : false;
                            double num = Convert.ToDouble(c2.Value);
                            if (tempa)
                            {
                                int[] aa = (int[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x / num).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])c1.Value;
                                result = aa.Zip(aa, (x, y) => x / num).ToArray();
                            }
                        }
                        else if ((t.Name.Equals(""Int32[]"") || t.Name.Equals(""Double[]"")))
                        {
                            bool tempb = t.Name == ""Int32[]"" ? true : false;
                            double num = Convert.ToDouble(c1.Value);
                            if (tempb)
                            {
                                int[] bb = (int[])c2.Value;
                                result = bb.Zip(bb, (x, y) => x / num).ToArray();
                            }
                            else
                            {
                                double[] bb = (double[])c2.Value;
                                result = bb.Zip(bb, (x, y) => x / num).ToArray();
                            }
                        }
                        else
                        {
                            result = Convert.ToDouble(c1.Value) / Convert.ToDouble(c2.Value);
                        }
                        return new Complex
                        {
                            Value = result
                        };
                    }

                }
            ";
            if (string.IsNullOrEmpty(expression))
            {
                return code;
            }
            return code.Replace("{0}", expression);
        }

        #region 目前没用
        /// <summary>
        /// 加载函数内容
        /// </summary>
        /// <returns></returns>
        private string LoadFunc()
        {
            DataSet ds = DataUtils.DB.GetDataSetFromSQL("select ID, FuncName 函数,Remarks 备注,LASTMODIFYUSER 登录人员,LASTMODIFYTIME 修改时间 from S_FuncName");
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string id = Convert.ToString(dr["ID"]);
                DataSet dSet = DataUtils.DB.GetDataSetFromSQL(string.Format("select FileStream from S_FuncName_Content where S_FuncName_ID='{0}'", id));
                foreach (DataRow row in dSet.Tables[0].Rows)
                {
                    if (Convert.IsDBNull(row["FileStream"]))
                    {
                        continue;
                    }
                    else
                    {
                        byte[] heByte = (byte[])row["FileStream"];
                        string strContent = System.Text.Encoding.UTF8.GetString(heByte);
                        sb.Append(strContent + "\r\n");
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        public void BindParamList(List<Params> paramList)
        {
            paramsList = paramList;
            if (paramsList == null)
            {
                codeCopy = codeCopy.Replace("{0}", "");
                return;
            }

            if (paramsList != null)
            {
                foreach (var item in paramsList)
                {
                    string str = @"public Complex " + item.param.ToUpper() + ";\r\n";
                    sb.Append(str);
                }
                codeCopy = codeCopy.Replace("{0}", sb.ToString());
            }

            ParamList parentFunclist = new ParamList() { paramID = "1", paramName = "变量", parent = null };
            ObservableCollection<ParamList> func = new ObservableCollection<ParamList>();
            foreach (Params dr in paramsList)
            {
                ParamList funclist = new ParamList();
                funclist.paramID = dr.param;
                funclist.paramName = dr.param;
                funclist.parent = parentFunclist;
                parentFunclist.Children.Add(funclist);
            }
            func.Add(parentFunclist);
            //tvParam.ItemsSource = func;
        }
        #endregion

        /// <summary>
        /// 绑定函数
        /// </summary>
        private void BindFuncList()
        {
            DataSet ds = DataUtils.DB.GetDataSetFromSQL("select A.ID, A.FuncName 函数,FileStream from S_FuncName A left join S_FuncName_Content B on A.ID=B.S_FuncName_ID");
            if (ds != null && ds.Tables[0] != null)
            {
                FuncList parentFunclist = new FuncList() { funcID = "0", funcName = "函数列表", parent = null };
                ObservableCollection<FuncList> func = new ObservableCollection<FuncList>();
                string strNewValue = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    FuncList funclist = new FuncList();
                    funclist.funcID = Convert.ToString(dr["ID"]);
                    funclist.funcName = Convert.ToString(dr["函数"]);
                    funclist.name = strNewValue;
                    funclist.parent = parentFunclist;
                    parentFunclist.Children.Add(funclist);
                }
                func.Add(parentFunclist);
                tvFuncList.ItemsSource = func;
            }
        }
       
        private string TempClass(string temp)
        {
            string code = @"
                using System;
                using System.Linq;
                class TempCalculate
                {
                    {0}                   
                }
                public struct Complex
                {
                    public object Value { get; set; }

                    public Complex(object Value)
                        : this()
                    {
                        this.Value = Value;
                    } 
                }  
            ";
            return code.Replace("{0}", temp);
        }

        /// <summary>
        /// 编译根据函数名获取参数
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="funcName">函数名称</param>
        /// <returns></returns>
        private string ComplierCode(string expression, string funcName, Boolean flag = false)
        {
            string result = "";
            string param = "";
            try
            {
                string code = TempClass(expression);
                CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider();
                CompilerParameters compilerParameters = new CompilerParameters();

                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Xaml.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");

                compilerParameters.CompilerOptions = "/t:library";
                compilerParameters.GenerateInMemory = true;
                CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, code);
                if (compilerResults.Errors.Count == 0)
                {
                    Assembly assembly = compilerResults.CompiledAssembly;
                    Type type = assembly.GetType("TempCalculate");
                    MethodInfo[] method = type.GetMethods();
                    foreach (var item in method)
                    {
                        if (item.Name.ToUpper().Equals(funcName.ToUpper()))
                        {
                            int i = 1;
                            ParameterInfo[] parameterInfo = item.GetParameters();
                            if (flag)
                            {
                                foreach (var paramInfo in parameterInfo)
                                {
                                    if (i == parameterInfo.Length)
                                    {
                                        param += paramInfo.ParameterType;
                                    }
                                    else
                                    {
                                        param += paramInfo.ParameterType + ",";
                                    }
                                    i++;
                                }
                            }
                            else
                            {
                                foreach (var paramInfo in parameterInfo)
                                {
                                    if (i == parameterInfo.Length)
                                    {
                                        param += " ";
                                    }
                                    else
                                    {
                                        param += " " + ",";
                                    }
                                    i++;
                                }
                            }
                            result = item.Name + "(" + param + ")";
                            break;
                        }
                    }
                    return result;
                }
                else if (compilerResults.Errors.Count > 0)
                {
                    throw new Exception(compilerResults.Errors[0].ErrorText);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 编译返回结果
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>string expression
        public object ComplierCodeResult()
        {
            try
            {
                if (string.IsNullOrEmpty(expression)) { return null; }
                string code = codeCopy.ToString().Replace("{1}", expression);
                CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider();
                CompilerParameters compilerParameters = new CompilerParameters();

                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Xaml.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");

                compilerParameters.CompilerOptions = "/t:library";
                compilerParameters.GenerateInMemory = true;
                CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, code);
                if (compilerResults.Errors.Count == 0)
                {
                    Assembly assembly = compilerResults.CompiledAssembly;
                    Type type = assembly.GetType("ExpressionCalculate");
                    object obj = Activator.CreateInstance(type);

                    Type type2 = assembly.GetType("Complex");
                    object obj2 = Activator.CreateInstance(type2);

                    //FieldInfo[] fi = type.GetFields();

                    foreach (var item in paramsList)
                    {
                        FieldInfo fi = type.GetField(item.param.ToUpper());
                        if (fi != null)
                        {
                            System.Type o = fi.FieldType;
                            if (o.Name.Equals("Complex"))
                            {
                                System.Reflection.PropertyInfo propertyInfo = type2.GetProperty("Value");
                                if (propertyInfo != null)
                                {
                                    propertyInfo.SetValue(obj2, item.value, null);
                                    fi.SetValue(obj, obj2);
                                    //object sss = fi.GetValue(obj);
                                }
                            }
                        }
                    }
                    MethodInfo method = type.GetMethod("Calculate");
                    object objValue = method.Invoke(obj, null);
                    return method.Invoke(obj, null);
                }
                else if (compilerResults.Errors.Count > 0)
                {
                    throw new Exception(compilerResults.Errors[0].ErrorText);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// 函数显示
        /// </summary>
        /// <param name="func"></param>
        public void ShowFunc(string func)
        {
            string sql = "";
            DataSet ds = null;
            if (string.IsNullOrEmpty(func))
            {
                sql = "select ID, FuncName 函数 from S_FuncName";
                ds = DataUtils.DB.GetDataSetFromSQL(sql);
            }
            else
            {
                sql = "select ID, FuncName 函数 from S_FuncName where FuncName like '%{0}%'";
                ds = DataUtils.DB.GetDataSetFromSQL(string.Format(sql, func));
            }
            if (ds != null && ds.Tables[0] != null)
            {
                FuncList parentFunclist = new FuncList() { funcID = "0", funcName = "函数列表", parent = null };
                ObservableCollection<FuncList> f = new ObservableCollection<FuncList>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    FuncList funclist = new FuncList();
                    funclist.funcID = Convert.ToString(dr["ID"]);
                    funclist.funcName = Convert.ToString(dr["函数"]);
                    funclist.parent = parentFunclist;
                    parentFunclist.Children.Add(funclist);
                }
                f.Add(parentFunclist);
                tvFuncList.ItemsSource = f;
                Expansion(tvFuncList, true);
            }
        }

        /// <summary>
        /// True 展开 false 收缩
        /// </summary>
        /// <param name="flag"></param>
        private void Expansion(TreeView tv, bool flag)
        {
            foreach (var item in tv.Items)
            {
                DependencyObject dObject = tv.ItemContainerGenerator.ContainerFromItem(item);
                if (dObject == null) continue;
                CollapseTreeviewItems(((TreeViewItem)dObject), flag, tv);
            }
        }

        /// <summary>
        /// 收缩子节点
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="IsExpanded">是否展开</param>
        private void CollapseTreeviewItems(TreeViewItem Item, bool IsExpanded, TreeView tv)
        {
            if (Item == null) return;
            Item.IsExpanded = IsExpanded;
            foreach (var item in Item.Items)
            {
                DependencyObject dObject = tv.ItemContainerGenerator.ContainerFromItem(item);

                if (dObject != null)
                {
                    ((TreeViewItem)dObject).IsExpanded = IsExpanded;

                    if (((TreeViewItem)dObject).HasItems)
                    {
                        CollapseTreeviewItems(((TreeViewItem)dObject), IsExpanded, tv);
                    }
                }
            }
        }

        public void LoadFuncName()
        {
            //DataSet ds = DataUtils.DB.GetDataSetFromSQL("select ID, FuncName 函数,Remarks 备注,LASTMODIFYUSER 登录人员,LASTMODIFYTIME 修改时间 from S_FuncName");
            //gridControl1.ItemsSource = ds.Tables[0];
            //gridControl1.Columns["ID"].Visible = false;
            //gridControl1.Columns["登录人员"].AllowEditing = DevExpress.Utils.DefaultBoolean.False; ;
            //gridControl1.Columns["修改时间"].AllowEditing = DevExpress.Utils.DefaultBoolean.False; ;
        }

        public string SaveFunc()
        {
            //try
            //{
            //    gridControl1.View.CommitEditing();
            //    DataTable dt = (DataTable)gridControl1.ItemsSource;
            //    string strSucess = "";
            //    string strFail = "";
            //    string strMessage = "{0}" + Environment.NewLine + "{1}";
            //    byte[] heByte = null;
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        string result = "";
            //        if ((dr.RowState == DataRowState.Modified) || (dr.RowState == DataRowState.Added))
            //        {
            //            heByte = System.Text.Encoding.UTF8.GetBytes(txtContent.Text);
            //            result = DataUtils.DB.ExecProc("Sys_Func",
            //                  new DBParameters(SqlDbType.VarChar, "@ACTION", "UPDATE"),
            //                  new DBParameters(SqlDbType.VarChar, "@ID", Convert.ToString(dr["ID"])),
            //                  new DBParameters(SqlDbType.VarChar, "@FuncName", Convert.ToString(dr["函数"])),
            //                  new DBParameters(SqlDbType.VarChar, "@Remarks", Convert.ToString(dr["备注"])),
            //                  new DBParameters(SqlDbType.VarChar, "@UPDATEUSER", DataUtils.StaticInfo.LoginUser)
            //                  );

            //        }
            //        if (dr.RowState == DataRowState.Deleted)
            //        {
            //            result = DataUtils.DB.ExecProc("Sys_Func",
            //                                           "@ACTION=" + "DELETE",
            //                                           "@ID=" + Convert.ToString(dr["ID", DataRowVersion.Original])
            //                                           );
            //        }

            //        if (!string.IsNullOrWhiteSpace(result))
            //        {
            //            if (result.StartsWith("OK"))
            //            {
            //                if (string.IsNullOrWhiteSpace(strSucess)) strSucess = "成功:";
            //                strSucess += Convert.ToString(dr["函数", (dr.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current]) + ";";
            //            }
            //            else
            //            {
            //                if (string.IsNullOrWhiteSpace(strFail)) strFail = "失败:";
            //                strFail += Convert.ToString(dr["函数"]) + ";";
            //            }
            //        }
            //    }
            //    return string.Format(strMessage, strSucess, strFail);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            return null;
        }

        public void SaveContent()
        {
            //if (string.IsNullOrEmpty(S_FuncName_ID)) { txtContent.Text = ""; return; }
            //byte[] heByte = System.Text.Encoding.UTF8.GetBytes(txtContent.Text);
            //string result = DataUtils.DB.ExecProc("Sys_Func_Content",
            //                 new DBParameters(SqlDbType.VarChar, "@ACTION", "UPDATE"),
            //                 new DBParameters(SqlDbType.VarChar, "@ID", "0"),
            //                 new DBParameters(SqlDbType.VarChar, "@S_FuncName_ID", S_FuncName_ID),
            //                 new DBParameters(SqlDbType.Image, "@FileStream", heByte)
            //                 );
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 函数搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTypeSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ShowFunc(txtTypeSearch.Text.Trim());
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void tvFuncList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FuncList func = (FuncList)tvFuncList.SelectedItem;
            if (func == null) return;
            if (func.parent != null)
            {
                DataSet ds = DataUtils.DB.GetDataSetFromSQL(string.Format("select * from S_FuncName_Content where S_FuncName_ID='{0}'", func.funcID));
                if (ds != null && ds.Tables[0] != null)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        if (Convert.IsDBNull(item["FileStream"]))
                        {
                            continue;
                        }
                        else
                        {
                            byte[] heByte = (byte[])item["FileStream"];
                            string strContent = System.Text.Encoding.UTF8.GetString(heByte);
                            string strNewValue = ComplierCode(strContent, func.funcName).ToUpper();
                            this.txtExpress.Text = this.txtExpress.Text.Insert(this.txtExpress.SelectionStart, strNewValue);
                        }
                    }
                }
            }
        }

        private void tvParam_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //ParamList param = (ParamList)tvParam.SelectedItem;
            //if (param == null) return;
            //if (param.parent != null)
            //{
            //    string strNewValue = param.paramName;
            //    this.txtExpress.Text = this.txtExpress.Text.Insert(this.txtExpress.SelectionStart, strNewValue);
            //}
        }

        private void btnClear_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //txtContent.Text = "";
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            LoadFuncName();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //try
            //{
            //    if (gridControl1.SelectedItem != null)
            //    {
            //        foreach (int handle in gridControl1.GetSelectedRowHandles())
            //        {
            //            ((DataRowView)gridControl1.GetRow(handle)).Delete();
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        private void btnSave_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //try
            //{
            //    string lastid = "";
            //    if ((gridControl1.View as TableView).FocusedRowData.Row != null)
            //    {
            //        lastid = Convert.ToString(((gridControl1.View as TableView).FocusedRowData.Row as DataRowView).Row["ID"]);
            //    }

            //    string result = SaveFunc();
            //    SaveContent();
            //    if (!string.IsNullOrEmpty(result.Trim()))
            //    {
            //        DXMessageBox.Show(result);
            //    }

            //    LoadFuncName();
            //    if (!string.IsNullOrEmpty(lastid))
            //    {
            //        int rowCount = (gridControl1.ItemsSource as DataTable).Rows.Count;
            //        for (int i = 0; i < rowCount; i++)
            //        {
            //            if (Convert.ToString((gridControl1.GetRow(i) as DataRowView).Row["ID"]) == lastid)
            //            {
            //                gridControl1.SelectedItem = gridControl1.GetRow(i);
            //                break;
            //            }
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        private void gridControl1_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            //try
            //{
            //    string strContent = "";
            //    string id = "";
            //    if (gridControl1.GetSelectedRowHandles().Count() == 0) return;
            //    int idx = gridControl1.GetSelectedRowHandles()[0];
            //    if (e.NewItem != null)
            //    {
            //        S_FuncName_ID = id = Convert.ToString(((DataRowView)e.NewItem)["ID"]);
            //        DataSet ds = DataUtils.DB.GetDataSetFromSQL(string.Format("select * from S_FuncName_Content where S_FuncName_ID='{0}'", id));
            //        if (ds.Tables[0] == null) { txtContent.Text = ""; return; }
            //        if (ds.Tables[0].Rows.Count == 0) { txtContent.Text = ""; return; }
            //        foreach (DataRow item in ds.Tables[0].Rows)
            //        {
            //            if (Convert.IsDBNull(item["FileStream"]))
            //            {
            //                txtContent.Text = "";
            //                return;
            //            }
            //            else
            //            {
            //                byte[] heByte = (byte[])item["FileStream"];
            //                strContent = System.Text.Encoding.UTF8.GetString(heByte);
            //                txtContent.Text = strContent;

            //            }
            //        }
            //    }
            //    else
            //    {
            //        S_FuncName_ID = "";
            //        txtContent.Text = "";
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        private void txtExpress_TextChanged(object sender, TextChangedEventArgs e)
        {
            //expression = txtExpress.Text.Trim().ToUpper();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Test();
            //BindParamList(paramList);
        }

        private void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if(tab.SelectedItem==Tabitem)
           {
               vm = (new ViewModelLocator()).CurrentTestPlanVm;
               BindFuncList();
           }
        }

        private void cmbType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            string strType = this.cmbType.SelectedItem.ToString();
            if (strType == "Normal")
            {
                this.txtStopFreq.IsEnabled = false;
            }
            else if (strType == "Max")
            {
                this.txtStopFreq.IsEnabled = true;
            }
            else if (strType == "Min")
            {
                this.txtStopFreq.IsEnabled = true;
            }
            else
            {
                this.txtStopFreq.IsEnabled = true;
            }
        }


        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    object obj = ComplierCodeResult(txtExpress.Text.Trim().ToUpper());
        //    //txtResult.Text = Convert.ToString(obj);
        //}
    }

    public class FuncList
    {
        public string funcID { get; set; }
        public string funcName { get; set; }
        public string name { get; set; }
        public ObservableCollection<FuncList> Children { get; set; }
        public FuncList parent { get; set; }
        public FuncList()
        {
            Children = new ObservableCollection<FuncList>();
        }
    }

    public class ParamList
    {
        public string paramID { get; set; }
        public string paramName { get; set; }
        public ObservableCollection<ParamList> Children { get; set; }
        public ParamList parent { get; set; }
        public ParamList()
        {
            Children = new ObservableCollection<ParamList>();
        }
    }

    public struct Params
    {
        public string param { get; set; }
        public object value { get; set; }
    }
}
