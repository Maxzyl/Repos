using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ATS_TR
{
    public class ApplicationUtils
    {
        public static void SaveException(Exception ex)
        {
            StringBuilder sbstr = new StringBuilder();
            sbstr.AppendLine("-------------------EXCEPTION BEGIN-------------------");
            while (ex != null)
            {
                sbstr.AppendLine(ex.Message + Environment.NewLine + ex.StackTrace);
                ex = ex.InnerException;
                if (ex != null)
                {
                    sbstr.AppendLine("-------------------EXCEPTION INNER-------------------");
                }
            }
            sbstr.AppendLine("-------------------EXCEPTION END---------------------");
            LOG.WriteError(sbstr.ToString());
        }

        //AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        public static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);
            foreach (Assembly aby in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (aby.FullName == assemblyName.FullName)
                {
                    return Assembly.LoadFrom(aby.Location);
                }
            }
            return null;
        }
    }

    public class LOG
    {
        private static readonly object obj = new object();
        public static bool ISDEBUG = true;

        public static void WriteLog(string content)
        {
            WriteLogs(content, "INFO");
        }

        public static void WriteError(string content)
        {
            WriteLogs(content, "ERRORLOG");
        }

        private static void WriteLogs(string content, string type)
        {
            lock (obj)
            {
                string filename = AppDomain.CurrentDomain.BaseDirectory + "\\LOG\\{0}\\{1}.LOG";
                filename = string.Format(filename, type, DateTime.Now.ToString("yyyy-MM-dd"));
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                }
                if (!string.IsNullOrEmpty(filename))
                {
                    if (!File.Exists(filename))
                    {
                        FileStream fs = File.Create(filename);
                        fs.Close();
                    }
                    if (File.Exists(filename))
                    {
                        StreamWriter sw = new StreamWriter(filename, true, System.Text.Encoding.UTF8);
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        sw.WriteLine("详情：" + content);
                        sw.WriteLine("----------------------------------------");
                        sw.Close();
                    }
                }
            }
        }

        public static void WriteLine(string str)
        {
            if (!ISDEBUG) return;
            string filename = AppDomain.CurrentDomain.BaseDirectory + "\\LOG\\DEBUG\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".LOG";
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }
            if (!string.IsNullOrEmpty(filename))
            {
                if (!File.Exists(filename))
                {
                    FileStream fs = File.Create(filename);
                    fs.Close();
                }
                if (File.Exists(filename))
                {
                    StreamWriter sw = new StreamWriter(filename, true, System.Text.Encoding.UTF8);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + str);
                    sw.Close();
                }
            }

        }
    }
}
