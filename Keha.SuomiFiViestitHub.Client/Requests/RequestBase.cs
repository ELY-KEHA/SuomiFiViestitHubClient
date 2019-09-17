using Newtonsoft.Json;

namespace Keha.SuomiFiViestitHub.Client.Requests
{
    // Full schema at https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
    internal class RequestBase
    {
        [JsonProperty(PropertyName = "aikaleima")]
        public long TimeStampUtcMs;

        [JsonProperty(PropertyName = "kutsuja")]
        public string CallerName;

        [JsonProperty(PropertyName = "palveluTunnus")]
        public string CallerId;
    }
}
