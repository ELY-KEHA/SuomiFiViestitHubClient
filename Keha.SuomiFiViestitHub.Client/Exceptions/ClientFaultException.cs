using System;

namespace Keha.SuomiFiViestitHub.Client.Exceptions
{
    /// <summary>
    /// Exception thrown when this Client doesn't work right or is outdated.
    /// If this occurs, first step is to check for Client updates. If no such exists, report as bug.
    /// </summary>
    public class ClientFaultException : Exception
    {
        /// <summary></summary>
        /// <param name="innerException">Caught JsonException</param>
        public ClientFaultException(Exception innerException) : base("JSON parsing error, this Client is outdated.", innerException) { }
    }
}
