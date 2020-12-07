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
			ReflectionCache<Donor>.ClonePropertyValues(donor, this);
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
