using System;

namespace BlazorBuddies.Core.Common
{
  public static class EntityFactory
  {
		public static School CreateSchool(string Name, string City, int Buddies)
		{
				return new School() {
					Id = Guid.NewGuid(),
					Name = Name,
					City = City,
					Buddies = Buddies
				};
		}

		public static Donor CreateDonor(string FirstName, string LastName, string EmailAddress, string StreetAddress1, string StreetAddress2, string City, string State, string PostalCode, string PhoneNumber, bool OptOut)
		{
			return new Donor() {
				Id = Guid.NewGuid(),
				FirstName = FirstName,
				LastName = LastName,
				EmailAddress = EmailAddress,
				StreetAddress1 = StreetAddress1,
				StreetAddress2 = StreetAddress2,
				City = City,
				State = State,
				PostalCode = PostalCode,
				PhoneNumber = PhoneNumber,
				OptOut = OptOut
			};
		}
	}
}
