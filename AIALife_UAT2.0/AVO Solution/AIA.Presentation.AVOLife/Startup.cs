using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AIA.Presentation.AVOLife.Startup))]
namespace AIA.Presentation.AVOLife
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
