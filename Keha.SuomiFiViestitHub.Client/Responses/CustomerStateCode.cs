namespace Keha.SuomiFiViestitHub.Client.Responses
{
    /// <summary>
    /// Indicates the status of customers Viestit-account, copied from: https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
    /// </summary>
    public enum CustomerStateCode
    {
        /// <summary>Customer has an account and it is active</summary>
        HasAccount = 300,
        /// <summary>Customer does not have an account</summary>
        NoAccount = 310
    }
}
