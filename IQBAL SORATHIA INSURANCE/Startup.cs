using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IQBAL_SORATHIA_INSURANCE.Startup))]
namespace IQBAL_SORATHIA_INSURANCE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
