using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenLevel.Wordpress.XmlRpc
{
    interface IWpXmlRpc
    {
        [XmlRpcMethod("wp.newPost")]
        void NewPost(int blog_id, string username, string password, Request.Content content);
    }
}
