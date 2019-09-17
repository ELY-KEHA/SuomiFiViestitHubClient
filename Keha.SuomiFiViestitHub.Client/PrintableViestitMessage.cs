using System.ComponentModel.DataAnnotations;

namespace Keha.SuomiFiViestitHub.Client
{
    /// <summary>
    /// Message and its content to be sent to the given recipient.
    /// Single printable PDF must be provided containing all message info in correct form.
    /// NOTE: The printed content must contain properly placed recipient name on the first page.
    /// </summary>
    public class PrintableViestitMessage
    {
        /// <summary>SocialSecurityNumber of the recipient</summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(11, MinimumLength = 11)]
        public string SocialSecurityNumber { get; set; }

        /// <summary>Unique Id of the message. No specific format is required, but it must be unique in the senders system e.g. "diaarinumero"</summary>
        [Required(AllowEmptyStrings = false)]
        public string Id { get; set; }

        /// <summary>Base64 encoded file that will be printed. NOTE: Max size sum of all files is 3MB</summary>
        [Required]
        public ViestitMessageFile File { get; set; }

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

        /// <summary>Address info for recipient</summary>
        [Required]
        public AddressInformation Address { get; set; }

        /// <summary>Name of the printing provider</summary>
        [Required]
        public string PrintingProvider { get; set; }

        /// <summary>Force sending also a printed version even if the recipient has an active Suomi.fi account</summary>
        public bool SendAlsoAsPrinted { get; set; } = false;

        /// <summary>Use only when testing the API! Disables sending the printed version</summary>
        public bool TestingOnlyDoNotSendPrinted { get; set; } = false;

        /// <summary>Will set message StatusCode to 220 when it is opened</summary>
        public bool ReadConfirmation { get; set; } = false;

        /// <summary>Forces a mandatory confirmation from the recipient of receiving this message. Sets message StatusCode to 230 when it is accepted.</summary>
        public bool ReceivedConfirmation { get; set; } = false;
    }
}
