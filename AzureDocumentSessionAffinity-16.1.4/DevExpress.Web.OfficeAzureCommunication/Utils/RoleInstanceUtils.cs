using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DevExpress.Web.OfficeAzureCommunication.Utils {
    public static class RoleInstanceUtils {
        public static IEnumerable<string> GetRoleInstanceAdressList(string DocumentServerRoleName) {
            var role = FindRoleByName(DocumentServerRoleName);
            var addresses = GetRoleInstancesIP(role);
            return addresses;
        }

        private static Role FindRoleByName(string DocumentServerRoleName) {
            var queryLondonCustomers = from role in RoleEnvironment.Roles.Values
                                       where role.Name == DocumentServerRoleName
                                       select role;
            return queryLondonCustomers.First();
        }

        private static IEnumerable<string> GetRoleInstancesIP(Role role) {
            var adresses = from instance in role.Instances
                           from endPoint in instance.InstanceEndpoints.Values
                           select endPoint.IPEndpoint != null ?
                               endPoint.IPEndpoint.Address.ToString() :
                                   endPoint.PublicIPEndpoint != null ?
                                       endPoint.PublicIPEndpoint.Address.ToString() : "";
            return adresses.Distinct();
        }
    }
}
