using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace OpenLevel
{
    public class SimpleGetSingleton : MonoBehaviour
    {
        public string uri = "https://raw.githubusercontent.com/OpenLevel/UnityWebClient/master/README.md";
        UnityWebClient _webClient;

        // Use this for initialization
        void Start()
        {
            _webClient = UnityWebClientSingleton.Instance;
            _webClient.Get(uri, HandleResponse);
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