using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace OpenLevel
{
    public class SimplePost : MonoBehaviour
    {
        public string uri = "http://httpbin.org/post"; // Echo server
        UnityWebClient _webClient;
        public string postName = "test";
        public string postValue = "testValue";
        [SerializeField]
        Dictionary<string, string> test = new Dictionary<string, string>();

        void Start()
        {
            _webClient = gameObject.AddComponent<UnityWebClient>();

            var postData = new Dictionary<string, string>();
            postData[postName] = postValue;

            _webClient.Post(uri, postData, HandleResponse);
        }

        void HandleResponse(UnityWebRequest request)
        {
            if (request.isError)
                Debug.LogWarning(request.error);
            else
                Debug.Log(request.downloadHandler.text);
        }
    }
}