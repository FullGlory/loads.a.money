using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Web.Routing;
using System.Web.Http;
using System;
using System.Collections.Generic;

namespace SpreadBet.Service
{
    public static class UnityConfig
    {
        public static void Resolve()
        {
            var container = new UnityContainer().LoadConfiguration("SpreadBet");
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory(new ControllerActivator()));
        }
    }

    public class ControllerActivator : IControllerActivator
    {
        IController IControllerActivator.Create(RequestContext requestContext, Type controllerType)
        {
            return GlobalConfiguration.Configuration.DependencyResolver.GetService(controllerType) as IController;
        }
    }

    public class UnityDependencyResolver : IDependencyResolver
    {
        private IUnityContainer _container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            this._container = container;
        }

        public object GetService(Type serviceType)
        {
            if (!this._container.IsRegistered(serviceType))
            {
                if (serviceType.IsAbstract || serviceType.IsInterface)
                {
                    return null;
                }
            }
            return this._container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._container.ResolveAll(serviceType);
        }
    }
}