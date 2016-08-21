using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

namespace OpenLevel
{
    public class SimpleCookie : MonoBehaviour
    {
        UnityWebClient _webClient;

        // Use this for initialization
        void Start()
        {
            _webClient = gameObject.AddComponent<UnityWebClient>();
            
            _webClient.Get("http://httpbin.org/cookies/set/testCookie/test",
                        (Action<UnityWebRequest>) PrintCookie +
                        ((UnityWebRequest request) =>
                        {
                            if (!request.isError)
                                _webClient.Get("http://httpbin.org/cookies/set/testCookie/test2", PrintCookie);
                        })
                        );
            
        }
        
        void PrintCookie(UnityWebRequest request)
        {
            if (request.isError)
                Debug.LogWarning(request.error);
            else
            {
                Debug.Log("Cookie : " + _webClient.Cookies[request.url]);
            }
        }
    }
}