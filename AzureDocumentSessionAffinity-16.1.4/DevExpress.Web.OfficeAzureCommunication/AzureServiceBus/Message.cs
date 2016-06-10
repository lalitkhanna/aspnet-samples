using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DevExpress.Web.OfficeAzureCommunication {

    public enum WorkSessionStatus { Loaded, AutoSaved, Hibernated, WokenUp, Removed }

    [DataContract(Namespace = BroadcastNamespaces.DataContractNamespace)]
    public class WorkSessionInfo {
        [DataMember]
        public Guid WorkSessionID { get; set; }
        [DataMember]
        public string DocumentId { get; set; }

        public WorkSessionStatus Status { get; private set; }

        public string HostServerName { get; set; }
        public string HostServerIP { get; set; }
        public string RoleInstanceId { get; set; }

        public long SenderTicks { get { return CreateTime.Ticks; }  }
        public DateTime CreateTime { get; set; }
        public DateTime ProcessedTime { get; set; }

        public WorkSessionInfo(Guid workSessionID, string documentId) {
            WorkSessionID = workSessionID;
            DocumentId = documentId;
        }

        internal void SetStatus(WorkSessionStatus wsStatus) {
            Status = wsStatus;
        }
    }

    public enum MessageOperation { UpdateAll, Add, Remove, WokenUp, Hibernated, AutoSaved }

    [DataContract(Namespace = BroadcastNamespaces.DataContractNamespace)]
    public class Message {
        [DataMember]
        public string RoleInstanceId { get; private set; }
        [DataMember]
        public string HostServerName { get; private set; }
        [DataMember]
        public string HostServerIP { get; private set; }
        [DataMember]
        public List<WorkSessionInfo> WorkSessions { get; private set; }
        [DataMember]
        public MessageOperation MessageOperation { get; private set; }
        #if DEBUG
        [DataMember]
        public string DiagnosticMassage { get; set; }
        #endif
        [DataMember]
        public DateTime CreateTime { get; private set; }

        public Message(string roleInstanceId, string hostServerName, string hostServerIP, MessageOperation messageOperation) 
            : this(roleInstanceId, hostServerName, hostServerIP, messageOperation, new List<WorkSessionInfo>()) {
        }

        public Message(string roleInstanceId, string hostServerName, string hostServerIP, MessageOperation messageOperation, List<WorkSessionInfo> workSessions) {
            RoleInstanceId = roleInstanceId;
            HostServerName = hostServerName;
            HostServerIP= hostServerIP;
            MessageOperation = messageOperation;
            WorkSessions = workSessions;
            CreateTime = DateTime.Now;
        }
    }
    
}