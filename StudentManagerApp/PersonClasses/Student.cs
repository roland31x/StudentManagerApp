using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;

namespace StudentManagerApp.PersonClasses
{
    public sealed class Student : Person
    {
        public override string Function { get; protected set; }
        //public int Year { get; set; }
        public List<Course> Courses { get; set; }
        public Student(string name, int id) : base(id, name)
        {
            Function = "Student";
            Courses = new List<Course>();
            Validate();         
        }

       
        public override void Destroy()
        {
            base.Destroy();
            while(Courses.Count > 0)
            {
                Courses[0].RemoveStudent(this);
            }
        }
        public void Enlist(Course cs)
        {
            Courses.Add(cs);
            Validate();
        }
        public void Delist(Course cs)
        {
            Courses.Remove(cs);
            Validate();
        }
        protected override bool InitialValidate()
        {
            bool isinitialvalid = true;
            if (Courses.Count == 0)
            {
                isinitialvalid = false;
            }

            initialValid = isinitialvalid;
            OnPropertyChanged("ValidColor");
            return isinitialvalid;
        }

        protected override bool TryValidate()
        {
            bool fullyValid = true;
            if (!base.TryValidate()) // need to use base validation first, then add special ones for each class                     
            {
                fullyValid = false;
            }

            if (!InitialValidate())
            {
                fullyValid =  false;
            }

            //if (Year == 0)
            //{
            //    Valid = false;
            //}          

            return fullyValid;
        }
        public override void Validate()
        {
            if (TryValidate())
            {
                fullyValid = true;
                OnPropertyChanged("ValidColor");
            }
            else
            {
                fullyValid = false;
                OnPropertyChanged("ValidColor");
            }
        }
        public override StackPanel MinimalListThis(StackPanel list)
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
            BindToControlElement(NameLabel, ContentControl.ContentProperty, this, "Name");

            Label IDLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 70,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            BindToControlElement(IDLabel, ContentControl.ContentProperty, this, "TrueID");

            Label EmailLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 350,
            };
            BindToControlElement(EmailLabel, ContentControl.ContentProperty, this, "Email");

            StackPanel StudentPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Children = { ValidatedLabel, NameLabel, IDLabel, EmailLabel },
                Tag = this,
            };

            list.Children.Add(StudentPanel);

            return StudentPanel;
        }
        public override string ToString()
        {
            return FullName + " " + Id + " " + Email;
        }
    }
}
