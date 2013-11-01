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

        public TemplateStore(string templatePath, string fileMask)
        {
            path = templatePath;
            mask = fileMask;

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
            return new TemplatesInfo(Directory.GetFiles(path, mask).Select(f => new FileInfo(f)));

        }

        public TemplatesInfo Get(string root)
        {
            root = root.ToLower();
            if (!templates.ContainsKey(root))
                throw new ArgumentException(string.Format("Root '{0}' not present in template store", root));

            return templates[root];
        }
    }
}
