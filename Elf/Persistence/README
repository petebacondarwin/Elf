The configuration of the persistance can be a bit confusing at first glance.
So here is an overview of the classes and how they stick together.

== NHibernate ==
The persistence mechanism is powered by NHibernate.
Traditionally NHibernate uses XML files to configure the mapping from code classes to database tables.

== FluentNHibernate ==
We are using the FluentNHibernate library to allow us to programmatically configure NHibernate.
This means that there are no XML files involved, the configuration is typed checked by the compiler and you get intellisense when configuring the system.
FluentNHibernate also, kindly, provides automatic mapping of code classes to database tables through conventions that we can specify.
The bottom-line convention that we are using is that all classes that will be mapped to tables will implement the IPersistent interface.

== Ninject ==
The FluentNHibernate library is very flexible so one must implement a number of classes to configure the different elements.
This can be confusing at first but does not take long to get used to.
Also this means that there are complicated dependencies between the various configuration classes.
To make it easier to use from the client perspective we have implemented the Ninject Dependency Injection Container.
This will automatically instantiate and wire up the classes that we need to get a fully configured NHibernate session.

== Ninject Activation Providers ==
Some of the classes we need are fine for being instantiated directly by Ninject but some not so simple or require extra processing.
In these cases we have implemented activation providers whose sole job is to do this complex instantiation of such classes.

== Persistence Object Dependencies ==
The following tree describes the dependencies between the objects that provide the persistence functionality:

NHibernate.ISession - access to persistent object from NHibernate
	=> (created by) Elf.Persistence.Configuration.SessionProvider
		=> (depends upon) NHibernate.ISessionFactory
			=> (created by) Elf.Persistence.Configuration.SessionFactoryProvider
				=> (depends upon) NHibernate.Cfg.Configuration
					=> (created by) Elf.Persistence.Configuration.ConfigurationProvider
						=> (depends upon) FluentNHibernate.Cfg.Db.IPersistenceConfigurer
							=> (provided by client) - Specifies the database connection to use (default is SqLite in-memory DB)
						=> (depends upon) FluentNHibernate.Automapping.AutoPersistenceModel
							=> (created by) Elf.Persistence.Configuration.PersistenceModelProvider
								=> (depends upon) FluentNHibernate.Automapping.IAutomappingConfiguration
									=> (implemented by) Elf.Persistence.Configuration.AutoMappingConfiguration
								=> (depends upon) Elf.Configuration.AssemblyList
						=> (uses) FluentNHibernate.Cfg.Fluently.Configure()

== Ninject Modules ==
All of this wiring is specified in Ninject modules, in which interfaces are bound to concrete classes.
Ninject gives you a lot of control over how objects are bound and for how long they live.
Most of the items above are specified as Singletons, which means that they are created once for each Ninject Kernel and used as needed.
The ISession object on the other hand is specified as PerRequest, which means that if we are in an ASP.NET application then the ISession
object only lasts as long as the current request and each new request gets a new ISession object.

== Example Use ==
When the Ninject kernel is requested to create an ISession object, Ninject goes off and creates all the objects shown above.
It wires them all together, passing the necessary objects into each dependent object's constructor and finally returns the ISession object to the client.
This would look a bit like this:

using (var kernel = SomeHelper.CreateKernel()) {
    using (var session = kernel.Get<ISession>()) {
		// Use the session object here
	}
}

Here the SomeHelper.CreateKernel() is doing some work to initialize and create the Ninject kernel for us.
Since both the Ninject kernel and NHibernate ISession objects implement IDisposable the using(){ ... } statements automatically dispose the objects at the end of the block.

== Creating the Ninject Kernel ==
To make configuration of the kernel itself more straightforward there is a settings class.
This provides access to the configurable aspects of what we have described here.
The base settings class looks a bit like this:

namespace Elf.Configuration {
    /// <summary>
    /// The basic settings for the system.
    /// </summary>
    /// <remarks>
    /// To modify these basic settings you can either instantiate the class and access its properties
    /// or more cleanly derive from it and implement your modifications in the derived class's constructor.
    /// </remarks>
    public class BaseSettings {
        /// <summary>
        /// Create a basic settings object for configuring the system.
        /// </summary>
        public BaseSettings();

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
        public IPersistenceConfigurer DatabaseSettings { get; protected set; }

        /// <summary>
        /// Create a new Dependency Injection container from the settings
        /// </summary>
        /// <returns>A newly configured kernel</returns>
        public virtual IKernel CreateKernel() {
	}
}

General usage of an instance of BaseSettings might be:

// Create the settings object
	var settings = new BaseSettings();
	// Add the currently executing assembly to the list of assemblies that will be searched for persistent types
	settings.Assemblies.Add(System.Reflection.Assembly.GetExecutingAssembly());
	// Get the kernel that will provide the ISession object, etc.
	return settings.CreateKernel();

Implementation of a derived settings class might be:
	/// <summary>
	/// We override the core module to establish our own database configuration
	/// </summary>
	public class MySettings : BaseSettings {
		public SiteSettings() : base() {
			// Change the database
			DatabaseSettings = SQLiteConfiguration.Standard.UsingFile("|DataDirectory|\\content.db");
			Assemblies.Add(System.Reflection.Assembly.GetAssembly(typeof(APersistentClass)));
		}
	}
