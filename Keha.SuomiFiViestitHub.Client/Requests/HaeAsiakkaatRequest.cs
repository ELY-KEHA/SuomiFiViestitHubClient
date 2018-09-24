using System.Collections.Generic;
using Newtonsoft.Json;

namespace Keha.SuomiFiViestitHub.Client.Requests
{
    internal class HaeAsiakkaatRequest : RequestBase
    {
        // Full schema at https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
        // Request JSON-schema
        [JsonProperty(PropertyName = "kyselyLaji")]
        public string RequestType { get; set; } = CustomerRequestType.Asiakkaat.ToString();

        [JsonProperty(PropertyName = "kyselyAlku")]
        public string StartingDate { get; set; } // Must be used when RequestType == "Kaikki" e.g. "01.09.2018"

        [JsonProperty(PropertyName = "kyselyLoppu")]
        public string EndingDate { get; set; } // Must be used when RequestType == "Kaikki" e.g. "01.09.2018"

        [JsonProperty(PropertyName = "asiakasTunnukset")]
        public List<string> CustomerIds { get; set; } // Used when RequestType == "Asiakkaat"
    }
}
