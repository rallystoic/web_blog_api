using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//  file System
using System.IO;


// additional folder
using finalBlog.Models;
using finalBlog.Helper;
using finalBlog.Services;

// Entityframework core 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Design;


// Json web token authentication section
using System.Text;
//jwt  
using Microsoft.IdentityModel.Tokens;
// Authentication JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;

// reverse proxy forward request to the app
using Microsoft.AspNetCore.HttpOverrides;

namespace finalBlog
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// binding appsetting.json connectionstring section to config object
			ConnectionStrings  config = new ConnectionStrings();
			Configuration.Bind("ConnectionStrings", config); // <--this
			services.AddSingleton(config);
			// cors policy
			services.AddCors(options =>
					{
					options.AddPolicy("cors",
							builder => 
							{
							builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
							}
							);
					}
					);
			services.AddControllers();
			//	DATABASE
			services.AddDbContext<BlogContext>(options => options.UseSqlite("Data Source=blog.db"));

			// DependencyInjection
			services.AddTransient<IAuthenticationService, AuthenticationService>();
			services.AddTransient<IImageService, ImageService>();
			//******************************************************
			// JWT AUTHENTICATION SECTION
			//******************************************************
			var SecretKey = Encoding.ASCII.GetBytes(config.Secret);
			//Authentication
			services.AddAuthentication(auth => {
					// authentication to jwt
					auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;        
					})
			.AddJwtBearer(token => {
					// for https
					token.RequireHttpsMetadata = false;
					token.SaveToken = true;
					token.TokenValidationParameters = new TokenValidationParameters {
					ValidateIssuerSigningKey = true,
					//Secretkey 
					IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
					ValidateIssuer = false,
					ValidateAudience = false,
					RequireExpirationTime = true
					};
					});
			// Authorization 
			services.AddAuthorization(options => {
					options.AddPolicy("Require_author",
							policy => policy.RequireRole("author","administrator")
							);
					options.AddPolicy("RequireAdministrator",
							policy => policy.RequireRole("administrator")
							);
					});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseForwardedHeaders(new ForwardedHeadersOptions
					{
					ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
					});
            app.UseRouting();
	    app.UseCors("cors");
	    // Authentication before Authorization for jwt to work
	    app.UseAuthentication();
            app.UseAuthorization();
            //app.UseHttpsRedirection();
	    app.UseStaticFiles();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
