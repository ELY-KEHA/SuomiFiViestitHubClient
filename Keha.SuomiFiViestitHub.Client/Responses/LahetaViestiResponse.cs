using Newtonsoft.Json;

namespace Keha.SuomiFiViestitHub.Client.Responses
{
    // Response JSON-schema
    // Full schema at https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
    // NOTE: We use Required.Always to throw JsonExceptions if API has changed
    // NOTE: This does NOT contain same content as ResponseBase
    internal class LahetaViestiResponse
    {
        [JsonProperty(Required = Required.Always, PropertyName = "aikaleima")]
        public long TimeStampEpoch { get; set; } // Epoch time with milliseconds

        [JsonProperty(Required = Required.Always, PropertyName = "tilaKoodi")]
        public LahetaViestiResponseStateCode StateCode { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "tilaKoodiKuvaus")]
        public string StateCodeDescription { get; set; }

        // NOTE: This is null when GetServiceState is called
        [JsonProperty(Required = Required.AllowNull, PropertyName = "sanomaTunniste")]
        public string MessageId { get; set; }
    }
}
