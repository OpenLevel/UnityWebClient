using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace OpenLevel.Wordpress.XmlRpc
{
    class XmlRpcReader
    {
        public static object ReadValue(XmlNode node)
        {
            var valueNode = node.SelectSingleNode("value");
            var typeNode = valueNode.FirstChild;

            switch (typeNode.Name)
            {
                case "array":
                    break;
                case "boolean":
                    return int.Parse(typeNode.InnerText) != 0;
                case "dateTime":
                case "dateTime.iso8601":
                    DateTime dateTime;
                    if (DateTime.TryParse(typeNode.InnerText, out dateTime))
                        return dateTime;
                    return DateTime.ParseExact(typeNode.InnerText, @"yyyyMMddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                case "double":
                    return double.Parse(typeNode.InnerText);
                case "i4":
                case "integer":
                    return int.Parse(typeNode.InnerText);
                case "base64":
                case "string":
                    return typeNode.InnerText;
            }

            return null;
        }

        public static T ReadStruct<T>(XmlNode node) where T : class, new()
        {
            T @struct = new T();
            var attribute = Attribute.GetCustomAttribute(
                typeof(T),
                typeof(XmlRpcStructAttribute))
                as XmlRpcStructAttribute
                ?? new XmlRpcStructAttribute(typeof(T));

            var fields = typeof(T).GetFields(attribute.Flags);

            Dictionary<string, Type> nameTypeDictionary = new Dictionary<string, Type>();

            foreach (var field in fields)
                nameTypeDictionary.Add(field.Name, field.FieldType);

            Dictionary<string, object> memberDictionary = new Dictionary<string, object>();
            foreach (XmlNode memberNode in node.SelectNodes("value/struct/member"))
            {
                string name = memberNode["name"].InnerText.Replace('-', '_');
                if(nameTypeDictionary.ContainsKey(name))
                {
                    Type type = nameTypeDictionary[name];

                    if (type.IsArray)
                    {
                        Type elementType = type.GetElementType();
                        var attr = Attribute.GetCustomAttribute(elementType, typeof(XmlRpcStructAttribute)) as XmlRpcStructAttribute;

                        if (attr != null)
                        {
                            memberDictionary.Add(name,
                                typeof(XmlRpcReader)
                                .GetMethod("ReadStructArray")
                                .MakeGenericMethod(elementType)
                                .Invoke(null, new object[] { memberNode })
                                );
                        }
                    }
                    else
                    {
                        var attr = Attribute.GetCustomAttribute(type, typeof(XmlRpcStructAttribute)) as XmlRpcStructAttribute;

                        if (attr != null)
                        {
                            memberDictionary.Add(name,
                                typeof(XmlRpcReader)
                                .GetMethod("ReadStruct")
                                .MakeGenericMethod(type)
                                .Invoke(null, new object[] { memberNode })
                                );
                        }
                        else
                        {
                            memberDictionary.Add(name, ReadValue(memberNode));
                        }
                    }
                }
            }

            foreach (var field in fields)
            {
                if (memberDictionary.ContainsKey(field.Name))
                    field.SetValue(@struct, memberDictionary[field.Name]);
            }

            return @struct;
        }

        public static T[] ReadStructArray<T>(XmlNode node) where T : class, new()
        {
            
            List<T> list = new List<T>();
            
            foreach (XmlNode valueNode in node.SelectNodes("value/array/data"))
                list.Add(ReadStruct<T>(valueNode));

            return list.ToArray<T>();
        }

        public static ICollection ReadArray(XmlNode node)
        {
            ArrayList list = new ArrayList();

            foreach (XmlNode valueNode in node.SelectNodes("value/array/data"))
                list.Add(ReadValue(valueNode));

            return list;
        }
    }
}
