using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataUtils
{
    public class LOGINFO
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
