using System.ComponentModel.DataAnnotations;

namespace Keha.SuomiFiViestitHub.Client
{
    /// <summary>
    /// Address information for the printed message.
    /// NOTE: The printed content must contain properly placed recipient name on the first page.
    /// </summary>
    public class AddressInformation
    {
        /// <summary>Recipient's full name, e.g. 'Matti Meikäläinen'</summary>
        [Required(AllowEmptyStrings = false)]
        public string RecipientName { get; set; }

        /// <summary>Street address, aka. 'lähiosoite'</summary>
        [Required(AllowEmptyStrings = false)]
        public string StreetAddress { get; set; }

        /// <summary>Postal code, aka. 'postinumero'</summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(5, MinimumLength = 5)]
        public string PostalCode { get; set; }

        /// <summary>City, aka. 'postitoimipaikka'</summary>
        [Required(AllowEmptyStrings = false)]
        public string City{ get; set; }

        /// <summary>Country code, e.g. 'FI'</summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(2, MinimumLength = 2)]
        public string CountryCode { get; set; }
    }
}
