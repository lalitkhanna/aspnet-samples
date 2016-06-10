using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.OfficeAzureCommunication.Utils;
using Microsoft.Web.Administration;

namespace Routing {
    public static class WebRoleConfiguration {
        static string DocumentServerRoleName;
        static int DocumentServerPort = 8080;

        public static void OnStart(string documentServerRoleName, int documentServerPort) {
            DocumentServerRoleName = documentServerRoleName;
            DocumentServerPort = documentServerPort;

            SetUpAppInitializationModule();
            SetUpAlwaysRunning();
            SetUpFarmDelayed();
        }
        
        static void SetUpAppInitializationModule() {
            using(var serverManager = new ServerManager()) {
                foreach(var application in serverManager.Sites.SelectMany(c => c.Applications)) {
                    application["preloadEnabled"] = true;
                }

                foreach(var appPool in serverManager.ApplicationPools) {
                    appPool["startMode"] = "AlwaysRunning";
                }

                serverManager.CommitChanges();
            }
        }

        // IS may not be fully configured during the startup task stage in the startup process, 
        // so role-specific data may not be available. 
        // Startup tasks that require role-specific data should use 
        // Microsoft.WindowsAzure.ServiceRuntime.RoleEntryPoint.OnStart.
        // https://azure.microsoft.com/en-us/documentation/articles/cloud-services-startup-tasks/
        static void SetUpAlwaysRuning() {
            string command = @"call ./Setup/SetUpAppPoolsAlwaysRunning.cmd";
            CommandLineUtils.CmdExecute(command);
        }

        static void SetUpFarmDelayed() { 
            int SetUpFarmDelay = 10000;
            System.Threading.Timer timer = new System.Threading.Timer(SetUpFarm, null, SetUpFarmDelay, System.Threading.Timeout.Infinite);
        }

        static void SetUpFarm(object state) {
            try {            
                DevExpress.Web.OfficeAzureRoutingServer.FarmConfigurationManager.SetUpFarm();
                var serverAdresses = RoleInstanceUtils.GetRoleInstanceAdressList(DocumentServerRoleName);
                DevExpress.Web.OfficeAzureRoutingServer.FarmConfigurationManager.AddServers(serverAdresses, DocumentServerPort);
            } catch(Exception) {  }
        }
    }
}
