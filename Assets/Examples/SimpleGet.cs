using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace OpenLevel
{
    public class SimpleGet : MonoBehaviour
    {
        public string uri = "https://raw.githubusercontent.com/OpenLevel/UnityWebClient/master/README.md";
        UnityWebClient _webClient;

        // Use this for initialization
        void Start()
        {
            _webClient = gameObject.AddComponent<UnityWebClient>();
            _webClient.Get(uri, HandleResponse);
        }

        void HandleResponse(UnityWebRequest request)
        {
            Debug.Log(request.downloadHandler.text);
        }
    }
}