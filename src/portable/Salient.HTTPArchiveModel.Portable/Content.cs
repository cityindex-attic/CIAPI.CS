using System;
using System.Runtime.Serialization;
namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// This object describes details about response content (embedded in response object).
    /// </summary>
    [DataContract]
    public class Content
    {
        /// <summary>
        /// size [number]
        /// Length of the returned content in bytes. Should be equal to response.bodySize 
        /// if there is no compression and bigger when the content has been compressed.
        /// </summary>
        [DataMember(Name = "size")]
        public virtual int Size { get; set; }

        /// <summary>
        /// compression [number, optional]
        /// Number of bytes saved. Leave out this field if the information is not available.
        /// </summary>
        [DataMember(Name = "compression")]
        public virtual int Compression { get; set; }

        /// <summary>
        /// mimeType [string]
        /// MIME type of the response text (value of the Content-Type response header). 
        /// The charset attribute of the MIME type is included (if available).
        /// </summary>
        [DataMember(Name = "mimeType")]
        public virtual string MimeType { get; set; }

        /// <summary>
        /// text [string, optional]
        /// Response body sent from the server or loaded from the browser cache. 
        /// This field is populated with textual content only. The text field is 
        /// either HTTP decoded text or a encoded (e.g. "base64") representation of 
        /// the response body. Leave out this field if the information is not available.
        /// </summary>
        [DataMember(Name = "text")]
        public virtual string Text { get; set; }

        /// <summary>
        /// encoding [string, optional] (new in 1.2)
        /// Encoding used for response text field e.g "base64". Leave out this field 
        /// if the text field is HTTP decoded (decompressed & unchunked), than trans-coded 
        /// from its original character set into UTF-8.
        /// </summary>
        [DataMember(Name = "encoding")]
        public virtual string Encoding { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        [DataMember(Name = "comment")]
        public virtual string Comment { get; set; }
    }
}