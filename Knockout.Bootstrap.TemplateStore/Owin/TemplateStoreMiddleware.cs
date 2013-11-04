using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Knockout.Bootstrap.TemplateStore.Owin
{
    public class TemplateStoreMiddleware : OwinMiddleware
    {
        private static TemplateStore templateStore;


        public TemplateStoreMiddleware(OwinMiddleware next) : base(next)
        {
        }


        internal static void Init(string templatePath, string fileMask)
        {
            templateStore = new TemplateStore(templatePath, fileMask);
        }
        
        public override Task Invoke(IOwinContext context)
        {
            var root = context.Request.Query["root"];
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = 200;
            
            var template = templateStore.Get(root);

            if (ClientCached(context.Request, template.LastModified))
            {
                response.StatusCode = 304;
                response.Headers["Content-Length"] = "0";

                var tcs = new TaskCompletionSource<object>();
                tcs.SetResult(null);
                return tcs.Task;
            }
            response.Headers["Last-Modified"] = template.LastModified.ToUniversalTime().ToString("r");
            response.Headers["Cache-Control"] = "no-cache";

            return context.Response.WriteAsync(template.Output);
        }

        private bool ClientCached(IOwinRequest request, DateTime contentModified)
        {
            string header = request.Headers["If-Modified-Since"];

            if (header != null)
            {
                DateTime isModifiedSince;
                if (DateTime.TryParse(header, out isModifiedSince))
                {
                    return isModifiedSince >= contentModified.AddSeconds(-1);
                }
            }

            return false;
        }
    }
}
