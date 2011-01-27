using NHibernate;

namespace Elf.Persistence {
    public interface IRepositoryConfiguration {
        NHibernate.Cfg.Configuration NHConfiguration { get; }
    }
}
