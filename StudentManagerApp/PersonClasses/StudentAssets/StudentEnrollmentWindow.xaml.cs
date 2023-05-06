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
        Course Current { get; set; }
        public StudentEnrollmentWindow(Course cs)
        {
            InitializeComponent();
            Current = cs;
            created = new StudentPage(cs);
            MainFrame.Navigate(created);
            Closed += StudentEnrollmentWindow_Closed;
        }

        private void StudentEnrollmentWindow_Closed(object? sender, EventArgs e)
        {
            foreach (StackPanel student in created.MainList.Children.OfType<StackPanel>().Where(x => x.Background == Brushes.Violet))
            {
                Current.AddStudent(student.Tag as Student);
            }
        }

    }
}
