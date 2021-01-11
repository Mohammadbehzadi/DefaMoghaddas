using Data.Domain.Entities;
using Data.Domain.ViewModels;
using Operations.Interfaces;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace DefaMoghaddas.Filters
{
    public class PermissionFilter : ActionFilterAttribute
    {
        public PermissionsEnum PermissioName { get; set; }
        private readonly IUserOperations<ApplicationUser> _iuserOperation;

        //public PermissionFilter()
        //{
        //    _iuserOperation = UnityConfig.GetConfiguredContainer().Resolve<IUserOperations<ApplicationUser>>();
        //}

        public string Roles { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionName = filterContext.RouteData.Values["action"];
            var contollerName = filterContext.RouteData.Values["controller"];


            var currentUserRoleIds = _iuserOperation.GetUserRoleIds(filterContext.RequestContext.HttpContext.User.Identity.Name);
            var permissions = _iuserOperation.GetPermissionsByRole(currentUserRoleIds);

            if (permissions.Contains(PermissioName.ToString())) return;

            if (actionName.ToString().ToLower().Contains("index"))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "Error"},
                    {"action", "Index"},
                });
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "Error"},
                    {"action", "PartialViewError"},
                });
            }
        }
    }
}