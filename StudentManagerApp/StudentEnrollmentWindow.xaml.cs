using StudentManagerApp.PersonClasses;
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

namespace StudentManagerApp
{
    /// <summary>
    /// Interaction logic for StudentEnrollmentWindow.xaml
    /// </summary>
    public partial class StudentEnrollmentWindow : Window
    {
        StudentPage created;
        List<Student> toAdd = new List<Student>();
        Course Current { get; set; }
        public StudentEnrollmentWindow(Course cs)
        {
            InitializeComponent();
            Current = cs;
            created = new StudentPage(cs);
            MainFrame.Navigate(created);
            MainFrame.Navigated += MainFrame_Navigated;
            Closed += StudentEnrollmentWindow_Closed;
        }

        private void StudentEnrollmentWindow_Closed(object? sender, EventArgs e)
        {
            foreach (Student student in toAdd)
            {
                Current.AddStudent(student);
            }
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            foreach (StackPanel sp in created.MainList.Children)
            {
                sp.MouseDown += Sp_MouseDown;
            }
        }

        private void Sp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel clicked = (StackPanel)sender;
            if(clicked.Background == Brushes.Violet || clicked.Background == Brushes.DeepSkyBlue)
            {
                toAdd.Remove(clicked.Tag as Student);
                clicked.Background = null;
            }
            else
            {
                clicked.Background = Brushes.Violet;
                toAdd.Add(clicked.Tag as Student);
            }
        }
    }
}
