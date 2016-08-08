using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace OpenLevel.Wordpress.XmlRpc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class XmlRpcArrayAttribute : Attribute
    {
        Type _type;
        BindingFlags _flags;
        bool _implicitMember;

        public XmlRpcArrayAttribute(Type type,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance,
            bool implicitMember = true)
        {
            _type = type;
            _flags = bindingFlags;
            _implicitMember = implicitMember;
        }

        public IEnumerable<KeyValuePair<string, object>> ToCollection(object @struct)
        {
            var fields = _type.GetFields(_flags);
            KeyValuePair<string, object>[] list = new KeyValuePair<string, object>[fields.Length];

            for (int i = 0; i < list.Length; i++)
            {
                var member = GetCustomAttribute(fields[i], typeof(XmlRpcMemberAttribute)) as XmlRpcMemberAttribute;

                if (member != null)
                    list[i] = new KeyValuePair<string, object>(member.Name, fields[i].GetValue(@struct));
                else if (_implicitMember)
                    list[i] = new KeyValuePair<string, object>(fields[i].Name, fields[i].GetValue(@struct));
            }

            return list;
        }
    }
}