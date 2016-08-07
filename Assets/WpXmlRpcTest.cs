using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using OpenLevel.Wordpress;

class WpXmlRpcTest : MonoBehaviour
{
    WpXmlRpcClient client;

    void Start()
    {
        client = gameObject.AddComponent<WpXmlRpcClient>();
        client.NewPost("openlevel", "open12#$", "UNITY TEST CONTENT", false);
    }
}