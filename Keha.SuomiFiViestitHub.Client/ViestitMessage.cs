using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Keha.SuomiFiViestitHub.Client
{
    /// <summary>
    /// Message and its content to be sent to the given recipient.
    /// Files and Links can be left null, string fields must contain something.
    /// </summary>
    public class ViestitMessage
    {
        /// <summary>SocialSecurityNumber of the recipient</summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(11, MinimumLength = 11)]
        public string SocialSecurityNumber { get; set; }
        
        /// <summary>Unique Id of the message. No specific format is required, but it must be unique in the senders system e.g. "diaarinumero"</summary>
        [Required(AllowEmptyStrings = false)]
        public string Id { get; set; }

        /// <summary>Array of Base64 encoded files to be attached to the message. NOTE: Max size sum of all files is 3MB</summary>
        public List<ViestitMessageFile> Files { get; set; }
        
        /// <summary>Array of links to be shown along with the message</summary>
        public List<ViestitMessageLink> Links { get; set; }
        
        /// <summary>Id shown to the user, "Viestit -palvelussa näytettävä asiakirjan tunniste"</summary>
        [Required(AllowEmptyStrings = false)]
        public string MsgId { get; set; }
        
        /// <summary>Topic of the message</summary>
        [Required(AllowEmptyStrings = false)]
        public string Topic { get; set; }
        
        /// <summary>Name of the sending authoritative / organization</summary>
        [Required(AllowEmptyStrings = false)]
        public string SenderName { get; set; }
        
        /// <summary>Actual text contents of the message</summary>
        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }
    }
}
