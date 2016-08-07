using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using OpenLevel.Wordpress.XmlRpc;

class WpXmlRpcTest : MonoBehaviour
{
    WpXmlRpcClient client;

    void Start()
    {
        client = gameObject.AddComponent<WpXmlRpcClient>();
        var content = new OpenLevel.Wordpress.XmlRpc.Request.Content();
        content.post_title = "published";
        content.post_status = "published";
        content.post_type = "published";
        client.NewPost(0, "openlevel", "open12#$", content);
    }
}