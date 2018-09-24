﻿using Keha.SuomiFiViestitHub.Client.Requests;
using Keha.SuomiFiViestitHub.Client.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace Keha.SuomiFiViestitHub.Client
{
    /// <summary>
    /// Client for using MIP-HUB ASTI-API for sending messages to users of Suomi.fi.
    /// ASTI-API is documented here: https://www.joinex.com/sites/mip.io/mip.io-asti/index.html
    /// Suomi.fi messages are documented here: https://esuomi.fi/palveluntarjoajille/viestit/
    /// </summary>
    public class HubClient : IHubClient
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly ClientConfiguration _configuration;
 
        private HubClient() { }

        /// <summary>
        /// HttpClient can be left null: a new will be initialized with given configuration.
        /// </summary>
        /// <param name="config">Configuration contains connection and account information</param>
        /// <param name="client">Can be given a Preconfigured HttpClient or a Mock for testing purposes.</param>
        public HubClient(ClientConfiguration config, HttpClient client = null)
        {
            _configuration = config;
            if (client != null)
            {
                _client = client;
            }
            else
            {
                InitClient();
            }
        }

        /// <summary>Initializes the HttpClient with configured properties</summary>
        private void InitClient()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");

            _client.BaseAddress = new Uri(_configuration.HubUrl + ":" + _configuration.HubPort + "/");
        }

        /// <summary>Initializes RequestBase with configured properties</summary>
        private RequestBase InitRequest(RequestBase request)
        {
            request.CallerName = _configuration.CallerName;
            request.CallerId = _configuration.ViestitAccountId;
            return request;
        }

        /// <summary>
        /// Simple async call for checking state of the Viestit-service.
        /// Only used in testing since all sent responses return with ResponseStateCode which is checked.
        /// </summary>
        public async Task<ServiceState> GetViestitServiceState()
        {
            var data = await HubApi.GetViestitServiceState.Post(_client, InitRequest(new RequestBase()));
            return new ServiceState { Code = data.StateCode, CodeDescription = data.StateCodeDescription };
        }

        /// <summary>
        /// Async call to check if person has an account in suomi.fi/viestit -service.
        /// Throws ClientFaultException in case of this Client working improperly.
        /// Throws custom Keha.SuomiFiViestitHub.Client.Exceptions on returned error codes.
        /// </summary>
        /// <returns>Returns true if Customer has a Viestit-account and it is active.</returns>
        public async Task<bool> CustomerHasAccount(string socialSecurityNumber)
        {
            var req = new HaeAsiakkaatRequest
            {
                CustomerIds = new List<string>(new[] { socialSecurityNumber })
            };
            InitRequest(req);

            var data = await HubApi.CustomerHasViestitAccount.Post(_client, req);
            return (data.CustomerStates[0].CustomerStateCode == CustomerStateCode.HasAccount && !data.CustomerStates[0].AccountPassive);
        }

        /// <summary>
        /// Sends messages to customers to suomi.fi/viestit -service.
        /// Throws ClientFaultException in case of this Client working improperly.
        /// Throws custom Keha.SuomiFiViestitHub.Client.Exceptions on returned error codes.
        /// </summary>
        /// <param name="msgList">Messages are given as a list of ViestitMessages</param>
        /// <returns>Returns the StateCode of the handled message along with supplementing information</returns>
        public async Task<List<SentMessageStatus>> SendMessageToViestit(List<ViestitMessage> msgList)
        {
            msgList.ForEach((msg) =>
            {
                Validator.ValidateObject(msg, new ValidationContext(msg), true);
                msg.Files?.ForEach((file) => Validator.ValidateObject(file, new ValidationContext(file), true));
                msg.Links?.ForEach((link) => Validator.ValidateObject(link, new ValidationContext(link), true));
            });

            var reqList = new List<Task<LisaaKohteitaResponse>>();
            foreach (ViestitMessage msg in msgList)
            {
                var req = new LisaaKohteitaRequest
                {
                    CustomerId = msg.SocialSecurityNumber,
                    Files = msg.Files != null
                        ? msg.Files.ConvertAll(ViestitMessageFile.ToRequestFile)
                        : new List<LisaaKohteitaRequest.RequestFile>(),
                    Links = msg.Links != null
                        ? msg.Links.ConvertAll(ViestitMessageLink.ToRequestLink)
                        : new List<LisaaKohteitaRequest.RequestLink>(),
                    Id = msg.Id,
                    MsgId = msg.MsgId,
                    Topic = msg.Topic,
                    SenderName = msg.SenderName,
                    Text = msg.Text
                };
                InitRequest(req);
                reqList.Add(HubApi.SendMessageToViestit.Post(_client, req));
            }

            var data = new List<LisaaKohteitaResponse>(await Task.WhenAll(reqList));
            return data.ConvertAll((response) => 
                new SentMessageStatus
                {
                    Id = response.Targets[0].Id,
                    SocialSecurityNumber = response.Targets[0].Customers[0].CustomerId,
                    ViestitId = response.Targets[0].Customers[0].ServiceMessageId,
                    MsgState = response.Targets[0].Customers[0].MessageStateCode,
                    MsgStateDescription = response.Targets[0].Customers[0].MessageStateDescription
                });
        }
    }
}
