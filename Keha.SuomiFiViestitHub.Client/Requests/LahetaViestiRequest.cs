using System.Collections.Generic;
using Newtonsoft.Json;

namespace Keha.SuomiFiViestitHub.Client.Requests
{
    // Full schema at https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
    // Request JSON-schema
    internal class LahetaViestiRequest : RequestBase
    {
        [JsonProperty(PropertyName = "asiakasTunnus")]
        public string CustomerId;

        [JsonProperty(PropertyName = "osoiteNimi")]
        public string RecipientName;

        [JsonProperty(PropertyName = "osoiteLahiosoite")]
        public string StreetAddress;

        [JsonProperty(PropertyName = "osoitePostinumero")]
        public string PostalCode;

        [JsonProperty(PropertyName = "osoitePostitoimipaikka")]
        public string City;

        [JsonProperty(PropertyName = "osoiteMaakoodi")]
        public string CountryCode;

        [JsonProperty(PropertyName = "tiedostot")]
        public List<RequestFile> Files;

        [JsonProperty(PropertyName = "viranomaisTunniste")]
        public string Id; // "Lähettäjän antama viestin yksilöivä tunniste"

        [JsonProperty(PropertyName = "lukuKuittaus")]
        public bool ReadConfirmation;

        [JsonProperty(PropertyName = "vastaanottoVahvistus")]
        public bool ReceivedConfirmation;

        [JsonProperty(PropertyName = "asiaNumero")]
        public string MsgId; // "Viestit -palvelussa näytettävä asiakirjan tunniste(esim.päätöksen numero)"

        [JsonProperty(PropertyName = "nimeke")]
        public string Topic;

        [JsonProperty(PropertyName = "lahettajaNimi")]
        public string SenderName; // "Lähettäneen viranomaisen tarkempi nimi."

        [JsonProperty(PropertyName = "kuvausTeksti")]
        public string Text;

        [JsonProperty(PropertyName = "emailLisatietoOtsikko")]
        public string EmailTopic;

        [JsonProperty(PropertyName = "emailLisatietoSisalto")]
        public string EmailText;

        [JsonProperty(PropertyName = "paperi")]
        public bool SendAlsoAsPrinted;

        [JsonProperty(PropertyName = "lahetaTulostukseen")]
        public bool UsePrinting; // Huom! Lähinnä testaukseen, tuotannossa pitää olla aina 'true'

        [JsonProperty(PropertyName = "tulostusToimittaja")]
        public string PrintingProvider;
    }
}
