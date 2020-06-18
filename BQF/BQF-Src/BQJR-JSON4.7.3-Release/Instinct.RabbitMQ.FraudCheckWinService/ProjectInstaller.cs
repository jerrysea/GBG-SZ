using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Instinct.RabbitMQ.FraudCheckWinService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            using (Util.SettingHelper setting = new Util.SettingHelper())
            {
                serviceInstaller1.ServiceName = setting.ServiceName;
                serviceInstaller1.DisplayName = setting.DisplayName;
                serviceInstaller1.Description = setting.Description;
            } 
        }
    }
}
