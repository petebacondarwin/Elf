namespace Elf.Tests {
    using Ninject;
    using Elf.Web.Mvc;
    using Elf.Web.Mvc.Configuration;

    public static class TestHelper {
        public static IKernel CreateKernel() {
            var settings = new MvcApplicationSettings();
            settings.Assemblies.Add(System.Reflection.Assembly.GetExecutingAssembly());
            return settings.CreateKernel();
        }
    }
}
