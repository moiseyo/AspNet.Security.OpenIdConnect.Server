﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/AspNet-OpenIdConnect-Server/Owin.Security.OpenIdConnect.Server
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Threading.Tasks;

namespace Owin.Security.OpenIdConnect.Server {
    /// <summary>
    /// Interface for OpenIdConnectServerOptions.Provider property used by Authorization
    /// Server to communicate with the web application while processing requests.
    /// </summary>
    public interface IOpenIdConnectServerProvider {
        /// <summary>
        /// Called to determine if an incoming request is treated as an authorization or token
        /// endpoint. If Options.AuthorizationEndpointPath or Options.TokenEndpointPath
        /// are assigned values, then handling this event is optional and context.IsAuthorizationEndpoint and context.IsTokenEndpoint
        /// will already be true if the request path matches.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task MatchEndpoint(OpenIdConnectMatchEndpointContext context);

        /// <summary>
        /// Called to validate that the context.ClientId is a registered "client_id", and that the context.RedirectUri a "redirect_uri" 
        /// registered for that client. This only occurs when processing the authorization endpoint. The application MUST implement this
        /// call, and it MUST validate both of those factors before calling context.Validated. If the context.Validated method is called
        /// with a given redirectUri parameter, then IsValidated will only become true if the incoming redirect URI matches the given redirect URI. 
        /// If context.Validated is not called the request will not proceed further. 
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task ValidateClientRedirectUri(OpenIdConnectValidateClientRedirectUriContext context);

        /// <summary>
        /// Called to validate that the origin of the request is a registered "client_id", and that the correct credentials for that client are
        /// present on the request. If the web application accepts Basic authentication credentials, 
        /// context.TryGetBasicCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request header. If the web 
        /// application accepts "client_id" and "client_secret" as form encoded POST parameters, 
        /// context.TryGetFormCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request body.
        /// If context.Validated is not called the request will not proceed further. 
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task ValidateClientAuthentication(OpenIdConnectValidateClientAuthenticationContext context);

        /// <summary>
        /// Called for each request to the authorization endpoint to determine if the request is valid and should continue. 
        /// The default behavior when using the OpenIdConnectServerProvider is to assume well-formed requests, with 
        /// validated client redirect URI, should continue processing. An application may add any additional constraints.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task ValidateAuthorizationRequest(OpenIdConnectValidateAuthorizationRequestContext context);

        /// <summary>
        /// Called for each request to the Token endpoint to determine if the request is valid and should continue. 
        /// The default behavior when using the OpenIdConnectServerProvider is to assume well-formed requests, with 
        /// validated client credentials, should continue processing. An application may add any additional constraints.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task ValidateTokenRequest(OpenIdConnectValidateTokenRequestContext context);

        /// <summary>
        /// Called when a request to the Token endpoint arrives with a "grant_type" of "authorization_code". This occurs after the authorization
        /// endpoint as redirected the user-agent back to the client with a "code" parameter, and the client is exchanging that for an "access_token".
        /// The claims and properties 
        /// associated with the authorization code are present in the context.Ticket. The application must call context.Validated to instruct the Authorization
        /// Server middleware to issue an access token based on those claims and properties. The call to context.Validated may be given a different
        /// AuthenticationTicket or ClaimsIdentity in order to control which information flows from authorization code to access token.
        /// The default behavior when using the OpenIdConnectServerProvider is to flow information from the authorization code to 
        /// the access token unmodified.
        /// See also http://tools.ietf.org/html/rfc6749#section-4.1.3
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task GrantAuthorizationCode(OpenIdConnectGrantAuthorizationCodeContext context);

        /// <summary>
        /// Called when a request to the Token endpoint arrives with a "grant_type" of "refresh_token". This occurs if your application has issued a "refresh_token" 
        /// along with the "access_token", and the client is attempting to use the "refresh_token" to acquire a new "access_token", and possibly a new "refresh_token".
        /// To issue a refresh token the an Options.RefreshTokenProvider must be assigned to create the value which is returned. The claims and properties 
        /// associated with the refresh token are present in the context.Ticket. The application must call context.Validated to instruct the 
        /// Authorization Server middleware to issue an access token based on those claims and properties. The call to context.Validated may 
        /// be given a different AuthenticationTicket or ClaimsIdentity in order to control which information flows from the refresh token to 
        /// the access token. The default behavior when using the OpenIdConnectServerProvider is to flow information from the refresh token to 
        /// the access token unmodified.
        /// See also http://tools.ietf.org/html/rfc6749#section-6
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task GrantRefreshToken(OpenIdConnectGrantRefreshTokenContext context);

        /// <summary>
        /// Called when a request to the Token endpoint arrives with a "grant_type" of "password". This occurs when the user has provided name and password
        /// credentials directly into the client application's user interface, and the client application is using those to acquire an "access_token" and 
        /// optional "refresh_token". If the web application supports the
        /// resource owner credentials grant type it must validate the context.Username and context.Password as appropriate. To issue an
        /// access token the context.Validated must be called with a new ticket containing the claims about the resource owner which should be associated
        /// with the access token. The application should take appropriate measures to ensure that the endpoint isn't abused by malicious callers.  . 
        /// The default behavior is to reject this grant type.
        /// See also http://tools.ietf.org/html/rfc6749#section-4.3.2
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task GrantResourceOwnerCredentials(OpenIdConnectGrantResourceOwnerCredentialsContext context);

