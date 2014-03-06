using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using CAS.Models;

namespace CAS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Country = Countries.countryList;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, Country = model.Country };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion


    }

    #region Country List
    public static class Countries
    {
        public static SelectListItem[] countryList = new[]
                {
                    new SelectListItem { Value = "AF", Text = "AFGHANISTAN"}, 
                    new SelectListItem { Value = "AX", Text = "ALAND ISLANDS"}, 
                    new SelectListItem { Value = "AL", Text = "ALBANIA"}, 
                    new SelectListItem { Value = "DZ", Text = "ALGERIA"}, 
                    new SelectListItem { Value = "AS", Text = "AMERICAN SAMOA"}, 
                    new SelectListItem { Value = "AD", Text = "ANDORRA"}, 
                    new SelectListItem { Value = "AO", Text = "ANGOLA"}, 
                    new SelectListItem { Value = "AI", Text = "ANGUILLA"}, 
                    new SelectListItem { Value = "AQ", Text = "ANTARCTICA"}, 
                    new SelectListItem { Value = "AG", Text = "ANTIGUA AND BARBUDA"}, 
                    new SelectListItem { Value = "AR", Text = "ARGENTINA"}, 
                    new SelectListItem { Value = "AM", Text = "ARMENIA"}, 
                    new SelectListItem { Value = "AW", Text = "ARUBA"}, 
                    new SelectListItem { Value = "AU", Text = "AUSTRALIA"}, 
                    new SelectListItem { Value = "AT", Text = "AUSTRIA"}, 
                    new SelectListItem { Value = "AZ", Text = "AZERBAIJAN"}, 
                    new SelectListItem { Value = "BS", Text = "BAHAMAS"}, 
                    new SelectListItem { Value = "BH", Text = "BAHRAIN"}, 
                    new SelectListItem { Value = "BD", Text = "BANGLADESH"}, 
                    new SelectListItem { Value = "BB", Text = "BARBADOS"}, 
                    new SelectListItem { Value = "BY", Text = "BELARUS"}, 
                    new SelectListItem { Value = "BE", Text = "BELGIUM"}, 
                    new SelectListItem { Value = "BZ", Text = "BELIZE"}, 
                    new SelectListItem { Value = "BJ", Text = "BENIN"}, 
                    new SelectListItem { Value = "BM", Text = "BERMUDA"}, 
                    new SelectListItem { Value = "BT", Text = "BHUTAN"}, 
                    new SelectListItem { Value = "BO", Text = "BOLIVIA, PLURINATIONAL STATE OF"}, 
                    new SelectListItem { Value = "BQ", Text = "BONAIRE, SINT EUSTATIUS AND SABA"}, 
                    new SelectListItem { Value = "BA", Text = "BOSNIA AND HERZEGOVINA"}, 
                    new SelectListItem { Value = "BW", Text = "BOTSWANA"}, 
                    new SelectListItem { Value = "BV", Text = "BOUVET ISLAND"}, 
                    new SelectListItem { Value = "BR", Text = "BRAZIL"}, 
                    new SelectListItem { Value = "IO", Text = "BRITISH INDIAN OCEAN TERRITORY"}, 
                    new SelectListItem { Value = "BN", Text = "BRUNEI DARUSSALAM"}, 
                    new SelectListItem { Value = "BG", Text = "BULGARIA"}, 
                    new SelectListItem { Value = "BF", Text = "BURKINA FASO"}, 
                    new SelectListItem { Value = "BI", Text = "BURUNDI"}, 
                    new SelectListItem { Value = "KH", Text = "CAMBODIA"}, 
                    new SelectListItem { Value = "CM", Text = "CAMEROON"}, 
                    new SelectListItem { Value = "CA", Text = "CANADA"}, 
                    new SelectListItem { Value = "CV", Text = "CAPE VERDE"}, 
                    new SelectListItem { Value = "KY", Text = "CAYMAN ISLANDS"}, 
                    new SelectListItem { Value = "CF", Text = "CENTRAL AFRICAN REPUBLIC"}, 
                    new SelectListItem { Value = "TD", Text = "CHAD"}, 
                    new SelectListItem { Value = "CL", Text = "CHILE"}, 
                    new SelectListItem { Value = "CN", Text = "CHINA"}, 
                    new SelectListItem { Value = "CX", Text = "CHRISTMAS ISLAND"}, 
                    new SelectListItem { Value = "CC", Text = "COCOS (KEELING) ISLANDS"}, 
                    new SelectListItem { Value = "CO", Text = "COLOMBIA"}, 
                    new SelectListItem { Value = "KM", Text = "COMOROS"}, 
                    new SelectListItem { Value = "CG", Text = "CONGO"}, 
                    new SelectListItem { Value = "CD", Text = "CONGO, THE DEMOCRATIC REPUBLIC OF THE"}, 
                    new SelectListItem { Value = "CK", Text = "COOK ISLANDS"}, 
                    new SelectListItem { Value = "CR", Text = "COSTA RICA"}, 
                    new SelectListItem { Value = "CI", Text = "CÔTE D'IVOIRE"}, 
                    new SelectListItem { Value = "HR", Text = "CROATIA"}, 
                    new SelectListItem { Value = "CU", Text = "CUBA"}, 
                    new SelectListItem { Value = "CW", Text = "CURAÇAO"}, 
                    new SelectListItem { Value = "CY", Text = "CYPRUS"}, 
                    new SelectListItem { Value = "CZ", Text = "CZECH REPUBLIC"}, 
                    new SelectListItem { Value = "DK", Text = "DENMARK"}, 
                    new SelectListItem { Value = "DJ", Text = "DJIBOUTI"}, 
                    new SelectListItem { Value = "DM", Text = "DOMINICA"}, 
                    new SelectListItem { Value = "DO", Text = "DOMINICAN REPUBLIC"}, 
                    new SelectListItem { Value = "EC", Text = "ECUADOR"}, 
                    new SelectListItem { Value = "EG", Text = "EGYPT"}, 
                    new SelectListItem { Value = "SV", Text = "EL SALVADOR"}, 
                    new SelectListItem { Value = "GQ", Text = "EQUATORIAL GUINEA"}, 
                    new SelectListItem { Value = "ER", Text = "ERITREA"}, 
                    new SelectListItem { Value = "EE", Text = "ESTONIA"}, 
                    new SelectListItem { Value = "ET", Text = "ETHIOPIA"}, 
                    new SelectListItem { Value = "FK", Text = "FALKLAND ISLANDS (MALVINAS)"}, 
                    new SelectListItem { Value = "FO", Text = "FAROE ISLANDS"}, 
                    new SelectListItem { Value = "FJ", Text = "FIJI"}, 
                    new SelectListItem { Value = "FI", Text = "FINLAND"}, 
                    new SelectListItem { Value = "FR", Text = "FRANCE"}, 
                    new SelectListItem { Value = "GF", Text = "FRENCH GUIANA"}, 
                    new SelectListItem { Value = "PF", Text = "FRENCH POLYNESIA"}, 
                    new SelectListItem { Value = "TF", Text = "FRENCH SOUTHERN TERRITORIES"}, 
                    new SelectListItem { Value = "GA", Text = "GABON"}, 
                    new SelectListItem { Value = "GM", Text = "GAMBIA"}, 
                    new SelectListItem { Value = "GE", Text = "GEORGIA"}, 
                    new SelectListItem { Value = "DE", Text = "GERMANY"}, 
                    new SelectListItem { Value = "GH", Text = "GHANA"}, 
                    new SelectListItem { Value = "GI", Text = "GIBRALTAR"}, 
                    new SelectListItem { Value = "GR", Text = "GREECE"}, 
                    new SelectListItem { Value = "GL", Text = "GREENLAND"}, 
                    new SelectListItem { Value = "GD", Text = "GRENADA"}, 
                    new SelectListItem { Value = "GP", Text = "GUADELOUPE"}, 
                    new SelectListItem { Value = "GU", Text = "GUAM"}, 
                    new SelectListItem { Value = "GT", Text = "GUATEMALA"}, 
                    new SelectListItem { Value = "GG", Text = "GUERNSEY"}, 
                    new SelectListItem { Value = "GN", Text = "GUINEA"}, 
                    new SelectListItem { Value = "GW", Text = "GUINEA-BISSAU"}, 
                    new SelectListItem { Value = "GY", Text = "GUYANA"}, 
                    new SelectListItem { Value = "HT", Text = "HAITI"}, 
                    new SelectListItem { Value = "HM", Text = "HEARD ISLAND AND MCDONALD ISLANDS"}, 
                    new SelectListItem { Value = "VA", Text = "HOLY SEE (VATICAN CITY STATE)"}, 
                    new SelectListItem { Value = "HN", Text = "HONDURAS"}, 
                    new SelectListItem { Value = "HK", Text = "HONG KONG"}, 
                    new SelectListItem { Value = "HU", Text = "HUNGARY"}, 
                    new SelectListItem { Value = "IS", Text = "ICELAND"}, 
                    new SelectListItem { Value = "IN", Text = "INDIA"}, 
                    new SelectListItem { Value = "ID", Text = "INDONESIA"}, 
                    new SelectListItem { Value = "IR", Text = "IRAN, ISLAMIC REPUBLIC OF"}, 
                    new SelectListItem { Value = "IQ", Text = "IRAQ"}, 
                    new SelectListItem { Value = "IE", Text = "IRELAND"}, 
                    new SelectListItem { Value = "IM", Text = "ISLE OF MAN"}, 
                    new SelectListItem { Value = "IL", Text = "ISRAEL"}, 
                    new SelectListItem { Value = "IT", Text = "ITALY"}, 
                    new SelectListItem { Value = "JM", Text = "JAMAICA"}, 
                    new SelectListItem { Value = "JP", Text = "JAPAN"}, 
                    new SelectListItem { Value = "JE", Text = "JERSEY"}, 
                    new SelectListItem { Value = "JO", Text = "JORDAN"}, 
                    new SelectListItem { Value = "KZ", Text = "KAZAKHSTAN"}, 
                    new SelectListItem { Value = "KE", Text = "KENYA"}, 
                    new SelectListItem { Value = "KI", Text = "KIRIBATI"}, 
                    new SelectListItem { Value = "KP", Text = "KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF"}, 
                    new SelectListItem { Value = "KR", Text = "KOREA, REPUBLIC OF"}, 
                    new SelectListItem { Value = "KW", Text = "KUWAIT"}, 
                    new SelectListItem { Value = "KG", Text = "KYRGYZSTAN"}, 
                    new SelectListItem { Value = "LA", Text = "LAO PEOPLE'S DEMOCRATIC REPUBLIC"}, 
                    new SelectListItem { Value = "LV", Text = "LATVIA"}, 
                    new SelectListItem { Value = "LB", Text = "LEBANON"}, 
                    new SelectListItem { Value = "LS", Text = "LESOTHO"}, 
                    new SelectListItem { Value = "LR", Text = "LIBERIA"}, 
                    new SelectListItem { Value = "LY", Text = "LIBYA"}, 
                    new SelectListItem { Value = "LI", Text = "LIECHTENSTEIN"}, 
                    new SelectListItem { Value = "LT", Text = "LITHUANIA"}, 
                    new SelectListItem { Value = "LU", Text = "LUXEMBOURG"}, 
                    new SelectListItem { Value = "MO", Text = "MACAO"}, 
                    new SelectListItem { Value = "MK", Text = "MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF"}, 
                    new SelectListItem { Value = "MG", Text = "MADAGASCAR"}, 
                    new SelectListItem { Value = "MW", Text = "MALAWI"}, 
                    new SelectListItem { Value = "MY", Text = "MALAYSIA"}, 
                    new SelectListItem { Value = "MV", Text = "MALDIVES"}, 
                    new SelectListItem { Value = "ML", Text = "MALI"}, 
                    new SelectListItem { Value = "MT", Text = "MALTA"}, 
                    new SelectListItem { Value = "MH", Text = "MARSHALL ISLANDS"}, 
                    new SelectListItem { Value = "MQ", Text = "MARTINIQUE"}, 
                    new SelectListItem { Value = "MR", Text = "MAURITANIA"}, 
                    new SelectListItem { Value = "MU", Text = "MAURITIUS"}, 
                    new SelectListItem { Value = "YT", Text = "MAYOTTE"}, 
                    new SelectListItem { Value = "MX", Text = "MEXICO"}, 
                    new SelectListItem { Value = "FM", Text = "MICRONESIA, FEDERATED STATES OF"}, 
                    new SelectListItem { Value = "MD", Text = "MOLDOVA, REPUBLIC OF"}, 
                    new SelectListItem { Value = "MC", Text = "MONACO"}, 
                    new SelectListItem { Value = "MN", Text = "MONGOLIA"}, 
                    new SelectListItem { Value = "ME", Text = "MONTENEGRO"}, 
                    new SelectListItem { Value = "MS", Text = "MONTSERRAT"}, 
                    new SelectListItem { Value = "MA", Text = "MOROCCO"}, 
                    new SelectListItem { Value = "MZ", Text = "MOZAMBIQUE"}, 
                    new SelectListItem { Value = "MM", Text = "MYANMAR"}, 
                    new SelectListItem { Value = "NA", Text = "NAMIBIA"}, 
                    new SelectListItem { Value = "NR", Text = "NAURU"}, 
                    new SelectListItem { Value = "NP", Text = "NEPAL"}, 
                    new SelectListItem { Value = "NL", Text = "NETHERLANDS"}, 
                    new SelectListItem { Value = "NC", Text = "NEW CALEDONIA"}, 
                    new SelectListItem { Value = "NZ", Text = "NEW ZEALAND"}, 
                    new SelectListItem { Value = "NI", Text = "NICARAGUA"}, 
                    new SelectListItem { Value = "NE", Text = "NIGER"}, 
                    new SelectListItem { Value = "NG", Text = "NIGERIA"}, 
                    new SelectListItem { Value = "NU", Text = "NIUE"}, 
                    new SelectListItem { Value = "NF", Text = "NORFOLK ISLAND"}, 
                    new SelectListItem { Value = "MP", Text = "NORTHERN MARIANA ISLANDS"}, 
                    new SelectListItem { Value = "NO", Text = "NORWAY"}, 
                    new SelectListItem { Value = "OM", Text = "OMAN"}, 
                    new SelectListItem { Value = "PK", Text = "PAKISTAN"}, 
                    new SelectListItem { Value = "PW", Text = "PALAU"}, 
                    new SelectListItem { Value = "PS", Text = "PALESTINIAN TERRITORY, OCCUPIED"}, 
                    new SelectListItem { Value = "PA", Text = "PANAMA"}, 
                    new SelectListItem { Value = "PG", Text = "PAPUA NEW GUINEA"}, 
                    new SelectListItem { Value = "PY", Text = "PARAGUAY"}, 
                    new SelectListItem { Value = "PE", Text = "PERU"}, 
                    new SelectListItem { Value = "PH", Text = "PHILIPPINES"}, 
                    new SelectListItem { Value = "PN", Text = "PITCAIRN"}, 
                    new SelectListItem { Value = "PL", Text = "POLAND"}, 
                    new SelectListItem { Value = "PT", Text = "PORTUGAL"}, 
                    new SelectListItem { Value = "PR", Text = "PUERTO RICO"}, 
                    new SelectListItem { Value = "QA", Text = "QATAR"}, 
                    new SelectListItem { Value = "RE", Text = "RÉUNION"}, 
                    new SelectListItem { Value = "RO", Text = "ROMANIA"}, 
                    new SelectListItem { Value = "RU", Text = "RUSSIAN FEDERATION"}, 
                    new SelectListItem { Value = "RW", Text = "RWANDA"}, 
                    new SelectListItem { Value = "BL", Text = "SAINT BARTHÉLEMY"}, 
                    new SelectListItem { Value = "SH", Text = "SAINT HELENA, ASCENSION AND TRISTAN DA CUNHA"}, 
                    new SelectListItem { Value = "KN", Text = "SAINT KITTS AND NEVIS"}, 
                    new SelectListItem { Value = "LC", Text = "SAINT LUCIA"}, 
                    new SelectListItem { Value = "MF", Text = "SAINT MARTIN (FRENCH PART)"}, 
                    new SelectListItem { Value = "PM", Text = "SAINT PIERRE AND MIQUELON"}, 
                    new SelectListItem { Value = "VC", Text = "SAINT VINCENT AND THE GRENADINES"}, 
                    new SelectListItem { Value = "WS", Text = "SAMOA"}, 
                    new SelectListItem { Value = "SM", Text = "SAN MARINO"}, 
                    new SelectListItem { Value = "ST", Text = "SAO TOME AND PRINCIPE"}, 
                    new SelectListItem { Value = "SA", Text = "SAUDI ARABIA"}, 
                    new SelectListItem { Value = "SN", Text = "SENEGAL"}, 
                    new SelectListItem { Value = "RS", Text = "SERBIA"}, 
                    new SelectListItem { Value = "SC", Text = "SEYCHELLES"}, 
                    new SelectListItem { Value = "SL", Text = "SIERRA LEONE"}, 
                    new SelectListItem { Value = "SG", Text = "SINGAPORE"}, 
                    new SelectListItem { Value = "SX", Text = "SINT MAARTEN (DUTCH PART)"}, 
                    new SelectListItem { Value = "SK", Text = "SLOVAKIA"}, 
                    new SelectListItem { Value = "SI", Text = "SLOVENIA"}, 
                    new SelectListItem { Value = "SB", Text = "SOLOMON ISLANDS"}, 
                    new SelectListItem { Value = "SO", Text = "SOMALIA"}, 
                    new SelectListItem { Value = "ZA", Text = "SOUTH AFRICA"}, 
                    new SelectListItem { Value = "GS", Text = "SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS"}, 
                    new SelectListItem { Value = "SS", Text = "SOUTH SUDAN"}, 
                    new SelectListItem { Value = "ES", Text = "SPAIN"}, 
                    new SelectListItem { Value = "LK", Text = "SRI LANKA"}, 
                    new SelectListItem { Value = "SD", Text = "SUDAN"}, 
                    new SelectListItem { Value = "SR", Text = "SURINAME"}, 
                    new SelectListItem { Value = "SJ", Text = "SVALBARD AND JAN MAYEN"}, 
                    new SelectListItem { Value = "SZ", Text = "SWAZILAND"}, 
                    new SelectListItem { Value = "SE", Text = "SWEDEN"}, 
                    new SelectListItem { Value = "CH", Text = "SWITZERLAND"}, 
                    new SelectListItem { Value = "SY", Text = "SYRIAN ARAB REPUBLIC"}, 
                    new SelectListItem { Value = "TW", Text = "TAIWAN, PROVINCE OF CHINA"}, 
                    new SelectListItem { Value = "TJ", Text = "TAJIKISTAN"}, 
                    new SelectListItem { Value = "TZ", Text = "TANZANIA, UNITED REPUBLIC OF"}, 
                    new SelectListItem { Value = "TH", Text = "THAILAND"}, 
                    new SelectListItem { Value = "TL", Text = "TIMOR-LESTE"}, 
                    new SelectListItem { Value = "TG", Text = "TOGO"}, 
                    new SelectListItem { Value = "TK", Text = "TOKELAU"}, 
                    new SelectListItem { Value = "TO", Text = "TONGA"}, 
                    new SelectListItem { Value = "TT", Text = "TRINIDAD AND TOBAGO"}, 
                    new SelectListItem { Value = "TN", Text = "TUNISIA"}, 
                    new SelectListItem { Value = "TR", Text = "TURKEY"}, 
                    new SelectListItem { Value = "TM", Text = "TURKMENISTAN"}, 
                    new SelectListItem { Value = "TC", Text = "TURKS AND CAICOS ISLANDS"}, 
                    new SelectListItem { Value = "TV", Text = "TUVALU"}, 
                    new SelectListItem { Value = "UG", Text = "UGANDA"}, 
                    new SelectListItem { Value = "UA", Text = "UKRAINE"}, 
                    new SelectListItem { Value = "AE", Text = "UNITED ARAB EMIRATES"}, 
                    new SelectListItem { Value = "GB", Text = "UNITED KINGDOM"}, 
                    new SelectListItem { Value = "US", Text = "UNITED STATES"}, 
                    new SelectListItem { Value = "UM", Text = "UNITED STATES MINOR OUTLYING ISLANDS"}, 
                    new SelectListItem { Value = "UY", Text = "URUGUAY"}, 
                    new SelectListItem { Value = "UZ", Text = "UZBEKISTAN"}, 
                    new SelectListItem { Value = "VU", Text = "VANUATU"}, 
                    new SelectListItem { Value = "VE", Text = "VENEZUELA, BOLIVARIAN REPUBLIC OF"}, 
                    new SelectListItem { Value = "VN", Text = "VIET NAM"}, 
                    new SelectListItem { Value = "VG", Text = "VIRGIN ISLANDS, BRITISH"}, 
                    new SelectListItem { Value = "VI", Text = "VIRGIN ISLANDS, U.S."}, 
                    new SelectListItem { Value = "WF", Text = "WALLIS AND FUTUNA"}, 
                    new SelectListItem { Value = "EH", Text = "WESTERN SAHARA"}, 
                    new SelectListItem { Value = "YE", Text = "YEMEN"}, 
                    new SelectListItem { Value = "ZM", Text = "ZAMBIA"}, 
                    new SelectListItem { Value = "ZW", Text = "ZIMBABWE"}, 

                };
    }
    #endregion
}