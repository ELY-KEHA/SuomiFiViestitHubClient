using Keha.SuomiFiViestitHub.Client.Exceptions;
using Keha.SuomiFiViestitHub.Client.Responses;

namespace Keha.SuomiFiViestitHub.Client.HubApi
{
    internal static class Common
    {
        /// <summary>
        /// Description messages copied from https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
        /// </summary>
        internal static bool HandleResponseStateCode(ResponseStateCode code)
        {
            switch (code)
            {
                case ResponseStateCode.Success: return true;
                case ResponseStateCode.FalseDataError: throw new FalseDataException();
                case ResponseStateCode.WrongAuthError: throw new AuthorizationException();
                case ResponseStateCode.NotMatchingIdsError: throw new NotMatchingIdsException();
                case ResponseStateCode.NotAllowedError: throw new ActionNotAllowedException();
                case ResponseStateCode.FalseSignatureError: throw new FalseSignatureException();
                case ResponseStateCode.OtherError: throw new OtherException();
                case ResponseStateCode.ConnectionError: throw new ConnectionException();
            }
            return true;
        }
        internal static bool HandleResponseStateCode(LahetaViestiResponseStateCode code)
        {
            switch (code)
            {
                case LahetaViestiResponseStateCode.SuccessButInProcess: return true;
                case LahetaViestiResponseStateCode.FalseDataError: throw new FalseDataException();
                case LahetaViestiResponseStateCode.WrongAuthError: throw new AuthorizationException();
                case LahetaViestiResponseStateCode.NotMatchingIdsError: throw new NotMatchingIdsException();
                case LahetaViestiResponseStateCode.NotAllowedError: throw new ActionNotAllowedException();
                case LahetaViestiResponseStateCode.FalseSignatureError: throw new FalseSignatureException();
                case LahetaViestiResponseStateCode.OtherError: throw new OtherException();
                case LahetaViestiResponseStateCode.ConnectionError: throw new ConnectionException();
            }
            return true;
        }
    }
}
