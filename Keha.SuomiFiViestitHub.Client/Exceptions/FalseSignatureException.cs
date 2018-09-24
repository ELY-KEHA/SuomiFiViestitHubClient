using System;

namespace Keha.SuomiFiViestitHub.Client.Exceptions
{
    /// <summary>
    /// Indicates that the message signature generated at used Hub does not match used AccountId.
    /// </summary>
    public class FalseSignatureException : Exception
    {
        /// <summary></summary>
        public FalseSignatureException() : base("ASTI: Allekirjoitus ei vastaa palvelutunnuksella muodostettua allekirjoitusta.") { }
    }
}
