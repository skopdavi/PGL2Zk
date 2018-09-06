using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGL2Zk.DataModel
{
	public class Teacher : Person
	{
		public Teacher(string osobniCislo, string prijmeni, string jmeno) : base(osobniCislo, prijmeni, jmeno)
		{
		}

		protected override string StagService()
		{
			return "rozvrhy/getRozvrhByUcitel";
		}

		protected override string IdKey()
		{
			return "ucitIdno";
		}
	}
}
