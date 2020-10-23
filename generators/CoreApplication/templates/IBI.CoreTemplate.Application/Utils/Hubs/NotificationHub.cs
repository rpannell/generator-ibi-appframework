using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Application.Utils.Hubs
{
    /// <summary>
    /// The SignarlR hub to handle notifications back and forth
    /// </summary>
    public class NotificationHub : Hub
    {
        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
    }
}