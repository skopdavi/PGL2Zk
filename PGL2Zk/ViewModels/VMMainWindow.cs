using PGL2Zk.DataModel;
using PGL2Zk.Helpers;
using PGL2Zk.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PGL2Zk.ViewModels
{
	public class VMMainWindow : ViewModelBase
	{
		private const string _studentGetByNameUrl = "student/najdiStudentyPodleJmena";
		private const string _teacherGetByNameUrl = "ucitel/najdiUcitelePodleJmena";
		private PersonHelper _persHelper;
		private PersonHelper _teacherHelper;
		private Window _mainWindow;

		#region Properties
		private ObservableCollection<Teacher> lstTeachers;
		public ObservableCollection<Teacher> LstTeachers
		{
			get { return lstTeachers; }
			set
			{
				lstTeachers = value;
				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<Student> lstStudents;
		public ObservableCollection<Student> LstStudents
		{
			get { return lstStudents; }
			set
			{
				lstStudents = value;
				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<Room> lstRooms;
		public ObservableCollection<Room> LstRooms
		{
			get { return lstRooms; }
			set
			{
				lstRooms = value;
				NotifyPropertyChanged();
			}
		}

		private string surNameStudent;
		public string SurNameStudent
		{
			get { return surNameStudent; }
			set
			{
				surNameStudent = value;
				NotifyPropertyChanged();
			}
		}

		private string firstNameStudent;
		public string FirstNameStudent
		{
			get { return firstNameStudent; }
			set
			{
				firstNameStudent = value;
				NotifyPropertyChanged();
			}
		}

		private string surNameTeacher;
		public string SurNameTeacher
		{
			get { return surNameTeacher; }
			set
			{
				surNameTeacher = value;
				NotifyPropertyChanged();
			}
		}

		private string firstNameTeacher;
		public string FirstNameTeacher
		{
			get { return firstNameTeacher; }
			set
			{
				firstNameTeacher = value;
				NotifyPropertyChanged();
			}
		}


		private DateTime dateFrom = DateTime.Today;
		public DateTime DateFrom
		{
			get { return dateFrom; }
			set
			{
				dateFrom = value;
				NotifyPropertyChanged();
			}
		}

		private DateTime dateTo = DateTime.Today.AddDays(7);
		public DateTime DateTo
		{
			get { return dateTo; }
			set
			{
				dateTo = value;
				NotifyPropertyChanged();
			}
		}

		private string roomNumber;
		public string RoomNumber
		{
			get { return roomNumber; }
			set
			{
				roomNumber = value;
				NotifyPropertyChanged();
			}
		}

		#endregion

		public VMMainWindow(Window mainWindow)
		{
			/*Teacher fiser = new Teacher("251", "Fišer","Jiří");
			Student skop = new Student("F17045", "Škop","David");
			fiser.LoadScheduler();
			skop.LoadScheduler();
            Event e = fiser.EventInTime(new WeekHour(DateTime.Now));*/
			_mainWindow = mainWindow;
			LstStudents = new ObservableCollection<Student>();
			LstTeachers = new ObservableCollection<Teacher>();
			LstRooms = new ObservableCollection<Room>();
			_persHelper = new PersonHelper(typeof(Student), _studentGetByNameUrl);
			_teacherHelper = new PersonHelper(typeof(Teacher), _teacherGetByNameUrl);
		}

		public void AddStudent()
		{
			foreach (var student in _persHelper.GetPeopleByName(SurNameStudent, FirstNameStudent))
			{
				LstStudents.Add(student as Student);
			}
		}

		public void RemoveStudent(Student student)
		{
			LstStudents.Remove(student);
		}

		public void AddTeacher()
		{
			foreach (var teacher in _teacherHelper.GetPeopleByName(SurNameTeacher, FirstNameTeacher))
			{
				LstTeachers.Add(teacher as Teacher);
			}
		}

		public void RemoveTeacher(Teacher teacher)
		{
			LstTeachers.Remove(teacher);
		}

		public void GetFreeTimesForAll()
		{
			List<IWeekScheduler> objects = new List<IWeekScheduler>();
			foreach (var teacher in LstTeachers)
				teacher.LoadScheduler();
			objects.AddRange(LstTeachers);
			foreach (var student in LstStudents)
				student.LoadScheduler();
			objects.AddRange(LstStudents);
			foreach (var room in LstRooms)
				room.LoadScheduler();
			objects.AddRange(LstRooms);
			List<DateTime> freeTimes = null;
			foreach (var obj in objects)
			{
				freeTimes = GetFreeTimesForPerson(freeTimes, obj);
			}
			if (freeTimes != null)
			{
				StringBuilder sb = new StringBuilder();
				if (freeTimes.Count > 10)
					sb.AppendLine(string.Format("Prvních 10 volných časů pro uvedené subjekty:"));
				else
					sb.AppendLine(string.Format("Volné časy pro uvedené subjekty:"));
				foreach (var time in freeTimes.Take(10))
					sb.AppendLine(string.Format("	Dne {0} v čase od {1} do {2}.", time.Date.ToString("dd. MM. yyyy"), time.Hour, time.AddHours(1).Hour));
				MessageBox.Show(_mainWindow, sb.ToString(), "Informace");
			}
			else
			{
				MessageBox.Show(_mainWindow, "Pro zadané období neexistuje pro zadané subjekty volný čas.", "Informace");
			}
		}

		private List<DateTime> GetFreeTimesForPerson(List<DateTime> freeTimes, IWeekScheduler person)
		{
			List<DateTime> ret = new List<DateTime>();
			if (freeTimes == null)
			{
				//Zjistit volné časy pro objekt
				DateTime dateNow = DateFrom;
				while (dateNow <= DateTo.AddDays(1).AddHours(-1))
				{
					if (dateNow.Hour < 7 || dateNow.Hour > 18)
					{
						dateNow = dateNow.AddHours(1);
						continue;
					}

					var ev = person.EventInTime(dateNow);
					if (ev == null)
						ret.Add(dateNow);
					dateNow = dateNow.AddHours(1);
				}
			}
			else if (freeTimes.Count == 0)
			{
				ret = null;
			}
			else
			{
				//Porovnat volné časy s časy objektu
				foreach (var dateNow in freeTimes)
				{
					var ev = person.EventInTime(dateNow);
					if (ev == null)
						ret.Add(dateNow);
				}
			}
			return ret;
		}

		public void AddRoom()
		{
			foreach (var room in RoomHelper.GetRoomByNumber(RoomNumber))
			{
				LstRooms.Add(room);
				room.LoadScheduler();
			}
		}

		public void RemoveRoom(Room room)
		{
			LstRooms.Remove(room);
		}
	}
}
