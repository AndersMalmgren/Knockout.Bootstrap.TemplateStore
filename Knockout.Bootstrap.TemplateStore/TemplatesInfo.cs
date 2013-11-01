using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Knockout.Bootstrap.TemplateStore
{
    public class TemplatesInfo
    {
        public Dictionary<string, string> Templates { get; private set; }
        public DateTime LastModified { get; private set; }
        public string Output { get; private set; }

        public TemplatesInfo(IEnumerable<FileInfo> files)
        {
            LastModified = files.Max(f => f.LastWriteTime);
            Templates = files
                .ToDictionary(f => Path.GetFileNameWithoutExtension(f.Name), f => File.ReadAllText(f.FullName));

            Output = Newtonsoft.Json.JsonConvert.SerializeObject(Templates);
        }
    }
}