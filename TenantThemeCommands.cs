using System.Linq;
using Orchard.Commands;
using Orchard.Environment.Configuration;
using Orchard.MultiTenancy.Services;

namespace TenantThemeActivator
{
    public class TenantThemeCommands : DefaultOrchardCommandHandler
    {
        private readonly ITenantService _tenantService;

        public TenantThemeCommands(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }
        [OrchardSwitch]
        public string TenantName { get; set; }

        [CommandName("tenant themes enable")]
        [CommandHelp(@"tenant themes enable <themeids> [/TenantName:tenant_name] \n\r\d theme ids are comma separated ")]
        [OrchardSwitches("TenantName")]
        public void ThemeEnable(string names)
        {
            string[] themeIds = names.Split(',');

            var tenant = _tenantService.GetTenants().FirstOrDefault(ss => ss.Name == TenantName);

            if (tenant != null)
            {
                _tenantService.UpdateTenant(
                    new ShellSettings
                    {
                        Name = tenant.Name,
                        RequestUrlHost = tenant.RequestUrlHost,
                        RequestUrlPrefix = tenant.RequestUrlPrefix,
                        DataProvider = tenant.DataProvider,
                        DataConnectionString = tenant.DataConnectionString,
                        DataTablePrefix = tenant.DataTablePrefix,
                        State = tenant.State,
                        Themes = themeIds
                    });
                Context.Output.WriteLine("Tenant {0} updated", TenantName);

            }
            else
            {
                Context.Output.WriteLine("There is no tenant named: {0}", TenantName);
            }
        }
    }

}