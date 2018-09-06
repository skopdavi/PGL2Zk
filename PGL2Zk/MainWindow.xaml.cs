using PGL2Zk.DataModel;
using PGL2Zk.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PGL2Zk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VMMainWindow _vm;

        public MainWindow()
        {
            _vm = new VMMainWindow(this);
            InitializeComponent();
            this.DataContext = _vm;
        }

		private void BtnRemoveStudent_Click(object sender, RoutedEventArgs e)
		{
			if (DgdStudents.SelectedItem != null)
				_vm.RemoveStudent(DgdStudents.SelectedItem as Student);
		}

		private void BtnAddStudent_Click(object sender, RoutedEventArgs e)
		{
			_vm.AddStudent();
		}

		private void BtnAddTeacher_Click(object sender, RoutedEventArgs e)
		{
			_vm.AddTeacher();
		}

		private void BtnRemoveTeacher_Click(object sender, RoutedEventArgs e)
		{
			if (DgdTeachers.SelectedItem != null)
				_vm.RemoveTeacher(DgdTeachers.SelectedItem as Teacher);
		}

		private void BtnSearchTimes_Click(object sender, RoutedEventArgs e)
		{
			_vm.GetFreeTimesForAll();
		}

		private void BtnAddRoom_Click(object sender, RoutedEventArgs e)
		{
			_vm.AddRoom();
		}

		private void BtnRemoveRoom_Click(object sender, RoutedEventArgs e)
		{
			if (DgdRooms.SelectedItem != null)
				_vm.RemoveRoom(DgdRooms.SelectedItem as Room);
		}
	}
}
