using System;
using System.IO;
using System.Net;
using System.Text;

namespace WeChat
{
    public class HttpService
    {
        #region POST
        /// <summary>
        /// Http Post
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public static string Post(string data, string url)
        {
            GC.Collect(); //垃圾回收，回收没有正常关闭的http连接

            string result = ""; //返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;

                //设置HttpWebRequest
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";

                StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);

                writer.Write(data);

                writer.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd().Trim();
                reader.Close();
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                Log.Error("HttpService", "ThreadAbortException: " + ex.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException ex)
            {
                Log.Error("HttpService", ex.ToString());

                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)ex.Response).StatusCode);
                    Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)ex.Response).StatusDescription);
                }
            }
            catch (Exception e)
            {
                Log.Error("HttpService", e.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }
        #endregion

        #region GET
        /// <summary>
        /// Http Get
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public static string Get(string url)
        {
            GC.Collect(); //垃圾回收，回收没有正常关闭的http连接

            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            //请求url以获取数据
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;

                //设置HttpWebRequest
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd().Trim();
                reader.Close();
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                Log.Error("HttpService", "ThreadAbortException: " + ex.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException ex)
            {
                Log.Error("HttpService", ex.ToString());
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)ex.Response).StatusCode);
                    Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)ex.Response).StatusDescription);
                }
            }
            catch (Exception ex)
            {
                Log.Error("HttpService", ex.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }
        #endregion
    }
}