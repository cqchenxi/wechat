using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WeChat
{
    public class CData : IXmlSerializable
    {
        private string text;

        public static implicit operator string(CData value)
        {
            return value.text;
        }

        public static implicit operator CData(string text)
        {
            return new CData(text);
        }

        public override string ToString()
        {
            return this.text;
        }

        public CData() : this(string.Empty)
        {
        }

        public CData(string value)
        {
            this.text = value;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(XmlReader reader)
        {
            this.text = reader.ReadElementContentAsString();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteCData(this.text);
        }
    }
}