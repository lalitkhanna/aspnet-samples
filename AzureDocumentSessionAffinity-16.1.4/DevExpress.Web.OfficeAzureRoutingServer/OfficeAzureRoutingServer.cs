using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Web.OfficeAzureCommunication;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DevExpress.Web.OfficeAzureRoutingServer {
    public static class OfficeAzureRoutingServer {
        public static void Init() {
            InterRoleCommunicator.SetUp();
        }
    }
}
