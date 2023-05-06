using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;

namespace StudentManagerApp.PersonClasses
{   
    public interface ICourse
    {
        public void Validate();
        public void Destroy();
        public StackPanel ListCourseForStudent(StackPanel panel, Student student);
        public StackPanel ListCourseForProf(StackPanel panel, Professor prof);
        public StackPanel ListCourse(StackPanel panel);
    }
    public class Course : ICourse, INotifyPropertyChanged
    {
        protected static Dictionary<int, Course> Courses = new Dictionary<int, Course>();
        string _Name;
        public string CourseName { get { return _Name; } set { _Name = value; OnPropertyChanged(); } }
        public int ID { get; private set; }
        public bool FullyValid { get; set; }
        public string TrueID { get { return ID.ToString("D7"); } }
        public int ProfCount { get { return Professors.Count; } }
        public List<Professor> Professors { get; private set; }
        public int StudentCount { get { return StudentList.Values.Count; } }
        public Dictionary<Student,Grade> StudentList { get; private set; }
        public Brush ValidColor
        {
            get
            {
                if (FullyValid)
                {
                    return Brushes.Transparent;
                }
                else return Brushes.Red;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(name));
        }
        public Course(string Name, int ID) 
        {
            Courses.Add(ID, this);
            _Name = Name;
            this.ID = ID;
            Professors = new List<Professor>();
            StudentList = new Dictionary<Student, Grade>();
            FullyValid = false;
        }
        public void Destroy()
        {
            while(StudentList.Keys.Count > 0)
            {
                RemoveStudent(StudentList.Keys.First());
            }
            while(Professors.Count > 0)
            {
                UnassignProf(Professors.First());
            }
            Courses.Remove(ID);

        }
        public void Validate()
        {
            if(ProfCount > 0 && StudentCount > 0)
            {
                FullyValid = true;
            }
            else
            {
                FullyValid = false;
            }
            OnPropertyChanged("ValidColor");
        }
        public static List<StackPanel> ListAllCourses(StackPanel panel)
        {
            List<StackPanel> ToReturn = new List<StackPanel>();
            foreach(Course cs in Courses.Values)
            {
                ToReturn.Add(cs.ListCourse(panel));
            }
            return ToReturn;
        }
        public StackPanel ListCourse(StackPanel panel)
        {
            Label ValidatedLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 30,
            };
            BindToControlElement(ValidatedLabel, Control.BackgroundProperty, this, "ValidColor");


            Label NameLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 250,
            };
            BindToControlElement(NameLabel, ContentControl.ContentProperty, this, "CourseName");

            Label IDLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 70,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            BindToControlElement(IDLabel, ContentControl.ContentProperty, this, "TrueID");

            Label STLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 150,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            BindToControlElement(STLabel, ContentControl.ContentProperty, this, "StudentCount");

            Label PFLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 200,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            BindToControlElement(PFLabel, ContentControl.ContentProperty, this, "ProfCount");

            StackPanel CoursePanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Children = { ValidatedLabel, NameLabel, IDLabel, STLabel, PFLabel },
                Tag = this,
            };
            panel.Children.Add(CoursePanel);

            return CoursePanel;
        }

        public StackPanel ListCourseForStudent(StackPanel panel, Student st)
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
                Children = { IDLabel, NameLabel, GrLabel, AVGLabel },
                Tag = this,
            };
            panel.Children.Add(stCoursePanel);

            return stCoursePanel;
        }
        public void AddStudent(Student t)
        {
            StudentList.Add(t, new Grade());
            t.Enlist(this);
            OnPropertyChanged("StudentCount");
            Validate();
        }
        public void RemoveStudent(Student t)
        {
            StudentList.Remove(t);
            t.Delist(this);
            OnPropertyChanged("StudentCount");
            Validate();
        }
        public void AssignProf(Professor pf)
        {
            Professors.Add(pf);
            pf.Enlist(this);
            OnPropertyChanged("ProfCount");
            Validate();
        }
        public void UnassignProf(Professor pf)
        {
            Professors.Remove(pf);
            pf.Delist(this);
            OnPropertyChanged("ProfCount");
            Validate();
        }
        public StackPanel ListCourseForProf(StackPanel panel, Professor pf)
        {
            return new StackPanel();
        }
        public static void BindToControlElement(Control control, DependencyProperty dp, Course source, string PropertyName)
        {
            Binding myBind = new Binding(PropertyName);
            myBind.Source = source;
            myBind.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(control, dp, myBind);
        }
    }
}
