using System;

namespace BlazorBuddies.Web.States
{
    public class ApplicationState
    {
        private int buddyCount;

        public event Action? OnBuddyCountChange;

        public int BuddyCount { get => buddyCount; set { buddyCount = value; OnBuddyCountChange?.Invoke(); } }
    }
}
