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
using System.Windows.Shapes;
using StudentManagerApp.PersonClasses;

namespace StudentManagerApp
{
    /// <summary>
    /// Interaction logic for PersonCourseWindow.xaml
    /// </summary>
    public partial class PersonCourseWindow : Window
    {
        public Person CurrentPerson { get; set;}
        public PersonCourseWindow(Person person)
        {
            InitializeComponent();          
            CurrentPerson = person;
            PersonName.Content = person.Name;
            if (person is Student)
            {
                EnlistStudentCourses();
            }
            else
            {
                EnlistProfCourses();
            }
        }

        private void EnlistProfCourses()
        {
            Professor Prof = (Professor)this.CurrentPerson;
            foreach(Course c in Prof.Courses)
            {
                c.ListCourseForProf(MainList, Prof);
            }
        }

        private void EnlistStudentCourses()
        {
            Student CurrentPerson = (Student)this.CurrentPerson;
            foreach (Course c in CurrentPerson.Courses)
            {
                c.ListCourseForStudent(MainList, CurrentPerson);
            }
        }
    }
}
