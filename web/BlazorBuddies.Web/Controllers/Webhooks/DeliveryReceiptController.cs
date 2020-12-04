using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Vonage.Messaging;
using Vonage.Utility;

namespace BlazorBuddies.Web.Controllers.Webhooks
{
	[Route("webhooks/dlr")]
	[ApiController]
	[AllowAnonymous] // TODO: Look into authenticated requests on these endpoints. We don't need random requests coming from unknown locations.
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

		[HttpPost]
		public async Task PostAsync()
		{
			// Personally, I don't like just blindly passing the body of the request to be parsed here. It makes integration testing difficult.
			// Newtonsoft JSON is also many many times slower than the new System.Text.JsonSerializer.
			// However, this is what's documented in the Vonage SDK, so this is what we'll use.
			// https://github.com/Vonage/vonage-dotnet-sdk#post-2
			var dlr = await WebhookParser.ParseWebhookAsync<DeliveryReceipt>(Request.Body, Request.ContentType);
			await _donorService.HandleDlr(dlr);
		}
	}
}
