[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AussieLink.WebUI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(AussieLink.WebUI.App_Start.NinjectWebCommon), "Stop")]

namespace AussieLink.WebUI.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Web.Http;
    using Ninject.Web.WebApi;
    using Contracts.Services;
    using Services.Services;
    using Services;
    using Contracts.UnitOfWork;
    using DAL.UnitOfWork;

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
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
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
            kernel.Bind<IAccountService>().To<AccountService>();
            kernel.Bind<IEmailService>().To<EmailService>();
            kernel.Bind<IJobAdService>().To<JobAdService>();
            kernel.Bind<IJobPostService>().To<JobPostService>();
            kernel.Bind<ICommentService>().To<CommentService>();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<IJobPostUnitOfWork>().To<JobPostUnitOfWork>();
            kernel.Bind<IJobPostCategoryUnitOfWork>().To<JobPostCategoryUnitOfWork>();
            kernel.Bind<IManageService>().To<ManageService>();
            kernel.Bind<ISharePostService>().To<SharePostService>();
            kernel.Bind<ISharePostUnitOfWork>().To<SharePostUnitOfWork>();
        }        
    }
}
