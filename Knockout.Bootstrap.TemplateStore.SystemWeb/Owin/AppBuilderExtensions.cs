using System.Web.Hosting;
using Owin;

namespace Knockout.Bootstrap.TemplateStore.SystemWeb.Owin
{
    public static class AppBuilderExtensions
    {
        public static void InitTemplateStore(this IAppBuilder app, string templateVirtualDirtectory, string fileMask)
        {
            Bootstrap.TemplateStore.Owin.AppBuilderExtensions.InitTemplateStore(app, HostingEnvironment.MapPath(templateVirtualDirtectory), fileMask);
        }

        public static void InitTemplateStore(this IAppBuilder app)
        {
            app.InitTemplateStore("~/Views", "*.htm*");
        }
    }
}
