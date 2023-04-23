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
using System.Windows.Navigation;
using System.Windows.Shapes;
using StudentManagerApp.PersonClasses;

namespace StudentManagerApp
{
    /// <summary>
    /// Interaction logic for StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Page
    {
        
        public StudentPage()
        {
            InitializeComponent();
            LoadUp();
            ListStudents();
        }

        void LoadUp()
        {
            for (int i = 50; i < 70; i++)
            {
                Professor pf = new Professor(i.ToString() + " Prof", i);
            }
            for (int i = 0; i < 50; i++)
            {
                Student st = new Student(i.ToString()+" Student", i);               
            }
        }
        void ListStudents()
        {
            Person.MinimalListItems<Student>(MainList);
        }
    }
}
