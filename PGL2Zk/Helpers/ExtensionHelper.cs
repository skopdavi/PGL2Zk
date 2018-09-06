using PGL2Zk.DataModel;
using PGL2Zk.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGL2Zk.Helpers
{
    public static class ExtensionHelper
    {
        /// <summary>
        /// Doplňující přetížená verze metody IWeekScheduler.EventInTime (pro zjednodušení volání). 
        /// Namísto instance třídy WeekHour očekává běžnější representaci času – instanci třídy DateTime 
        /// </summary>
        public static Event EventInTime(this IWeekScheduler scheduler,
            DateTime time)
        {
            return scheduler.EventInTime(new WeekHour(time));
        }

        /// <summary>
        /// Pomocná (rozšiřující) metoda pro parsování názvu dne (v souladu s daty získanými ze Stagu
        /// očekává česká jména dnů v týdnu, začínající malým písmenem. Jména dnů získává z objektu třídy 
        /// CultureInfo.
        /// </summary>
        public static DayOfWeek ParseDayOfWeek(this string text)
        {
            var culture = new System.Globalization.CultureInfo("cs-CZ");
            var days = culture.DateTimeFormat.DayNames;
            return (DayOfWeek)Array.IndexOf(days, text.ToLower());
        }
    }
}
