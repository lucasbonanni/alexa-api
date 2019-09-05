using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AwesomeAlexaSkill.Startup))]
namespace AwesomeAlexaSkill
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}