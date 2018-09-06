using Newtonsoft.Json.Linq;
using PGL2Zk.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGL2Zk.Helpers
{
	public class PersonHelper
	{
		private string _url;
		private Type _type;

		public PersonHelper(Type type, string url)
		{
			_url = url;
			_type = type;
		}

		public List<Person> GetPeopleByName(string surName, string firstName)
		{
			List<Person> ret = new List<Person>();
			ServiceHelper loader = new ServiceHelper("https://ws.ujep.cz/ws/services/rest");
			string key="";
			var sargs = new Dictionary<string, string>(); //parametry webové služby
			sargs["prijmeni"] = surName;
			sargs["jmeno"] = firstName;
			sargs["lang"] = "cs";
			JObject actions = loader.LoadObject(_url, sargs); ///čte data  ze Stagu

			if (_type == typeof(Student))
			{
				key = "student";
			}
			else if (_type == typeof(Teacher))
			{
				key = "ucitel";
			}

			foreach (JToken token in (JArray)actions[key])
			{ //pro každou rozvrhovou akci
				JObject action = (JObject)token; //po přetypování na JObject
				try
				{
					Person pers=null;
					if (_type == typeof(Student))
					{
						pers = new Student(action.Value<string>("osCislo"), action.Value<string>("prijmeni"), action.Value<string>("jmeno"));
					}
					else if (_type == typeof(Teacher))
					{
						pers = new Teacher(action.Value<string>("ucitIdno"), action.Value<string>("prijmeni"), action.Value<string>("jmeno"));
					}
					if(pers!=null)
					ret.Add(pers);
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
