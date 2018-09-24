using Keha.SuomiFiViestitHub.Client.Responses;

namespace Keha.SuomiFiViestitHub.Client
{
    /// <summary>
    /// State of the Viestit-service
    /// </summary>
    public class ServiceState
    {
        /// <summary>State as a code</summary>
        public ResponseStateCode Code { get; set; }
        /// <summary>State as a description</summary>
        public string CodeDescription { get; set; }
    }
}
