using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CAS.Startup))]
namespace CAS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
