using System;

namespace OpenLevel.Wordpress.XmlRpc
{
    [AttributeUsage(AttributeTargets.Method)]
    public class XmlRpcMethodAttribute : Attribute
    {
        string _method = "";

        public string Method
        {
            get
            {
                return _method;
            }
        }

        // NOTE : 메소드 이름과 xmlrpc 메소드 이름이 같을 경우 생략 가능하게 할 필요는 있을듯 함.
        public XmlRpcMethodAttribute(string method)
        {
            _method = method;
        }
    }
}