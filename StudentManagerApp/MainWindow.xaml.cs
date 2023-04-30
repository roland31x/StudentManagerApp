using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using StudentManagerApp.PersonClasses;

namespace StudentManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadUp();
            MainFrame.Navigated += MainFrame_Navigated;
        }
        void LoadUp()
        {
            for(int i = 0; i < 100; i++)
            {
                Student st = new Student($"{i} Student", i);
            }
            for(int i = 0; i < 10; i++)
            {
                Course cs = new Course($"Course{i}", i);
            }
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            MainFrame.NavigationService.RemoveBackEntry();
        }

        private void ShowStudentsPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StudentPage());
            App.CurrentPage = MainFrame.Content as StudentPage;
        }

        private void ShowCoursesPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CoursesPage());
            App.CurrentPage = MainFrame.Content as CoursesPage;
        }

        private void ShowProfessorsPage(object sender, RoutedEventArgs e)
        {

        }

        private void ShowPersonnelPage(object sender, RoutedEventArgs e)
        {

        }
    }
}
