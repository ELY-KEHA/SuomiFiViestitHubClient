using System.Collections.Generic;
using System.Threading.Tasks;

namespace Keha.SuomiFiViestitHub.Client
{
    /// <summary>
    /// Interface for client
    /// </summary>
    public interface IHubClient
    {
        /// <summary></summary>
        Task<ServiceState> GetViestitServiceState();

        /// <summary></summary>
        Task<bool> CustomerHasAccount(string socialSecurityNumber);

        /// <summary></summary>
        Task<List<SentMessageStatus>> SendMessageToViestit(List<ViestitMessage> msgList);
    }
}
