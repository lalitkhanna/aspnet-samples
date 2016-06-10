using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Web.OfficeAzureCommunication;

namespace DevExpress.Web.OfficeAzureRoutingServer {
    public static class FarmConfigurationManager {
        static object locker = new object();

        public static void SetUpFarm() {
            lock(locker) {
                try {
                    IISWebFarmConfigurationUtils.SetUp(WebFarmConfiguration.FarmName);
                } catch { }
            }
        }

        public static string GetServerAddress(string workSessionID) { 
            var ws = WorkSessionServer.GetWorkSessionInfoByID(workSessionID);
            if(ws != null)
                return ws.HostServerIP;
            return null;
        }

        public static void AddServers(IEnumerable<string> serverAddresses, int port) {
            lock(locker) {
                var farmName = WebFarmConfiguration.FarmName;
                foreach(var serverAddress in serverAddresses) {
                    IISWebFarmConfigurationUtils.AddServer(farmName, new ServerInfo(serverAddress, port));
                }
            }
        }
    }
}
