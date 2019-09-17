using Newtonsoft.Json;

namespace Keha.SuomiFiViestitHub.Client.Requests
{
    internal class RequestLink
    {
        [JsonProperty(PropertyName = "kuvaus")]
        public string Description;

        [JsonProperty(PropertyName = "url")]
        public string Url;
    }
}
