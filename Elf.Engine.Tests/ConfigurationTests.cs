using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Elf.Configuration;
using Elf.Persistence;

namespace Elf.Engine.Tests {
    [TestFixture]
    public class ConfigurationTests {
        [Test]
        public void TestDatabaseConfigFromEmptySection() {
            var section = new DatabaseSection();
            DatabaseConfig config = new DatabaseConfig(section);

            Assert.That(config.BatchSize, Is.EqualTo(25));
            Assert.That(config.CacheProviderClass, Is.EqualTo("NHibernate.Cache.NoCacheProvider, NHibernate"));
            Assert.That(config.Caching, Is.EqualTo(false));
            Assert.That(config.ChildrenLaziness, Is.EqualTo("extra"));
            Assert.That(config.ConnectionStringName, Is.EqualTo("ElfDB"));
            Assert.That(config.Flavour, Is.EqualTo(DatabaseFlavour.AutoDetect));
            Assert.That(config.TablePrefix, Is.EqualTo(""));
            Assert.That(config.HibernateProperties, Is.Not.Null);
            Assert.That(config.HibernateProperties, Has.Count.EqualTo(0));
        }
        [Test]
        public void TestDatabaseConfigFromAppConfigSection() {
            var section = System.Configuration.ConfigurationManager.GetSection("elf/database") as DatabaseSection;
            Assert.That(section, Is.Not.Null);

            DatabaseConfig config = new DatabaseConfig(section);

            Assert.That(config.ConnectionStringName, Is.EqualTo("other_elfdb"));
            Assert.That(config.TablePrefix, Is.EqualTo("new_"));
            Assert.That(config.HibernateProperties, Is.Not.Null);
            Assert.That(config.HibernateProperties, Has.Count.EqualTo(1));
            Assert.That(config.HibernateProperties["someItem"], Is.EqualTo("someValue"));

            Assert.That(config.Flavour, Is.EqualTo(DatabaseFlavour.SqLite));
        }

        [Test]
        public void TestDatabaseConfigFromAppConfigSectionGroup() {
            var sectionGroup = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None).GetSectionGroup("elf") as SectionGroup;
            Assert.That(sectionGroup, Is.Not.Null);
            Assert.That(sectionGroup.Database, Is.Not.Null);

            DatabaseConfig config = new DatabaseConfig(sectionGroup);

            Assert.That(config.ConnectionStringName, Is.EqualTo("other_elfdb"));
            Assert.That(config.TablePrefix, Is.EqualTo("new_"));
            Assert.That(config.HibernateProperties, Is.Not.Null);
            Assert.That(config.HibernateProperties, Has.Count.EqualTo(1));
            Assert.That(config.HibernateProperties["someItem"], Is.EqualTo("someValue"));
        }

    }
}
