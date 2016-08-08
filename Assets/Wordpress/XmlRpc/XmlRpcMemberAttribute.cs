using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace OpenLevel.Wordpress.XmlRpc
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class XmlRpcMemberAttribute : Attribute
    {
        string _name;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public bool Implict
        {
            get
            {
                return _name == null;
            }
        }

        public XmlRpcMemberAttribute()
        {
        }

        public XmlRpcMemberAttribute(string name)
        {
        }
    }
}