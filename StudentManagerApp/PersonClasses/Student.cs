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
        public StackPanel StudentPanel { get; private set; }
        public override string Function { get; protected set; }
        //public int Year { get; set; }

        public List<Course> Courses = new List<Course>();
        public Student(string name, int id) : base(id, name)
        {

            Function = "Student";

            Validate();

            Label ValidatedLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 30,
            };
            BindToControlElement(ValidatedLabel, this, "WasFullyValidated");


            Label NameLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 250,
            };
            BindToControlElement(NameLabel, this, "Name");

            Label IDLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 70,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            BindToControlElement(IDLabel, this, "TrueID");

            Label EmailLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 350,
            };
            BindToControlElement(EmailLabel, this, "Email");

            StudentPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Children = { ValidatedLabel, NameLabel, IDLabel, EmailLabel }
            };

            StudentPanel.MouseEnter += StudentPanel_MouseEnter;
            StudentPanel.MouseLeave += StudentPanel_MouseLeave;
            StudentPanel.MouseDown += StudentPanel_MouseDown;
        }

        private void StudentPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PersonInfoWindow pif = new PersonInfoWindow(this);
            pif.ShowDialog();
        }

        private void StudentPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            StudentPanel.Background = null;
        }

        private void StudentPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            StudentPanel.Background = Brushes.LightPink;
        }

        protected override bool TryValidate()
        {
            if (!base.TryValidate()) // need to use base validation first, then add special ones for each class                     
            {
                return false;
            }

            bool Valid = true;

            //if (Year == 0)
            //{
            //    Valid = false;
            //}
            if(Courses.Count == 0)
            {
                Valid = false;
            }

            return Valid;
        }
        public override void Validate()
        {
            if (TryValidate())
            {
                WasFullyValidated = string.Empty;
            }
            else
            {
                WasFullyValidated = "!";
            }
        }
        public override void MinimalListThis(StackPanel list)
        {
            list.Children.Add(StudentPanel);
        }
        public override string ToString()
        {
            return FullName + " " + Id + " " + Email;
        }
    }
}
