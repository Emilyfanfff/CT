using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Assignment_CT.Startup))]
namespace Assignment_CT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