        /// <summary>
        /// Called when a request to the Token endpoint arrives with a "grant_type" of "client_credentials". This occurs when a registered client
        /// application wishes to acquire an "access_token" to interact with protected resources on it's own behalf, rather than on behalf of an authenticated user. 
        /// If the web application supports the client credentials it may assume the context.ClientId has been validated by the ValidateClientAuthentication call.
        /// To issue an access token the context.Validated must be called with a new ticket containing the claims about the client application which should be associated
        /// with the access token. The application should take appropriate measures to ensure that the endpoint isn't abused by malicious callers.
        /// The default behavior is to reject this grant type.
        /// See also http://tools.ietf.org/html/rfc6749#section-4.4.2
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task GrantClientCredentials(OpenIdConnectGrantClientCredentialsContext context);

        /// <summary>
        /// Called when a request to the Token andpoint arrives with a "grant_type" of any other value. If the application supports custom grant types
        /// it is entirely responsible for determining if the request should result in an access_token. If context.Validated is called with ticket
        /// information the response body is produced in the same way as the other standard grant types. If additional response parameters must be
        /// included they may be added in the final TokenEndpoint call.
        /// See also http://tools.ietf.org/html/rfc6749#section-4.5
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task GrantCustomExtension(OpenIdConnectGrantCustomExtensionContext context);

        /// <summary>
        /// Called at the final stage of an incoming authorization endpoint request before the execution continues on to the web application component 
        /// responsible for producing the html response. Anything present in the OWIN pipeline following the Authorization Server may produce the
        /// response for the authorization page. If running on IIS any ASP.NET technology running on the server may produce the response for the 
        /// authorization page. If the web application wishes to produce the response directly in the AuthorizationEndpoint call it may write to the 
        /// context.Response directly and should call context.RequestCompleted to stop other handlers from executing. If the web application wishes
        /// to grant the authorization directly in the AuthorizationEndpoint call it cay call context.OwinContext.Authentication.SignIn with the
        /// appropriate ClaimsIdentity and should call context.RequestCompleted to stop other handlers from executing.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task AuthorizationEndpoint(OpenIdConnectAuthorizationEndpointContext context);

        /// <summary>
        /// Called before the AuthorizationEndpoint redirects its response to the caller.
        /// The response could contain an access token when using implicit flow or
        /// an authorization code when using the authorization code flow.
        /// If the web application wishes to produce the authorization response directly in the AuthorizationEndpoint call it may write to the 
        /// context.Response directly and should call context.RequestCompleted to stop other handlers from executing.
        /// This call may also be used to add additional response parameters to the authorization response.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task AuthorizationEndpointResponse(OpenIdConnectAuthorizationEndpointResponseContext context);

        /// <summary>
        /// Called by the client applications to retrieve the OpenID Connect configuration associated with this instance.
        /// If the web application wishes to produce the configuration metadata directly in this call, it may write to the 
        /// context.Response directly and should call context.RequestCompleted to stop the default behavior from executing.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task ConfigurationEndpoint(OpenIdConnectConfigurationEndpointContext context);

        /// <summary>
        /// Called before the authorization server starts emitting the OpenID Connect configuration associated with this instance.
        /// If the web application wishes to produce the configuration metadata directly in this call, it may write to the 
        /// context.Response directly and should call context.RequestCompleted to stop the default behavior from executing.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task ConfigurationEndpointResponse(OpenIdConnectConfigurationEndpointResponseContext context);

        /// <summary>
        /// Called by the client applications to retrieve the OpenID Connect JSON Web Key set associated with this instance.
        /// If the web application wishes to produce the JSON Web Key set directly in this call, it may write to the 
        /// context.Response directly and should call context.RequestCompleted to stop the default behavior from executing.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task CryptoEndpoint(OpenIdConnectCryptoEndpointContext context);

        /// <summary>
        /// Called before the authorization server starts emitting the OpenID Connect JSON Web Key set associated with this instance.
        /// If the web application wishes to produce the JSON Web Key set directly in this call, it may write to the 
        /// context.Response directly and should call context.RequestCompleted to stop the default behavior from executing.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task CryptoEndpointResponse(OpenIdConnectCryptoEndpointResponseContext context);

        /// <summary>
        /// Called at the final stage of a successful Token endpoint request. An application may implement this call in order to do any final 
        /// modification of the claims being used to issue access or refresh tokens. This call may also be used in order to add additional 
        /// response parameters to the Token endpoint's json response body.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task TokenEndpoint(OpenIdConnectTokenEndpointContext context);

        /// <summary>
        /// Called before the TokenEndpoint redirects its response to the caller. 
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task TokenEndpointResponse(OpenIdConnectTokenEndpointResponseContext context);
    }
}
