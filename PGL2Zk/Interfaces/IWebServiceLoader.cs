using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGL2Zk.Interfaces
{
    /// <summary>
    /// Rozhraní jednoduchého stahovače REST webových služeb vracejících JSON dokumenty.
    /// </summary>
    public interface IWebServiceLoader
    {
        JObject LoadObject(string service, Dictionary<string, string> arguments);
    }

}
