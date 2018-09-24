using System;

namespace Keha.SuomiFiViestitHub.Client.Exceptions
{
    /// <summary>
    /// Exception for undefined errors at the called Viestit-service.
    /// </summary>
    public class OtherException : Exception
    {
        /// <summary></summary>
        public OtherException() : base("ASTI: Muu virhe käsittelyssä.") { }
    }
}