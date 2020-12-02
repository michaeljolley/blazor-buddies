using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorBuddies.Web.FrontMatter
{
	public partial class Counter
	{
		private Timer? _timer;
		private int _target;

		[Parameter]
		public int Target { get => _target; set { _target = value; SetBuddyCount(); } }

		[Parameter]
		public bool Animate { get; set; }

		int currentCount = 0;

		private void SetBuddyCount()
		{
			if (Animate) {
				_timer = new Timer(async _ => {
					switch (currentCount < Target) {
						case false:
							currentCount -= 6;
							if (currentCount <= Target) {
								currentCount = Target;
								_timer?.Dispose();
							}
							break;
						case true:
							currentCount += 6;
							if (currentCount >= Target) {
								currentCount = Target;
								_timer?.Dispose();
							}
							break;
					}
					await InvokeAsync(StateHasChanged);
				}, null, 250, 1);
			}
			else {
				currentCount = Target;
			}
		}

		protected override Task OnInitializedAsync()
		{
			SetBuddyCount();
			return base.OnInitializedAsync();
		}
	}
}
