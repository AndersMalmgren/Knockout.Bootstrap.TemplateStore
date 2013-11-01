using Owin;

namespace Knockout.Bootstrap.TemplateStore.Owin
{
    public static class AppBuilderExtensions
    {
        public static void InitTemplateStore(this IAppBuilder app, string templatePath, string fileMask)
        {
            TemplateStoreMiddleware.Init(templatePath, fileMask);
            app.Map("/templates", subApp => subApp.Use<TemplateStoreMiddleware>());
        }
    }
}
