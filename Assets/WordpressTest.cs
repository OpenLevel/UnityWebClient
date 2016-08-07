using UnityEngine;
using System.Collections;
using OpenLevel;
using OpenLevel.Wordpress;
using LitJson;
using UnityEngine.Networking;

/*
*-- Auth Response JSON --*
{
"status": "ok",
"cookie": "test|1471516227|9ijdw9OyGxeewmJ969mMEQAZqCoIwHzkJIDpht4vkfG|3f1466d7944bcb4ffd6fcd10bb0eca2d4a170f741f6f6f3debc23e39d04d7394",
"cookie_name": "wordpress_logged_in_5c016e8f0f95f039102cbe8366c5c7f3",
"user": {
"id": 2,
"username": "test",
"nicename": "test",
"email": "test@test.com",
"url": "http:\/\/test.com",
"registered": "2016-08-04 10:26:51",
"displayname": "test test",
"firstname": "test",
"lastname": "test",
"nickname": "test",
"description": "",
"capabilities": {
"subscriber": true
},
"avatar": null
}
}
*/

public class WpJsonTest : MonoBehaviour
{
    WpJsonClient client;
    
    void Start()
    {
        client.Info(InfoHandler);
    }

    void InfoHandler(UnityWebRequest www)
    {
        JsonData json = JsonMapper.ToObject(www.downloadHandler.text);

        if (json.Keys.Contains("status") && json["status"].Equals("ok"))
        {
            client.Auth("openlevel", "open12#$", AuthHandler);
        }
    }

    void AuthHandler(UnityWebRequest www)
    {
        JsonData json = JsonMapper.ToObject(www.downloadHandler.text);

        if (json.Keys.Contains("status") &&
            json["status"].Equals("ok") &&
            json.Keys.Contains("cookie_name") &&
            json.Keys.Contains("cookie"))
        {
            client.Cookies.Set(www.url, json["cookie_name"].ToString(), json["cookie"].ToString());
            client.CreatePost("UNITY TEST TITLE", "UNITY TEST CONTENT", null);
        }
    }
}
