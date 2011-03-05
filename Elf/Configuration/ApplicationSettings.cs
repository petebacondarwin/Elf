namespace Elf.Configuration {
    using System.Collections.Generic;
    using System.Reflection;
    using FluentNHibernate.Cfg.Db;
    using Ninject;
    using Ninject.Modules;

    /// <summary>
    /// The basic settings for the system.
    /// </summary>
    /// <remarks>
    /// To modify these basic settings you can either instantiate the class and access its properties
    /// or more cleanly derive from it and implement your modifications in the derived class's constructor.
    /// </remarks>
    public class ApplicationSettings {
        /// <summary>
        /// Create a basic settings object for configuring the system.
        /// </summary>
        public ApplicationSettings() {
            Modules = new List<INinjectModule>();
            Assemblies = new AssemblyList();

            SetupDefaultAssemblyList();
            SetupDefaultDatabase();
        }

        /// <summary>
        /// A list of modules in addition to the <c>CoreModule</c> to be loaded into the Ninject Kernel
        /// </summary>
        public IList<INinjectModule> Modules { get; private set; }
        /// <summary>
        /// A list of assemblies that Elf will search when looking for types, such as persistent entities.
        /// </summary>
        public AssemblyList Assemblies { get; private set; }
        /// <summary>
        /// The configuration of the database to use to store persistent entities
        /// </summary>
        public IPersistenceConfigurer DatabaseSettings { get; set; }

        /// <summary>
        /// Create a new Dependency Injection container from the settings
        /// </summary>
        /// <returns>A newly configured kernel</returns>
        public virtual IKernel CreateKernel() {
            // Create a new kernel from the core module.
            IKernel kernel = new StandardKernel(new CoreServices());

            // Bind the specified assembly list and database configuration
            BindAssemblyListToKernel(kernel);
            BindDatabaseConfigurationToKernel(kernel);

            // Load any other modules, these may override binding specified above
            kernel.Load(Modules);

            return kernel;
        }

        /// <summary>
        /// Bind the list of assemblies to the kernel
        /// </summary>
        /// <param name="kernel">The kernel to which to bind</param>
        /// <remarks>
        /// Override this if you want to modify how the list of assemblies is bound to the kernel.
        /// Currently the list at <c>Settings.Assemblies</c> is bound in singleton scope.
        /// </remarks>
        protected virtual void BindAssemblyListToKernel(IKernel kernel) {
            kernel.Bind<AssemblyList>().ToConstant(Assemblies).InSingletonScope();
        }
        /// <summary>
        /// Bind the database configuration to the kernel
        /// </summary>
        /// <param name="kernel">The kernel to which to bind</param>
        /// <remarks>
        /// Override this if you want to modify how the database configuration is bound to the kernel.
        /// Currently the list at <c>Settings.DatabaseSettings</c> is bound in singleton scope.
        /// </remarks>
        protected virtual void BindDatabaseConfigurationToKernel(IKernel kernel) {
            kernel.Bind<IPersistenceConfigurer>().ToConstant(DatabaseSettings).InSingletonScope();
        }


        /// <summary>
        /// Configure the default assembly list
        /// </summary>
        /// <remarks>
        /// By default we only include this assembly, which contains the ContentItem persistent entity.
        /// </remarks>
        private void SetupDefaultAssemblyList() {
            Assemblies.Add(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Configure the default database
        /// </summary>
        /// <remarks>
        /// It is worth knowing that these InMemory databases only last as long as the ISession
        /// so we have to use the same session to 1) Create the schema and 2) Interact with the database
        /// It is not enough to ask the Kernel to provide us with an ISession whenever we want it because
        /// outside of a HttpContext the ISession object is transient and a new one is constructed each
        /// time Kernel.Get is called.
        /// The best way is to get an ISession from the Kernel then pass it to objects that need it,
        /// constructing them manually and not allowing the Kernel to construct them for us since it will
        /// pass them a new ISession.
        /// </remarks>
        private void SetupDefaultDatabase() {
            DatabaseSettings = SQLiteConfiguration.Standard.InMemory();//.ShowSql();
        }
    }
}
