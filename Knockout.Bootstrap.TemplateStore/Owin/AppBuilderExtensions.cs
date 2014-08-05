using Owin;

namespace Knockout.Bootstrap.TemplateStore.Owin
{
    public static class AppBuilderExtensions
    {
        public static void InitTemplateStore(this IAppBuilder app, string templatePath, string fileMask, bool deep)
        {
            TemplateStoreMiddleware.Init(templatePath, fileMask, deep);
            app.Map("/templates", subApp => subApp.Use<TemplateStoreMiddleware>());
        }

        public static void InitTemplateStore(this IAppBuilder app, string templatePath, string fileMask)
        {
            app.InitTemplateStore(templatePath, fileMask, false);
        }
    }
}
