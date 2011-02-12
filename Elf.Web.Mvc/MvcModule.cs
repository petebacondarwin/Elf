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
        }
    }
}
