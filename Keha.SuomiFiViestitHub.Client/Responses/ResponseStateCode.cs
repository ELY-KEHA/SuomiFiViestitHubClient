namespace Keha.SuomiFiViestitHub.Client.Responses
{
    /// <summary>
    /// State code returned on every request, copied from: https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
    /// </summary>
    public enum ResponseStateCode
    {
        /// <summary>Request successful</summary>
        Success = 0,
        /// <summary>Sent JSON was faulty</summary>
        FalseDataError = 400,
        /// <summary>Used Viestit-AccountId not matching authentication data</summary>
        WrongAuthError = 403,
        /// <summary>Used id not matching msgId (msgId sent earlier with another Viestit-AccountId)</summary>
        NotMatchingIdsError = 404,
        /// <summary>Operation not allowed for used Viestit-AccountId</summary>
        NotAllowedError = 405,
        /// <summary>Used signature error, problem at hub</summary>
        FalseSignatureError = 406,
        /// <summary>Unknown error</summary>
        OtherError = 450,
        /// <summary>Viestit-service down</summary>
        ConnectionError = 453
    }
}
