namespace Elf.Tests {
    using Ninject;
    using Elf.Web.Mvc;

    public static class TestHelper {
        public static IKernel CreateKernel() {
            var settings = new MvcSettings();
            settings.Assemblies.Add(System.Reflection.Assembly.GetExecutingAssembly());
            return settings.CreateKernel();
        }
    }
}
