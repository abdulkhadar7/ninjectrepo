[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NInject.API.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NInject.API.App_Start.NinjectWebCommon), "Stop")]

namespace NInject.API.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Services;
    using Data.UnitOfWork;
    using Ninject.Web.Common.WebHost;
    using Resolver;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                RegisterTypes(kernel);
               
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //kernel.Bind<IProductServices>().To<ProductServices>();
            //kernel.Bind<UnitOfWork>().To<UnitOfWork>();
        }

        public static void RegisterTypes(IKernel kernel)
        {

            //Component initialization via MEF
            ComponentLoader.LoadContainer(kernel, ".\\bin", "NInject.API*.dll");
              ComponentLoader.LoadContainer(kernel, ".\\bin", "NInject.Data*.dll");
            ComponentLoader.LoadContainer(kernel, ".\\bin", "NInject.Services*.dll");
            // ComponentLoader.LoadContainer(kernel, ".\\bin", "BusinessServices.dll");

        }
    }
}
