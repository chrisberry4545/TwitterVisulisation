using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TwitterApiGraphics.Startup))]
namespace TwitterApiGraphics
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
