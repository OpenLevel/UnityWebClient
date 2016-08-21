using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace OpenLevel
{
    public class SimpleCookie : MonoBehaviour
    {
        UnityWebClient _webClient;

        // Use this for initialization
        void Start()
        {
            _webClient = gameObject.AddComponent<UnityWebClient>();
            
            _webClient.Get("http://httpbin.org/cookies/set/testCookie/test", HandleCookie);
            
        }
        
        void HandleCookie(UnityWebRequest request)
        {
            Debug.Log(request.url);
            Debug.Log(request.downloadHandler.text);
            Debug.Log(_webClient.Cookies[request.url]);
            _webClient.Get("http://httpbin.org/cookies/set/testCookie/test2", HandleCookie2);
        }

        void HandleCookie2(UnityWebRequest request)
        {
            Debug.Log(request.url);
            Debug.Log(request.downloadHandler.text);
            Debug.Log(_webClient.Cookies[request.url]);
        }
    }
}