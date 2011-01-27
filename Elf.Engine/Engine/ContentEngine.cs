using System;
using Elf.Persistence;
using NHibernate;

namespace Elf.Engine {
    public class ContentEngine {
        IRepository repository;

        public ContentEngine(IRepository repository) {
            this.repository = repository;
        }

        public ContentEngine() {
            this.repository = new Repository(
                new RepositoryConfiguration(
                    FluentNHibernate.Cfg.Db.SQLiteConfiguration.Standard.UsingFile("content.db"),
                    new AutomappingConfiguration(),
                    new AppDomainAssemblySelector()));
        }

        public ISession OpenSession() {
            return repository.OpenSession();
        }
    }
}