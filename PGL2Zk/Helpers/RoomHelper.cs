using Newtonsoft.Json.Linq;
using PGL2Zk.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGL2Zk.Helpers
{
	public static class RoomHelper
	{
		private const string _url = "mistnost/getMistnostiInfo";

		public static List<Room> GetRoomByNumber(string number)
		{
			List<Room> ret = new List<Room>();
			ServiceHelper loader = new ServiceHelper("https://ws.ujep.cz/ws/services/rest");
			var sargs = new Dictionary<string, string>(); //parametry webové služby
			sargs["cisloMistnosti"] = number;
			sargs["pracoviste"] = "%";
			sargs["typ"] = "%";
			sargs["zkrBudovy"] = "%";
			sargs["lang"] = "cs";
			JObject actions = loader.LoadObject(_url, sargs); ///čte data  ze Stagu
			foreach (JToken token in (JArray)actions["mistnostInfo"])
			{ //pro každou rozvrhovou akci
				JObject action = (JObject)token; //po přetypování na JObject
				try
				{
					Room room = new Room(action.Value<string>("zkrBudovy"), action.Value<string>("cisloMistnosti"), action.Value<string>("pracoviste"), action.Value<string>("typ"), action.Value<string>("adresaBudovy"));

					ret.Add(room);
				}
				catch (InvalidCastException)
				{ //FIXME: řešit i řádky s netypickým formátováním nebo chybějícími daty
					continue; //špatně formátované řádky jsou ignorovány
				}
			}
			return ret;
		}
	}
}
