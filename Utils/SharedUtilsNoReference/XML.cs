using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SharedUtilsNoReference
{
    public static class XML
    {
        public static string SerializeToXml<T>(T value)
        {
            string ris = string.Empty;
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, value);
                ris = writer.ToString();
            }
            return ris;
        }

        public static string SerializeToXml<T>(T value, Type[] ExtraTypes)
        {
            string ris = string.Empty;
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), ExtraTypes);
                serializer.Serialize(writer, value);
                ris = writer.ToString();
            }
            return ris;
        }

        public static T DeserializeFromXml<T>(string value)
        {
            T ris;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(value))
            {
                ris = (T)serializer.Deserialize(reader);
            }
            return ris;
        }
    }
}
