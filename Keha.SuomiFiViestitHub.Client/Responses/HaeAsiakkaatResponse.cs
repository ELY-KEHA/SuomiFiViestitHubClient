using Newtonsoft.Json;

namespace Keha.SuomiFiViestitHub.Client.Responses
{
    // Response JSON-schema
    // Full schema at https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
    // NOTE: We use Required.Always to throw JsonExceptions if API has changed
    internal class HaeAsiakkaatResponse : ResponseBase
    {
        [JsonProperty(Required = Required.Always, PropertyName = "asiakasTilat")]
        public CustomerState[] CustomerStates { get; set; }

        public class CustomerState
        {
            [JsonProperty(Required = Required.Always, PropertyName = "asiakasTunnus")]
            public string CustomerId { get; set; }

            [JsonProperty(Required = Required.Always, PropertyName = "tila")]
            public CustomerStateCode CustomerStateCode { get; set; }

            [JsonProperty(Required = Required.Default, PropertyName = "tiliPassivoitu")]
            public bool AccountPassive { get; set; }

            [JsonProperty(Required = Required.Default, PropertyName = "tilaPvm")]
            public long? AccountStateEpoch { get; set; } // Epoch time with milliseconds

            [JsonProperty(Required = Required.Always, PropertyName = "haettuPvm")]
            public long RequestEpoch { get; set; } // Epoch time with milliseconds
        }
    }
}
