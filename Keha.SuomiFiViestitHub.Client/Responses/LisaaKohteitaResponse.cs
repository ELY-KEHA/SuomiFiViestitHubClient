using Newtonsoft.Json;
using System.Collections.Generic;

namespace Keha.SuomiFiViestitHub.Client.Responses
{
    // Response JSON-schema
    // Full schema at https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
    // NOTE: We use Required.Always to throw JsonExceptions if API has changed
    internal class LisaaKohteitaResponse : ResponseBase
    {
        [JsonProperty(Required = Required.Default, PropertyName = "kohdeMaara")]
        public int TargetCount { get; set; } // NOTE: Is always 1

        [JsonProperty(Required = Required.Always, PropertyName = "kohteet")]
        public List<MessageState> Targets { get; set; } // NOTE: Is always length 1 since sending messages doesn't allow multiple

        public class MessageState
        {
            [JsonProperty(Required = Required.Always, PropertyName = "viranomaisTunniste")]
            public string Id { get; set; }

            [JsonProperty(Required = Required.Always, PropertyName = "asiakkaat")]
            public List<Customer> Customers { get; set; } // NOTE: Is always length 1

            public class Customer
            {
                [JsonProperty(Required = Required.Always, PropertyName = "asiakasTunnus")]
                public string CustomerId { get; set; } // e.g. sotu

                [JsonProperty(Required = Required.Default, PropertyName = "tunnusTyyppi")]
                public string CustomerIdType { get; set; } // Is always "SSN" (hub does not support y-tunnus, aka. "CRN" yet)

                [JsonProperty(Required = Required.Always, PropertyName = "asiointitiliTunniste")]
                public string ServiceMessageId { get; set; } // "Asian yksilöivä tunniste asiointitilillä"

                [JsonProperty(Required = Required.Always, PropertyName = "kohteenTila")]
                public MessageStateCode MessageStateCode { get; set; }

                [JsonProperty(Required = Required.Always, PropertyName = "kohteenTilaKuvaus")]
                public string MessageStateDescription { get; set; }
            }
        }
    }
}
