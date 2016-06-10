using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpress.Web.OfficeAzureCommunication.Utils {
    public static class WebConfigUtils {
        public static string GetAppSetting(string key) {
            return GetAppSetting(key, null);
        }
        public static string GetAppSetting(string key, string defaultValue) {
            string value = null;
            try {
                value = ConfigurationManager.AppSettings[key];
            } catch { }
        
            return value != null ? value : defaultValue;
        }
    }
}
