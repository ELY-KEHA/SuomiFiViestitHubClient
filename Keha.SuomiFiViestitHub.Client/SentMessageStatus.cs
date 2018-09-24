using Keha.SuomiFiViestitHub.Client.Responses;

namespace Keha.SuomiFiViestitHub.Client
{
    /// <summary>
    /// Contains returned state information for the message processed by Viestit-service. It is up to the user how the StateCodes are processed.
    /// </summary>
    public struct SentMessageStatus
    {
        /// <summary>Receiving person's SSN</summary>
        public string SocialSecurityNumber;
        /// <summary>Same as ViestitMessage.Id, "Lähettäjän antama viestin yksilöivä tunniste"</summary>
        public string Id;
        /// <summary>Id given by the Viestit-service</summary>
        public string ViestitId;
        /// <summary>State code indicates whether the message was saved or something went wrong</summary>
        public MessageStateCode MsgState;
        /// <summary>Text description text of the code</summary>
        public string MsgStateDescription;
    }
}
