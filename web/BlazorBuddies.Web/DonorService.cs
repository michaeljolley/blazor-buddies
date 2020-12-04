using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BlazorBuddies.Core.Common;
using BlazorBuddies.Core.Data;
using BlazorBuddies.Web.Hubs;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Vonage;
using Vonage.Messaging;

namespace BlazorBuddies.Web
{
	public class DonorService
	{
		private readonly VonageClient _client;
		private readonly IConfiguration _config;
		private readonly BuddyDbContext _db;
		private readonly IHubContext<DonorHub> _hub;

		public DonorService(BuddyDbContext db, VonageClient client, IConfiguration config, IHubContext<DonorHub> hub)
		{
			_db = db;
			_client = client;
			_config = config;
			_hub = hub;
		}

		//TODO: Add pagination
		public async Task<List<Donor>> GetDonors()
		{
			return await _db.Donors.ToListAsync();
		}

		public async Task HandleDlr(DeliveryReceipt dlr)
		{
			var donors = (await _db.Donors.Where(x => x.PhoneNumber == dlr.Msisdn).ToListAsync()).ToDonorModels().ToList();
			donors.ForEach(d => {
				if ((dlr.Status == DlrStatus.failed) || (dlr.Status == DlrStatus.rejected)) {
					d.NumberReachable = false;
					d.ContactSuccessful = false;
				}
				else {
					d.ContactSuccessful = true;
				}
			});

			//foreach(var d in donors) {
			//	await _hub.SendDonor(d);
			//}
			await _db.SaveChangesAsync();
		}

		public async Task HandleInbound(InboundSms sms)
		{
			if (sms.Text.ToUpper().Contains("STOP")) {
				var donorsToOptOut = await _db.Donors.Where(d => d.PhoneNumber == sms.Msisdn).ToListAsync();
				donorsToOptOut.ForEach(d => d.OptOut = true);
				await _db.SaveChangesAsync();
			}
		}

		public async Task ContactDonors(IEnumerable<Guid> userIds, string message)
		{
			var donors = await _db.Donors.Where(d => userIds.Any(x => x == d.Id) && !d.OptOut && d.NumberReachable).ToListAsync();
			foreach (var donor in donors) {
				var msg = new SendSmsRequest {
					To = donor.PhoneNumber,
					From = _config["VONAGE_NUMBER"],
					Text = message + " Reply STOP to opt out."
				};
				var response = await _client.SmsClient.SendAnSmsAsync(msg);
				await Task.Delay(1000);
			}
		}
	}
}
