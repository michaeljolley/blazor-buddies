using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

namespace BlazorBuddies.Web.Hubs
{
	public class DonorHub : Hub
	{
		public async Task SendDonor(DonorModel donor)
		{
			await Clients.All.SendAsync("HandleDlr", donor);
		}
	}
}
