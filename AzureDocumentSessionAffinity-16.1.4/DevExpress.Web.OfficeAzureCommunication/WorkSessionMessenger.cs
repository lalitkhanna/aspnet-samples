using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Web.OfficeAzureCommunication.Utils;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DevExpress.Web.OfficeAzureCommunication {
    public static class WorkSessionMessenger {

        public static void SendUpdateAllMessage(List<WorkSessionInfo> workSessions) {
            MessageOperation operation = MessageOperation.UpdateAll;
            var message = CreateMessage(operation, workSessions);

            InterRoleCommunicator.SendMessage(message);
        }

        public static void SendMessage(MessageOperation operation, Guid workSessionID, string documentID) {
            var message = CreateMessage(operation, workSessionID, documentID);
            InterRoleCommunicator.SendMessage(message);
        }

        static Message CreateMessage(MessageOperation operation, List<WorkSessionInfo> workSessions) {
            var message = new Message(
                RoleEnvironment.CurrentRoleInstance.Id,
                System.Environment.MachineName,
                NetUtils.GetLocalIPAddress(),
                operation
            );
            message.WorkSessions.AddRange(workSessions);
            return message;
        }

        static Message CreateMessage(MessageOperation operation, Guid workSessionID, string documentID) {
            Message message = CreateMessage(operation, new List<WorkSessionInfo>());
            message.WorkSessions.Add(new WorkSessionInfo(workSessionID, documentID));
            return message;
        }

    }
}
