using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SymtCalc
{
    public class Calculation
    {
        public Dictionary<string, object> paramDictionary = new Dictionary<string, object>();
        public StringBuilder stringBuilder = new StringBuilder();
        public Dictionary<string, string> preDictionary = new Dictionary<string, string>();
        string structure = @"
                       using System;
                       using System.Linq;
                       using SystemFunction;
                       public class ExpressionCalculate : OperationFunction
                       {
                            {0}
                            public object Calculate()
                            {
                                object result=null;
                                object obj = {1};
                                System.Type type = obj.GetType();
                                if (type.Name.Equals(""OperatorOverload""))
                                {
                                    OperatorOverload operatorOverload = (OperatorOverload)obj;
                                    result=operatorOverload.Value;
                                }
                                else
                                {
                                    result=obj;
                                }
                                return result;
                            }
                       }";

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public void AddParam(string paramName, object value)
        {
            if (!paramDictionary.ContainsKey(paramName))
            {
                paramDictionary.Add(paramName, value);
            }
        }

        /// <summary>
        /// 参数赋值
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public void SetParam(string paramName, object value)
        {
            if (paramDictionary.ContainsKey(paramName))
            {
                paramDictionary[paramName] = value;
            }
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public object Calucate(string expression)
        {
            try
            {
                if (string.IsNullOrEmpty(expression)) { return null; }
                LoadParam();
                string code = structure.Replace("{0}", stringBuilder.ToString()).Replace("{1}", expression.ToUpper());
                CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider();
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Xaml.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");
                compilerParameters.ReferencedAssemblies.Add("SystemFunction.dll");

                compilerParameters.CompilerOptions = "/t:library";
                compilerParameters.GenerateInMemory = true;
                CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, code);
                if (compilerResults.Errors.Count > 0)
                {
                    throw new Exception(compilerResults.Errors[0].ErrorText);
                }
                else
                {
                    Assembly assembly = compilerResults.CompiledAssembly;
                    Type type = assembly.GetType("ExpressionCalculate");
                    object obj = Activator.CreateInstance(type);

                    foreach (var item in paramDictionary.Keys)
                    {
                        FieldInfo fi = type.GetField(item.ToUpper());
                        if (fi != null)
                        {
                            System.Type type2 = fi.FieldType;
                            object obj2 = Activator.CreateInstance(type2);
                            if (type2.Name.Equals("OperatorOverload"))
                            {
                                System.Reflection.PropertyInfo propertyInfo = type2.GetProperty("Value");
                                if (propertyInfo != null)
                                {
                                    propertyInfo.SetValue(obj2, paramDictionary[item], null);
                                    fi.SetValue(obj, obj2);
                                }
                            }
                        }
                    }
                    MethodInfo method = type.GetMethod("Calculate");
                    return method.Invoke(obj, null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }


     
        /// <summary>
        /// 加载字段
        /// </summary>
        private void LoadParam()
        {
            stringBuilder.Clear();
            foreach (var param in paramDictionary.Keys)
            {
                string field = @"public OperatorOverload " + param.ToUpper() + ";\r\n";
                stringBuilder.Append(field);
            }
        }
    }
}
