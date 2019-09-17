using System;
using Keha.SuomiFiViestitHub.Client.Exceptions;

namespace Keha.SuomiFiViestitHub.Client.Responses
{
    /// <summary>
    /// State code returned from lahetaviesti-api, copied from: https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
    /// NOTE: This is some kind of mishmash of Response and Message StateCodes
    /// </summary>
    public enum LahetaViestiResponseStateCode
    {
        /// <summary>"Kutsu onnistunut ja laitettu käsittelyyn asiointitilipalvelussa, mutta se ei vielä näy asiakkaan asiointitilillä. Lopullinen vastaus on haettavissa erikseen erillisellä kutsulla"</summary>
        SuccessButInProcess = 202,
        /// <summary>"Kutsuviesti on sisällöltään tai muodoltaan virheellinen"</summary>
        FalseDataError = 400,
        /// <summary>"Viranomaistunnus ei vastaa autentikoitua"</summary>
        WrongAuthError = 403,
        /// <summary>"Palvelutunnus ei vastaa viranomaistunnusta"</summary>
        NotMatchingIdsError = 404,
        /// <summary>"Toiminto ei sallittu viranomaiselle"</summary>
        NotAllowedError = 405,
        /// <summary>"Allekirjoitus ei vastaa palvelutunnuksella muodostettua allekirjoitusta"</summary>
        FalseSignatureError = 406,
        /// <summary>"Kohdepalvelu ei vastaa"</summary>
        ConnectionError = 453,
        /// <summary>"Asian tietosisällössä virheitä"</summary>
        FaultyDataError = 525,
        /// <summary>"Muu virhe"</summary>
        OtherError = 550,
    }
    internal static class LahetaViestiResponseStateCodeExtensions
    {
        internal static MessageStateCode ToMessageStateCode(this LahetaViestiResponseStateCode code)
        {
            var exists = Enum.IsDefined(typeof(MessageStateCode), (MessageStateCode)code);
            if (!exists)
            {
                throw new ClientFaultException(new Exception("Unknown LahetaViestiResponseStateCode"));
            }
            return (MessageStateCode)code;
        }
    }
}
