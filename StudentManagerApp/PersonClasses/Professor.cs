using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace StudentManagerApp.PersonClasses
{
    public class Professor : Person
    {
        public StackPanel ProfPanel { get; private set; }
        public override string Function { get; protected set; }

        public List<Course> Courses = new List<Course>();

        public List<Course> LeadingCourses = new List<Course>();
        public Professor(string name, int id) : base(id, name)
        {
            
            Function = "Professor";

            Validate();

            Label ValidatedLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 30,
                Content = WasFullyValidated,
            };

            Label NameLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 250,
                Content = FullName,
            };
            Label IDLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 70,
                Content = TrueID,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            Label EmailLabel = new Label()
            {
                Style = (Style)Application.Current.Resources["LabelStyle"],
                Width = 350,
                Content = Email,
            };

            ProfPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Children = { ValidatedLabel, NameLabel, IDLabel, EmailLabel }
            };

            ProfPanel.MouseEnter += ProfPanel_MouseEnter;
            ProfPanel.MouseLeave += ProfPanel_MouseLeave;
            ProfPanel.MouseDown += ProfPanel_MouseDown;
        }
       
        private void ProfPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ProfPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ProfPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
        protected override bool TryValidate()
        {
            if (!base.TryValidate()) // need to use base validation first, then add special ones for each class                     
            {
                return false;
            }
            // add class validation rules below
            bool Valid = true;

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
            list.Children.Add(ProfPanel);
        }

    }
}
