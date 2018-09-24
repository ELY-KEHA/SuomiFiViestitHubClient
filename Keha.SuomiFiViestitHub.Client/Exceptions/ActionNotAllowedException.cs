using System;

namespace Keha.SuomiFiViestitHub.Client.Exceptions
{
    /// <summary>
    /// Action not allowed for used MessageId or AccountId.
    /// </summary>
    public class ActionNotAllowedException : Exception
    {
        /// <summary></summary>
        public ActionNotAllowedException() : base("ASTI: Toiminto ei ole sallittu kyseiselle viranomaistunnukselle ja palvelutunnukselle") { }
    }
}