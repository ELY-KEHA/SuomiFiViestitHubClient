using System;

namespace Keha.SuomiFiViestitHub.Client.Exceptions
{
    /// <summary>
    /// Used Id of the message does not match used Viestit-AccountId.
    /// </summary>
    public class NotMatchingIdsException : Exception
    {
        /// <summary></summary>
        public NotMatchingIdsException() : base("ASTI: Palvelutunnus ei vastaa viranomaistunnusta.") { }
    }
}