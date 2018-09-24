using Newtonsoft.Json;

namespace Keha.SuomiFiViestitHub.Client.Responses
{
    // Full schema at https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
    internal class ResponseBase
    {
        [JsonProperty(Required = Required.Always, PropertyName = "aikaleima")]
        public long TimeStampEpoch { get; set; } // Epoch time with milliseconds

        [JsonProperty(Required = Required.Always, PropertyName = "tilaKoodi")]
        public ResponseStateCode StateCode { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "tilaKoodiKuvaus")]
        public string StateCodeDescription { get; set; }

        // NOTE: This is null when GetServiceState is called
        [JsonProperty(Required = Required.AllowNull, PropertyName = "sanomaTunniste")]
        public string CallId { get; set; }
    }
}
