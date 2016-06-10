using System;

namespace DevExpress.Web.OfficeAzureRoutingServer {
    public class ServerInfo {
        public ServerInfo(string address, int? httpPort = null, int? httpsPort = null, int? weight = null) {
            Address = address;
            HttpPort = httpPort;
            HttpsPort = httpsPort;
            Weight = weight;
        }
        public string Address { get; private set; }
        public int? HttpPort { get; private set; }
        public int? HttpsPort { get; private set; }
        public int? Weight { get; private set; }

        public bool Customized { get { return HttpPort.HasValue || HttpsPort.HasValue || Weight.HasValue; } }
    }
}
