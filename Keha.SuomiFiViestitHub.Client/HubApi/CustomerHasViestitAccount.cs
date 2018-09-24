using Keha.SuomiFiViestitHub.Client.Exceptions;
using Keha.SuomiFiViestitHub.Client.Requests;
using Keha.SuomiFiViestitHub.Client.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Keha.SuomiFiViestitHub.Client.HubApi.Common;

namespace Keha.SuomiFiViestitHub.Client.HubApi
{
    internal static class CustomerHasViestitAccount
    {
        internal static async Task<HaeAsiakkaatResponse> Post(HttpClient client, HaeAsiakkaatRequest initiatedRequest)
        {
            var response = await client.PostAsJsonAsync("api/asti/haeasiakkaita", initiatedRequest);

            try
            {
                var json = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException((int)response.StatusCode + " HttpRequestException, Content: " + json);
                }

                // Check first just the state code
                HandleResponseStateCode(JObject.Parse(json).SelectToken("tilaKoodi").ToObject<ResponseStateCode>());

                return JsonConvert.DeserializeObject<HaeAsiakkaatResponse>(
                    json,
                    new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });
            }
            // Any JsonExceptions mean that the API has changed, and client needs update
            catch (JsonException e)
            {
                throw new ClientFaultException(e);
            }
        }
    }
}
