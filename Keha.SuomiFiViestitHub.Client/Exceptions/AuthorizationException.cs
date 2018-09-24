using System;

namespace Keha.SuomiFiViestitHub.Client.Exceptions
{
    /// <summary>
    /// Used Id of the message does not match Authorization.
    /// </summary>
    public class AuthorizationException : Exception
    {
        /// <summary></summary>
        public AuthorizationException() : base("ASTI: Viranomaistunnus ei vastaa autentikaatiotietoa.") { }
    }
}
