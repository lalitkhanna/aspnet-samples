using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DevExpress.Web.OfficeAzureCommunication.Diagnostic {
#if DEBUG

    public static class WorkSessionServerView {
        
        public static string CreateDianosticPageHtml(string serverName, Func<string, string> getHeaderValue) {
            StringBuilder html = new StringBuilder();
            html.AppendLine(GetStylesBlock());
            html.AppendFormat("{0}<hr/>", WorkSessionServerView.GetHeader(serverName));
            if(getHeaderValue != null)
                html.AppendFormat("{0}<hr/>", WorkSessionServerView.GetServerList(getHeaderValue));
            html.AppendFormat("{0}<hr/>", WorkSessionServerView.GetWorkSessionTable());
            html.AppendFormat("{0}<hr/>", WorkSessionServerView.GetTraceLog());
            return html.ToString();
        }
        public static void CreateDianosticPageControl(Control container, string serverName) {
            container.Controls.Clear();
            var html = CreateDianosticPageHtml(serverName, null);
            LiteralControl wrapper = new LiteralControl(html);
            container.Controls.Add(wrapper);
        }

        static string GetHeader(string serverName) {
            var sb = new StringBuilder();
            sb.Append(@"<h1>");
            sb.AppendFormat(@"<span>{0}</span>", serverName);
            sb.AppendFormat(@"<span>{0}</span>", DateTime.Now.ToShortTimeString());
            sb.Append(@"</h1>");
            return sb.ToString();
        }
        static string GetWorkSessionTable() {
            var sb = new StringBuilder();
            sb.AppendLine("<h2>Routing Table</h2>");
            sb.AppendLine("<table class='log'>");
            var row = string.Format("<tr><th>Work Session ID</th><th>Document ID</th><th>RoleInstance Name</th><th>Server Name</th><th>IP</th><th>createdAt</th><th>processed At</th><th>delay(sec)</th><th>status</th></tr>");
            sb.AppendLine(row);

            WorkSessionServer.ForEachWorkSession((id, ws) => {
                row = string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>",
                    id, 
                    ws.DocumentId,
                    ws.RoleInstanceId, 
                    ws.HostServerName, 
                    ws.HostServerIP, 
                    ws.CreateTime, 
                    ws.ProcessedTime,
                    (ws.ProcessedTime - ws.CreateTime).Seconds,
                    Enum.GetName(typeof(WorkSessionStatus), ws.Status)
                );
                sb.AppendLine(row);
            });

            sb.AppendLine("</table>");
            return sb.ToString();
        }

        static string GetServerList(Func<string, string> getHeaderValue) {
            var sb = new StringBuilder();

            sb.AppendLine("<h2>Web Farm configuration</h2>");
            sb.Append("<table class='log'>");
            sb.Append("<tr><th>address</th><th>hash</th></tr>");

            var addresses = DevExpress.Web.OfficeAzureCommunication.Utils.RoleInstanceUtils.GetRoleInstanceAdressList("Azure.WebRole.DocumentSite");
            foreach(var address in addresses) {
                sb.AppendFormat("<tr><td>{0}</td><td><a href='javascript:void(0)' onclick='document.cookie=\"ARRAffinity=;\"; document.cookie=\"ARRAffinity={1}\"'>{1}</a></td></tr>", address, getHeaderValue(address));
            }

            sb.AppendLine("</table>");
            return sb.ToString();
        }

        static string GetTraceLog() {
            var sb = new StringBuilder();

            sb.Append("<h2>Trace log</h2>");
            sb.Append("<table class='log list'>");

            var logs = DevExpress.Web.OfficeAzureCommunication.Diagnostic.Logger.GetLog();
            foreach(var log in logs) {
                sb.Append(string.Format("<tr><td>{0}</td></tr>", log));
            }

            sb.AppendLine("</table>");
            return sb.ToString();
        }
        
        static string GetStylesBlock() {
            return @"
                <style type='text/css'> 
                    table.log td { 
                        text-align: center; 
                        padding: 2px 10px; 
                    }
                    table.log.list td { 
                        text-align: left; 
                    }
                    span { 
                        padding: 0px 10px; 
                    }
                </style>
            ";
        }
    }
    
#endif
}
