using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemFunction
{
        public struct OperatorOverload
        {
            public object Value { get; set; }

            public OperatorOverload(object Value)
                : this()
            {
                this.Value = Value;
            }

            public static OperatorOverload operator +(OperatorOverload c1, object obj)
            {

                System.Type type = c1.Value.GetType();
                System.Type t = obj.GetType();
                double num = 0.0;
                object result = null;
                if (t.Name.Equals("Int32") || t.Name.Equals("Double"))
                {
                    num = Convert.ToDouble(obj);
                }
                if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                {
                    bool temp = type.Name == "Int32[]" ? true : false;
                    if (temp)
                    {
                        int[] aa = (int[])c1.Value;
                        result = aa.Zip(aa, (x, y) => x + num).ToArray();
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
                return new OperatorOverload
                {
                    Value = result
                };
            }

            public static OperatorOverload operator +(object obj, OperatorOverload c2)
            {
                System.Type type = c2.Value.GetType();
                System.Type t = obj.GetType();
                double num = 0.0;
                object result = null;
                if (t.Name.Equals("Int32") || t.Name.Equals("Double"))
                {
                    num = Convert.ToDouble(obj);
                }
                if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                {
                    bool temp = type.Name == "Int32[]" ? true : false;
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
                return new OperatorOverload
                {
                    Value = result
                };
            }

            public static OperatorOverload operator +(OperatorOverload c1, OperatorOverload c2)
            {
                System.Type type = c1.Value.GetType();
                System.Type t = c2.Value.GetType();
                object result = null;
                if ((type.Name.Equals("Int32[]") || type.Name.Equals("Double[]")) && (t.Name.Equals("Int32[]") || t.Name.Equals("Double[]")))
                {
                    bool temp = type.Name == "Int32[]" ? true : false;
                    bool flag = t.Name == "Int32[]" ? true : false;
                    if (temp && flag)
                    {
                        int[] aa = (int[])c1.Value;
                        int[] bb = (int[])c2.Value;
                        if (aa.Length != bb.Length)
                        {
                            throw new Exception("两数组长度不等,不能进行运算!");
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
                            throw new Exception("两数组长度不等,不能进行运算!");
                        }
                        result = aa.Zip(bb, (x, y) => x + y).ToArray();
                    }
                    else if (!temp && flag)
                    {
                        double[] aa = (double[])c1.Value;
                        int[] bb = (int[])c2.Value;
                        if (aa.Length != bb.Length)
                        {
                            throw new Exception("两数组长度不等,不能进行运算!");
                        }
                        result = aa.Zip(bb, (x, y) => x + y).ToArray();
                    }
                    else if (!temp && !flag)
                    {
                        double[] aa = (double[])c1.Value;
                        double[] bb = (double[])c2.Value;
                        if (aa.Length != bb.Length)
                        {
                            throw new Exception("两数组长度不等,不能进行运算!");
                        }
                        result = aa.Zip(bb, (x, y) => x + y).ToArray();
                    }
                }
                else if ((type.Name.Equals("Int32[]") || type.Name.Equals("Double[]")))
                {
                    bool tempa = type.Name == "Int32[]" ? true : false;
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
                else if ((t.Name.Equals("Int32[]") || t.Name.Equals("Double[]")))
                {
                    bool tempb = t.Name == "Int32[]" ? true : false;
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
                return new OperatorOverload
                {
                    Value = result
                };
            }

            public static OperatorOverload operator -(OperatorOverload c1, object obj)
            {
                System.Type type = c1.Value.GetType();
                System.Type t = obj.GetType();
                double num = 0.0;
                object result = null;
                if (t.Name.Equals("Int32") || t.Name.Equals("Double"))
                {
                    num = Convert.ToDouble(obj);
                }
                if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                {
                    bool temp = type.Name == "Int32[]" ? true : false;
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
                return new OperatorOverload
                {
                    Value = result
                };
            }

            public static OperatorOverload operator -(object obj, OperatorOverload c2)
            {
                System.Type type = c2.Value.GetType();
                System.Type t = obj.GetType();
                double num = 0.0;
                object result = null;
                if (t.Name.Equals("Int32") || t.Name.Equals("Double"))
                {
                    num = Convert.ToDouble(obj);
                }
                if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                {
                    bool temp = type.Name == "Int32[]" ? true : false;
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
                return new OperatorOverload
                {
                    Value = result
                };
            }

            public static OperatorOverload operator -(OperatorOverload c1, OperatorOverload c2)
            {
                System.Type type = c1.Value.GetType();
                System.Type t = c2.Value.GetType();
                object result = null;
                if ((type.Name.Equals("Int32[]") || type.Name.Equals("Double[]")) && (t.Name.Equals("Int32[]") || t.Name.Equals("Double[]")))
                {
                    bool temp = type.Name == "Int32[]" ? true : false;
                    bool flag = t.Name == "Int32[]" ? true : false;
                    if (temp && flag)
                    {
                        int[] aa = (int[])c1.Value;
                        int[] bb = (int[])c2.Value;
                        if (aa.Length != bb.Length)
                        {
                            throw new Exception("两数组长度不等,不能进行运算!");
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
                            throw new Exception("两数组长度不等,不能进行运算!");
                        }
                        result = aa.Zip(bb, (x, y) => x - y).ToArray();
                    }
                    else if (!temp && flag)
                    {
                        double[] aa = (double[])c1.Value;
                        int[] bb = (int[])c2.Value;
                        if (aa.Length != bb.Length)
                        {
                            throw new Exception("两数组长度不等,不能进行运算!");
                        }
                        result = aa.Zip(bb, (x, y) => x - y).ToArray();
                    }
                    else if (!temp && !flag)
                    {
                        double[] aa = (double[])c1.Value;
                        double[] bb = (double[])c2.Value;
                        if (aa.Length != bb.Length)
                        {
                            throw new Exception("两数组长度不等,不能进行运算!");
                        }
                        result = aa.Zip(bb, (x, y) => x - y).ToArray();
                    }
                }
                else if ((type.Name.Equals("Int32[]") || type.Name.Equals("Double[]")))
                {
                    bool tempa = type.Name == "Int32[]" ? true : false;
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
                else if ((t.Name.Equals("Int32[]") || t.Name.Equals("Double[]")))
                {
                    bool tempb = t.Name == "Int32[]" ? true : false;
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
                return new OperatorOverload
                {
                    Value = result
                };
            }

            public static OperatorOverload operator *(OperatorOverload c1, object obj)
            {
                System.Type type = c1.Value.GetType();
                System.Type t = obj.GetType();
                double num = 0.0;
                object result = null;
                if (t.Name.Equals("Int32") || t.Name.Equals("Double"))
                {
                    num = Convert.ToDouble(obj);
                }
                if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                {
                    bool temp = type.Name == "Int32[]" ? true : false;
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
                return new OperatorOverload
                {
                    Value = result
                };
            }

            public static OperatorOverload operator *(object obj, OperatorOverload c2)
            {
                System.Type type = c2.Value.GetType();
                System.Type t = obj.GetType();
                double num = 0.0;
                object result = null;
                if (t.Name.Equals("Int32") || t.Name.Equals("Double"))
                {
                    num = Convert.ToDouble(obj);
                }
                if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                {
                    bool temp = type.Name == "Int32[]" ? true : false;
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
                return new OperatorOverload
                {
                    Value = result
                };
            }

            public static OperatorOverload operator *(OperatorOverload c1, OperatorOverload c2)
            {
                System.Type type = c1.Value.GetType();
                System.Type t = c2.Value.GetType();
                object result = null;
                if ((type.Name.Equals("Int32[]") || type.Name.Equals("Double[]")) && (t.Name.Equals("Int32[]") || t.Name.Equals("Double[]")))
                {
                    bool temp = type.Name == "Int32[]" ? true : false;
                    bool flag = t.Name == "Int32[]" ? true : false;
                    if (temp && flag)
                    {
                        int[] aa = (int[])c1.Value;
                        int[] bb = (int[])c2.Value;
                        if (aa.Length != bb.Length)
                        {
                            throw new Exception("两数组长度不等,不能进行运算!");
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
                            throw new Exception("两数组长度不等,不能进行运算!");
                        }
                        result = aa.Zip(bb, (x, y) => x * y).ToArray();
                    }
                    else if (!temp && flag)
                    {
                        double[] aa = (double[])c1.Value;
                        int[] bb = (int[])c2.Value;
                        if (aa.Length != bb.Length)
                        {
                            throw new Exception("两数组长度不等,不能进行运算!");
                        }
                        result = aa.Zip(bb, (x, y) => x * y).ToArray();
                    }
                    else if (!temp && !flag)
                    {
                        double[] aa = (double[])c1.Value;
                        double[] bb = (double[])c2.Value;
                        if (aa.Length != bb.Length)
                        {
                            throw new Exception("两数组长度不等,不能进行运算!");
                        }
                        result = aa.Zip(bb, (x, y) => x * y).ToArray();
                    }
                }
                else if ((type.Name.Equals("Int32[]") || type.Name.Equals("Double[]")))
                {
                    bool tempa = type.Name == "Int32[]" ? true : false;
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
                else if ((t.Name.Equals("Int32[]") || t.Name.Equals("Double[]")))
                {
                    bool tempb = t.Name == "Int32[]" ? true : false;
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
                return new OperatorOverload
                {
                    Value = result
                };
            }

            public static OperatorOverload operator /(OperatorOverload c1, object obj)
            {
                System.Type type = c1.Value.GetType();
                System.Type t = obj.GetType();
                double num = 0.0;
                object result = null;
                if (t.Name.Equals("Int32") || t.Name.Equals("Double"))
                {
                    num = Convert.ToDouble(obj);
                }
                if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                {
                    bool temp = type.Name == "Int32[]" ? true : false;
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
                return new OperatorOverload
                {
                    Value = result
                };
            }

            public static OperatorOverload operator /(object obj, OperatorOverload c2)
            {
                System.Type type = c2.Value.GetType();
                System.Type t = obj.GetType();
                double num = 0.0;
                object result = null;
                if (t.Name.Equals("Int32") || t.Name.Equals("Double"))
                {
                    num = Convert.ToDouble(obj);
                }
                if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                {
                    bool temp = type.Name == "Int32[]" ? true : false;
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
                return new OperatorOverload
                {
                    Value = result
                };
            }

            public static OperatorOverload operator /(OperatorOverload c1, OperatorOverload c2)
            {
                System.Type type = c1.Value.GetType();
                System.Type t = c2.Value.GetType();
                object result = null;
                if ((type.Name.Equals("Int32[]") || type.Name.Equals("Double[]")) && (t.Name.Equals("Int32[]") || t.Name.Equals("Double[]")))
                {
                    bool temp = type.Name == "Int32[]" ? true : false;
                    bool flag = t.Name == "Int32[]" ? true : false;
                    if (temp && flag)
                    {
                        int[] aa = (int[])c1.Value;
                        int[] bb = (int[])c2.Value;
                        if (aa.Length != bb.Length)
                        {
                            throw new Exception("两数组长度不等,不能进行运算!");
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
                            throw new Exception("两数组长度不等,不能进行运算!");
                        }
                        result = aa.Zip(bb, (x, y) => x / y).ToArray();
                    }
                    else if (!temp && flag)
                    {
                        double[] aa = (double[])c1.Value;
                        int[] bb = (int[])c2.Value;
                        if (aa.Length != bb.Length)
                        {
                            throw new Exception("两数组长度不等,不能进行运算!");
                        }
                        result = aa.Zip(bb, (x, y) => x / y).ToArray();
                    }
                    else if (!temp && !flag)
                    {
                        double[] aa = (double[])c1.Value;
                        double[] bb = (double[])c2.Value;
                        if (aa.Length != bb.Length)
                        {
                            throw new Exception("两数组长度不等,不能进行运算!");
                        }
                        result = aa.Zip(bb, (x, y) => x / y).ToArray();
                    }
                }
                else if ((type.Name.Equals("Int32[]") || type.Name.Equals("Double[]")))
                {
                    bool tempa = type.Name == "Int32[]" ? true : false;
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
                else if ((t.Name.Equals("Int32[]") || t.Name.Equals("Double[]")))
                {
                    bool tempb = t.Name == "Int32[]" ? true : false;
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
                return new OperatorOverload
                {
                    Value = result
                };
            }
        }

        public class OperationFunction
        {
            /// <summary>
            /// 求绝对值
            /// </summary>
            /// <param name="num">数字或数组</param>
            /// <returns></returns>
            public static OperatorOverload ABS(object num)
            {
                try
                {
                    object result = null;
                    System.Type t = num.GetType();
                    if (t.Name.Equals("OperatorOverload"))
                    {
                        OperatorOverload OperatorOverload = (OperatorOverload)num;
                        System.Type type = OperatorOverload.Value.GetType();
                        if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                        {
                            bool temp = type.Name == "Int32[]" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Abs(x)).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Abs(x)).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(OperatorOverload.Value);
                            result = System.Math.Abs(temp);
                        }
                        OperatorOverload.Value = result;
                        return OperatorOverload;
                    }
                    else if (t.Name.Equals("Int32[]") || t.Name.Equals("Double[]"))
                    {
                        bool temp = t.Name == "Int32[]" ? true : false;
                        if (temp)
                        {
                            int[] aa = (int[])num;
                            result = aa.Zip(aa, (x, y) => System.Math.Abs(x)).ToArray();
                        }
                        else
                        {
                            double[] aa = (double[])num;
                            result = aa.Zip(aa, (x, y) => System.Math.Abs(x)).ToArray();
                        }
                        OperatorOverload OperatorOverload = new OperatorOverload(result);
                        return OperatorOverload;
                    }
                    else
                    {
                        double temp = Convert.ToDouble(num);
                        result = System.Math.Abs(temp);
                        OperatorOverload OperatorOverload = new OperatorOverload(result);
                        return OperatorOverload;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }

            /// <summary>
            /// 产生x1与x2之间的随机数
            /// </summary>
            /// <param name="x1"></param>
            /// <param name="x2"></param>
            /// <returns></returns>
            public static double RANDBETWEEN(int x1, int x2)
            {
                System.Random d = new System.Random();
                double result = d.Next(x1, x2);
                return result;
            }

            /// <summary>
            /// 正弦
            /// </summary>
            /// <param name="num">数字或数组</param>
            /// <returns></returns>
            public static OperatorOverload SIN(object num)
            {
                try
                {
                    System.Type t = num.GetType();
                    object result = null;
                    if (t.Name.Equals("OperatorOverload"))
                    {
                        OperatorOverload OperatorOverload = (OperatorOverload)num;
                        System.Type type = OperatorOverload.Value.GetType();
                        if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                        {
                            bool temp = type.Name == "Int32[]" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Sin(x)).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Sin(x)).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(OperatorOverload.Value);
                            result = System.Math.Sin(temp);
                        }
                        OperatorOverload.Value = result;
                        return OperatorOverload;
                    }
                    else
                    {
                        double temp = Convert.ToDouble(num);
                        result = System.Math.Sin(temp);
                        OperatorOverload OperatorOverload = new OperatorOverload(result);
                        return OperatorOverload;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }

            /// <summary>
            /// 开根号
            /// </summary>
            /// <param name="num">数字或数组</param>
            /// <returns></returns>
            public static OperatorOverload SQRT(object num)
            {
                try
                {
                    System.Type t = num.GetType();
                    object result = null;
                    if (t.Name.Equals("OperatorOverload"))
                    {
                        OperatorOverload OperatorOverload = (OperatorOverload)num;
                        System.Type type = OperatorOverload.Value.GetType();
                        if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                        {
                            bool temp = type.Name == "Int32[]" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Sqrt(x)).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Sqrt(x)).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(OperatorOverload.Value);
                            result = System.Math.Sqrt(temp);
                        }
                        OperatorOverload.Value = result;
                        return OperatorOverload;
                    }
                    else
                    {
                        double temp = Convert.ToDouble(num);
                        result = System.Math.Sqrt(temp);
                        OperatorOverload OperatorOverload = new OperatorOverload(result);
                        return OperatorOverload;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }

            }

            /// <summary>
            /// TAN
            /// </summary>
            /// <param name="num">数字或数组</param>
            /// <returns></returns>
            public static OperatorOverload TAN(object num)
            {
                try
                {
                    System.Type t = num.GetType();
                    object result = null;
                    if (t.Name.Equals("OperatorOverload"))
                    {
                        OperatorOverload OperatorOverload = (OperatorOverload)num;
                        System.Type type = OperatorOverload.Value.GetType();
                        if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                        {
                            bool temp = type.Name == "Int32[]" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Tan(x)).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Tan(x)).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(OperatorOverload.Value);
                            result = System.Math.Tan(temp);
                        }
                        OperatorOverload.Value = result;
                        return OperatorOverload;
                    }
                    else
                    {
                        double temp = Convert.ToDouble(num);
                        result = System.Math.Tan(temp);
                        OperatorOverload OperatorOverload = new OperatorOverload(result);
                        return OperatorOverload;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }

            }

            /// <summary>
            /// 求和
            /// </summary>
            /// <param name="num">数字或数组</param>
            /// <returns></returns>
            public static double SUM(params object[] obj)
            {
                try
                {
                    double tempNum = 0.0;
                    foreach (var item in obj)
                    {
                        if (item.GetType().Name.Equals("OperatorOverload"))
                        {
                            OperatorOverload OperatorOverload = (OperatorOverload)item;
                            if (OperatorOverload.Value.GetType().Name.Equals("Int32[]") || OperatorOverload.Value.GetType().Name.Equals("Double[]"))
                            {
                                bool temp = OperatorOverload.Value.GetType().Name == "Int32[]" ? true : false;
                                if (temp)
                                {
                                    int[] aa = (int[])OperatorOverload.Value;
                                    tempNum += aa.Sum();
                                }
                                else
                                {
                                    double[] aa = (double[])OperatorOverload.Value;
                                    tempNum += aa.Sum();
                                }
                            }
                            else
                            {
                                double temp = Convert.ToDouble(OperatorOverload.Value);
                                tempNum += temp;
                            }
                        }
                        else
                        {
                            tempNum += Convert.ToDouble(item);
                        }
                    }
                    return tempNum;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }

            }

            /// <summary>
            /// 求平均
            /// </summary>
            /// <param name="num">数字或数组</param>
            /// <returns></returns>
            public static double AVERAGE(params object[] obj)
            {
                try
                {
                    System.Type t = obj.GetType();
                    double tempNum = 0.0;
                    double result = 0.0;
                    bool flag = false;
                    bool isok = false;
                    foreach (var item in obj)
                    {
                        if (item.GetType().Name.Equals("OperatorOverload"))
                        {
                            OperatorOverload OperatorOverload = (OperatorOverload)item;
                            if (OperatorOverload.Value.GetType().Name.Equals("Int32[]") || OperatorOverload.Value.GetType().Name.Equals("Double[]"))
                            {
                                flag = true;
                                bool temp = OperatorOverload.Value.GetType().Name == "Int32[]" ? true : false;
                                if (temp)
                                {
                                    int[] aa = (int[])OperatorOverload.Value;
                                    tempNum += aa.Average();
                                }
                                else
                                {
                                    double[] aa = (double[])OperatorOverload.Value;
                                    tempNum += aa.Average();
                                }
                            }
                            else
                            {
                                isok = true;
                                double temp = Convert.ToDouble(OperatorOverload.Value);
                                tempNum += temp;
                            }
                        }

                        else
                        {
                            isok = true;
                            tempNum += Convert.ToDouble(item);
                        }
                        if (flag && isok)
                        {
                            throw new Exception("数组与数字不能混合运算求平均!");
                        }
                    }
                    result = tempNum / obj.Length;
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }

            /// <summary>
            /// 余弦
            /// </summary>
            /// <param name="num">数字或数组</param>
            /// <returns></returns>
            public static OperatorOverload COS(object num)
            {
                try
                {
                    System.Type t = num.GetType();
                    object result = null;
                    if (t.Name.Equals("OperatorOverload"))
                    {
                        OperatorOverload OperatorOverload = (OperatorOverload)num;
                        System.Type type = OperatorOverload.Value.GetType();
                        if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                        {
                            bool temp = type.Name == "Int32[]" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Cos(x)).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Cos(x)).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(OperatorOverload.Value);
                            result = System.Math.Cos(temp);
                        }
                        OperatorOverload.Value = result;
                        return OperatorOverload;
                    }
                    else
                    {
                        double temp = Convert.ToDouble(num);
                        result = System.Math.Cos(temp);
                        OperatorOverload OperatorOverload = new OperatorOverload(result);
                        return OperatorOverload;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }

            /// <summary>
            /// 求最大值
            /// </summary>
            /// <param name="num">数字或数组</param>
            /// <returns></returns>
            public static double? MAX(params object[] obj)
            {
                try
                {
                    System.Type t = obj.GetType();
                    double? num1 = null;
                    double? tempNum = null;
                    bool flag = false;
                    bool isok = false;
                    foreach (var item in obj)
                    {
                        if (item.GetType().Name.Equals("OperatorOverload"))
                        {
                            OperatorOverload OperatorOverload = (OperatorOverload)item;
                            if (OperatorOverload.Value.GetType().Name.Equals("Int32[]") || OperatorOverload.Value.GetType().Name.Equals("Double[]"))
                            {
                                flag = true;
                                bool temp = OperatorOverload.Value.GetType().Name == "Int32[]" ? true : false;
                                if (temp)
                                {
                                    int[] aa = (int[])OperatorOverload.Value;
                                    tempNum = aa.Max();
                                    if (num1 == null)
                                    {
                                        num1 = tempNum;
                                    }

                                    if (tempNum >= num1)
                                    {
                                        num1 = tempNum;
                                    }
                                }
                                else
                                {
                                    double[] aa = (double[])OperatorOverload.Value;
                                    tempNum = aa.Max();
                                    if (num1 == null)
                                    {
                                        num1 = tempNum;
                                    }

                                    if (tempNum >= num1)
                                    {
                                        num1 = tempNum;
                                    }
                                }
                            }
                            else
                            {
                                isok = true;
                                tempNum = Convert.ToDouble(OperatorOverload.Value);
                                if (num1 == null)
                                {
                                    num1 = tempNum;
                                }

                                if (tempNum >= num1)
                                {
                                    num1 = tempNum;
                                }
                            }
                        }
                        else
                        {
                            isok = true;
                            tempNum = Convert.ToDouble(item);
                            if (num1 == null)
                            {
                                num1 = tempNum;
                            }

                            if (tempNum >= num1)
                            {
                                num1 = tempNum;
                            }
                        }
                        if (flag && isok)
                        {
                            throw new Exception("数组与数字不能混合运算求最大值!");
                        }
                    }
                    return num1;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }

            /// <summary>
            /// E指数
            /// </summary>
            /// <param name="num">数字或数组</param>
            /// <returns></returns>
            public static OperatorOverload EXP(object num)
            {
                try
                {
                    System.Type t = num.GetType();
                    object result = null;
                    if (t.Name.Equals("OperatorOverload"))
                    {
                        OperatorOverload OperatorOverload = (OperatorOverload)num;
                        System.Type type = OperatorOverload.Value.GetType();
                        if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                        {
                            bool temp = type.Name == "Int32[]" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Exp(x)).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Exp(x)).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(OperatorOverload.Value);
                            result = System.Math.Exp(temp);
                        }
                        OperatorOverload.Value = result;
                        return OperatorOverload;
                    }
                    else
                    {
                        double temp = Convert.ToDouble(num);
                        result = System.Math.Exp(temp);
                        OperatorOverload OperatorOverload = new OperatorOverload(result);
                        return OperatorOverload;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }

            }

            /// <summary>
            /// 求最小值
            /// </summary>
            /// <param name="num">数字或数组</param>
            /// <returns></returns>
            public static double? MIN(params object[] obj)
            {
                try
                {
                    System.Type t = obj.GetType();
                    double? num1 = null;
                    double? tempNum = null;
                    bool flag = false;
                    bool isok = false;
                    foreach (var item in obj)
                    {
                        if (item.GetType().Name.Equals("OperatorOverload"))
                        {
                            OperatorOverload OperatorOverload = (OperatorOverload)item;
                            if (OperatorOverload.Value.GetType().Name.Equals("Int32[]") || OperatorOverload.Value.GetType().Name.Equals("Double[]"))
                            {
                                flag = true;
                                bool temp = OperatorOverload.Value.GetType().Name == "Int32[]" ? true : false;
                                if (temp)
                                {
                                    int[] aa = (int[])OperatorOverload.Value;
                                    tempNum = aa.Min();
                                    if (num1 == null)
                                    {
                                        num1 = tempNum;
                                    }

                                    if (tempNum <= num1)
                                    {
                                        num1 = tempNum;
                                    }
                                }
                                else
                                {
                                    double[] aa = (double[])OperatorOverload.Value;
                                    tempNum = aa.Min();
                                    if (num1 == null)
                                    {
                                        num1 = tempNum;
                                    }

                                    if (tempNum <= num1)
                                    {
                                        num1 = tempNum;
                                    }
                                }
                            }
                            else
                            {
                                isok = true;
                                tempNum = Convert.ToDouble(OperatorOverload.Value);
                                if (num1 == null)
                                {
                                    num1 = tempNum;
                                }

                                if (tempNum <= num1)
                                {
                                    num1 = tempNum;
                                }
                            }
                        }
                        else
                        {
                            isok = true;
                            tempNum = Convert.ToDouble(item);
                            if (num1 == null)
                            {
                                num1 = tempNum;
                            }
                            if (tempNum <= num1)
                            {
                                num1 = tempNum;
                            }
                        }
                        if (flag && isok)
                        {
                            throw new Exception("数组与数字不能混合运算求最小值!");
                        }
                    }
                    return num1;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }

            /// <summary>
            /// 求方差
            /// </summary>
            /// <param name="num">数字或数组</param>
            /// <returns></returns>
            public static double? STDEV(params object[] obj)
            {
                try
                {
                    System.Type t = obj.GetType();
                    double? tempNum = 0.0;
                    double? result = 0.0;
                    double? total = 0.0;
                    double? avg = 0.0;
                    bool flag = false;
                    bool isok = false;
                    foreach (var item in obj)
                    {
                        if (item.GetType().Name.Equals("OperatorOverload"))
                        {
                            OperatorOverload OperatorOverload = (OperatorOverload)item;
                            if (OperatorOverload.Value.GetType().Name.Equals("Int32[]") || OperatorOverload.Value.GetType().Name.Equals("Double[]"))
                            {
                                flag = true;
                                bool temp = OperatorOverload.Value.GetType().Name == "Int32[]" ? true : false;
                                if (temp)
                                {
                                    int[] aa = (int[])OperatorOverload.Value;
                                    tempNum = aa.Average();
                                    foreach (int x in aa)
                                    {
                                        total += (x - tempNum) * (x - tempNum);
                                    }
                                    result = result / aa.Length;
                                }
                                else
                                {
                                    double[] aa = (double[])OperatorOverload.Value;
                                    tempNum = aa.Average();
                                    foreach (int x in aa)
                                    {
                                        total += (x - tempNum) * (x - tempNum);
                                    }
                                    result = result / aa.Length;
                                }
                            }
                            else
                            {
                                isok = true;
                                tempNum += Convert.ToDouble(OperatorOverload.Value);
                            }
                        }

                        else
                        {
                            isok = true;
                            tempNum += Convert.ToDouble(item);
                        }

                        if (flag && isok)
                        {
                            throw new Exception("数组与数字不能混合运算方差!");
                        }
                    }
                    avg = tempNum / obj.Length;
                    foreach (double item in obj)
                    {
                        total += (item - avg) * (item - avg);
                    }
                    result = total / obj.Length;
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }

            }

            /// <summary>
            /// 以10为第的对数
            /// </summary>
            /// <param name="num">数字或数组</param>
            /// <returns></returns>
            public static OperatorOverload LOG10(object num)
            {
                try
                {
                    System.Type t = num.GetType();
                    object result = null;
                    if (t.Name.Equals("OperatorOverload"))
                    {
                        OperatorOverload OperatorOverload = (OperatorOverload)num;
                        System.Type type = OperatorOverload.Value.GetType();
                        if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                        {
                            bool temp = type.Name == "Int32[]" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Log10(x)).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Log10(x)).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(OperatorOverload.Value);
                            result = System.Math.Log10(temp);
                        }
                        OperatorOverload.Value = result;
                        return OperatorOverload;
                    }
                    else
                    {
                        double temp = Convert.ToDouble(num);
                        result = System.Math.Log10(temp);
                        OperatorOverload OperatorOverload = new OperatorOverload(result);
                        return OperatorOverload;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            public static OperatorOverload POW(object num1, object num2)
            {
                try
                {
                    System.Type t1 = num1.GetType();
                    System.Type t2 = num2.GetType();
                    object result = null;

                    //OperatorOverload OperatorOverloadI = (OperatorOverload)num2;
                    //double a = Convert.ToDouble(OperatorOverloadI.Value);
                    if (t1.Name.Equals("OperatorOverload") && !t2.Name.Equals("OperatorOverload"))
                    {
                        OperatorOverload OperatorOverload = (OperatorOverload)num1;
                        System.Type type = OperatorOverload.Value.GetType();
                        double a = Convert.ToDouble(num2);
                        if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                        {
                            bool temp = type.Name == "Int32[]" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])OperatorOverload.Value;

                                result = aa.Zip(aa, (x, y) => System.Math.Pow(x, a)).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])OperatorOverload.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Pow(x, a)).ToArray();
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(OperatorOverload.Value);
                            double temp2 = Convert.ToDouble(num2);
                            result = System.Math.Pow(temp, temp2);
                        }
                        OperatorOverload.Value = result;
                        return OperatorOverload;
                    }
                    else if (!t1.Name.Equals("OperatorOverload") && t2.Name.Equals("OperatorOverload"))
                    {
                        OperatorOverload OperatorOverload = (OperatorOverload)num2;
                        System.Type type = OperatorOverload.Value.GetType();
                        double a = Convert.ToDouble(num1);
                        if (type.Name.Equals("Int32[]") || type.Name.Equals("Double[]"))
                        {
                            bool temp = type.Name == "Int32[]" ? true : false;
                            if (temp)
                            {
                                int[] aa = (int[])OperatorOverload.Value;
                                double[] resultClone = new double[aa.Length];
                                for (int j = 0; j < aa.Length; j++)
                                {
                                    resultClone[j] = System.Math.Pow(a, aa[j]);
                                }
                                result = resultClone;
                            }
                            else
                            {
                                double[] aa = (double[])OperatorOverload.Value;
                                double[] resultClone = new double[aa.Length];
                                for (int j = 0; j < aa.Length; j++)
                                {
                                    resultClone[j] = System.Math.Pow(a, aa[j]);
                                }
                                result = resultClone;
                            }
                        }
                        else
                        {
                            double temp = Convert.ToDouble(OperatorOverload.Value);
                            result = System.Math.Pow(a, temp);
                        }
                        OperatorOverload.Value = result;
                        return OperatorOverload;
                    }
                    else if (t1.Name.Equals("OperatorOverload") && t2.Name.Equals("OperatorOverload"))
                    {
                        OperatorOverload OperatorOverload1 = (OperatorOverload)num1;
                        System.Type type1 = OperatorOverload1.Value.GetType();
                        OperatorOverload OperatorOverload2 = (OperatorOverload)num2;
                        System.Type type2 = OperatorOverload2.Value.GetType();
                        if ((type1.Name.Equals("Int32[]") || type1.Name.Equals("Double[]")) && (type2.Name.Equals("Int32") || type2.Name.Equals("Double")))
                        {
                            bool temp = type1.Name == "Int32[]" ? true : false;
                            double a = Convert.ToDouble(OperatorOverload2.Value);
                            if (temp)
                            {
                                int[] aa = (int[])OperatorOverload1.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Pow(x, a)).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])OperatorOverload1.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Pow(x, a)).ToArray();
                            }
                        }
                        else if ((type2.Name.Equals("Int32[]") || type2.Name.Equals("Double[]")) && (type1.Name.Equals("Int32") || type1.Name.Equals("Double")))
                        {
                            bool temp = type2.Name == "Int32[]" ? true : false;
                            double a = Convert.ToDouble(OperatorOverload1.Value);
                            if (temp)
                            {
                                int[] aa = (int[])OperatorOverload2.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Pow(a, x)).ToArray();
                            }
                            else
                            {
                                double[] aa = (double[])OperatorOverload2.Value;
                                result = aa.Zip(aa, (x, y) => System.Math.Pow(a, x)).ToArray();
                            }
                        }
                        else
                        {
                            double temp1 = Convert.ToDouble(OperatorOverload1.Value);
                            double temp2 = Convert.ToDouble(OperatorOverload2.Value);
                            result = System.Math.Pow(temp1, temp2);
                        }
                        OperatorOverload OperatorOverload = new OperatorOverload(result);
                        return OperatorOverload;
                    }
                    else
                    {
                        double temp = Convert.ToDouble(num1);
                        double a = Convert.ToDouble(num2);
                        result = System.Math.Pow(temp, a);
                        OperatorOverload OperatorOverload = new OperatorOverload(result);
                        return OperatorOverload;

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }

            /// <summary>
            /// Pi
            /// </summary>
            /// <returns></returns>
            public static double PI()
            {
                double result = 0.0;
                result = Math.PI;
                return result;
            }

            /// <summary>
            /// RAND
            /// </summary>
            /// <returns></returns>
            public static double RAND()
            {
                System.Random d = new System.Random();
                double result = d.Next();
                return result;
            }
        }
}
