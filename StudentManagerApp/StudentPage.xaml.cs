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
            for (int i = 1; i <= 10; i++)
            {
                Student st = new Student(i.ToString()+" Student", i);               
            }
        }
        void ListStudents()
        {
            Person.MinimalListItems<Student>(MainList);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int nextID = 0;
            for (int i = 1; i < Person.FullPersonList.Keys.Max(); i++)
            {
                bool found = true; ;
                foreach (int key in Person.FullPersonList.Keys)
                {
                    if (i == key)
                    {
                        found = false;
                    }
                }
                if (found)
                {
                    nextID = i;
                    break;
                }
            }
            if (nextID == 0)
            {
                nextID = Person.FullPersonList.Keys.Max() + 1;
            }
            Student ToAdd = new Student("Edit-This And-This", nextID);
            PersonInfoWindow pif = new PersonInfoWindow(ToAdd);
            pif.ShowDialog();
            if (MessageBox.Show($"Student {ToAdd.Name} created, press OK to save it.","Save?",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                ToAdd.MinimalListThis(MainList);
            }
            else
            {
                ToAdd.Destroy();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainList.Children.Clear();
            TextBox context = sender as TextBox;
            if (context.Text == string.Empty) 
            {
                ListStudents();
            }
            else
            {
                foreach (Student st in Person.FullPersonList.Values.OfType<Student>().Where(x => x.Name.Contains(context.Text)))
                {
                    st.MinimalListThis(MainList);
                }
            }
        }
    }
}
