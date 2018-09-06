using Newtonsoft.Json.Linq;
using PGL2Zk.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PGL2Zk.Helpers
{
    public class ServiceHelper : IWebServiceLoader
    {
        /// <summary>
        /// Základní URL webové služby (přesněji skupiny webových služeb).
        /// </summary>
        public string BaseUrl { get; private set; }
        public ServiceHelper(string baseUrl)
        {
            this.BaseUrl = baseUrl;

        }

        /// <summary>
        /// Čte výsledek služby jako JSON dokument, který následně parsuje.
        /// </summary>
        /// <returns>The object.</returns>
        public JObject LoadObject(string service, Dictionary<string, string> arguments)
        {
            // sestavení URL
            string template = "{0}/{1}?{2}outputFormat=json";
            string argstr = "";
            //konstrukce seznamu argumedntů
            foreach (string key in arguments.Keys)
            {
                argstr += key + "=" + arguments[key] + "&";
            }
            //sestavení výsledného URL
            string url = string.Format(template, this.BaseUrl,
                service, argstr);
            // stažení obsahu pomocí WebClienta
            WebClient client = new WebClient();
            if (GlobalHelper.UserName != string.Empty)
                client.Credentials = new NetworkCredential() { UserName = GlobalHelper.UserName, Password = GlobalHelper.Password };
			client.Encoding = Encoding.UTF8;
            string reply = client.DownloadString(url);
            // parsování výsledku (STAG vrací formálně pole, jehož prvním a jediným členem je objekt nesoucí
            // příslušné informace
            JArray root = (JArray)JToken.Parse(reply);
            return (JObject)(root[0]);
        }
    }
}