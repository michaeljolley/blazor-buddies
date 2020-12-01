using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBuddies.Core.Common
{
    public class School
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Buddies { get; set; }
    }
}
