using System.Collections.Generic;
using Newtonsoft.Json;

namespace Keha.SuomiFiViestitHub.Client.Requests
{
    // Full schema at https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
    // Request JSON-schema
    internal class LisaaKohteitaRequest : RequestBase
    {
        [JsonProperty(PropertyName = "asiakasTunnus")]
        public string CustomerId;

        [JsonProperty(PropertyName = "tiedostot")]
        public List<RequestFile> Files;

        [JsonProperty(PropertyName = "linkit")]
        public List<RequestLink> Links;

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

        public class RequestFile
        {
            [JsonProperty(PropertyName = "koko")]
            public int Size;

            [JsonProperty(PropertyName = "sisalto")]
            public string Base64Content;

            [JsonProperty(PropertyName = "muoto")]
            public string FileMimeType; // "application/pdf"

            [JsonProperty(PropertyName = "nimi")]
            public string Name;
        }

        public class RequestLink
        {
            [JsonProperty(PropertyName = "kuvaus")]
            public string Description;

            [JsonProperty(PropertyName = "url")]
            public string Url;
        }
    }
}
