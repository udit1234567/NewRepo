using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AIA.Presentation.AVOLife.Models;
using System.Threading;
using System.Web.Security;
using AIA.Life.Repository.AIAEntity;
using System.Security.Cryptography;
using System.Text;
using System.DirectoryServices;
using log4net;
using AIA.CrossCutting;
using AIA.Life.Models.UserManagement;
using AIA.Life.Business.UserManagement;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        AVOAIALifeEntities Entities = new AVOAIALifeEntities();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        [AllowAnonymous]
        public ActionResult SetCulture(string culture)
        {
            HttpCookie myCookie = new HttpCookie("_culture");
            DateTime now = DateTime.Now;
            string NumberOfDays = System.Configuration.ConfigurationManager.AppSettings["CookieExpiry"].ToString();
            // Set the cookie value.
            myCookie.Value = culture;//.EncryptData(); ;
            // Set the cookie expiration date.
            myCookie.Expires = now.AddDays(Convert.ToInt32(NumberOfDays));
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            // Add the cookie.
            Response.Cookies.Add(myCookie);
            ViewBag.RefreshPage = true;
            return Json("", JsonRequestBehavior.AllowGet);
        }
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
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(LoginViewModel model, string returnUrl)
        //{

        //    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);

        //    if (Url.IsLocalUrl(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Contact", "Home");
        //    }
        //}
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //var membershipUser = System.Web.Security.Membership.Providers["UserMembershipProvider"];
            //MembershipUser loginDetails = membershipUser.GetUser(model.UserName, false);
            //var currentUser = UserManager.GetLoginsAsync(model.UserName);
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var UserDetails = Entities.tblUserDetails.Where(a => a.LoginID == model.UserName).FirstOrDefault();
            var loginDetails = Entities.AspNetUsers.Where(a => a.UserName == model.UserName).FirstOrDefault();
            var result = await SignInManager.PasswordSignInAsync(model.UserName, "Pass@123", model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:


                    //if (UserDetails != null)
                    //{
                    //    if (UserDetails.LastPasswordChangedDate == UserDetails.CreationDate)
                    //    {
                    //        TempData["IsFirstTimeLogin"] = "True";
                    //        return RedirectToAction("ChangePassword", "UserManagement", new { ReturnURL = returnUrl, Username = model.UserName });
                    //    }
                    //    else if (loginDetails.LockoutEnabled == false)
                    //    {

                    //       ModelState.AddModelError("", "Your account has been locked out because of a maximum number of incorrect login attempts.You will NOT be able to login until you contact a site administrator and have your account unlocked.");                           
                    //       return View(model);
                    //    }
                    //    else
                    //    {
                    //        loginDetails.AccessFailedCount = 0;
                    //        loginDetails.LockoutEnabled = true;
                    //        Entities.SaveChanges();
                    //        return RedirectToLocal(returnUrl);
                    //    }
                    //}
                    //else
                    //{
                    //    loginDetails.AccessFailedCount = 0;
                    //    loginDetails.LockoutEnabled = true;
                    //    Entities.SaveChanges();
                    //    return RedirectToLocal(returnUrl);
                    //}
                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                    int passwordFailedCount = 0;
                    if (loginDetails != null)
                    {
                        if (!string.IsNullOrEmpty(model.UserName))
                        {
                            Guid? UserId = Entities.tblUserDetails.Where(b => b.LoginID == model.UserName).Select(b => b.UserID).FirstOrDefault();
                            if (UserId != null)
                            {
                                passwordFailedCount = Entities.AspNetUsers.Where(a => a.UserName == model.UserName).Select(a => a.AccessFailedCount).FirstOrDefault();
                                passwordFailedCount++;
                                loginDetails.AccessFailedCount = passwordFailedCount;
                                if (passwordFailedCount == 5)
                                    loginDetails.LockoutEnabled = false;
                            }
                            Entities.SaveChanges();
                        }
                        if (loginDetails.LockoutEnabled == false || passwordFailedCount == 5)
                        {
                            ModelState.AddModelError("", "Your Account has been Lock.");
                            return View(model);
                        }
                        else if (passwordFailedCount == 4)
                        {
                            ModelState.AddModelError("", "You have 1 Attempt Left.");
                            return View(model);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(model);

                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                    }
                default:
                    //ViewBag.LoginErrMsg = "Invalid login attempt.";
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
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
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        // [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
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
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
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
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
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
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
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
        // [HttpPost]
        // [ValidateAntiForgeryToken]//It prevent external post requests,so nobody can use this method from external sites.
        public ActionResult LogOff()
        {
            string role = Entities.usp_GetCurrentUserRole(System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            if (role == "UW User" || role == "SUPADMIN" || System.Web.Configuration.WebConfigurationManager.AppSettings["PublishEnvironment"] == "SIT")
                return RedirectToAction("Index", "Home");
            else
                return Redirect("https://imotest.aialife.com.lk/");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region IMO Redirection
        [HttpPost]
        [AllowAnonymous]
        public ActionResult RedirectAvo()
        {
            string msg = Request["Msg"];
            string userName = "";
            string token = string.Empty;
            string key = "ho01otrf77155gxsdfcbhhg1671bhgghh1";
            if (!string.IsNullOrEmpty(msg))
            {
                var sp = msg.Split('|');
                userName = sp[0];
                token = sp[1];
            }
            SHA1Managed sha1 = new SHA1Managed();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(userName + "|" + key));
            StringBuilder sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }
            string res = sb.ToString();
            AVOAIALifeEntities entities = new AVOAIALifeEntities();
            bool userStatus = entities.tblUserDetails.Where(a => a.LoginID == userName).Select(a => a.Status).FirstOrDefault() ?? false;
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            if (res == token && userStatus)
            {
                var result = SignInManager.PasswordSignIn(userName, "Pass@123", false, shouldLockout: false);
                if (result == SignInStatus.Success)
                    return RedirectToLocal("Home/Index");
                else
                    return View("~/Views/Home/UnAuthorized.cshtml");
            }
            else
                return View("~/Views/Home/UnAuthorized.cshtml");
        }

        #endregion

        #region LDAP Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LdapLogin(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                string role = Entities.usp_GetCurrentUserRole(model.UserName).FirstOrDefault();
                bool loginStat = false;
                //var userActiveStatus = Entities.tblUserDetails.Where(u => u.LoginID == model.UserName).Select(s=>s.Status).FirstOrDefault();                       
                //if (role == "UW User" || role == "SUPADMIN" || System.Web.Configuration.WebConfigurationManager.AppSettings["PublishEnvironment"] == "SIT")
                //{
                    //if (role == "UW User")
                    //{
                    //    if (userActiveStatus == false)
                    //    {
                    //        ViewBag.Status = "InActive";
                    //        return View("~/Views/Home/UnAuthorized.cshtml");
                    //    }
                    //}

                    var base64EncodedBytes = System.Convert.FromBase64String(model.Password);
                    string pwd = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                    string ldapDomain = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPDomain"];
                    string ldapBase = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAPBase"];
                    Boolean bAuthenticateUser = AuthenticateUser("LDAP://" + ldapDomain + "/" + ldapBase, model.UserName, pwd);
                    if (bAuthenticateUser == true || System.Web.Configuration.WebConfigurationManager.AppSettings["PublishEnvironment"] == "SIT")
                    {
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["PublishEnvironment"] == "SIT")
                        {
                            //if(System.Web.Configuration.WebConfigurationManager.AppSettings["SITPassword"] == pwd)
                            //{
                            loginStat = true;
                            //}
                        }
                        else
                        {
                            loginStat = true;
                        }
                    }
                    else
                    {
                        var userActiveStatus1 = UserManagementController.CheckADUserStatus(model.UserName);
                        if (userActiveStatus1 != "Success" && role == "UW User")
                        {
                            if (userActiveStatus1 == "User is Disabled/Terminated")
                            {
                                ViewBag.Credentials = "UnAuthorized";
                                return View("~/Views/Home/UnAuthorized.cshtml");
                            }
                            else
                            {
                                ViewBag.Credentials = "UnAuthorized";
                                return View("~/Views/Home/UnAuthorized.cshtml");
                            }

                        }
                        else
                        {

                        var result = SignInManager.PasswordSignIn(model.UserName, pwd, false, shouldLockout: true);
                            if (result == SignInStatus.Success)
                            {
                                return RedirectToLocal(returnUrl);
                            }
                           if(result == SignInStatus.LockedOut)
                            {

                                ViewBag.Credentials = "LockOut";
                                return View("~/Views/Home/UnAuthorized.cshtml");

                            }
                            else
                            {
                                ViewBag.Credentials = "Invalid";
                                return View("~/Views/Home/UnAuthorized.cshtml");
                            }

                        }
                    }
                    if (loginStat)
                    {
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["PublishEnvironment"] == "SIT" && System.Web.Configuration.WebConfigurationManager.AppSettings["SITPassword"] == pwd)
                            pwd = "Pass@123";

                        var result = SignInManager.PasswordSignIn(model.UserName, pwd, false, shouldLockout: true);

                        if (result == SignInStatus.Success)
                        {
                            return RedirectToLocal(returnUrl);
                        }
                        if (result == SignInStatus.LockedOut)
                        {

                            ViewBag.Credentials = "UnAuthorized";
                            return View("~/Views/Home/UnAuthorized.cshtml");

                        }
                        else
                        {
                            ViewBag.Credentials = "Invalid";
                            return View("~/Views/Home/UnAuthorized.cshtml");
                        }
                    }
                    else
                        ViewBag.Credentials = "Invalid";
                    return View("~/Views/Home/UnAuthorized.cshtml");
                //}
                //else
                //{
                //    ViewBag.Credentials = "UnAuthorized";
                //    return View("~/Views/Home/UnAuthorized.cshtml");
                //}
            }
            catch (Exception ex)
            {
                return View("~/Views/Home/UnAuthorized.cshtml");
            }
        }

        public static Boolean AuthenticateUser(String strPath, String strUserName, String strPassword)
        {
            try
            {
                using (DirectoryEntry deAuthenticate = new DirectoryEntry(strPath, strUserName, strPassword))
                {
                    // if user is verified then it will welcome then 
                    try
                    {
                        //deAuthenticate.RefreshCache();
                        Object obj = deAuthenticate.NativeObject;
                        // TODO: add your specific tasks here
                        //DirectorySearcher search = new DirectorySearcher(deAuthenticate);

                        //search.PropertiesToLoad.Add("DisplayName");
                        ////search.PropertiesToLoad.Add("employeeID");
                        //search.PropertiesToLoad.Add("mail");
                        ////search.PropertiesToLoad.Add("SapPrimeDomText");
                        //SearchResult result = search.FindOne();
                    }
                    catch (Exception ex)
                    {
                        log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                        Logger.Error(ex);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [AllowAnonymous]
        public ActionResult InValidPage()
        {
            return View();
        }
        #endregion

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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
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
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}