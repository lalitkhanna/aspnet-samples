using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.OfficeAzureCommunication.Utils;
using Microsoft.Web.Administration;

namespace DocumentSite {

   public static class WebRoleConfiguration {
        public static void OnStart() {
            SetUpAppInitializationModule();
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
    }

}
