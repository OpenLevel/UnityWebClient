using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace OpenLevel.Wordpress.XmlRpc.Models
{
    [XmlRpcStruct(typeof(Fault))]
    public class Fault
    {
        public Fault()
        {
            faultCode = 0;
            faultString = "Not initialized";
        }
        public Fault(int code, string message)
        {
            faultCode = code;
            faultString = message;
        }
        public int faultCode;
        public string faultString;
    }

    [XmlRpcStruct(typeof(PostContent))]
    public struct PostContent
    {
        public string post_type;
        public string post_status;
        public string post_title;
        public int? post_author;
        public string post_excerpt;
        public string post_content;
        public DateTime? post_date;
        public DateTime? post_date_gmt;
        public string post_format;
        public string post_name; //: Encoded URL(slug)
        public string post_password;
        public string comment_status;
        public string ping_status;
        public int? sticky;
        public int? post_thumbnail;
        public int? post_parent;
        public Enclosure enclosure;
        public Dictionary<string, int> terms;
        public Dictionary<string, string> terms_names;
    }

    [XmlRpcStruct(typeof(Enclosure))]
    public class Enclosure
    {
        public string url;
        public int length;
        public string type;
    }

    [XmlRpcStruct(typeof(UsersBlog))]
    public class UsersBlog
    {
        public bool isAdmin;
        public string url;
        public string blogid;
        public string blogName;
    }

    [XmlRpcStruct(typeof(Post))]
    public class Post
    {
        public string post_id;
        public string post_title;
        public DateTime post_date;
        public DateTime post_date_gmt;
        public DateTime post_modified;
        public DateTime post_modified_gmt;
        public string post_status;
        public string post_type;
        public string post_format;
        public string post_name;
        public string post_author; // author id
        public string post_password;
        public string post_excerpt;
        public string post_content;
        public string post_parent;
        public string post_mime_type;
        public string link;
        public string guid;
        public int menu_order;
        public string comment_status;
        public string ping_status;
        public bool sticky;
        //struct post_tumbnail;
        public Term[] terms;
        public Enclosure enclosure;
    }

    [XmlRpcStruct(typeof(Term))]
    public class Term
    {
        /// <summary>
        /// Store for the name property
        /// </summary>
        public string term_id;
        public string name;
        public string slug;
        public string term_group;
        public string term_taxonomy_id;
        public string taxonomy;
        public string description;
        public string parent;
        public int count;
        public string filter;
    }

    [XmlRpcStruct(typeof(TermContent))]
    public class TermContent
    {
        public string name;
        public string taxonomy;
        public string slug; // Optional.
        public string description; // Optional.
        public int? parent; // Optional.
    }

    [XmlRpcStruct(typeof(MediaItem))]
    public class MediaItem
    {
        public string attachment_id;
        public DateTime date_created_gmt;
        /// <summary>
        /// ID of the parent post.
        /// </summary>
        public int parent;
        /// <summary>
        /// URL to the media item itself (the actual .jpg/.pdf/etc file, eg http://domain.tld/wp-content/uploads/2013/09/foo.jpg)
        /// </summary>
        public string link;
        public string title;
        public string caption;
        public string description;
        public MediaItemMetadata metadata;
        public PostThumbnailImageMeta image_meta;
        /// <summary>
        /// URL to the media item thumbnail (eg http://domain.tld/wp-content/uploads/2013/09/foo-150x150.jpg)
        /// </summary>
        public string thumbnail;
    }

    [XmlRpcStruct(typeof(MediaItemMetadata))]
    public class MediaItemMetadata
    {
        public int width;
        public int height;
        /// <summary>
        /// The filename, including path from the uploads directory (eg "2013/09/foo.jpg")
        /// </summary>
        public string file;
        public MediaItemSize[] sizes;
    }

    [XmlRpcStruct(typeof(MediaItemSize))]
    public class MediaItemSize
    {
        /// <summary>
        /// The filename of this version of the media item, at this size, without the path (eg. "foo-768x1024.jpg" or "foo-940x198.jpg")
        /// </summary>
        public string file;
        public string width;
        public string height;
        /// <summary>
        /// image/jpeg or ...
        /// </summary>
        public string mime_type;
    }

    [XmlRpcStruct(typeof(PostThumbnailImageMeta))]
    public class PostThumbnailImageMeta
    {
        public int aperture;
        public string credit;
        public string camera;
        public string caption;
        public int created_timestamp;
        public string copyright;
        public int focal_length;
        public int iso;
        public int shutter_speed;
        public string title;
    }

    [XmlRpcStruct(typeof(PostThumbnailImageMeta))]
    public class GetPostsFilter
    {
        public string post_type;
        public string post_status;
        public int? number;
        public int? offset;
        public string orderby;
        public string order;
    }

    [XmlRpcStruct(typeof(PostThumbnailImageMeta))]
    public class GetTermsFilter
    {
        public int? number;
        public int? offset;
        public string orderby;
        public string order;
        /// <summary>
        /// Whether to return terms with count=0.
        /// </summary>
        public bool? hide_empty;
        /// <summary>
        /// Restrict to terms with names that contain (case-insensitive) this value.
        /// </summary>
        public string search;
    }

    // TODO : meta-fields
    [XmlRpcStruct(typeof(PostThumbnailImageMeta))]
    public class PostType
    {
        public string name;
        public string label;
        public bool hierarchical;
        public bool @public;
        public bool show_ui;
        public bool _builtin;
        public bool has_archive;
        //struct supports : Features supported by the theme as keys, values always true. See post_type_supports.
        //struct labels1
        //struct cap2
        public bool map_meta_cap;
        public int menu_position;
        public string menu_icon;
        public bool show_in_menu;
        //array taxonomies;
    }

    // TODO : meta-fields
    [XmlRpcStruct(typeof(PostThumbnailImageMeta))]
    public class Taxonomy
    {
        public string name;
        public string label;
        public bool hierarchical;
        public bool @public;
        public bool show_ui;
        public bool _builtin;
        //struct labels1;
        //struct cap2;
        //array object_type3;
    }
}
