using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpress.Web.OfficeAzureCommunication {
    public static class MessageDispatcher {
        public static void ProcessMessage(Message msg) {
            #if DEBUG
            DevExpress.Web.OfficeAzureCommunication.Diagnostic.Logger.Log(msg);
            #endif

            PreprocessMessage(msg);

            switch(msg.MessageOperation) {
                case MessageOperation.UpdateAll:
                    UpdateAll(msg);
                break;
                case MessageOperation.Add:
                    Add(msg);
                break;
                case MessageOperation.Remove:
                    Remove(msg);
                break;
                case MessageOperation.Hibernated:
                case MessageOperation.AutoSaved:
                case MessageOperation.WokenUp:
                    ChangeStatus(msg);
                break;
            }
        }

        static void PreprocessMessage(Message msg) {
            foreach(var w in msg.WorkSessions) {
                w.RoleInstanceId = msg.RoleInstanceId;
                w.HostServerName = msg.HostServerName;
                w.HostServerIP = msg.HostServerIP;
                w.CreateTime = msg.CreateTime;
                w.ProcessedTime = DateTime.Now;
            }
        }

        static void UpdateAll(Message msg) {
            WorkSessionServer.UpdateAllWSFromOneServer(msg);
        }
        static void Add(Message msg) {
            WorkSessionServer.Add(msg);
        }
        static void Remove(Message msg) {
            WorkSessionServer.Remove(msg);
        }
        static void ChangeStatus(Message msg) {
            WorkSessionServer.ChangeStatus(msg);
        }
    }
}
