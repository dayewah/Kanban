using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;

namespace Kanban
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [PermissionSet(SecurityAction.Demand, Name ="FullTrust")]//for filesystem watcher
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Seed.Run(false);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();

        }
    }
}
