using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGL2Zk.DataModel
{
	/// <summary>
	/// Representace časového údaje orientovaná na využití v rozvrzích orientovaných na akce 
	/// s týdenní periodicitou (např. školní rozvrhy). Navíc počítá s hodinovou granularitou 
	/// (údaj určuje vždy celou hodinu).
	/// </summary>
	public class WeekHour
	{
		public int Year { get; private set; } //rok (v ISO týdenní representaci nemusí být shodný s kalendářním!)
		public int Week { get; private set; } //ISO týden (viz 
		public DayOfWeek DayOfWeek { get; private set; } // den v týdnu
		public int Hour { get; private set; } //hodina (0-23)

		/// <summary>
		/// Základní konstruktor (konstruuje WeekHour z jednotlivých částí). Nekontroluje správnost
		/// vstupních údajů (roku, týdne [1-52 resp. 53] a hodiny (0-23))
		/// </summary>
		public WeekHour(int year, int week, DayOfWeek dayOfWeek, int hour)
		{
			//FIXED//FIXME: kontrola vstupních dat, při neplatnosti výjimka ArgumentException
			try
			{
				DateTime temp = new DateTime(year, 1, 1).AddDays(7 * (week - 1));
				int diff = (int)dayOfWeek-(int)temp.DayOfWeek ;
				temp = temp.AddDays(diff);
				if (!(hour >= 0 && hour <= 23))
					throw new ArgumentException();
			}
			catch (Exception ex)
			{
				throw new ArgumentException();
			}
			this.Year = year;
			this.Week = week;
			this.DayOfWeek = dayOfWeek;
			this.Hour = hour;
		}

		/// <summary>
		/// Pomocný konstruktor konstruující objekt s WeekHour na základě běžnější representace DataTime
		/// (den v měsící, měsíc, rok, 
		/// </summary>
		public WeekHour(DateTime dt)
		{
			this.DayOfWeek = dt.DayOfWeek;
			if (DayOfWeek >= DayOfWeek.Monday && DayOfWeek <= DayOfWeek.Wednesday)
			{
				dt = dt.AddDays(3);
			}
			this.Year = dt.Year; //FIXED//FIXME: oprava v kalendářní roce pro první ISO týdny na rozhraní roku 
			this.Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear
				(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday); //FIXME: neověřeno

			this.Hour = dt.Hour;
		}

		public override string ToString()
		{
			return string.Format("{0}-{1}-{2} {3}",
				Year, Week, DayOfWeek, Hour);
		}
	}
}
