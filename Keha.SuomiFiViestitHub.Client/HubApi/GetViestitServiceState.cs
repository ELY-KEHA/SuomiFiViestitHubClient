using Keha.SuomiFiViestitHub.Client.Exceptions;
using Keha.SuomiFiViestitHub.Client.Requests;
using Keha.SuomiFiViestitHub.Client.Responses;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Keha.SuomiFiViestitHub.Client.HubApi
{
    internal static class GetViestitServiceState
    {
        internal static async Task<ResponseBase> Post(HttpClient client, RequestBase initiatedRequest)
        {
            var response = await client.PostAsJsonAsync("api/asti/haetilatieto", initiatedRequest);
            try
            {
                var json = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException((int)response.StatusCode + " HttpRequestException, Content: " + json);
                }

                return JsonConvert.DeserializeObject<ResponseBase>(
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

