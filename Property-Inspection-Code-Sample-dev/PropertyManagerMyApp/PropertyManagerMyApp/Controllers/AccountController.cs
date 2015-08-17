﻿using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using SuiteLevelWebApp.Utils;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SuiteLevelWebApp.Controllers
{
    public class AccountController : Controller
    {
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            // Remove all cache entries for this user and send an OpenID Connect sign-out request.
            string usrObjectId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            AuthenticationContext authContext = new AuthenticationContext(AADAppSettings.Authority, new NaiveSessionCache(usrObjectId));
            authContext.TokenCache.Clear();

            HttpContext.GetOwinContext().Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult ConsentApp()
        {
            string strResource = Request.QueryString["resource"];
            string strRedirectController = Request.QueryString["redirect"];

            string authorizationRequest = String.Format(
                "/common/oauth2/authorize?response_type=code&client_id={0}&resource={1}&redirect_uri={2}",
                    AADAppSettings.AuthorizationUri,
                    Uri.EscapeDataString(AADAppSettings.ClientId),
                    Uri.EscapeDataString(strResource),
                    Uri.EscapeDataString(String.Format("{0}/{1}", this.Request.Url.GetLeftPart(UriPartial.Authority).ToString(), strRedirectController))
                    );

            return new RedirectResult(authorizationRequest);
        }

        public ActionResult AdminConsentApp()
        {
            string strResource = Request.QueryString["resource"];
            string strRedirectController = Request.QueryString["redirect"];

            string authorizationRequest = String.Format(
                "/common/oauth2/authorize?response_type=code&client_id={0}&resource={1}&redirect_uri={2}&prompt={3}",
                    AADAppSettings.AuthorizationUri,
                    Uri.EscapeDataString(AADAppSettings.ClientId),
                    Uri.EscapeDataString(strResource),
                    Uri.EscapeDataString(String.Format("{0}/{1}", this.Request.Url.GetLeftPart(UriPartial.Authority).ToString(), strRedirectController)),
                    Uri.EscapeDataString("admin_consent")
                    );

            return new RedirectResult(authorizationRequest);
        }

        public void RefreshSession()
        {
            string strRedirectController = Request.QueryString["redirect"];

            HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = String.Format("/{0}", strRedirectController) }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
        }
    }
}