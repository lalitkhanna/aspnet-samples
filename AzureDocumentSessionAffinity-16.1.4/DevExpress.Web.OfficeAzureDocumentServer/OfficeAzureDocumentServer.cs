using System;
using System.Collections.Generic;
using DevExpress.Web.OfficeAzureCommunication;
using DevExpress.Web.Office.Internal;
using Microsoft.WindowsAzure.ServiceRuntime;
using DevExpress.Web.OfficeAzureCommunication.Utils;

namespace DevExpress.Web.OfficeAzureDocumentServer {
    public static class OfficeAzureDocumentServer {
        
        public static void Init() {
            InterRoleCommunicator.SetUp();
            InitControlsConjunction();
            InitPingService();
        }

        static void InitControlsConjunction() {
            WorkSessionAdminTools.Created += WorkSessionAdminTools_Created;
            WorkSessionAdminTools.Disposed += WorkSessionAdminTools_Disposed;
            WorkSessionAdminTools.AutoSaved += WorkSessionAdminTools_AutoSaved;
            WorkSessionAdminTools.Hibernated += WorkSessionAdminTools_Hibernated;
            WorkSessionAdminTools.WokeUp += WorkSessionAdminTools_WokeUp;
        }

        static void InitPingService() {
            string broadcastIntervalSetting = WebConfigUtils.GetAppSetting("ServerStatusBroadcastInterval", PingService.DefaultInterval.ToString());
            int broadcastInterval;
            if(int.TryParse(broadcastIntervalSetting, out broadcastInterval))
                PingService.Start(broadcastInterval);
        }


        static void WorkSessionAdminTools_WokeUp(IWorkSession workSession, DocumentDiagnosticEventArgs e) {
            WorkSessionMessenger.SendMessage(MessageOperation.WokenUp, workSession.ID, workSession.DocumentInfo.DocumentId);
            #if DEBUG
            Log(workSession, "WokenUp");
            #endif
        }

        static void WorkSessionAdminTools_Hibernated(IWorkSession workSession, DocumentDiagnosticEventArgs e) {
            WorkSessionMessenger.SendMessage(MessageOperation.Hibernated, workSession.ID, workSession.DocumentInfo.DocumentId);
            #if DEBUG
            Log(workSession, "Hibernated");
            #endif
        }

        static void WorkSessionAdminTools_AutoSaved(IWorkSession workSession, DocumentDiagnosticEventArgs e) {
            WorkSessionMessenger.SendMessage(MessageOperation.AutoSaved, workSession.ID, workSession.DocumentInfo.DocumentId);
            #if DEBUG
            Log(workSession, "AutoSaved");
            #endif
        }

        static void WorkSessionAdminTools_Created(IWorkSession workSession, DocumentDiagnosticEventArgs e) {
            WorkSessionMessenger.SendMessage(MessageOperation.Add, workSession.ID, workSession.DocumentInfo.DocumentId);
            #if DEBUG
            Log(workSession, "Add");
            #endif
        }

        static void WorkSessionAdminTools_Disposed(IWorkSession workSession, DocumentDiagnosticEventArgs e) {
            WorkSessionMessenger.SendMessage(MessageOperation.Remove, workSession.ID, workSession.DocumentInfo.DocumentId);
            #if DEBUG
            Log(workSession, "Remove");
            #endif
        }

        
        #if DEBUG
        private static void Log(IWorkSession workSession, string eventName) {
            DevExpress.Web.OfficeAzureCommunication.Diagnostic.Logger.Log(string.Format("WorkSessionRegisterer: {0} {1} {2}", eventName, workSession.ID, workSession.DocumentInfo.DocumentId));
        }
        #endif

    }
}
