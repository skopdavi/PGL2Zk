using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGL2Zk.DataModel
{
    /// <summary>
    /// Representuje událost v kalendáři (s granularitou hodina a s výše uvedenou týdenní representací).
    /// </summary>
    public class Event
    {
        public WeekHour Start { get; private set; } // počáteční hodina
        public int Duration { get; private set; } // trvání v hodinách (událost vždy musí zaujímat celé hodiny)
        public string Description { get; private set; } //stručný popis

        public Event(WeekHour start, int duration, string description)
        {
            Start = start;
            Duration = duration;
            Description = description;
        }

        /// <summary>
        /// Metoda testuje, zda (celá) hodina representovaná údajem time (třídy WeekHour) leží uvnitř časového
        /// intervalu události (v naší representaci tam leží buď celá nebo vůbec) 
        /// </summary>
        public bool WeekHourIsInside(WeekHour time)
        {
            return time.Year == this.Start.Year &&
                time.Week == this.Start.Week && //FIXME: eventy přes hranici týdne (malá priorita)
                time.DayOfWeek == this.Start.DayOfWeek && //FIXME: eventy přes hranici dne (malá priorita)
                time.Hour >= this.Start.Hour &&
                time.Hour <= this.Start.Hour + Duration - 1;
        }
    }
}
