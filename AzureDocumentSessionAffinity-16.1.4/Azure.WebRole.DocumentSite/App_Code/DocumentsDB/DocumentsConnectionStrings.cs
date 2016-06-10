using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;

namespace DocumentSite {
    public static class DocumentsConnectionStrings {
        static string CONNECTION_STRING_KEY = "DocumentsConnectionString";

        public static string Current {
            get { return CloudConfigurationManager.GetSetting(CONNECTION_STRING_KEY); }
        }
    }
}