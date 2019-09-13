namespace Keha.SuomiFiViestitHub.Client.Responses
{
    /// <summary>
    /// State code of added message. Descriptions of values are in Finnish, copied from: https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
    /// </summary>
    public enum MessageStateCode
    {
        // "Näissä tilanteissa kutsu tallentui asiointitilille"

        /// <summary>"Kutsu onnistunut ja tallennettu asiointitilille. Jos ei toimitettavia liitetiedostoja, voidaan käsitellä loppuun kutsun yhteydessä"</summary>
        Success = 200,
        /// <summary>"Kutsu onnistunut ja laitettu käsittelyyn asiointitilipalvelussa, mutta se ei vielä näy asiakkaan asiointitilillä. Lopullinen vastaus on haettavissa erikseen erillisellä kutsulla"</summary>
        SuccessButInProcess = 202,
        /// <summary>"Viesti on luettu asiakkaan toimesta"</summary>
        SuccessHasBeenRead = 220,
        /// <summary>"Viesti on kuitattu luetuksi asiakkaan toimesta - todisteellinen tiedoksianto"</summary>
        SuccessHasBeenAcknowledged = 230,

        // "Näissä tilanteissa asiaa ei ole tallennettu asiointitilipalveluun"

        /// <summary>"Annetulla tunnisteella löytyy jo asia, joka on tallennettu asiointitilipalveluun eikä se ole virheellinen"</summary>
        TargetIdAlreadyExistsError = 520,
        /// <summary>"Liitoksen kohdetta (Viittaus) ei löydy tai se on eri asiakkaan asia"</summary>
        ReferenceError = 521,
        /// <summary>"Asiakas ei ota vastaan asioita asiointitilille"</summary>
        CustomerPassiveError = 524,
        /// <summary>"Asian tietosisällössä virheitä"</summary>
        FaultyDataError = 525,
        /// <summary>"Ei sallittu liitetiedoston tyyppi"</summary>
        FiletypeNotAllowedError = 528,
        /// <summary>"Liian iso liitetiedoston koko"</summary>
        FilesTooLargeError = 529,
        /// <summary>"Muu virhe"</summary>
        OtherError = 550,
        /// <summary>"Liitettä ei saatu haettua tallennuspalvelusta"</summary>
        CouldNotFetchFileFromService = 580

        // NOTE: "Tiloja 522 ja 523 ei palauteta tässä, koska virustarkitus tapahtuu työjonon kautta"
        // FileSavingError = 522, // Not in use - "ongelma liitetiedoston tallennuksessa"
        // FileVirusCheckError = 523, // Not in use - "ongelma liitetiedoston virustarkistuksessa"
    }
}
