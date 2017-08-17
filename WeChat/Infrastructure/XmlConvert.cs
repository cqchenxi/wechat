using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WeChat
{
    public static class XmlConvert
    {
        public static string SerializeObject(object obj)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType(), new XmlRootAttribute("xml"));

                XmlSerializerNamespaces serializerNamespaces = new XmlSerializerNamespaces();
                serializerNamespaces.Add("", ""); //忽略XML命名空间

                StringBuilder sb = new StringBuilder();

                XmlWriterSettings xws = new XmlWriterSettings
                {
                    Indent = true, //缩进元素
                    OmitXmlDeclaration = true //忽略XML声明
                };

                XmlWriter xw = XmlWriter.Create(sb, xws);

                xmlSerializer.Serialize(xw, obj, serializerNamespaces);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Log.Error("XmlConvert", ex.Message);
                return null;
            }
        }
    }
}