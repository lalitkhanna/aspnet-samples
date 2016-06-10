using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpress.Web.OfficeAzureCommunication {
    using WorkSessionServerDict = System.Collections.Concurrent.ConcurrentDictionary<System.Guid, WorkSessionInfo>;

    public delegate void DocumentServerAddedEventHandler(IEnumerable<string> serverNames);

    public static class WorkSessionServer {
        static object locker = new object();
        static event DocumentServerAddedEventHandler DocumentServerAddedCore;

        public static event DocumentServerAddedEventHandler DocumentServerAdded {
            add {
                if(!value.Method.IsStatic)
                    throw new NotSupportedException("Only Static event handlers are allowed for the DocumentServerAdded event");

                if(DocumentServerAddedCore == null || !DocumentServerAddedCore.GetInvocationList().Contains(value))
                    DocumentServerAddedCore += value;
            }
            remove {
                DocumentServerAddedCore -= value;
            }
        }

        static WorkSessionServerDict all = new WorkSessionServerDict();
        static WorkSessionServerDict All {
            get { return all; }
        }

        internal static void Add(Message msg) {
            lock(locker) {
                BeginUpdate();
                try {

                    foreach(var w in msg.WorkSessions) {
                        All.AddOrUpdate(w.WorkSessionID, w, (k, wo) => w);
                    }

                } finally {
                    EndUpdate();
                }
            }
        }

        internal static void Remove(Message msg) {
            lock(locker) {
                BeginUpdate();
                try {

                    WorkSessionInfo deletedWorkSessionInfo;
                    foreach(var w in msg.WorkSessions) {
                        bool found = All.TryGetValue(w.WorkSessionID, out deletedWorkSessionInfo);
                        if(found)
                            All.TryRemove(w.WorkSessionID, out deletedWorkSessionInfo);
                    }

                } finally {
                    EndUpdate();
                }
            }
        }

        internal static void UpdateAllWSFromOneServer(Message msg) {
            lock(locker) {
                BeginUpdate();
                try {

                    HashSet<Guid> upatedWorkSessionIDs = new HashSet<Guid>();
                    foreach(var w in msg.WorkSessions) {
                        All.GetOrAdd(w.WorkSessionID, w);
                        upatedWorkSessionIDs.Add(w.WorkSessionID);
                    }

                    WorkSessionInfo deletedWorkSessionInfo;
                    foreach(var p in All.Where(v => v.Value.HostServerName == msg.HostServerName)) {
                        if(!upatedWorkSessionIDs.Contains(p.Key)) {
                            All.TryRemove(p.Key, out deletedWorkSessionInfo);
                        }
                    }

                } finally {
                    EndUpdate();
                }
            }
        }

        internal static void ChangeStatus(Message msg) {
            lock(locker) {
                BeginUpdate();
                try {
                    var wsStatus = GetStatusFromOperation(msg.MessageOperation);
                    WorkSessionInfo currentWorkSessionInfo;
                    foreach(var w in msg.WorkSessions) {
                        bool found = All.TryGetValue(w.WorkSessionID, out currentWorkSessionInfo);
                        if(found && currentWorkSessionInfo.HostServerName == msg.HostServerName) {
                            currentWorkSessionInfo.SetStatus(wsStatus);
                        }
                    }

                } finally {
                    EndUpdate();
                }
            }
        }


        private static WorkSessionStatus GetStatusFromOperation(MessageOperation messageOperation) {
            switch(messageOperation) {
                case MessageOperation.AutoSaved:
                    return WorkSessionStatus.AutoSaved;
                case MessageOperation.Hibernated:
                    return WorkSessionStatus.Hibernated;
                case MessageOperation.WokenUp:
                    return WorkSessionStatus.WokenUp;
                case MessageOperation.Add:
                    return WorkSessionStatus.Loaded;
                case MessageOperation.Remove:
                    return WorkSessionStatus.Removed;
            }
            return WorkSessionStatus.Loaded;
        }

        public static void ForEachWorkSession(Action<Guid, WorkSessionInfo> action) {
            lock(locker) {
                BeginUpdate();
                try {

                    foreach(var kvp in All) {
                        action(kvp.Key, kvp.Value);
                    }

                } finally {
                    EndUpdate();
                }
            }
        }

        public static WorkSessionInfo GetWorkSessionInfoByID(string strWorkSessionID) { 
            Guid workSessionID;
            if (Guid.TryParse(strWorkSessionID, out workSessionID)) {
                return GetWorkSessionInfoByID(workSessionID);
            }
            return null;
        }
        public static WorkSessionInfo GetWorkSessionInfoByID(Guid workSessionID) { 
            lock(locker) {
                foreach(var kvp in All) {
                    if(Guid.Equals(workSessionID, kvp.Value.WorkSessionID))
                        return kvp.Value;
                }
                return null;
            }
        }
        public static WorkSessionInfo GetWorkSessionInfoByDocumentID(string documentID) { 
            lock(locker) {
                foreach(var kvp in All) {
                    if(Guid.Equals(documentID, kvp.Value.DocumentId))
                        return kvp.Value;
                }
                return null;
            }
        }

        internal static HashSet<string> GetAllDocumentServers() {
            lock(locker) {
                var servers = new HashSet<string>();
                foreach(var kvp in All) {
                    servers.Add(kvp.Value.HostServerName);
                }

                return servers;
            }
        }
        
        public static List<WorkSessionInfo> GetServerWorkSessionInfos(string serverName) {
            var infos = new List<WorkSessionInfo>();
            foreach(var p in All.Where(v => string.Equals(v.Value.HostServerName, serverName, StringComparison.InvariantCultureIgnoreCase)).Select(x=>x.Value)) {
                infos.Add(new WorkSessionInfo(p.WorkSessionID, p.DocumentId));
            }
            return infos;
        }

        static ServerListStateChangedController workSessionServerStateController = new ServerListStateChangedController(
            GetAllDocumentServers,
            OnServersAdded,
            OnServersRemoved);

        static void OnServersAdded(IEnumerable<string> servers) { 
            RaiseDocumentServerAdded(servers);
        }
        static void OnServersRemoved(IEnumerable<string> servers) { 

        }

        static void BeginUpdate() {
            workSessionServerStateController.BeginUpdate();
        }
        static void EndUpdate() {
            workSessionServerStateController.EndUpdate();
        }

        static void RaiseDocumentServerAdded(IEnumerable<string> serverNames) {
            if(DocumentServerAddedCore != null)
                DocumentServerAddedCore(serverNames);
        }

    }

}
