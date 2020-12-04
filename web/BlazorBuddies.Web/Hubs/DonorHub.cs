using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
