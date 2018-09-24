using System;

namespace Keha.SuomiFiViestitHub.Client.Exceptions
{
    /// <summary>
    /// Indicates that the Viestit-service is down.
    /// If this occurs, the operation should be tried again later.
    /// </summary>
    public class ConnectionException : Exception
    {
        /// <summary></summary>
        public ConnectionException() : base("ASTI: Kohdepalvelu ei vastaa.") { }
    }
}
