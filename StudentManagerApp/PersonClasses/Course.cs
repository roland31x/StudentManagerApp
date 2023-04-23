using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagerApp.PersonClasses
{   
    public class Course
    {
        protected static Dictionary<int, Course> Courses = new Dictionary<int, Course>();
        public string CourseName { get; private set; }
        public int ID { get; private set; }
        public Professor? LeadProfessor { get; private set; }
        public List<Professor> Professors { get; private set; }
        public List<Student> StudentList { get; private set; }
        public Course(string Name, int ID) 
        {
            Courses.Add(ID, this);
            CourseName = Name;
            this.ID = ID;
            Professors = new List<Professor>();
            StudentList = new List<Student>();
        }
    }
}
