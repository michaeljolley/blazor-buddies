using BlazorBuddies.Web.States;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorBuddies.Web.Controllers
{
	// This should really be api/count but just to keep the same route
	// This is what we'll use.
	[Route("count")]
	[ApiController]
#if DEBUG

	// Only in debug should this be anonymous. It's just for testing the counter object on the page.
	[AllowAnonymous]
#endif
	public class CountController : ControllerBase
	{
		private readonly ApplicationState _appState;

		/// <inheritdoc />
		public CountController(ApplicationState appState)
		{
			_appState = appState;
		}

		[HttpGet]
		public int Get()
		{
			return _appState.BuddyCount;
		}

		// This should *really* be a POST, but since it's just for debugging, we'll leave it as a get.
		[HttpGet("{count}")]
		public void Set(int count)
		{
			_appState.BuddyCount = count;
		}
	}
}
