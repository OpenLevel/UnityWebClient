using System;
using System.Collections;
using System.Reflection;

namespace OpenLevel.Wordpress.XmlRpc.Request
{
    [XmlRpcStruct(typeof(Content))]
    public struct Content
    {
        public string post_type;
        public string post_status;
        public string post_title;
        int post_author;
        string post_excerpt;
        string post_content;
        DateTime post_date_gmt; //datetime post_date_gmt | post_date;
        string post_format;
        string post_name; //: Encoded URL(slug)
        string post_password;
        string comment_status;
        string ping_status;
        int? sticky;
        int? post_thumbnail;
        int? post_parent;
    }
}
