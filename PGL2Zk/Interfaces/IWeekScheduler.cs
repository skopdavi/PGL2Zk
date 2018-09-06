using PGL2Zk.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGL2Zk.Interfaces
{
    /// <summary>
    /// Rozhraní, které sdílejí objekty, které poskytují informaci, jaké události se zúčastňují v rámci hodiny
    /// specifikované parametrem třídy WeekHour. 
    /// </summary>
    public interface IWeekScheduler
    {
        /// <summary>
        /// Metoda vrací událost, která se koná v daný čas (v danou hodinu). Pokud taková událost není pak vrací
        /// null!
        /// </summary>
        Event EventInTime(WeekHour time);
    }
}
