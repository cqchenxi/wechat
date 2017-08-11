using System;
using System.IO;
using System.Web;

namespace WeChat
{
    public class Log
    {
        //日志路径
        public static string path = HttpContext.Current.Request.PhysicalApplicationPath + "Logs";

        public static void Debug(string className, string content)
        {
            WriteLog("DEBUG", className, content);
        }

        public static void Info(string className, string content)
        {
            WriteLog("INFO", className, content);
        }

        public static void Error(string className, string content)
        {
            WriteLog("ERROR", className, content);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="className">类名</param>
        /// <param name="content">内容</param>
        protected static void WriteLog(string type, string className, string content)
        {
            //如果日志目录不存在就创建
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

            FileStream fileStream = new FileStream(filename, FileMode.Append, FileAccess.Write);

            //创建或打开日志文件，向日志文件末尾追加记录
            StreamWriter writer = new StreamWriter(fileStream);

            //向日志文件写入内容
            string write_content = time + " " + type + " " + className + " " + content;
            writer.WriteLine(write_content);

            //关闭日志文件
            writer.Close();
        }
    }
}