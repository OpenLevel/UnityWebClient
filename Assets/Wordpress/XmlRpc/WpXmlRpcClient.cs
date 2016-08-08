using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Xml;

namespace OpenLevel.Wordpress.XmlRpc
{
    using KeyValue = KeyValuePair<string, object>;

    public class WpXmlRpcClient : UnityWebClient
    {
        public string xmlRpcUri = "http://localhost/xmlrpc.php";
        protected string _username;
        protected string _password;

        public bool IsAuthed
        {
            get;
            protected set;
        }

        public int BlogId
        {
            get;
            set;
        }

        #region protected Methods

        #region Posts
        protected void XmlRpcPost(string xmlString, Action<XmlNode> onSuccess, Action<Models.Fault> onFailure)
        {
            Post(xmlRpcUri, xmlString, www =>
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(www.downloadHandler.text);

                var faultNode = xml.SelectSingleNode("methodResponse/fault");

                if (faultNode == null)
                {
                    var paramNode = xml.SelectSingleNode("methodResponse/params/param");

                    if (onSuccess != null)
                    {
                        if (paramNode != null)
                            onSuccess(paramNode);
                        else
                            onFailure(new Models.Fault(2, "params/param node is null."));
                    }
                    else
                    {
                        onFailure(new Models.Fault(1, "onSuccess handler is null."));
                    }
                }
                else if (onFailure != null)
                {
                    onFailure(XmlRpcReader.ReadStruct<Models.Fault>(faultNode));
                }
            });
        }

