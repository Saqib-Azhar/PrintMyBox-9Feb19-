using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Practicing_OAuth.Startup))]
namespace Practicing_OAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
