using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace OpenLevel.Wordpress.XmlRpc.Request
{
    public struct Content
    {
        string post_type;
        string post_status;
        string post_title;
        int post_author;
        string post_excerpt;
        string post_content;
        //datetime post_date_gmt | post_date;
        string post_format;
        string post_name; //: Encoded URL(slug)
        string post_password;
        string comment_status;
        string ping_status;
        int sticky;
        int post_thumbnail;
        int post_parent;
    }
}

namespace OpenLevel.Wordpress
{
    public class WpXmlRpcClient : UnityWebClient, IWordpressClient
    {
        public string xmlRpcUri = "http://localhost/xmlrpc.php";
        string _username;
        string _password;

        string buildXMLRPCRequest(Hashtable FieldArray, string MethodName)
        {
            StringBuilder builder = new StringBuilder();

            // 당장은 String.Format해도 되지만 나중에 추가할 게 더 있으니 StringBuilder로 작업
            builder.AppendFormat(
@"<?xml version=""1.0"" encoding=""iso-8859-1""?>
<simpleRPC version=""0.9"">
<methodCall>
<methodName>{0}</methodName>
<vector type=""struct"">
{1}
</vector>
</methodCall>
</simpleRPC>",
                                MethodName,
                                buildNode(FieldArray));

            return builder.ToString();
        }

        string buildNode(Hashtable FieldArray)
        {
            string ReturnList = "";

            foreach (DictionaryEntry Item in FieldArray)
            {

                string TypeName = "int";
                string NodeType = "scalar";

                Type myType = Item.Value.GetType();
                string fieldValue = "";

                if (myType == typeof(string))
                {
                    TypeName = "string";
                    fieldValue = Item.Value.ToString();
                }

                if (myType == typeof(Hashtable))
                {
                    fieldValue = buildNode(Item.Value as Hashtable);
                    NodeType = "vector";
                    TypeName = "struct";
                }

                if (myType == typeof(int))
                {
                    fieldValue = Item.Value.ToString();
                    TypeName = "int";
                }

                var ThisNode = "\n<" + NodeType + " type=\"" + TypeName + "\" id=\"" + Item.Key + "\">" + fieldValue + "</" + NodeType + ">";
                ReturnList += ThisNode;
            }

            return ReturnList;
        }

        public void Auth(string username, string password, Action<Response.Auth> handler)
        {
            _username = username;
            _password = password;
        }

        //[XmlRpcMethod("wp.netPost")]
        public void NewPost(string username, string password, string content, bool publish)
        {
            Hashtable data = new Hashtable();
            data.Add("username", username);
            data.Add("password", password);
            data.Add("content", content);
            data.Add("publish", publish);

            Post(xmlRpcUri, buildXMLRPCRequest(data, "wp.newPost"), www =>
            {
                Debug.Log(www.downloadHandler.text);
            });
        }
    }
}