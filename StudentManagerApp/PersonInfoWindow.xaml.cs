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
        public bool[] DoBOK = new bool[] { false, false, false };
        public bool isEditing = false;
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
                //DrawProfessorInfo();
            }
        }

        private void PersonInfoWindow_Closed(object? sender, EventArgs e)
        {
            CurrentPerson.Validate();
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
            bool DoBWasSet = false;
            if(CurrentPerson.DateOfBrith != null)
            {
                DoBWasSet = true;
            }
            TextBox DoBYeartb = new TextBox();
            if(DoBWasSet)
            {
                DoBYeartb.Text = CurrentPerson.DateOfBrith!.Value.Year.ToString();
            }
            else
            {
                DoBYeartb.Text = "YYYY";
            }
            DoBYeartb.TextChanged += DoBYeartb_TextChanged;

            TextBox DoBMonthtb = new TextBox();
            if (DoBWasSet)
            {
                DoBMonthtb.Text = CurrentPerson.DateOfBrith!.Value.Month.ToString();
            }
            else
            {
                DoBMonthtb.Text = "MM";
            }
            DoBMonthtb.TextChanged += DoBMonthtb_TextChanged;

            TextBox DoBDaytb = new TextBox();
            if (DoBWasSet)
            {
                DoBDaytb.Text = CurrentPerson.DateOfBrith!.Value.Day.ToString();
            }
            else
            {
                DoBDaytb.Text = "DD";
            }
            DoBDaytb.TextChanged += DoBDaytb_TextChanged;

            Button SaveDoB = new Button() { Content = "Save", Visibility = Visibility.Collapsed, Margin = new Thickness(30,0,0,0) };
            SaveDoB.Click += SaveDoB_Click;          

            StackPanel DoBPanel = new StackPanel() { Orientation = Orientation.Horizontal, Children = { DoB, DoBYeartb, new Label() { Content = @"//", }, DoBMonthtb, new Label() { Content = @"//", }, DoBDaytb, SaveDoB } };

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
            foreach(StackPanel sp in InfoPanel.Children)
            {
                foreach(TextBox tb in sp.Children.OfType<TextBox>())
                {
                    tb.IsEnabled = false;
                }
            }
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

        private void DoBYeartb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox year = (TextBox)sender;
            if (int.TryParse(year.Text, out int value))
            {
                if (value > 0 && value < DateTime.Now.Year)
                {
                    DoBOK[0] = true;
                    CheckDoB(sender);
                }
            }
            else
            {
                DoBOK[0] = false;
            }
        }

        private void DoBMonthtb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox month = (TextBox)sender;
            if (int.TryParse(month.Text, out int value))
            {
                if (value > 0 && value <= 12)
                {
                    DoBOK[1] = true;
                    CheckDoB(sender);
                }
            }
            else
            {
                DoBOK[1] = false;
            }
        }

        private void DoBDaytb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox day = (TextBox)sender;
            if(int.TryParse(day.Text, out int value))
            {
                if(value > 0 && value <= 31)
                {
                    DoBOK[2] = true;
                    CheckDoB(sender);
                }
            }
            else
            {
                DoBOK[2] = false;
            }
        }
        void CheckDoB(object sender)
        {
            int check = 0;
            foreach (bool ok in DoBOK)
            {
                if (ok)
                {
                    check++;
                }           
            }
            if(check == 3)
            {
                foreach (Button b in ((StackPanel)(sender as TextBox)!.Parent).Children.OfType<Button>())
                {
                    b.Visibility = Visibility.Visible;
                }
            }          
        }
        private void SaveDoB_Click(object sender, RoutedEventArgs e)
        {
            string[] date = new string[3];
            int i = 0;
            foreach (TextBox tb in ((StackPanel)(sender as Button)!.Parent).Children.OfType<TextBox>())
            {
                date[i] = tb.Text;
                i++;
            }
            CurrentPerson.SetDoB(new DateTime(year: int.Parse(date[0]), month: int.Parse(date[1]), day: int.Parse(date[2])));
            (sender as Button)!.Visibility = Visibility.Collapsed;
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
