using NHibernate;
using Elf.Persistence.Configuration;
using Ninject.Parameters;
namespace Elf.Web.Mvc {
    /// <summary>
    /// The Mvc specific Ninject bindings
    /// </summary>
    public class MvcModule : Ninject.Modules.NinjectModule {
        /// <summary>
        /// Load the bindings specified in this module into the kernel.
        /// </summary>
        public override void Load() {
            Kernel.Bind<IControllerFinder>().To<ControllerFinder>().InSingletonScope();
            Kernel.Bind<Routing.ContentRoute>().ToSelf().InSingletonScope();
        }
    }
}
