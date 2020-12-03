using System;

namespace BlazorBuddies.Core.Common
{
	public static class EntityFactory
	{
		public static School CreateSchool(string name, string city, int buddies)
		{
			return new School {
				Id = Guid.NewGuid(),
				Name = name,
				City = city,
				Buddies = buddies
			};
		}

		public static Donor CreateDonor(string firstName, string lastName, string emailAddress, string streetAddress1, string streetAddress2, string city,
			string state, string postalCode, string phoneNumber, bool optOut)
		{
			return new Donor {
				Id = Guid.NewGuid(),
				FirstName = firstName,
				LastName = lastName,
				EmailAddress = emailAddress,
				StreetAddress1 = streetAddress1,
				StreetAddress2 = streetAddress2,
				City = city,
				State = state,
				PostalCode = postalCode,
				PhoneNumber = phoneNumber,
				OptOut = optOut
			};
		}
	}
}
