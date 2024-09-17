using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Services.CouponAPI.Extensions
{
	public static class WebApplicationBuilderExtensions
	{
		public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder) {
			//agregamos autenticacion para cupones
			var appiSetting = builder.Configuration.GetSection("ApiSettings");
			var secret = appiSetting.GetValue<string>("Secret");
			var issuer = appiSetting.GetValue<string>("Issuer");
			var audience = appiSetting.GetValue<string>("Audience");
			var key = Encoding.ASCII.GetBytes(secret);
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x => {
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = issuer,
					ValidAudience = audience,
					ValidateAudience = true

				};
			});
			return builder;
		}

	}
}
