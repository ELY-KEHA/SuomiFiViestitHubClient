namespace Keha.SuomiFiViestitHub.Client
{
    /// <summary>
    /// Configuration contains connection and account information
    /// </summary>
    public class ClientConfiguration
    {
        /// <summary>Url of the Viestit-Hub e.g. 'http://justexampleurlofhub.fi'</summary>
        public string HubUrl { get; set; }
        /// <summary>Port of the Viestit-Hub e.g. '8090'</summary>
        public string HubPort { get; set; }
        /// <summary>Account name of the calling organization</summary>
        public string CallerName { get; set; }
        /// <summary>Account Id given to the organization when registered to the Viestit-service</summary>
        public string ViestitAccountId { get; set; }
    }
}
