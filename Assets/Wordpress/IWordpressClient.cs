using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenLevel.Wordpress.Response
{
    public struct Auth
    {

    }
}

namespace OpenLevel.Wordpress
{
    interface IWordpressClient
    {
        void Auth(string username, string password, Action<Response.Auth> handler);
    }
}
