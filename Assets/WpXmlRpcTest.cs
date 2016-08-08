using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using OpenLevel.Wordpress.XmlRpc;
using System.Collections;

class WpXmlRpcTest : MonoBehaviour
{
    WpXmlRpcClient client;

    void Start()
    {
        client = gameObject.AddComponent<WpXmlRpcClient>();
        client.Auth("openlevel", "open12#$", () =>
        {
            client.GetUsersBlogs(blogs =>
            {
                client.GetPost(int.Parse(blogs[0].blogid), 1, post =>
                {
                    Debug.Log(post.post_title);
                }
                , null);
            }
            , null);
        }, null);
    }
}