using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DevExpress.Web.OfficeAzureCommunication {
    //public class OfficeWebRoleTest : OfficeWebRole {
    //    public OfficeWebRoleTest(string documentServerWebRoleKey, RoleInstance balancerWebRole) : base(documentServerWebRoleKey, balancerWebRole) { }
    //    public OfficeWebRoleTest(RoleInstance documentServerWebRole, string balancerWebRoleKey) : base(documentServerWebRole, balancerWebRoleKey) { }

    //    public void SendAdd() {
    //        CheckSendingOperation();

    //        var message = new Message(CurrentInstanceId, MessageOperation.Add);
    //        message.workSessions.Add(new WorkSessionInfo(Guid.NewGuid(), string.Format("DocId{0}", Guid.NewGuid())));
    //        Communacator.Publish(message);
    //    }
    //}

    //public static class AzureServiceLocator {
    //    static string LoadBalancerRoleName { get; set; }
    //    static bool initialized = false;

    //    public static void Initialize(string loadBalancerRoleName) { 
    //        LoadBalancerRoleName = loadBalancerRoleName;
    //        initialized = true;
    //    }

    //    static List<RoleInstance> FindLoadBalancers() { 
    //        if(!initialized) throw new InvalidOperationException("Using before initialization");
    //        if(string.IsNullOrEmpty(LoadBalancerRoleName)) throw new InvalidOperationException("loadBalancerRoleName can't be empty");

    //        var loadBalancers = new List<RoleInstance>();

    //        foreach(var roleKey in RoleEnvironment.Roles.Keys) {
    //            if(roleKey != LoadBalancerRoleName) continue;

    //            foreach(RoleInstance instance in RoleEnvironment.Roles[roleKey].Instances) {
    //                loadBalancers.Add(instance);
    //            }
    //        }

    //        return loadBalancers;
    //    }
    //}

    //using SessionDictCore = System.Collections.Concurrent.ConcurrentDictionary<System.Guid, WorkSession>;

    //public class WorkSession {
    //    public string DocumentID { get; set; }
    //    public WorkSession(string documentID) {
    //        DocumentID = documentID;
    //    }
    //}

    //public static class WorkSessions {
    //    static object syncRoot = new Object();
    //    private static volatile SessionDictCore sessions;
    //    public static SessionDictCore Sessions {
    //        get {
    //            if(sessions == null) {
    //                lock(syncRoot) {
    //                    if(sessions == null) {
    //                        sessions = new SessionDictCore();
    //                    }
    //                }
    //            }

    //            return sessions;
    //        }
    //    }
    //}

    //public static class WorkSessionsHelper {
    //    public static void AddNRandomWorkSessions(int addCount) {
    //        while(addCount > 0) {
    //            var workSessionId = Guid.NewGuid();
    //            var documentId = Guid.NewGuid();
    //            var workSession = new WorkSession(documentId.ToString());
    //            bool success = WorkSessions.Sessions.TryAdd(workSessionId, workSession);
    //            if(success)
    //                addCount--;
    //        }
    //    }
    //    public static DocumentSetStateMessage CreateMessage(string serverName) {
    //        var message = new DocumentSetStateMessage(serverName);
    //        foreach(var workSession in WorkSessions.Sessions) {
    //            WorkSessionInfo workSessionInfo = new WorkSessionInfo(workSession.Key, workSession.Value.DocumentID);
    //            message.workSessions.Add(workSessionInfo);
    //        }
    //        return message;
    //    }
    //}
    //public class WorkSessionInfo {
    //    public Guid Id { get; private set; }
    //    public string DocumentId { get; private set; }

    //    public WorkSessionInfo(Guid id, string documentId) {
    //        Id = id;
    //        DocumentId = documentId;
    //    }
    //}
    //public class DocumentSetStateMessage {
    //    public string ServerName { get; private set; }
    //    public List<WorkSessionInfo> workSessions { get; private set; }

    //    public DocumentSetStateMessage(string serverName) {
    //        ServerName = serverName;
    //        List<WorkSessionInfo> workSessions = new List<WorkSessionInfo>();
    //    }
    //}

}
