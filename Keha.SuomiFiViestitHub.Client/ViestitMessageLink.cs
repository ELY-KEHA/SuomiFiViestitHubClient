using System.ComponentModel.DataAnnotations;
using Keha.SuomiFiViestitHub.Client.Requests;

namespace Keha.SuomiFiViestitHub.Client
{
    /// <summary>
    /// Urls with description shown along with the message text
    /// </summary>
    public class ViestitMessageLink
    {
        /// <summary></summary>
        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        /// <summary></summary>
        [Required(AllowEmptyStrings = false)]
        public string Url { get; set; }

        internal static RequestLink ToRequestLink(ViestitMessageLink link)
        {
            return new RequestLink
            {
                Description = link.Description,
                Url = link.Url
            };
        }
    }
}
