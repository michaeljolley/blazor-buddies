using BlazorBuddies.Core.Common;
using BlazorBuddies.Core.Data;
using BlazorBuddies.Web.States;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBuddies.Web.Extensions
{
	static internal class IApplicationBuilderExtensions
	{
		static internal IApplicationBuilder InitAppState(this IApplicationBuilder builder)
		{
			var state = builder.ApplicationServices.GetRequiredService<ApplicationState>();
			var dbContextFactory = builder.ApplicationServices.GetRequiredService<IDbContextFactory<BuddyDbContext>>();
			using var dbContext = dbContextFactory.CreateDbContext();
#if DEBUG
			// Let's make sure our database is always up-to-date while developing
			if (dbContext.Database.GetPendingMigrations().Any()) {
				dbContext.Database.Migrate();
			}
			// Let's include at least one school with buddies while developing
			if (!dbContext.Schools.Any()) {
				dbContext.Schools.Add(new School {
					Id = Guid.NewGuid(),
					Name = "Test",
					City = "TestVille",
					Buddies = 707
				});
				dbContext.SaveChanges();
			}
#endif
			state.BuddyCount = dbContext.Schools.Sum(s => s.Buddies);
			return builder;
		}
	}
}
