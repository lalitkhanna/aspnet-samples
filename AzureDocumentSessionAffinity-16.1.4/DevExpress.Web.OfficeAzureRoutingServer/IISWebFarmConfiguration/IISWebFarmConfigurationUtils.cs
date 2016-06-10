using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.Administration;
using ConfigurationElementAttributes = System.Collections.Generic.Dictionary<string, string>;

namespace DevExpress.Web.OfficeAzureRoutingServer
{
    class IISWebFarmConfigurationUtils
    {
        const string WEB_FARMS_TAGNAME = "webFarms";
        const string WEB_FARM_TAGNAME = "webFarm";

        internal static ConfigurationElement SetUp(string farmName, bool createUrlRewriteRule = true, bool useClientAffinity = true)
        {
            using (ServerManager manager = new ServerManager())
            {
                ConfigurationElementCollection webFarmsConfiguration = GetWebFarmsConfiguration(manager);
                ConfigurationElement webFarm = GetWebFarmConfiguration(webFarmsConfiguration, farmName);
                if (webFarm == null)
                {
                    webFarm = webFarmsConfiguration.CreateElement(WEB_FARM_TAGNAME);
                    webFarm["name"] = farmName;
                    webFarmsConfiguration.Add(webFarm);
                    manager.CommitChanges();

                    if (createUrlRewriteRule)
                        IISUrlRewriterConfigurationUtils.CreateURLRewriteRule(farmName);
                    if (useClientAffinity)
                        SetApplicationRequestRoutingParameters(farmName);
                }
                return webFarm;
            }
        }

        static void SetApplicationRequestRoutingParameters(string farmName)
        {
            using (ServerManager manager = new ServerManager())
            {
                ConfigurationElementCollection webFarmsConfiguration = GetWebFarmsConfiguration(manager);
                ConfigurationElement webFarm = GetWebFarmConfiguration(webFarmsConfiguration, farmName);
                if (webFarm != null)
                {
                    webFarm.GetChildElement("applicationRequestRouting").GetChildElement("affinity")["useCookie"] = true;
                    manager.CommitChanges();
                }
            }
        }

        internal static bool AddServer(string farmName, ServerInfo endpoint)
        {
            return AddServers(farmName, new List<ServerInfo>() { endpoint });
        }
        internal static bool AddServers(string farmName, IEnumerable<ServerInfo> endpoints)
        {
            using(ServerManager manager = new ServerManager()) {
                ConfigurationElementCollection webFarmsConfiguration = GetWebFarmsConfiguration(manager);
                ConfigurationElement webFarm = GetWebFarmConfiguration(webFarmsConfiguration, farmName);
                if(webFarm != null) {
                    ConfigurationElementCollection servers = webFarm.GetCollection();
                    foreach(ServerInfo point in endpoints) {
                        ConfigurationElement serverConfiguration = ConfigurationElementUtils.FindElement(servers, "server", new Dictionary<string, string>() { { "address", point.Address } } );
                        if (serverConfiguration == null) { 
                            serverConfiguration = servers.CreateElement("server");
                            serverConfiguration["address"] = point.Address;
                            serverConfiguration["enabled"] = true;

                            ConfigurationElement connectionConfiguration = serverConfiguration.GetChildElement("applicationRequestRouting");
                            if(point.HttpPort.HasValue)
                                connectionConfiguration["httpPort"] = point.HttpPort;
                            if(point.HttpsPort.HasValue)
                                connectionConfiguration["httpsPort"] = point.HttpsPort;
                            if(point.Weight.HasValue)
                                connectionConfiguration["weight"] = point.Weight;
                            
                            servers.Add(serverConfiguration);
                        }
                    }
                    manager.CommitChanges();
                    return true;
                }
            }
            return false;
        }

        static ConfigurationElementCollection GetWebFarmsConfiguration(ServerManager manager)
        {
            return manager.GetApplicationHostConfiguration().GetSection(WEB_FARMS_TAGNAME).GetCollection();
        }
        static ConfigurationElement GetWebFarmConfiguration(ConfigurationElementCollection webFarmConfiguration, string farmName)
        {
            var attributes = new ConfigurationElementAttributes() { { "name", farmName } };
            return ConfigurationElementUtils.FindElement(webFarmConfiguration, WEB_FARM_TAGNAME, attributes);
        }
    }
}
