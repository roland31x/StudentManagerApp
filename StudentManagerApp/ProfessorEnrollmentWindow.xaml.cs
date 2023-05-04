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
    /// Interaction logic for ProfessorEnrollmentWindow.xaml
    /// </summary>
    public partial class ProfessorEnrollmentWindow : Window
    {
        ProfessorPage created;
        Course Current { get; set; }
        public ProfessorEnrollmentWindow(Course cs)
        {
            InitializeComponent();
            Current = cs;
            created = new ProfessorPage(cs);
            MainFrame.Navigate(created);
            Closed += StudentEnrollmentWindow_Closed;
        }

        private void StudentEnrollmentWindow_Closed(object? sender, EventArgs e)
        {
            foreach (StackPanel student in created.MainList.Children.OfType<StackPanel>().Where(x => x.Background == Brushes.Violet))
            {
                Current.AssignProf(student.Tag as Professor);
            }
        }
    }
}
