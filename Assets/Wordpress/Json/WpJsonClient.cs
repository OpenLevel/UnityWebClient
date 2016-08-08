using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#elif UNITY_5_2 || UNITY_5_3
using UnityEngine.Experimental.Networking;
#else
#error It requires Unity 5.2.0 or higher.
#endif

namespace OpenLevel.Wordpress
{
    public class WpJsonClient : UnityWebClient
    {
        public string apiUri = "http://localhost/api/";

        //public void Auth(string username, string password, Action<Response.Auth> handler)
        //{

        //}

        public void Info(Action<UnityWebRequest> handler)
        {
            Get("info/", handler);
        }

        public void Auth(string username, string password, Action<UnityWebRequest> handler)
        {
            GetNonce("user", "generate_auth_cookie", (string nonce) =>
            {
                Dictionary<string, string> data = new Dictionary<string, string>()
                {
                    {"nonce", nonce},
                    {"username", WWW.EscapeURL(username)},
                    {"password", WWW.EscapeURL(password)},
                };

                Post("user/generate_auth_cookie/?insecure=cool", data, handler);
            });
        }

        public void CreatePost(string title, string content, Action<UnityWebRequest> handler)
        {
            title = WWW.EscapeURL(title);
            content = WWW.EscapeURL(content);

            GetNonce("posts", "create_post", (string nonce) =>
            {
                Get("posts/create_post/?nonce=" + nonce + "&title=" + title + "&content=" + content, handler);
            });
        }

        void GetNonce(string controller, string method, Action<string> handler)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                {"controller", controller},
                {"method", method},
            };

            Post("get_nonce/",
                data,
                (UnityWebRequest www) =>
                {
                    JsonData json = JsonMapper.ToObject(www.downloadHandler.text);
                    handler(json["nonce"].ToString());
                });
        }
    }
}