﻿using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Data.Domain.Entities;
using Data.Domain.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using DefaMoghaddas.Comon;
using DAL.Classes;
using Operations.Interfaces;
using System.Data.Entity;
using System.Linq;
using DefaMoghaddas.Filters;

namespace DefaMoghaddas.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUser _userManager;
        private readonly IUserOperations<ApplicationUser> _iuserOperation;
        private ApplicationUserManagment _applicationUserManager;
        private ApplicationUserSignIn _applicationUserSignInManager;
        
        public AccountController()
        {
        }

        public AccountController(ApplicationUser userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUser UserManager {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUser>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationUserSignIn ApplicationUserSignInManager
        {
            get
            {
                return _applicationUserSignInManager ?? HttpContext.GetOwinContext().Get<ApplicationUserSignIn>();
            }
            private set { _applicationUserSignInManager = value; }
        }

        public ApplicationUserManagment ApplicationUserManager
        {
            get
            {
                return _applicationUserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManagment>();
            }
            private set
            {
                _applicationUserManager = value;
            }
        }

        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                GetUserAdditionalInfo(new LoginViewModel() { UserName = User.Identity.Name });
                return RedirectToSameDomain(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        public ActionResult LoginAjax()
        {
            if (User.Identity.IsAuthenticated)
            {
                var goToUrl = string.Format("window.location = '{0}'", Url.Action("Index", "Home"));
                return new JavaScriptResult { Script = goToUrl };
            }
            return PartialView("_Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                GetUserAdditionalInfo(model);
                return RedirectToSameDomain(returnUrl);
            }

            if (!ModelState.IsValid)
                return View(model);

            var user = ApplicationUserManager.FindByName(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Error");
                return View(model);
            }

            var result = ApplicationUserSignInManager.PasswordSignIn(model.UserName, model.Password, false, true);
            switch (result)
            {
                case SignInStatus.Success:
                    GetUserAdditionalInfo(model);

                    ApplicationUserManager.ResetAccessFailedCount(user.Id);
                    return RedirectToSameDomain(returnUrl);

                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", "Error");
                    return View(model);

                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });

                default:
                    ModelState.AddModelError("", "Error");
                    return View(model);
            }
        }

                
        private void GetUserAdditionalInfo(LoginViewModel model)
        {
            var applicationUser = _iuserOperation.Get().Include(q => q.Person).SingleOrDefault(q => q.UserName == model.UserName);
            if (applicationUser != null)
            {
                Session[ConstantKeys.CurrentUserBranchName] = applicationUser.Person != null ? applicationUser.Person.Name : string.Empty;
                Session[ConstantKeys.CurrentUserBranchId] = applicationUser.Person != null ? applicationUser.Person.Id : Guid.Empty;

                var roleIds = _iuserOperation.GetUserRoleIds(model.UserName);
                Session[ConstantKeys.CurrentUserPermission] = _iuserOperation.GetPermissionsByRole(roleIds);
                Session[ConstantKeys.CurrentUserRoleIds] = applicationUser.Roles.Select(q => q.RoleId);
            }
        }

        private ActionResult RedirectToSameDomain(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            var url = Server.UrlDecode(returnUrl);
            if (Request.Url == null || url == null || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return RedirectToAction("Index", "Home");
            }

            var uri = new Uri(url);
            var host = uri.Host;
            var currentHost = Request.Url.Host;
            var currentDomain = currentHost.Split('.')[1] + "." + currentHost.Split('.')[2];
            if (host.Contains(currentDomain))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string parentBranchId)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName };
                user.Email = model.Email;
                var result = await ApplicationUserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(parentBranchId))
                    {
                        Guid parentId;
                        var parentBranch = Guid.TryParse(parentBranchId, out parentId);
                        _iuserOperation.AddBranchToUser(user.Id, parentId);
                    }

                    //                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("UserList");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


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
                var user = await ApplicationUserManager.FindByNameAsync(model.Email);
                if (user == null || !(await ApplicationUserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                    return View();
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
            if (code == null) 
            {
                return View("Error");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(model.Email);
        //        if (user == null)
        //        {
        //            ModelState.AddModelError("", "No user found.");
        //            return View();
        //        }
        //        IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("ResetPasswordConfirmation", "Account");
        //        }
        //        else
        //        {
        //            AddErrors(result);
        //            return View();
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await ApplicationUserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
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


        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "کلمه عبور تغییر یافت."
                : message == ManageMessageId.SetPasswordSuccess ? "کلمه عبور اجاد شد."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "یک خطا رخ داد. با مدیر سیستم تماس بگیرید."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

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
                    IdentityResult result = await ApplicationUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
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
                    IdentityResult result = await ApplicationUserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
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


        private bool HasPassword()
        {
            var user = ApplicationUserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }


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
            var user = await ApplicationUserManager.FindAsync(loginInfo.Login);
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
            var result = await ApplicationUserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
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
                var result = await ApplicationUserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await ApplicationUserManager.AddLoginAsync(user.Id, info.Login);
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
            var linkedAccounts = ApplicationUserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && ApplicationUserManager != null)
            {
                ApplicationUserManager.Dispose();
                ApplicationUserManager = null;
            }
            base.Dispose(disposing);
        }


        [PermissionFilter(PermissioName = PermissionsEnum.ViewAdministratorPanel)]
        public ActionResult UserList()
        {
            var model = _iuserOperation.Get().Include(q => q.Person).ToList();
            return View(model);
        }

        //[HttpPost]
        //public ActionResult UserList([DataSourceRequest] DataSourceRequest request)
        //{
        //    var model = _iuserOperation.Get().Include(q => q.Person).ToList();
        //    return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}


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
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
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
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
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
}