using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpress.Web.OfficeAzureCommunication {

    public partial class InterRoleCommunicator {
        static object syncRoot = new Object();
        static volatile InterRoleCommunicator Instance;

        public static void SetUp() {
            SetUp(new ServiceBusSettingsFromWebConfig());
        }
        public static void SetUp(ServiceBusSettings serviceBusSettings) {
            if(Instance == null) {
                lock(syncRoot) {
                    if(Instance == null) {
                        Instance = new InterRoleCommunicator(serviceBusSettings);
                    }
                }
            }
        }

        InterRoleCommunicator(ServiceBusSettings serviceBusSettings) {
            ServiceBusSettings = serviceBusSettings;
            Subscribe();
        }

        ServiceBusPublisher<IMessageServiceChannel> publisher;
        ServiceBusSubscriber<MessageService> subscriber;

        ServiceBusSettings ServiceBusSettings { get; set; }
       
        ServiceBusPublisher<IMessageServiceChannel> Publisher {
            get {
                if(publisher == null)
                    publisher = new ServiceBusPublisher<IMessageServiceChannel>(ServiceBusSettings);
                return publisher;
            }
        }

        internal static void SendMessage(Message msg) {
            lock(syncRoot) {
                if(Instance == null)
                    throw new InvalidOperationException("Call the OfficeAzureDocumentServer.Init(...) static method on Application_Start of Document Server's Global.asax");
                Instance.Publish(msg);
            }
        }
        
        void Publish(Message msg) {
            Publisher.Channel.Publish(msg);
        }
        void Subscribe() {
            if(subscriber == null)
                subscriber = new ServiceBusSubscriber<MessageService>(ServiceBusSettings);
        }
    }

}