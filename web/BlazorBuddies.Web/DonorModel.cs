using BlazorBuddies.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace BlazorBuddies.Web
{
	public class DonorModel : Donor
	{
		public bool Selected { get; set; } = false;
		public bool? ContactSuccessful { get; set; }

		public DonorModel(Donor donor)
		{
			foreach (var property in typeof(Donor).GetProperties(BindingFlags.Public|BindingFlags.Instance)) {
				property.SetValue(this, property.GetValue(donor));
			}
		}
	}

	public static class DonorExtensions
  {
		public static DonorModel ToDonorModel(this Donor donor)
		{
			return new DonorModel(donor);
		}

		public static IEnumerable<DonorModel> ToDonorModels(this IEnumerable<Donor> donors)
		{
			return donors.Select(x => x.ToDonorModel());
		}
  }
}
