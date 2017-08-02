using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WeChat.Models
{
    public class Log
    {
        //在网站根目录下创建日志目录
        public static string path = HttpContext.Current.Request.PhysicalApplicationPath + "Logs";

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="className">类名</param>
        /// <param name="content">内容</param>
        public static void Write(string type, string className, string content)
        {
            //如果日志目录不存在就创建
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

            //创建或打开日志文件，向日志文件末尾追加记录
            StreamWriter writer = File.AppendText(filename);

            //向日志文件写入内容
            string write_content = time + " " + type + " " + className + ": " + content;
            writer.WriteLine(write_content);

            //关闭日志文件
            writer.Close();
        }
    }
}