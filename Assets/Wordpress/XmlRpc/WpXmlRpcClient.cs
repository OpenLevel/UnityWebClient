using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace OpenLevel.Wordpress.XmlRpc.Request
{
    
}

namespace OpenLevel.Wordpress.XmlRpc
{
    public class WpXmlRpcClient : UnityWebClient, IWpXmlRpc
    {
        public string xmlRpcUri = "http://localhost/xmlrpc.php";
        string _username;
        string _password;

        string buildXMLRPCRequest(IEnumerable<KeyValuePair<string, object>> FieldArray, string MethodName)
        {
            StringBuilder builder = new StringBuilder();

            // 당장은 String.Format해도 되지만 나중에 추가할 게 더 있으니 StringBuilder로 작업
            builder.AppendFormat(
@"<?xml version=""1.0"" encoding=""iso-8859-1""?>
<methodCall>
<methodName>{0}</methodName>
<params>
{1}
</params>
</methodCall>",
                                MethodName,
                                buildNode(FieldArray));

            return builder.ToString();
        }

        string buildNode(IEnumerable<KeyValuePair<string, object>> FieldArray, bool isStruct = false)
        {
            string ReturnList = "";

            foreach (var Item in FieldArray)
            {
                if (Item.Value == null && isStruct)
                    continue;

                string TypeName = "int";
                Type myType = Item.Value.GetType();
                string fieldValue = "";

                var structAttribute = Attribute.GetCustomAttribute(myType, typeof(XmlRpcStructAttribute)) as XmlRpcStructAttribute;

                if (structAttribute != null)
                {
                    fieldValue = buildNode(structAttribute.ToCollection(Item.Value), true);
                    TypeName = "struct";
                }
                else if (myType == typeof(string))
                {
                    TypeName = "string";
                    fieldValue = Item.Value.ToString();
                }
                else if (myType == typeof(int) || myType == typeof(int?))
                {
                    fieldValue = Item.Value.ToString();
                    TypeName = "int";
                }

                string ThisNode;

                if(isStruct)
                    ThisNode = String.Format("<member><name>{2}</name><value><{0}>{1}</{0}></value></member>", TypeName, fieldValue, Item.Key);
                else
                    ThisNode = String.Format("<value><{0}>{1}</{0}></value>", TypeName, fieldValue);
                ///var ThisNode = "\n<" + NodeType + " type=\"" + TypeName + "\" id=\"" + Item.Key + "\">" + fieldValue + "</" + NodeType + ">";
                ReturnList += ThisNode;
            }

            return ReturnList;
        }

        public void Auth(string username, string password, Action<Response.Auth> handler)
        {
            _username = username;
            _password = password;
        }
        
        public void NewPost(int blog_id, string username, string password, Request.Content content)
        {
            var data = new List<KeyValuePair<string, object>>();

            // The order of items is IMPORTANT.
            data.Add(new KeyValuePair<string, object>("blog_id", blog_id));
            data.Add(new KeyValuePair<string, object>("username", username));
            data.Add(new KeyValuePair<string, object>("password", password));
            data.Add(new KeyValuePair<string, object>("content", content));

            Debug.Log(buildXMLRPCRequest(data, "wp.newPost"));

            Post(xmlRpcUri, buildXMLRPCRequest(data, "wp.newPost"), www =>
            {
                Debug.Log(www.downloadHandler.text);
            });
        }
    }
}