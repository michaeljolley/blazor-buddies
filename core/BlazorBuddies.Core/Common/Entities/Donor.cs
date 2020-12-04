using System;

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
				private string _phoneNumber;
        public string PhoneNumber 
				{ 
					get 
					{ 
						return _phoneNumber; 
					} 
					set 
					{
						_phoneNumber = value;
						NumberReachable = true;
					} 
				}
				public bool NumberReachable { get; set; } = true; 
    }
}
