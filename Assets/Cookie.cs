using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLevel
{
    // NOTE : RFC says cookie names are case sensitive.
    // TODO : Supports exipires, http only and secure option.
    /// <summary>
    /// The Cookie class gets and sets properties of cookies.
    /// </summary>
    public class Cookie : Dictionary<string, string>
    {
        public Cookie() : base() { }
        public Cookie(IDictionary<string, string> cookies) : base(cookies) { }
        public Cookie(string cookieString) : base()
        {
            Set(cookieString);
        }

        /// <summary>
        /// Gets the value of a cookie by name.
        /// </summary>
        public string Get(string name)
        {
            return this[name];
        }

        /// <summary>
        /// Sets the value of cookie by name. Creates and add new cookie if not exist.
        /// </summary>
        /// <param name="clearExisting">if true, removes all the existing cookie</param>
        public void Set(string name, string value, bool clearExisting = false)
        {
            if (ContainsKey(name))
                this[name] = value;
            else
                Add(name, value);
        }

        /// <summary>
        /// Sets multiple cookies from the dictionary.
        /// </summary>
        /// <param name="cookies">cookies to set</param>
        /// <param name="clearExisting">if true, removes all the existing cookie</param>
        public void Set(IDictionary<string, string> cookies, bool clearExisting = false)
        {
            if(clearExisting) Clear();
            foreach (var cookie in cookies)
                Set(cookie.Key, cookie.Value);
        }

        /// <summary>
        /// Sets multiple cookies from the cookie string.
        /// </summary>
        /// <param name="cookieString">cookie string to set</param>
        /// <param name="clearExisting">if true, removes all the existing cookie</param>
        public void Set(string cookieString, bool clearExisting = false)
        {
            Clear();
            if (String.IsNullOrEmpty(cookieString))
                return;

            string[] cookie_components = cookieString.Split(';');
            foreach (string kv in cookie_components)
            {
                int pos = kv.IndexOf('=');
                if (pos == -1)
                {
                    continue;
                }
                else
                {
                    string key = kv.Substring(0, pos);
                    string val = kv.Substring(pos + 1);

                    Set(key, val);
                }
            }
        }

        /// <summary>
        /// Resturns a cookie string represents the cookie data.
        /// </summary>
        public override string ToString()
        {
            // return String.Concat(this.Select(kv => String.Format("{0}={1};", kv.Key, kv.Value)).ToArray());
            StringBuilder builder = new StringBuilder();

            foreach(var kv in this)
                builder.AppendFormat("{0}={1};", kv.Key, kv.Value);

            return builder.ToString();
        }
    }
}
