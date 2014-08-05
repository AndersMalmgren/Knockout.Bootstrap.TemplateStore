using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Knockout.Bootstrap.TemplateStore
{
    public class TemplateStore
    {
        private string path;
        private Dictionary<string, TemplatesInfo> templates;
        private string mask;
        private readonly bool deep;

        public TemplateStore(string templatePath, string fileMask, bool deep)
        {
            path = templatePath;
            mask = fileMask;
            this.deep = deep;

            var watcher = new FileSystemWatcher(path, mask);
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;
            watcher.Changed += (s, e) => LoadTemplatesWithRetry();

            LoadTemplates();
        }

        private void LoadTemplatesWithRetry()
        {
            while (true)
            {
                try
                {
                    LoadTemplates();
                    break;
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }

        }

        private void LoadTemplates()
        {
            var folders = Directory.GetDirectories(path);
            templates = folders.ToDictionary(f => Path.GetFileName(f).ToLower(), LoadFolder);
        }

        private TemplatesInfo LoadFolder(string path)
        {
            var files = Directory.GetFiles(path, mask, deep ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select(f => new FileInfo(f));
            if (!files.Any()) return null;
            return new TemplatesInfo(files);

        }

        public TemplatesInfo Get(string root)
        {
            root = root.ToLower();
            if (!templates.ContainsKey(root))
                throw new ArgumentException(string.Format("Root '{0}' not present in template store", root));

            var views = templates[root];
            if (views == null)
                throw new Exception(string.Format("Root '{0}' has no templates", root));

            return views;
        }
    }
}
