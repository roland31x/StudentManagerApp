using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace StudentManagerApp.PersonClasses
{
    public class Professor : Person
    {
        public override string Function { get; protected set; }

        public List<Course> Courses = new List<Course>();

        public List<Course> LeadingCourses = new List<Course>();
        public Professor(string name, int id) : base(id, name)
        {
            
            Function = "Professor";

            Validate();

           
        }
       
        private void ProfPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PersonInfoWindow pif = new PersonInfoWindow(this);
            pif.ShowDialog();
        }

        private void ProfPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as StackPanel).Background = null;
        }

        private void ProfPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as StackPanel).Background = Brushes.LimeGreen;
        }
        protected override bool InitialValidate()
        {
            bool isinitialvalid = true;
            //if (Courses.Count == 0)
            //{
            //    isinitialvalid = false;
            //}

            initialValid = isinitialvalid;
            OnPropertyChanged("ValidColor");
            return isinitialvalid;
        }
        protected override bool TryValidate()
        {
            bool isFullyValid = true;
            if (!base.TryValidate()) // need to use base validation first, then add special ones for each class                     
            {
                isFullyValid = false;
            }
            if (!InitialValidate())
            {
                isFullyValid = false;
            }
            // add class validation rules below
            

            return isFullyValid;
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

            StackPanel ProfPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Children = { ValidatedLabel, NameLabel, IDLabel, EmailLabel }
            };

            ProfPanel.MouseEnter += ProfPanel_MouseEnter;
            ProfPanel.MouseLeave += ProfPanel_MouseLeave;
            ProfPanel.MouseDown += ProfPanel_MouseDown;
            list.Children.Add(ProfPanel);
        }

    }
}
