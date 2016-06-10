using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace DevExpress.Web.OfficeAzureRoutingServer {

    class HeaderCookieHelper {
        const string CookieHeaderName = "Cookie";

        public static void PatchHeaderCookieGUID(HttpApplication application, string cookieNameToPatch, string newCookieValue) {
            string cookies = GetRequestHeaderValue(application, CookieHeaderName);
            string patchedCookies = HeaderCookieHelper.PatchCookies(cookies, cookieNameToPatch, newCookieValue);
            HeaderCookieHelper.SetRequestHeaderValue(application, CookieHeaderName, patchedCookies);
        }

        static string GetRequestHeaderValue(HttpApplication application, string headerName) {
            if(application.Request.Headers.AllKeys.Contains(headerName)) {
                return application.Request.Headers[headerName];
            }
            return string.Empty;
        }

        static void SetRequestHeaderValue(HttpApplication application, string headerName, string headerValue) {
            NameValueCollection headers = application.Request.Headers;
            headers.GetType().GetProperty("IsReadOnly", BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase).SetValue(headers, false, null);
            if(headers.AllKeys.Contains<string>(headerName)) {
                headers[headerName] = headerValue;
            } else {
                headers.Add(headerName, headerValue);
            }
        }

        static string PatchCookies(string cookies, string cookieName, string cookieValue) {
            if(CheckCookieExists(cookieName, cookies)) {
                cookies = ReplaceCookieGUID(cookies, cookieName, cookieValue);
            } else {
                cookies = AppendCookieGUID(cookies, cookieName, cookieValue);
            }
            return cookies;
        }

        static bool CheckCookieExists(string cookieName, string cookieValue) {
            return cookieValue.Contains(cookieName + "=");
        }

        static string AppendCookieGUID(string cookies, string cookieName, string cookieValue) {
            if(!string.IsNullOrEmpty(cookies))
                cookies += ";";
            cookies += "=" + cookieValue;
            return cookies;
        }

        static string ReplaceCookieGUID(string cookies, string cookieName, string cookieValue) {
            Regex regex = new Regex(cookieName + "=[0-9A-Fa-f]{64}");
            string replacement = cookieName + "=" + cookieValue;
            return regex.Replace(cookies, replacement);
        }

    }
}
