using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Vonage.Messaging;
using Vonage.Utility;

namespace BlazorBuddies.Web.Controllers.Webhooks
{
	[Route("webhooks/dlr")]
	[ApiController]
	public class DeliveryReceiptController : ControllerBase
	{
		private readonly DonorService _donorService;

		/// <inheritdoc />
		public DeliveryReceiptController(DonorService donorService)
		{
			_donorService = donorService;
		}

		[HttpGet]
		public Task GetAsync()
		{
			var dlr = WebhookParser.ParseQuery<DeliveryReceipt>(HttpContext.Request.Query);
			return _donorService.HandleDlr(dlr);
		}
	}
}
