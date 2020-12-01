using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBuddies.Core.Common
{
    public class Donor
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public Boolean OptOut { get; set; }
        public string PhoneNumber { get; set; }
    }
}
