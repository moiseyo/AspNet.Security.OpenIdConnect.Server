using System;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace Nancy.Client {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureOidcClientDemo(app);
        }

        private static void ConfigureOidcClientDemo(IAppBuilder app) {
            app.SetDefaultSignInAsAuthenticationType("ClientCookie");

            // Insert a new cookies middleware in the pipeline to store the user
            // identity after he has been redirect from the identity provider.
            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = "ClientCookie",
                CookieName = CookieAuthenticationDefaults.CookiePrefix + "ClientCookie",
                ExpireTimeSpan = TimeSpan.FromMinutes(5)
            });

            var key = new InMemorySymmetricSecurityKey(Convert.FromBase64String("Srtjyi8wMFfmP9Ub8U2ieVGAcrP/7gK3VM/K6KfJ/fI="));

            // Insert a new OIDC client middleware in the pipeline.
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions {
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = OpenIdConnectAuthenticationDefaults.AuthenticationType,
                SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType(),

                // Note: these settings must match the application details inserted in
                // the database at the server level (see ApplicationContextInitializer.cs).
                ClientId = "myClient",
                ClientSecret = "secret_secret_secret",
                RedirectUri = "http://localhost:56765/oidc",

                Scope = "openid",

                // Note: these settings must match the endpoints and the token
                // parameters defined in Startup.cs at the server level.
                Configuration = new OpenIdConnectConfiguration {
                    AuthorizationEndpoint = "http://localhost:55938/oauth2/authorize",
                    TokenEndpoint = "http://localhost:55938/oauth2/access_token"
                },
                TokenValidationParameters = new TokenValidationParameters() {
                    ValidAudience = "myClient",
                    ValidIssuer = "urn:authServer",
                    IssuerSigningKey = key
                }
            });

            app.UseNancy(options => options.PerformPassThrough = context => context.Response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}