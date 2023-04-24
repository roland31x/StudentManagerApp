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

        public List<Course> Courses = new List<Course>();
        public Student(string name, int id) : base(id, name)
        {
            Function = "Student";
            Validate();         
        }

        private void StudentPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PersonInfoWindow pif = new PersonInfoWindow(this);
            pif.ShowDialog();
        }

        private void StudentPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as StackPanel).Background = null;
        }

        private void StudentPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as StackPanel).Background = Brushes.LightPink;
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
        public override void MinimalListThis(StackPanel list)
        {
            Label ValidatedLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 30,
            };

            Binding mybind1 = new Binding("ValidColor");
            mybind1.Source = this;
            BindingOperations.SetBinding(ValidatedLabel, Control.BackgroundProperty, mybind1);


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

            StackPanel StudentPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Children = { ValidatedLabel, NameLabel, IDLabel, EmailLabel }
            };

            StudentPanel.MouseEnter += StudentPanel_MouseEnter;
            StudentPanel.MouseLeave += StudentPanel_MouseLeave;
            StudentPanel.MouseDown += StudentPanel_MouseDown;

            list.Children.Add(StudentPanel);
        }
        public override string ToString()
        {
            return FullName + " " + Id + " " + Email;
        }
    }
}
