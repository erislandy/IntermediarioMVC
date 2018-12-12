using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IntermediarioMVC.Startup))]
namespace IntermediarioMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
