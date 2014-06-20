using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RSSFeedApp.Startup))]
namespace RSSFeedApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
