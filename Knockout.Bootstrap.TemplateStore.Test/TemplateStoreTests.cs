using System;
using System.IO;
using System.Runtime.Remoting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Knockout.Bootstrap.TemplateStore.Test
{
    public class TemplateStoreTest
    {
        protected TemplateStore Run(Action<DirectoryInfo> init, bool deep)
        {
            var id = Guid.NewGuid();
            var path = Path.Combine(Directory.GetCurrentDirectory(), id.ToString());
            var dirInfo = Directory.CreateDirectory(path);

            init(dirInfo);

            var store = new TemplateStore(path, "*.htm*", deep);

            Directory.Delete(path, true);
            return store;
        }
    }

    [TestClass]
    public class When_loading_templates_with_empty_folder : TemplateStoreTest
    {
        private bool success;

        [TestInitialize]
        public void Context()
        {
            var store = Run(d => d.CreateSubdirectory("Home"), false);
            try
            {
                var home = store.Get("Home");
            }
            catch (Exception e)
            {
                success = true;
            }
        }

        [TestMethod]
        public void It_should_not_crash()
        {
            Assert.IsTrue(success);
        }
    }

    [TestClass]
    public class When_loading_templates_with_a_deep_folder_structure : TemplateStoreTest
    {
        private TemplatesInfo home;

        [TestInitialize]
        public void Context()
        {
            var store = Run(Create, true);
            home = store.Get("Home");
        }

        private void Create(DirectoryInfo directoryInfo)
        {
            var sub = directoryInfo.CreateSubdirectory("Home").CreateSubdirectory("Sub");
            File.WriteAllText(Path.Combine(sub.FullName, "SubView.html"), "My View");
        }

        [TestMethod]
        public void It_should_not_crash()
        {
            Assert.AreEqual(1, home.Templates.Count);
        }
    }
}
