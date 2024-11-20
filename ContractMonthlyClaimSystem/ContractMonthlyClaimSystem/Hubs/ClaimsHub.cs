using Microsoft.AspNetCore.SignalR;

namespace ContractMonthlyClaimSystem.Hubs
{
   
        public class ClaimsHub : Hub
        {
            // This method can be used to broadcast updates to connected clients
            public async Task SendClaimStatusUpdate(int claimId, string status)
            {
                // This will send the status update to all connected clients
                await Clients.All.SendAsync("ClaimStatusUpdated", claimId, status);
            }
        }
    }