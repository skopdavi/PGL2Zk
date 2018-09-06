using Newtonsoft.Json.Linq;
using PGL2Zk.Helpers;
using PGL2Zk.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGL2Zk.DataModel
{
	public class Room : IWeekScheduler, IHasTimeTable
	{
		private const string _url = "rozvrhy/getRozvrhByMistnost";

		public Event EventInTime(WeekHour time)
		{
			foreach (Event e in scheduler)
			{
				if (e.WeekHourIsInside(time))
					return e;
			}
			return null;
		}

		private List<Event> scheduler = new List<Event>();

		private string building;
		public string Building
		{
			get
			{
				return building;
			}
			set
			{
				building = value;
			}
		}

		private string roomNumber;
		public string RoomNumber
		{
			get
			{
				return roomNumber;
			}
			set
			{
				roomNumber = value;
			}
		}

		private string workPlace;
		public string WorkPlace
		{
			get
			{
				return workPlace;
			}
			set
			{
				workPlace = value;
			}
		}

		private string type;
		public string Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}

		private string address;
		public string Address
		{
			get
			{
				return address;
			}
			set
			{
				address = value;
			}
		}

		public Room(string building, string roomNumber,string workPlace,string type,string address)
		{
			this.building = building;
			this.roomNumber = roomNumber;
			this.type = type;
			this.workPlace = workPlace;
			this.address = address;
		}

		public void LoadScheduler()
		{
			ServiceHelper loader = new ServiceHelper("https://ws.ujep.cz/ws/services/rest");

			var sargs = new Dictionary<string, string>(); //parametry webové služby
			sargs["budova"] = building;
			sargs["mistnost"] = roomNumber;
			JObject actions = loader.LoadObject(_url, sargs); ///čte data  ze Stagu
			foreach (JToken token in (JArray)actions["rozvrhovaAkce"])
			{ //pro každou rozvrhovou akci
				JObject action = (JObject)token; //po přetypování na JObject
				string description = string.Format("{0}/{1} {2} {3}", //získává popis (identifikaci předmětu)
					action.Value<string>("katedra"), action.Value<string>("predmet"),
					action.Value<string>("nazev"), action.Value<string>("typAkce"));
				try
				{
					int fromWeek = action.Value<int>("tydenOd"); //a údaje o čase akce
					int toWeek = action.Value<int>("tydenDo");
					int fromHour = action.Value<int>("hodinaOd") + 6; //hodiny ve Stagu jsou vyučovací (1 hodina = 7:00-8:00)
					int toHour = action.Value<int>("hodinaDo") + 6;
					int year = action.Value<int>("rok");

					//FIXME: neřeší akce trvající přes hranici roku (není kritické, takové akce zatím nejsou)
					if (fromWeek < 32) //letní semestr je v dalším roce
						year++;

					string weekType = action.Value<string>("tydenZkr"); //další zpracování řídí typ týdenního opakování
					DayOfWeek dayOfWeek = action.Value<string>("den").ParseDayOfWeek();

					for (int week = fromWeek; week <= toWeek; week++)
					{ //a nyní pro každý, týden v němž se akce koná ...
						if (weekType == "K" ||  //akce je každý týden v širším rozsahu (typicky semestr)
							weekType == "J" ||  //akce je typicky jednorázová (jen v daném týdnu)
							(weekType == "S" && week % 2 == 0) || //akce je jen v sudém týdnu (a proto se přidá jen někdy) 
							(weekType == "L" && week % 2 == 1))  //akce je v lichém týdnu (proto se přidá jen někdy)
							scheduler.Add(new Event(  // ... přidá do seznamu
								new WeekHour(year, week, dayOfWeek, fromHour),
								toHour - fromHour + 1,
								description));
					}
				}
				catch (InvalidCastException)
				{ //FIXME: řešit i řádky s netypickým formátováním nebo chybějícími daty
					continue; //špatně formátované řádky jsou ignorovány
				}
			}
		}
	}
}
