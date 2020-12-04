using BlazorBuddies.Core.Data;
using BlazorBuddies.Web.Extensions;
using BlazorBuddies.Web.Hubs;
using BlazorBuddies.Web.States;

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

using Vonage;
using Vonage.Messaging;
using Vonage.Request;
using Vonage.Utility;

namespace BlazorBuddies.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			Configuration = configuration;
			Environment = environment;
		}

		public IConfiguration Configuration { get; }
		public IWebHostEnvironment Environment { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAdB2C"));

			services.AddControllersWithViews().AddMicrosoftIdentityUI();
			
			services.AddAuthorization(options => options.FallbackPolicy = options.DefaultPolicy);

			var creds = Credentials.FromApiKeyAndSecret(Configuration["VONAGE_API_KEY"], Configuration["VONAGE_API_SECRET"]);
			services.AddSingleton(new VonageClient(creds));

			services.AddSignalR();

			// Core Services
			services.AddRazorPages(options => {
				options.Conventions.AllowAnonymousToFolder("/");
				options.Conventions.AllowAnonymousToFolder("/donorHub");
			});
			services.AddServerSideBlazor(options => options.DetailedErrors = Environment.IsDevelopment()).AddMicrosoftIdentityConsentHandler();

			services.AddResponseCaching();
			services.AddResponseCompression(options => {
				options.Providers.Add<BrotliCompressionProvider>();
				options.Providers.Add<GzipCompressionProvider>();
			});

			//Recommended approach is to create DbContexts in Blazor Server-Side using a DbContextFactory
			services.AddDbContextFactory<BuddyDbContext>(opt =>
				opt.UseSqlServer(Configuration.GetConnectionString("BuddyDb"), b => b.MigrationsAssembly("BlazorBuddies.Web")));
			services.AddDbContext<BuddyDbContext>();
			services.AddScoped<DonorService>();

			// States
			services.AddSingleton<ApplicationState>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, ApplicationState applicationState, ILogger<Startup> logger)
		{
			if (Environment.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			else {
				app.UseExceptionHandler("/Error");

				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			// Security Settings
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			// Basic Site Settings
			app.UseResponseCompression();
			app.UseResponseCaching();
			app.UseStaticFiles(new StaticFileOptions {
				// Second reference is required because Blazor does interesting things with the first
				// This is used to control static file cache time
				OnPrepareResponse = ctx => ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={3600}")
			});

			// Initialize the application state
			app.InitAppState();

			// Routing 
			app.UseRouting();

			//app.UseAuthentication();
			//app.UseAuthorization();

			_ = app.UseEndpoints(endpoints => {
				endpoints.MapControllers();

				endpoints.MapHub<DonorHub>("/donorHub");

				// Health Check
				// TODO: Use the actual HealthCheck libraries in ASP.NET to handle DB health checks.
				endpoints.MapGet("/healthcheck",
					async context => {
						// make a call to fast EF method to validate db connection set efValid false if db issue
						var efValid = true;

						if (!efValid) {
							// reply non-200 on db fail
							context.Response.StatusCode = StatusCodes.Status428PreconditionRequired;
						}

						// Return ok
						await context.Response.CompleteAsync().ConfigureAwait(false);
					});


				// Blazor
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");

				logger.LogInformation("Application Started");
			});
		}
	}
}
