using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace GooglePlus.Web.Classes
{
    public class SpringControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// Create the controller.
        /// </summary>
        /// <param name="requestContext">Accepts a <see cref="RequestContext"/>.</param>
        /// <param name="controllerName">Accepts a string controller name.</param>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            IController controller;

            string controllerClassName = String.Format("{0}Controller", controllerName);
            if (SpringContext.Current.ContainsObjectDefinition(controllerClassName))
            {
                controller = SpringContext.Resolve<IController>(controllerClassName);
            }
            else
            {
                controller = base.CreateController(requestContext, controllerName);
            }
            return controller;
        }
        /// <summary>
        /// Releases the controller.
        /// </summary>
        /// <param name="controller">Accepts an <see cref="IController"/></param>
        public override void ReleaseController(IController controller)
        {
            IDisposable disposable = controller as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}