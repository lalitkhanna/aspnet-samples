using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Web.OfficeAzureCommunication.Utils;

namespace DevExpress.Web.OfficeAzureCommunication {
    public class ServiceBusSettings {
        public virtual string ServiceBusURISchema { get; private set; }
        public virtual string ServiceNamespace { get; private set; }
        public virtual string ServicePath { get; private set; }
        public virtual string SharedAccessKeyName { get; private set; }
        public virtual string SharedAccessKey { get; private set; }

        public ServiceBusSettings(string serviceNamespace, 
                                    string servicePath,
                                    string sharedAccessKeyName, 
                                    string sharedAccessKey) 
            : this("sb", serviceNamespace, servicePath, sharedAccessKeyName, sharedAccessKey) {
        }
        public ServiceBusSettings(string serviceBusURISchema, 
                                    string serviceNamespace, 
                                    string servicePath,
                                    string sharedAccessKeyName, 
                                    string sharedAccessKey) {
            ServiceBusURISchema = serviceBusURISchema;
            ServiceNamespace = serviceNamespace;
            ServicePath = servicePath;
            SharedAccessKeyName = sharedAccessKeyName;
            SharedAccessKey = sharedAccessKey;
        }
        protected ServiceBusSettings ()	{ }
    }

    public class ServiceBusSettingsFromWebConfig : ServiceBusSettings {
        public override string ServiceBusURISchema { get  { return WebConfigUtils.GetAppSetting("ServiceBusURISchema", "sb"); }  }
        public override string ServiceNamespace    { get  { return WebConfigUtils.GetAppSetting("ServiceBusNamespace"); }  }
        public override string ServicePath         { get  { return WebConfigUtils.GetAppSetting("ServiceBusPath"); }  }
        public override string SharedAccessKeyName { get  { return WebConfigUtils.GetAppSetting("ServiceBusSharedAccessKeyName"); }  }
        public override string SharedAccessKey     { get  { return WebConfigUtils.GetAppSetting("ServiceBusSharedAccessKey"); }  }

        public ServiceBusSettingsFromWebConfig() { }
    }

    public static class BroadcastNamespaces {
        public const string DataContractNamespace = "http://DataContractNamespace";
        public const string ServiceContractNamespace = "http://ServiceContractNamespace";
    }
}