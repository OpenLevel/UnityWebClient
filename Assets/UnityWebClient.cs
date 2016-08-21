using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#elif UNITY_5_2 || UNITY_5_3
using UnityEngine.Experimental.Networking;
#else
#error It requires Unity 5.2.0 or higher.
#endif

namespace OpenLevel
{
    public class UnityWebClient : MonoBehaviour
    {
        protected CookieContainer _cookies = new CookieContainer();

        public CookieContainer Cookies
        {
            get
            {
                return _cookies;
            }
        }

        /// <summary>
        /// Dispose request after each a request handler
        /// </summary>
        public bool AutoDisposeRequest
        {
            get;
            set;
        }

        IEnumerator CRequest(UnityWebRequest request, Action<UnityWebRequest> handler)
        {
            if (_cookies.Contains(request.url))
            {
                request.SetRequestHeader("cookie", _cookies[request.url].ToString());
            }

            // UnityWebRequest has some redirect problems. prevent auto redirect and do it manually.
            int redirectLimit = request.redirectLimit;
            int redirectCount = 0;
            request.redirectLimit = 0;

            while (redirectCount <= redirectLimit)
            {
                if (_cookies.Contains(request.url))
                    request.SetRequestHeader("cookie", _cookies[request.url].ToString());

                yield return request.Send();

                string cookieString = request.GetResponseHeader("set-cookie");

                if (!String.IsNullOrEmpty(cookieString))
                    _cookies.Set(request.url, cookieString);

                // 301, 302 Redirect
                if (request.responseCode == 301 ||
                    request.responseCode == 302)
                {
                    var newRequest = new UnityWebRequest(new Uri(request.url).GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped)
                                                    + request.GetResponseHeader("location"),
                                                    request.method,
                                                    request.downloadHandler,
                                                    request.uploadHandler);
                    // To recycle the handlers
                    request.disposeDownloadHandlerOnDispose = false;
                    request.disposeUploadHandlerOnDispose = false;
                    request.Dispose();
                    request = newRequest;

                    redirectCount++;
                }
                else
                {
                    break;
                }
            }

            if (handler != null)
                handler(request);

            if (AutoDisposeRequest)
                request.Dispose();
        }

        /// <summary>
        /// Sends a given UnityWebReqeust
        /// </summary>
        public void Request(UnityWebRequest request, Action<UnityWebRequest> handler)
        {
            StartCoroutine(CRequest(request, handler));
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
        public void Post(string uri, byte[] postData, Action<UnityWebRequest> handler)
        {
            UnityWebRequest www;

            if (postData == null || postData.Length == 0)
                www = UnityWebRequest.Get(uri);
            else
            {
                www = UnityWebRequest.Put(uri, postData); // HACK
                www.method = UnityWebRequest.kHttpVerbPOST;
            }

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
            {
                www = UnityWebRequest.Put(uri, postData); // HACK
                www.method = UnityWebRequest.kHttpVerbPOST;
            }

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