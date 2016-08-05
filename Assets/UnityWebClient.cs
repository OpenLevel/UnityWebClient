using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenLevel
{
    public class UnityWebClient : MonoBehaviour
    {
        CookieContainer _cookies;

        public CookieContainer Cookies
        {
            get
            {
                return _cookies;
            }
        }

        void Awake()
        {
            _cookies = new CookieContainer();
        }

        /// <summary>
        /// Creates and sends a UnityWebReqeust configured for GET
        /// </summary>
        void Get(string uri, Action<UnityWebRequest> handler)
        {
            StartCoroutine(CPostOrGet(uri, null, handler));
        }

        /// <summary>
        /// Creates and sends a UnityWebReqeust configured for POST
        /// </summary>
        void Post(string uri, IDictionary<string, string> formFields, Action<UnityWebRequest> handler)
        {
            StartCoroutine(CPostOrGet(uri, formFields, handler));
        }
 
        IEnumerator CPostOrGet(string uri, IDictionary<string, string> formFields, Action<UnityWebRequest> handler)
        {
            UnityWebRequest www;

            if (formFields == null)
                www = UnityWebRequest.Get(uri);
            else
                www = UnityWebRequest.Post(uri, new Dictionary<string, string>(formFields));

            if (_cookies.Contains(www.url))
            {
                www.SetRequestHeader("cookie", _cookies[www.url].ToString());
                Debug.Log(_cookies[www.url].ToString());
            }

            yield return www.Send();
            
            string cookieString = www.GetResponseHeader("set-cookie");

            if(!String.IsNullOrEmpty(cookieString))
                _cookies.Set(www.url, www.GetResponseHeader("set-cookie"));

            if (handler != null)
                handler(www);
        }
    }
}