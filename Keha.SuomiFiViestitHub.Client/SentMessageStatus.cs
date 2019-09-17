using Keha.SuomiFiViestitHub.Client.Responses;

namespace Keha.SuomiFiViestitHub.Client
{
    /// <summary>
    /// Contains returned state information for the message processed by Viestit-service. It is up to the user how the StateCodes are processed.
    /// </summary>
    public struct SentMessageStatus
    {
        /// <summary>State code indicates whether the message was saved or something went wrong</summary>
        public MessageStateCode StateCode;
        /// <summary>Text description text of the code</summary>
        public string StateDescription;
        /// <summary>Viestit-service message id, "Sanomatunniste", for debugging purposes</summary>
        public string Id;
    }
}
