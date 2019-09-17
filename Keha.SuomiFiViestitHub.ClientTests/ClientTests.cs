using Keha.SuomiFiViestitHub.Client;
using Keha.SuomiFiViestitHub.Client.Exceptions;
using Keha.SuomiFiViestitHub.Client.Responses;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Keha.SuomiFiViestitHub.ClientTests
{
    public class ClientTest
    {
        private const string FakeUrl = "http://notexistingurl.com";
        private const string FakePort = "8080";
        private const string CallerName = "testName";
        private const string CallerId = "testId";

        private static Uri GetExpectedUri(string path) // e.g. "/haetilatieto"
        {
            return new Uri(FakeUrl + ":" + FakePort + "/api/asti" + path);
        }

        private readonly Mock<HttpMessageHandler> _handlerMock = new Mock<HttpMessageHandler>();

        private HubClient GetClient(HttpResponseMessage mockResponse)
        {
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(mockResponse));
            var httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri(FakeUrl + ":" + FakePort + "/")
            };
            return new HubClient(
                new ClientConfiguration
                {
                    HubUrl = FakeUrl,
                    HubPort = FakePort,
                    CallerName = CallerName,
                    ViestitAccountId = CallerId
                },
                httpClient);
        }

        [Fact]
        public async void TestHttpStatusNotOk()
        {
            // ARRANGE
            var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("EmptyMessage") };
            var client = GetClient(mockResponse);

            // ACT + ASSERT
            await Assert.ThrowsAsync<HttpRequestException>(async () => await client.GetViestitServiceState());
        }

        [Fact]
        public async void TestBasicRequestHasCorrectContent()
        {
            // ARRANGE
            dynamic responseMessage = new JObject();
            responseMessage.tilaKoodi = ResponseStateCode.Success;
            responseMessage.tilaKoodiKuvaus = "TestiKuvaus";
            responseMessage.sanomaTunniste = "123-123";
            responseMessage.aikaleima = "123";

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseMessage.ToString()) };
            var client = GetClient(mockResponse);

            // ACT
            await client.GetViestitServiceState();

            // ASSERT
            Func<HttpContent, bool> requestContainsCorrectData = ( content =>
            {
                var data = JsonConvert.DeserializeObject<dynamic>(content.ReadAsStringAsync().Result);
                return data.kutsuja == CallerName && data.palveluTunnus == CallerId;
            });

            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri == GetExpectedUri("/haetilatieto")
                    && requestContainsCorrectData(req.Content)
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async void TestChangedApiFields()
        {
            // ARRANGE
            dynamic responseMessage = new JObject();
            responseMessage.apiChangedField = "SomeValue";

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseMessage.ToString()) };
            var client = GetClient(mockResponse);

            // ACT + ASSERT
            // Client will send ClientFaultException if API sent JSON doesn't match hardcoded one
            await Assert.ThrowsAsync<ClientFaultException>(async () => await client.GetViestitServiceState());

            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri == GetExpectedUri("/haetilatieto")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        
        [Theory]
        [InlineData(ResponseStateCode.ConnectionError, typeof(ConnectionException))]
        [InlineData(ResponseStateCode.FalseDataError, typeof(FalseDataException))]
        [InlineData(ResponseStateCode.FalseSignatureError, typeof(FalseSignatureException))]
        [InlineData(ResponseStateCode.NotAllowedError, typeof(ActionNotAllowedException))]
        [InlineData(ResponseStateCode.NotMatchingIdsError, typeof(NotMatchingIdsException))]
        [InlineData(ResponseStateCode.OtherError, typeof(OtherException))]
        [InlineData(ResponseStateCode.WrongAuthError, typeof(AuthorizationException))]
        public async void TestResponseStateCodesExceptions(ResponseStateCode code, Type exception)
        {
            // ARRANGE
            dynamic responseMessage = new JObject();
            responseMessage.tilaKoodi = code;
            responseMessage.tilaKoodiKuvaus = "TestiKuvaus";
            responseMessage.sanomaTunniste = "123-123";

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseMessage.ToString()) };
            var client = GetClient(mockResponse);

            // ACT
            var thrownException = await Record.ExceptionAsync(() => client.CustomerHasAccount("123456-7890"));

            // ASSERT
            Assert.NotNull(thrownException);
            Assert.Equal(thrownException.GetType(), exception);
        }


        [Theory]
        [InlineData(CustomerStateCode.HasAccount)]
        [InlineData(CustomerStateCode.NoAccount)]
        public async void TestGetCustomer(CustomerStateCode code)
        {
            // ARRANGE
            const string ssn = "123456-7890";

            dynamic responseMessage = new JObject();
            responseMessage.tilaKoodi = ResponseStateCode.Success;
            responseMessage.tilaKoodiKuvaus = "TestiKuvaus";
            responseMessage.sanomaTunniste = "123-123";
            responseMessage.aikaleima = "123";
            responseMessage.asiakasTilat = new JArray();
            dynamic asiakas = new JObject();
            asiakas.asiakasTunnus = ssn;
            asiakas.tila = code;
            asiakas.tilaPvm = null;
            asiakas.tiliPassivoitu = false; // We are only interested in the code
            asiakas.haettuPvm = (long)12345;
            responseMessage.asiakasTilat.Add(asiakas);

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseMessage.ToString()) };
            var client = GetClient(mockResponse);

            // ACT
            var returnBool = await client.CustomerHasAccount(ssn);

            // ASSERT
            Func<HttpContent, bool> requestContainsCorrectData = (content =>
            {
                var data = JsonConvert.DeserializeObject<dynamic>(content.ReadAsStringAsync().Result);
                return data.asiakasTunnukset[0] == ssn
                    && data.kyselyLaji == "Asiakkaat";
            });

            Assert.Equal(code == CustomerStateCode.HasAccount, returnBool);
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri == GetExpectedUri("/haeasiakkaita")
                    && requestContainsCorrectData(req.Content)
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("1234567890112233")]
        public async void TestAddTargetsValidation(string ssn)
        {
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Empty") };
            var client = GetClient(mockResponse);

            // ACT
            var thrownException = await Record.ExceptionAsync(() => client.SendMessageToViestit(new List<ViestitMessage> { new ViestitMessage
            {
                SocialSecurityNumber = ssn,
                Id = "DummyText",
                MsgId = "DummyText",
                Topic = "DummyText",
                SenderName = "DummyText",
                Text = "DummyText",
            }}));

            // ASSERT
            Assert.NotNull(thrownException);
            Assert.Equal(typeof(ValidationException), thrownException.GetType());
        }

        [Fact]
        public async void TestAddTargets()
        {
            // ARRANGE
            const string ssn = "123456-7890";
            const MessageStateCode msgCode = MessageStateCode.Success;
            const string msgCodeDescription = "Asia on tallennettuna ja näkyy asiakkaalle.";
            const string sentMsgId = "SPAv2-1234-test";
            const string viestitId = "TiliTunniste";
            const string sanomaTunniste = "123-123";

            // request
            const string sentMsgNumber = "ViestinTunniste";
            const string sentTopic = "Otsikko";
            const string sentName = "Lähettäjänimi";
            const string sentText = "Sisältötekstiä.";

            var linkList = new List<ViestitMessageLink>
            {
                new ViestitMessageLink { Url = "TestUrl.com", Description = "UrlDescription" }
            };
            var fileList = new List<ViestitMessageFile>
            { 
                new ViestitMessageFile { Name = "Filename", Size = 10, Content = "Base64 text", ContentType = "application/pdf" }
            };

            var viestitMessage = new ViestitMessage
            {
                SocialSecurityNumber = ssn,
                Id = sentMsgId,
                MsgId = sentMsgNumber,
                Topic = sentTopic,
                SenderName = sentName,
                Text = sentText,
                Links = linkList,
                Files = fileList
            };

            // response
            dynamic responseMessage = new JObject();
            responseMessage.tilaKoodi = ResponseStateCode.Success;
            responseMessage.tilaKoodiKuvaus = "TestiKuvaus";
            responseMessage.sanomaTunniste = sanomaTunniste;
            responseMessage.aikaleima = (long)12345;
            responseMessage.kohdeMaara = 1;
            responseMessage.kohteet = new JArray();
            dynamic kohde = new JObject();
            kohde.viranomaisTunniste = sentMsgId;
            kohde.asiakkaat = new JArray();
            responseMessage.kohteet.Add(kohde);
            dynamic asiakas = new JObject();
            asiakas.asiakasTunnus = ssn;
            asiakas.tunnusTyyppi = "SSN";
            asiakas.asiointitiliTunniste = viestitId;
            asiakas.kohteenTila = msgCode;
            asiakas.kohteenTilaKuvaus = msgCodeDescription;
            kohde.asiakkaat.Add(asiakas);

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseMessage.ToString()) };
            var client = GetClient(mockResponse);

            // ACT
            var responseData = await client.SendMessageToViestit(new List<ViestitMessage> { viestitMessage });

            // ASSERT
            Func<HttpContent, bool> requestContainsCorrectData = (content =>
            {
                var data = JsonConvert.DeserializeObject<dynamic>(content.ReadAsStringAsync().Result);
                return data.asiakasTunnus == ssn
                    && data.viranomaisTunniste == sentMsgId
                    && data.asiaNumero == sentMsgNumber
                    && data.nimeke == sentTopic
                    && data.lahettajaNimi == sentName
                    && data.kuvausTeksti == sentText
                    && data.tiedostot.Count == 1
                    && data.linkit.Count == 1
                    && data.lukuKuittaus == false
                    && data.vastaanottoVahvistus == false;
            });

            Assert.Equal(responseData[0].StateCode, msgCode);
            Assert.Equal(responseData[0].StateDescription, msgCodeDescription);
            Assert.Equal(responseData[0].Id, sanomaTunniste);

            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri == GetExpectedUri("/lisaakohteita")
                    && requestContainsCorrectData(req.Content)
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData(false)]
        [InlineData(true)]
        public async void TestSendPrintableMessage(bool? sendToPrinting)
        {
            // ARRANGE
            const string ssn = "123456-7890";
            const MessageStateCode msgCode = MessageStateCode.SuccessButInProcess;
            const string msgCodeDescription = "Asia on tallennettuna ja näkyy asiakkaalle.";
            const string sentMsgId = "SPAv2-1234-test";
            const long timeStamp = 1568615412083;
            const string sanomaTunniste = "123-123";

            // request
            const string sentMsgNumber = "ViestinTunniste";
            const string sentTopic = "Otsikko";
            const string sentName = "Lähettäjänimi";
            const string sentText = "Sisältötekstiä.";

            const string recipientName = "Maisa Testaaja";
            const string streetAddress = "Lähitie 123 a 36";
            const string postalCode = "12345";
            const string city = "Betonila";
            const string countryCode = "FI";

            const string printingProvider = "Printtifirma";
            var file = new ViestitMessageFile { Name = "Filename", Size = 10, Content = "Base64 text", ContentType = "application/pdf" };

            var printableMessage = new PrintableViestitMessage
            {
                SocialSecurityNumber = ssn,
                Id = sentMsgId,
                File = file,
                MsgId = sentMsgNumber,
                Topic = sentTopic,
                SenderName = sentName,
                Text = sentText,
                Address = new AddressInformation
                {
                    RecipientName = recipientName,
                    StreetAddress = streetAddress,
                    PostalCode = postalCode,
                    City = city,
                    CountryCode = countryCode
                },
                PrintingProvider = printingProvider
            };

            if (sendToPrinting.HasValue)
            {
                printableMessage.TestingOnlyDoNotSendPrinted = sendToPrinting.Value;
            }

            // response
            dynamic responseMessage = new JObject();
            responseMessage.aikaleima = timeStamp;
            responseMessage.tilaKoodi = msgCode;
            responseMessage.tilaKoodiKuvaus = msgCodeDescription;
            responseMessage.sanomaTunniste = sanomaTunniste;

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseMessage.ToString()) };
            var client = GetClient(mockResponse);

            // ACT
            var responseData = await client.SendPrintableMessageToViestit(new List<PrintableViestitMessage> { printableMessage });

            // ASSERT
            Func<HttpContent, bool> requestContainsCorrectData = content =>
            {
                var data = JsonConvert.DeserializeObject<dynamic>(content.ReadAsStringAsync().Result);
                return data.asiakasTunnus == ssn
                    && data.osoiteNimi == recipientName
                    && data.osoiteLahiosoite == streetAddress
                    && data.osoitePostinumero == postalCode
                    && data.osoitePostitoimipaikka == city
                    && data.osoiteMaakoodi == countryCode
                    && data.tiedostot.Count == 1
                    && data.viranomaisTunniste == sentMsgId
                    && data.asiaNumero == sentMsgNumber
                    && data.nimeke == sentTopic
                    && data.lahettajaNimi == sentName
                    && data.kuvausTeksti == sentText
                    && data.lukuKuittaus == false
                    && data.vastaanottoVahvistus == false
                    && data.paperi == false
                    && data.lahetaTulostukseen == !(sendToPrinting ?? false); // NOTE: Default must be true!
            };

            Assert.Equal(responseData[0].StateCode, msgCode);
            Assert.Equal(responseData[0].StateDescription, msgCodeDescription);
            Assert.Equal(responseData[0].Id, sanomaTunniste);

            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri == GetExpectedUri("/lahetaviesti")
                    && requestContainsCorrectData(req.Content)
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}

