using Newtonsoft.Json;

namespace Keha.SuomiFiViestitHub.Client.Requests
{
    internal class RequestFile
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
}
