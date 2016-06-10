using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpress.Web.OfficeAzureRoutingServer
{
    public static class WebFarmConfiguration
    {
        private const string defaultFarmName = "ARRAffinity";
        
        public static string FarmName { get { return defaultFarmName; } }
    }

    public static class RoutingConfiguration {
        private const string defaultAffinityCookieName = "ARRAffinity";
        private const string queryStringParameterName = "dxwsid";
        private const string requestParamKeyName = "ASPxOfficeWorkSessionID";
        
        public static string AffinityCookieName { get { return defaultAffinityCookieName; } }
        public static string QueryStringParameterName { get { return queryStringParameterName; } }
        public static string RequestParamKeyName { get { return requestParamKeyName; } }
        public static bool UseCookie { get { return true; } }
    }
}