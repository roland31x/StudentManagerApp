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
    /// Interaction logic for PersonInfoWindow.xaml
    /// </summary>
    public partial class PersonInfoWindow : Window
    {
        public Person CurrentPerson { get; set; }
        public bool isEditing = false;
        DatePicker dobPick;
        public PersonInfoWindow(Person person)
        {
            InitializeComponent();
            Closed += PersonInfoWindow_Closed;
            CurrentPerson = person;
            DrawDefaultPerson();
            if(CurrentPerson is Student)
            {
                DrawStudentInfo();
            }
            else if(CurrentPerson is Professor)
            {
                DrawProfessorInfo();
            }
        }

        private void PersonInfoWindow_Closed(object? sender, EventArgs e)
        {
            CurrentPerson.Validate();
        }
        public void DrawProfessorInfo()
        {
            Professor CurrentPerson = (Professor)this.CurrentPerson;
            Label courselb = new Label()
            {
                Content = "Courses:",
                Margin = new Thickness(0, 0, 10, 0),
            };
            StackPanel CoursesPanel = new StackPanel() { Orientation = Orientation.Horizontal, Children = { courselb } };
            InfoPanel.Children.Add(CoursesPanel);
            ScrollViewer sw = new ScrollViewer() { Height = 75, Width = 360 };
            StackPanel crspnl = new StackPanel();
            sw.Content = crspnl;
            foreach(Course c in CurrentPerson.Courses)
            {
                crspnl.Children.Add(new Label() { Content = c.CourseName, Margin = new Thickness(40,0,0,0) });
            }
            InfoPanel.Children.Add(sw);

        }
        public void DrawStudentInfo()
        {
            Student CurrentPerson = (Student)this.CurrentPerson;
            // Year - gonna work on it 
            //Label yr = new Label()
            //{
            //    Content = "Year:",
            //    Margin = new Thickness(0, 0, 10, 0),
            //};

            //TextBox yrtb = new TextBox()
            //{
            //    Text = CurrentPerson.FirstName,
            //};

            //yrtb.TextChanged += DoBYeartb_TextChanged;

            //StackPanel FirstNamePanel = new StackPanel() { Orientation = Orientation.Horizontal, Children = { yr, yrtb } };

            //InfoPanel.Children.Add(FirstNamePanel);

            Label courselb = new Label()
            {
                Content = "Courses:",
                Margin = new Thickness(0, 0, 10, 0),
            };

            Label courselbc = new Label()
            {
                Content = CurrentPerson.Courses.Count,
            };
            Button CShow = new Button()
            {
                Content = "Show",
                Margin = new Thickness(30, 0, 0, 0),
            };
            CShow.Click += CoursesListAll;

            StackPanel CoursesPanel = new StackPanel() { Orientation = Orientation.Horizontal, Children = { courselb, courselbc, CShow } };

            InfoPanel.Children.Add(CoursesPanel);

        }

        private void CoursesListAll(object sender, RoutedEventArgs e)
        {
            PersonCourseWindow pcw = new PersonCourseWindow(this.CurrentPerson);
            pcw.ShowDialog();
        }

        public void DrawDefaultPerson()
        {
            //Person has the following default properties, every single instance and derived instance has: Fullname ( first & last ), ID (non-editable), Function (non-editable), DoB, Business Email (non-editable), Personal email, phone number.

            IDLabel.Content = CurrentPerson.TrueID;
            FunctionLabel.Content = CurrentPerson.Function;
            

            //FirstName boxes
            Label fn = new Label()
            {
                Content = "FirstName:",
                Margin = new Thickness(0, 0, 10, 0),
            };

            TextBox fntb = new TextBox()
            {
                Text = CurrentPerson.FirstName,
            };

            fntb.TextChanged += FirstNameValidation;

            StackPanel FirstNamePanel = new StackPanel() { Orientation = Orientation.Horizontal, Children = { fn, fntb } };

            InfoPanel.Children.Add(FirstNamePanel);
            
            // LastName boxes

            Label ln = new Label()
            {
                Content = "LastName:",
                Margin = new Thickness(0, 0, 10, 0),
            };

            TextBox lntb = new TextBox()
            {
                Text = CurrentPerson.LastName,
            };

            lntb.TextChanged += LastNameValidation;

            StackPanel LastNamePanel = new StackPanel() { Orientation = Orientation.Horizontal, Children = { ln, lntb } };

            InfoPanel.Children.Add(LastNamePanel);

            //DoB boxes
            Label DoB = new Label()
            {
                Content = "Date Of Birth:",
                Margin = new Thickness(0, 0, 10, 0),
            };
            DatePicker datePicker = new DatePicker() { Margin = new Thickness(0, 0, 10, 0) };
            datePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
            dobPick = datePicker;
            if(CurrentPerson.DateOfBrith == null)
            {
                datePicker.SelectedDate = null;
            }
            else
            {
                datePicker.SelectedDate = CurrentPerson.DateOfBrith;
            }         

            StackPanel DoBPanel = new StackPanel() { Orientation = Orientation.Horizontal, Children = { DoB, datePicker} };

            InfoPanel.Children.Add(DoBPanel);

            // email panel
            Label personalemail = new Label()
            {
                Content = "Personal Email:",
                Margin = new Thickness(0, 0, 10, 0),
            };

            TextBox emailtb = new TextBox();

            if(CurrentPerson.PersonalEmail == null)
            {
                emailtb.Text = "Not set.";
            }
            else
            {
                emailtb.Text = CurrentPerson.PersonalEmail;
            }

            emailtb.TextChanged += EmailValidation;

            StackPanel PersonalEmailPanel = new StackPanel() { Orientation = Orientation.Horizontal, Children = { personalemail, emailtb } };

            InfoPanel.Children.Add(PersonalEmailPanel);

            // phone panel
            Label phonelabel = new Label()
            {
                Content = "Phone Number:",
                Margin = new Thickness(0, 0, 10, 0),
            };

            TextBox phonetb = new TextBox();

            if (CurrentPerson.Phone == null)
            {
                phonetb.Text = "Not set.";
            }
            else
            {
                phonetb.Text = CurrentPerson.Phone;
            }

            phonetb.TextChanged += PhoneValidation;

            StackPanel PhonePanel = new StackPanel() { Orientation = Orientation.Horizontal, Children = { phonelabel, phonetb } };

            InfoPanel.Children.Add(PhonePanel);

            dobPick.IsEnabled = false;
            foreach(StackPanel sp in InfoPanel.Children.OfType<StackPanel>())
            {
                foreach(TextBox tb in sp.Children.OfType<TextBox>())
                {
                    tb.IsEnabled = false;
                }
            }
        }

        private void DatePicker_SelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            CurrentPerson.SetDoB(dobPick.SelectedDate);
        }

        private void PhoneValidation(object sender, TextChangedEventArgs e)
        {
            TextBox EmailTextBox = (TextBox)sender;
            if (EmailTextBox.Text.Length == 10)
            {
                CurrentPerson.SetPhone(EmailTextBox.Text);
            }
            if (EmailTextBox.Text == string.Empty)
            {
                CurrentPerson.SetPhone(null!);
            }
        }

        private void EmailValidation(object sender, TextChangedEventArgs e)
        {
            TextBox EmailTextBox = (TextBox)sender;
            if (EmailTextBox.Text.Contains('@'))
            {
                CurrentPerson.SetPersonalEmail(EmailTextBox.Text);
            }
            if(EmailTextBox.Text == string.Empty)
            {
                CurrentPerson.SetPersonalEmail(null!);
            }
        }

        private void LastNameValidation(object sender, TextChangedEventArgs e)
        {
            if (TextBox_DefaultName_TextChanged_Validation(sender,e))
            {
                CurrentPerson.LastName = ((TextBox)sender).Text; 
            }
        }

        private void FirstNameValidation(object sender, TextChangedEventArgs e)
        {
            if (TextBox_DefaultName_TextChanged_Validation(sender, e))
            {
                CurrentPerson.FirstName = ((TextBox)sender).Text;
            }
        }

        private bool TextBox_DefaultName_TextChanged_Validation(object sender, TextChangedEventArgs e)
        {
            TextBox current = (TextBox)sender;
            if(current.Text == string.Empty)
            {
                current.BorderBrush = Brushes.Red;
                return false;
            }
            else
            {
                current.BorderBrush = Brushes.Gray;
                return true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isEditing)
            {
                isEditing = false;
                dobPick.IsEnabled = false;
                (sender as Button)!.Content = "Edit";
                foreach (StackPanel sp in InfoPanel.Children)
                {
                    foreach (TextBox tb in sp.Children.OfType<TextBox>())
                    {
                        tb.IsEnabled = false;
                    }
                }
            }
            else
            {
                isEditing = true;
                dobPick.IsEnabled = true;
                (sender as Button)!.Content = "Stop Editing";
                foreach (StackPanel sp in InfoPanel.Children)
                {
                    foreach (TextBox tb in sp.Children.OfType<TextBox>())
                    {
                        tb.IsEnabled = true;
                    }
                }
            }
        }
    }
}
