using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StudentManagerApp.PersonClasses
{   
    public interface ICourse
    {
        public void Validate();

        public void ListCourseForStudent(StackPanel panel, Student student);
        public void ListCourseForProf(StackPanel panel, Professor prof);
        public void ListCourse(StackPanel panel);
    }
    public class Course : ICourse
    {
        protected static Dictionary<int, Course> Courses = new Dictionary<int, Course>();
        public string CourseName { get; private set; }
        public int ID { get; private set; }
        public string TrueID { get { return ID.ToString("D7"); } }
        public Professor? LeadProfessor { get; private set; }
        public List<Professor> Professors { get; private set; }
        public Dictionary<Student,Grade> StudentList { get; private set; }
        public Course(string Name, int ID) 
        {
            Courses.Add(ID, this);
            CourseName = Name;
            this.ID = ID;
            Professors = new List<Professor>();
            StudentList = new Dictionary<Student, Grade>();
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }

        public void ListCourse(StackPanel panel)
        {
            
        }

        public void ListCourseForStudent(StackPanel panel, Student st)
        {
            Label IDLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 125,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };

            Binding mybind1 = new Binding("TrueID");
            mybind1.Source = this;
            BindingOperations.SetBinding(IDLabel, ContentControl.ContentProperty, mybind1);


            Label NameLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 335,
            };

            Binding mybind2 = new Binding("CourseName");
            mybind2.Source = this;
            BindingOperations.SetBinding(NameLabel, ContentControl.ContentProperty, mybind2);

            Label GrLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 185,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };

            Binding mybind3 = new Binding("StringValue");
            mybind3.Source = StudentList[st];
            BindingOperations.SetBinding(GrLabel, ContentControl.ContentProperty, mybind3);

            Label AVGLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 75,
            };
            Binding mybind4 = new Binding("AVGGrade");
            mybind4.Source = StudentList[st];
            BindingOperations.SetBinding(AVGLabel, ContentControl.ContentProperty, mybind4);

            StackPanel stCoursePanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Children = { IDLabel, NameLabel, GrLabel, AVGLabel }
            };
            panel.Children.Add(stCoursePanel);
        }
        public void AddStudent(Student t)
        {
            StudentList.Add(t, new Grade());
            t.Courses.Add(this);
        }
        public void ListCourseForProf(StackPanel panel, Professor pf)
        {
           
        }
    }
    public class Grade
    {
        public List<int> Grades { get; private set; }
        public string StringValue { get { return ToString(); } }
        public double AVGGrade { get { return AverageGrade(); } }
        public Grade()
        {
            Grades = new List<int>();
        }
        public Grade(List<int> grades)
        {
            Grades = grades;
        }
        public double AverageGrade()
        {
            double i = 1;
            double result = 0;
            foreach(int gr in Grades)
            {
                result += gr;
                i++;
            }
            return Math.Round(result / i, 2);
        }
        public void AddGrade(int gr)
        {
            Grades.Add(gr);
        }
        public void CorrectGrade(int which, int newgr) // warning this is experimental
        {
            try
            {
                Grades[which] = newgr;
            }
            catch(Exception)
            {
                MessageBox.Show("grade doesn't exist");
            }
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(int gr in Grades)
            {
                stringBuilder.Append(gr);
                stringBuilder.Append(' ');                
            }

            return stringBuilder.ToString();
        }
    }
}
