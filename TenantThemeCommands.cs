using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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

        [CommandName("tenant themes enable")]
        [CommandHelp(@"tenant themes enable <themeIds>")]
        public void ThemeEnable(string names)
        {
            string[] themeIds = names.Split(',');

            var tenant = _tenantService.GetTenants().FirstOrDefault(ss => ss.Name == ShellSettings.DefaultName);

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
                Context.Output.WriteLine("Tenant {0} updated", ShellSettings.DefaultName);

            }
            else
            {
                Context.Output.WriteLine("There is no tenant named: {0}", ShellSettings.DefaultName);
            }
        }
    }

}