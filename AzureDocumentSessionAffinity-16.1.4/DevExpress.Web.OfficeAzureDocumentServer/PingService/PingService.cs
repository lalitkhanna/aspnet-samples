using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Web.OfficeAzureCommunication;

namespace DevExpress.Web.OfficeAzureDocumentServer {

    class PingService : TimeoutService {
        static object syncRoot = new Object();
        public static readonly TimeSpan DefaultInterval = TimeSpan.FromSeconds(30);
        TimeSpan interval = DefaultInterval;

        private PingService() { }

        private static volatile PingService instance;
        protected static PingService Singleton {
            get {
                if(instance == null) {
                    lock(syncRoot) {
                        if(instance == null) {
                            instance = new PingService();
                        }
                    }
                }

                return instance;
            }
        }

        protected override TimeSpan Interval {
            get { return interval; }
        }

        protected internal void SetInterval(int seconds) {
            this.interval = TimeSpan.FromSeconds(seconds); 
        }

        protected override void OnServiceCore() {
            var workSessionInfos = WorkSessionServer.GetServerWorkSessionInfos(System.Environment.MachineName);
            if(workSessionInfos.Count > 0)
                WorkSessionMessenger.SendUpdateAllMessage(workSessionInfos);
        }

        public static void Start(int broadcastInterval) {
            Singleton.SetInterval(broadcastInterval);
            Singleton.StartInternal();
        }
        public static void Stop() {
            Singleton.StopInternal();
        }

    }
}
