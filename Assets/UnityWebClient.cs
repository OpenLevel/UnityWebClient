using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenLevel
{
    public class UnityWebClient : MonoBehaviour
    {
        protected CookieContainer _cookies;

        public CookieContainer Cookies
        {
            get
            {
                return _cookies;
            }
        }

        protected virtual void Awake()
        {
            _cookies = new CookieContainer();
        }

        IEnumerator CRequest(UnityWebRequest www, Action<UnityWebRequest> handler)
        {
            if (_cookies.Contains(www.url))
            {
                www.SetRequestHeader("cookie", _cookies[www.url].ToString());
                Debug.Log(_cookies[www.url].ToString());
            }

            yield return www.Send();

            string cookieString = www.GetResponseHeader("set-cookie");

            if (!String.IsNullOrEmpty(cookieString))
                _cookies.Set(www.url, www.GetResponseHeader("set-cookie"));

            if (handler != null)
                handler(www);
        }

        /// <summary>
        /// Sends a given UnityWebReqeust
        /// </summary>
        public void Request(UnityWebRequest www, Action<UnityWebRequest> handler)
        {
            StartCoroutine(CRequest(www, handler));
        }

        /// <summary>
        /// Creates and sends a UnityWebReqeust configured for GET
        /// </summary>
        public void Get(string uri, Action<UnityWebRequest> handler)
        {
            StartCoroutine(CRequest(UnityWebRequest.Get(uri), handler));
        }

        /// <summary>
        /// Creates and sends a UnityWebReqeust configured for POST
        /// </summary>
        public void Post(string uri, IDictionary<string, string> formFields, Action<UnityWebRequest> handler)
        {
            UnityWebRequest www;

            if (formFields == null || formFields.Count == 0)
                www = UnityWebRequest.Get(uri);
            else
                www = UnityWebRequest.Post(uri, new Dictionary<string, string>(formFields));

            StartCoroutine(CRequest(www, handler));
        }

        /// <summary>
        /// Creates and sends a UnityWebReqeust configured for POST
        /// </summary>
        public void Post(string uri, string postData, Action<UnityWebRequest> handler)
        {
            UnityWebRequest www;

            if (postData == null || String.IsNullOrEmpty(postData))
                www = UnityWebRequest.Get(uri);
            else
                www = UnityWebRequest.Post(uri, postData);

            StartCoroutine(CRequest(www, handler));
        }

        /// <summary>
        /// Creates and sends a UnityWebReqeust configured for PUT
        /// </summary>
        public void Put(string uri, byte[] bodyData, Action<UnityWebRequest> handler)
        {
            UnityWebRequest www;

            if (bodyData == null || bodyData.Length == 0)
                www = UnityWebRequest.Get(uri);
            else
                www = UnityWebRequest.Put(uri, bodyData);

            StartCoroutine(CRequest(www, handler));
        }

        /// <summary>
        /// Creates and sends a UnityWebReqeust configured for PUT with UTF8
        /// </summary>
        public void Put(string uri, string bodyData, Action<UnityWebRequest> handler)
        {
            Put(uri, System.Text.Encoding.UTF8.GetBytes(bodyData), handler);
        }

        /// <summary>
        /// Creates and sends a UnityWebReqeust configured for PUT with given encoding
        /// </summary>
        public void Put(string uri, string bodyData, Action<UnityWebRequest> handler, System.Text.Encoding encoding)
        {
            Put(uri, encoding.GetBytes(bodyData), handler);
        }

        /// <summary>
        /// Creates and sends a UnityWebReqeust configured for DELETE
        /// </summary>
        public void Delete(string uri, Action<UnityWebRequest> handler)
        {
            StartCoroutine(CRequest(UnityWebRequest.Delete(uri), handler));
        }

        /// <summary>
        /// Creates and sends a UnityWebReqeust configured for HEAD
        /// </summary>
        public void Head(string uri, Action<UnityWebRequest> handler)
        {
            StartCoroutine(CRequest(UnityWebRequest.Head(uri), handler));
        }
    }
}