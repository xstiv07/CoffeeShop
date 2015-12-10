using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CustomerStatusApp.Startup))]
namespace CustomerStatusApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
