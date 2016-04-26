using System.Configuration;
using System.IO;
using System.Web;

namespace MiniCarSales.Utility
{
    public class CommonFunction
    {
        public static string GetDatabaseFilePath()
        {
            var path = ConfigurationManager.AppSettings["DatabaseFilePath"];

            return HttpContext.Current.Server.MapPath(path);
        }

        public static string GetUploadPath()
        {
            var path = ConfigurationManager.AppSettings["UploadFilePath"];
       
            return Path.Combine(HttpContext.Current.Request.UrlReferrer.OriginalString, path);
        }

        public static string GetUploadLocation()
        {
            var path = ConfigurationManager.AppSettings["UploadFilePath"];

            return HttpContext.Current.Server.MapPath(path);
        }
    }
}