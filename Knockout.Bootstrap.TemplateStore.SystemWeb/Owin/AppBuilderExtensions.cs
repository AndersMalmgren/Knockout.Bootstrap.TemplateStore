using System.Web.Hosting;
using Owin;

namespace Knockout.Bootstrap.TemplateStore.SystemWeb.Owin
{
    public static class AppBuilderExtensions
    {
        public static void InitTemplateStore(this IAppBuilder app, string templateVirtualDirtectory, string fileMask, bool deep)
        {
            Bootstrap.TemplateStore.Owin.AppBuilderExtensions.InitTemplateStore(app, HostingEnvironment.MapPath(templateVirtualDirtectory), fileMask, deep);
        }

        public static void InitTemplateStore(this IAppBuilder app, string templateVirtualDirtectory, string fileMask)
        {
            Bootstrap.TemplateStore.Owin.AppBuilderExtensions.InitTemplateStore(app, HostingEnvironment.MapPath(templateVirtualDirtectory), fileMask, false);
        }

        public static void InitTemplateStore(this IAppBuilder app, bool deep)
        {
            app.InitTemplateStore("~/Views", "*.htm*", deep);
        }

        public static void InitTemplateStore(this IAppBuilder app)
        {
            app.InitTemplateStore(false);
        }
    }
}
