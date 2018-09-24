using System;

namespace Keha.SuomiFiViestitHub.Client.Exceptions
{
    /// <summary>
    /// Data sent to Viestit-service contains errors.
    /// </summary>
    public class FalseDataException : Exception
    {
        /// <summary></summary>
        public FalseDataException() : base("ASTI: Kutsuviesti on sisällöltään tai muodoltaan virheellinen.") { }
    }
}