using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace OpenLevel.Wordpress.XmlRpc
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class XmlRpcStructAttribute : Attribute
    {
        Type _type;

        public XmlRpcStructAttribute(Type type)
        {
            _type = type;
        }

        public IEnumerable<KeyValuePair<string, object>> ToCollection(object @struct)
        {
            var fields = _type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            KeyValuePair<string, object>[] list = new KeyValuePair<string, object>[fields.Length];

            for(int i = 0; i < list.Length; i++)
                list[i] = new KeyValuePair<string, object>(fields[i].Name, fields[i].GetValue(@struct));

            return list;
        }
    }
}