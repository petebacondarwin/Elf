namespace Elf.Web.Mvc.Configuration {
    using NHibernate;
    using Elf.Persistence.Configuration;
    using Ninject.Parameters;

    /// <summary>
    /// Services provided by the Mvc library
    /// </summary>
    public class MvcServices : Ninject.Modules.NinjectModule {
        /// <summary>
        /// Load the bindings specified in this module into the kernel.
        /// </summary>
        public override void Load() {
            Kernel.Bind<IControllerFinder>().To<ControllerFinder>().InSingletonScope();
        }
    }
}
