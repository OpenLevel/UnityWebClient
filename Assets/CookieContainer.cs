using System;
using System.Collections.Generic;

namespace OpenLevel
{
    /// <summary>
    /// The CookieContainer class provides methods to store, retrieve, and manage multiple cookies by doamin.
    /// </summary>
    public class CookieContainer
    {
        Dictionary<string, Cookie> _cookies = new Dictionary<string, Cookie>();

        /// <summary>
        /// Removes all of the cookies.
        /// </summary>
        public void Clear()
        {
            _cookies.Clear();
        }

        /// <summary>
        /// Determines whether the container contains cookie of the specified uri's domain.
        /// </summary>
        public bool Contains(Uri uri)
        {
            return _cookies.ContainsKey(uri.Host);
        }

        /// <summary>
        /// Determines whether the container contains cookie of the specified uri's domain.
        /// </summary>
        public bool Contains(string uri)
        {
            return Contains(new Uri(uri));
        }

        /// <summary>
        /// Gets the cookie of the specified uri's domain.
        /// </summary>
        public Cookie Get(Uri uri)
        {
            return _cookies[uri.Host];
        }

        /// <summary>
        /// Gets the cookie of the specified uri's domain.
        /// </summary>
        public Cookie Get(string uri)
        {
            return Get(new Uri(uri));
        }

        /// <summary>
        /// Sets the cookie of the specified uri's domain.
        /// </summary>
        /// <param name="clearExisting">if true, removes the existing cookie</param>
        public void Set(Uri uri, Cookie cookie, bool clearExisting = false)
        {
            string host = uri.Host;
            if (_cookies.ContainsKey(host))
                _cookies[host].Set(cookie, clearExisting);
            else
                _cookies.Add(host, cookie);
        }

        /// <summary>
        /// Sets the cookie of the specified uri's domain.
        /// </summary>
        /// <param name="clearExisting">if true, removes the existing cookie</param>
        public void Set(string uri, Cookie cookie, bool clearExisting = false)
        {
            Set(uri, cookie, clearExisting);
        }

        /// <summary>
        /// Sets the cookie of the specified uri's domain.
        /// </summary>
        /// <param name="clearExisting">if true, removes the existing cookie</param>
        public void Set(Uri uri, string cookieString, bool clearExisting = false)
        {
            string host = uri.Host;
            if (_cookies.ContainsKey(host))
                _cookies[host].Set(cookieString, clearExisting);
            else
                _cookies.Add(host, new Cookie(cookieString));
        }

        /// <summary>
        /// Sets the cookie of the specified uri's domain.
        /// </summary>
        /// <param name="clearExisting">if true, removes the existing cookie</param>
        public void Set(string uri, string cookieString, bool clearExisting = false)
        {
            Set(new Uri(uri), cookieString, clearExisting);
        }

        /// <summary>
        /// Sets the cookie of the specified uri's domain.
        /// </summary>
        /// <param name="clearExisting">if true, removes all the existing cookie</param>
        public void Set(Uri uri, string name, string value, bool clearExisting = false)
        {
            string host = uri.Host;
            if (_cookies.ContainsKey(host))
                _cookies[host].Set(name, value);
            else
            {
                Cookie cookie = new Cookie();
                cookie.Set(name, value);
                _cookies.Add(host, cookie);
            }
        }

        /// <summary>
        /// Sets the cookie of the specified uri's domain.
        /// </summary>
        /// <param name="clearExisting">if true, removes all the existing cookie</param>
        public void Set(string uri, string name, string value, bool clearExisting = false)
        {
            Set(new Uri(uri), name, value, clearExisting);
        }

        /// <summary>
        /// Gets the cookie of the specified uri's domain.
        /// </summary>
        public Cookie this[Uri uri]
        {
            get
            {
                return Get(uri);
            }
            set
            {
                Set(uri, value);
            }
        }

        /// <summary>
        /// Gets the cookie of the specified uri's domain.
        /// </summary>
        public Cookie this[string uri]
        {
            get
            {
                return Get(uri);
            }
            set
            {
                Set(uri, value);
            }
        }
    }
}
