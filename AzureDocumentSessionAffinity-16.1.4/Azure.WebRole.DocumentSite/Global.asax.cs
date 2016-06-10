using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using DevExpress.Web.OfficeAzureCommunication;
using DevExpress.Web.OfficeAzureDocumentServer;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DocumentSite {
    public class Global : System.Web.HttpApplication {

        protected void Application_Start(object sender, EventArgs e) {
            OfficeAzureDocumentServer.Init();

            HibernationInit();
        }

        private void HibernationInit() {
            DevExpress.Web.Office.DocumentManager.HibernationStoragePath = Server.MapPath("~/App_Data/HibernationStorage/");
            DevExpress.Web.Office.DocumentManager.HibernateTimeout = TimeSpan.FromMinutes(1);
            DevExpress.Web.Office.DocumentManager.HibernateAllDocumentsOnApplicationEnd = true;
            DevExpress.Web.Office.DocumentManager.EnableHibernation = true;
        }

        protected void Session_Start(object sender, EventArgs e) {

        }

        protected void Application_BeginRequest(object sender, EventArgs e) {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) {

        }

        protected void Application_Error(object sender, EventArgs e) {
        }

        protected void Session_End(object sender, EventArgs e) {

        }

        protected void Application_End(object sender, EventArgs e) {
        }
    }
}