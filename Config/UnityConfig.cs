using Data.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using Operations.Immplements;
using Operations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Config
{
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            container.RegisterType(typeof(IBaseRepository<>), typeof(IBaseRepository<>));
            container.RegisterType<IRoleOperations<IdentityRole>, RoleOperations>();
            container.RegisterType<IUserOperations<ApplicationUser>, UserOperations>();
            container.RegisterType<IEntityRepository<Permissions>, EntityRepository<Permissions>>();
            container.RegisterType<IEntityRepository<Person>, EntityRepository<Person>>();
            container.RegisterType<IEntityRepository<ActionLog>, EntityRepository<ActionLog>>();
            container.RegisterType<IEntityRepository<Email>, EntityRepository<Email>>();
            container.RegisterType<IEntityRepository<UserEmail>, EntityRepository<UserEmail>>();
            container.RegisterType<IEntityRepository<PermissionsRoles>, EntityRepository<PermissionsRoles>>();
            //container.RegisterType<IEntityRepository<Atm>, EntityRepository<Atm>>();
            //container.RegisterType<IEntityRepository<AtmHistory>, EntityRepository<AtmHistory>>();

        }
    }
}