        protected void GetPost(int blogId,
                            string username,
                            string password,
                            int postId,
                            Action<Models.Post> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));
            data.Add(new KeyValue("post_id", postId));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.getPost", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadStruct<Models.Post>(node));
                },
                onFailure);
        }

        protected void GetPosts(int blogId,
                            string username,
                            string password,
                            int postId,
                            Models.GetPostsFilter filter,
                            Action<Models.Post[]> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));
            data.Add(new KeyValue("post_id", postId));
            data.Add(new KeyValue("filter", filter));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.getPosts", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadStructArray<Models.Post>(node));
                },
                onFailure);
        }

        protected void GetPosts(int blogId,
                            string username,
                            string password,
                            int postId,
                            Action<Models.Post[]> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            GetPosts(blogId, username, password, postId, null, onSuccess, onFailure);
        }

        protected void NewPost(int blogId,
                            string username,
                            string password,
                            Models.PostContent content,
                            Action<string> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));
            data.Add(new KeyValue("content", content));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.newPost", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadValue(node).ToString());
                },
                onFailure);
        }

        protected void EditPost(int blogId,
                            string username,
                            string password,
                            int postId,
                            Models.PostContent content,
                            Action<bool> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));
            data.Add(new KeyValue("post_id", postId));
            data.Add(new KeyValue("content", content));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.editPost", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadValue(node) as bool? ?? false);
                },
                onFailure);
        }

        protected void DeletePost(int blogId,
                            string username,
                            string password,
                            int postId,
                            Action<bool> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));
            data.Add(new KeyValue("post_id", postId));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.deletePost", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadValue(node) as bool? ?? false);
                },
                onFailure);
        }

        protected void GetPostType(int blogId,
                            string username,
                            string password,
                            string postTypeName,
                            Action<Models.PostType> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));
            data.Add(new KeyValue("post_type_name", postTypeName));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.getPostType", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadStruct<Models.PostType>(node));
                },
                onFailure);
        }

        // TODO : FILTER
        protected void GetPostTypes(int blogId,
                            string username,
                            string password,
                            Action<Models.PostType[]> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.getPostTypes", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadStructArray<Models.PostType>(node));
                },
                onFailure);
        }
        #endregion Posts

        #region Taxonomies
        protected void GetTaxonomy(int blogId,
                            string username,
                            string password,
                            string taxonomy,
                            Action<Models.Taxonomy> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));
            data.Add(new KeyValue("taxonomy", taxonomy));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.getTaxonomy", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadStruct<Models.Taxonomy>(node));
                },
                onFailure);
        }

        protected void GetTaxonomies(int blogId,
                            string username,
                            string password,
                            Action<Models.Taxonomy[]> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.getTaxonomies", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadStructArray<Models.Taxonomy>(node));
                },
                onFailure);
        }

        protected void GetTerm(int blogId,
                           string username,
                           string password,
                           string taxonomy,
                           int termId,
                           Action<Models.Term> onSuccess,
                           Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));
            data.Add(new KeyValue("taxonomy", taxonomy));
            data.Add(new KeyValue("term_id", termId));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.getTerm", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadStruct<Models.Term>(node));
                },
                onFailure);
        }

        protected void GetTerms(int blogId,
                           string username,
                           string password,
                           string taxonomy,
                           Models.GetTermsFilter filter,
                           Action<Models.Term[]> onSuccess,
                           Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));
            data.Add(new KeyValue("taxonomy", taxonomy));
            data.Add(new KeyValue("filter", filter));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.getTerms", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadStructArray<Models.Term>(node));
                },
                onFailure);
        }

        protected void GetTerms(int blogId,
                           string username,
                           string password,
                           string taxonomy,
                           Action<Models.Term[]> onSuccess,
                           Action<Models.Fault> onFailure)
        {
            GetTerms(blogId, username, password, taxonomy, null, onSuccess, onFailure);
        }

        protected void NewTerm(int blogId,
                           string username,
                           string password,
                           Models.TermContent content,
                           Action<string> onSuccess,
                           Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));
            data.Add(new KeyValue("content", content));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.newTerm", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadValue(node).ToString());
                },
                onFailure);
        }

        protected void EditTerm(int blogId,
                           string username,
                           string password,
                           int termId,
                           Models.TermContent content,
                           Action<bool> onSuccess,
                           Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));
            data.Add(new KeyValue("term_id", termId));
            data.Add(new KeyValue("content", content));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.editTerm", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadValue(node) as bool? ?? false);
                },
                onFailure);
        }

        protected void DeleteTerm(int blogId,
                           string username,
                           string password,
                           string taxonomy,
                           int termId,
                           Action<bool> onSuccess,
                           Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();

            data.Add(new KeyValue("blog_id", blogId));
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));
            data.Add(new KeyValue("taxonomy", taxonomy));
            data.Add(new KeyValue("term_id", termId));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.deleteTerm", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadValue(node) as bool? ?? false);
                },
                onFailure);
        }
        #endregion Taxonomies

        protected void GetUsersBlogs(string username, string password, Action<Models.UsersBlog[]> onSuccess, Action<Models.Fault> onFailure)
        {
            var data = new List<KeyValue>();
            
            data.Add(new KeyValue("username", username));
            data.Add(new KeyValue("password", password));

            XmlRpcPost(XmlRpcWriter.BuildRequest("wp.getUsersBlogs", data),
                node =>
                {
                    onSuccess(XmlRpcReader.ReadStructArray<Models.UsersBlog>(node));
                },
                onFailure);
        }

        #endregion protected Methods

        #region Public Methods
        public void Auth(string username, string password, Action onSuccess, Action<Models.Fault> onFailure)
        {
            GetUsersBlogs(username,
                password,
                blog =>
                {
                    _username = username;
                    _password = password;
                    IsAuthed = true;
                    if (onSuccess != null) onSuccess();
                },
                onFailure);
        }

        public void GetUsersBlogs(Action<Models.UsersBlog[]> onSuccess, Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            GetUsersBlogs(_username, _password, onSuccess, onFailure);
        }

        public void GetPost(int blogId,
                            int postId,
                            Action<Models.Post> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            GetPost(blogId, _username, _password, postId, onSuccess, onFailure);
        }

        public void GetPosts(int postId,
                            Models.GetPostsFilter filter,
                            Action<Models.Post[]> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            GetPosts(BlogId, _username, _password, postId, filter, onSuccess, onFailure);
        }

        public void GetPosts(int postId,
                            Action<Models.Post[]> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            GetPosts(BlogId, _username, _password, postId, null, onSuccess, onFailure);
        }

        public void NewPost(Models.PostContent content,
                            Action<string> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            NewPost(BlogId, _username, _password, content, onSuccess, onFailure);
        }

        public void EditPost(int postId,
                            Models.PostContent content,
                            Action<bool> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            EditPost(BlogId, _username, _password, postId, content, onSuccess, onFailure);
        }

        public void DeletePost(int postId,
                            Action<bool> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            DeletePost(BlogId, _username, _password, postId, onSuccess, onFailure);
        }

        public void GetPostType(string postTypeName,
                            Action<Models.PostType> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            GetPostType(BlogId, _username, _password, postTypeName, onSuccess, onFailure);
        }

        // TODO : FILTER
        public void GetPostTypes(Action<Models.PostType[]> onSuccess, Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            GetPostTypes(BlogId, _username, _password, onSuccess, onFailure);
        }

        public void GetTaxonomy(string taxonomy,
                            Action<Models.Taxonomy> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            GetTaxonomy(BlogId, _username, _password, taxonomy, onSuccess, onFailure);
        }

        public void GetTaxonomies(Action<Models.Taxonomy[]> onSuccess, Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            GetTaxonomies(BlogId, _username, _password, onSuccess, onFailure);
        }

        public void GetTerm(string taxonomy,
                            int termId,
                            Action<Models.Term> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            GetTerm(BlogId, _username, _password, taxonomy, termId, onSuccess, onFailure);
        }

        public void GetTerms(string taxonomy,
                            Action<Models.Term[]> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            GetTerms(BlogId, _username, _password, taxonomy, onSuccess, onFailure);
        }

        public void GetTerms(string taxonomy,
                            Models.GetTermsFilter filter,
                            Action<Models.Term[]> onSuccess,
                            Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            GetTerms(BlogId, _username, _password, taxonomy, filter, onSuccess, onFailure);
        }

        public void NewTerm(Models.TermContent content,
                           Action<string> onSuccess,
                           Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            NewTerm(BlogId, _username, _password, content, onSuccess, onFailure);
        }

        public void EditTerm(int termId,
                           Models.TermContent content,
                           Action<bool> onSuccess,
                           Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            EditTerm(BlogId, _username, _password, termId, content, onSuccess, onFailure);
        }

        protected void DeleteTerm(string taxonomy,
                           int termId,
                           Action<bool> onSuccess,
                           Action<Models.Fault> onFailure)
        {
            if (!IsAuthed) throw new InvalidCredentialException();
            DeleteTerm(BlogId, _username, _password, taxonomy, termId, onSuccess, onFailure);
        }
        #endregion Public Methods
    }
}