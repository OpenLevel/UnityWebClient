using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenLevel.Wordpress.XmlRpc
{
    class XmlRpcWriter
    {
        public static string BuildRequest(string methodName, IEnumerable<KeyValuePair<string, object>> @params)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat(@"<?xml version=""1.0"" encoding=""UTF-8""?><methodCall><methodName>{0}</methodName><params>",
                                methodName);
            BuildNode(builder, @params);
            builder.Append(@"</params></methodCall>");

            return builder.ToString();
        }

        static void BuildNode(StringBuilder builder, IEnumerable<KeyValuePair<string, object>> @params, bool isStruct = false)
        {
            foreach (var kv in @params)
            {
                if (kv.Value == null && isStruct)
                    continue;

                Type type = kv.Value.GetType();
                string typeName = "int";
                string value = "";

                var structAttribute = Attribute.GetCustomAttribute(type, typeof(XmlRpcStructAttribute)) as XmlRpcStructAttribute;
                var arrayAttribute = Attribute.GetCustomAttribute(type, typeof(XmlRpcArrayAttribute)) as XmlRpcArrayAttribute;

                if (structAttribute != null)
                {
                    builder.AppendFormat("<member><name>{0}</name><value><struct>", kv.Key);
                    BuildNode(builder, structAttribute.ToCollection(kv.Value), true);
                    builder.Append("</struct></value></member>");
                }
                else if (arrayAttribute != null)
                {
                    builder.Append("<array><data>");
                    BuildNode(builder, structAttribute.ToCollection(kv.Value), false);
                    builder.Append("</data></array>");
                }
                else if (type == typeof(IDictionary<string, object>))
                {
                    IDictionary<string, object> dictionary = (IDictionary<string, object>)kv.Value;

                    foreach (var item in dictionary)
                    {
                        builder.AppendFormat("<struct><member><name>{0}</name>{1}</member></struct>", item.Key, item.Value);
                    }
                }
                else
                {
                    if (type == typeof(string))
                    {
                        typeName = "string";
                        value = kv.Value.ToString();
                    }
                    else if (type == typeof(int))
                    {
                        typeName = "int";
                        value = kv.Value.ToString();
                    }
                    else if (type == typeof(DateTime))
                    {
                        DateTime dateTime = (DateTime)kv.Value;
                        typeName = "dateTime.iso8601";
                        value = dateTime.ToString("s");
                    }

                    builder.AppendFormat("<value><{0}>{1}</{0}></value>", typeName, value);
                }
            }
        }
    }
}
