using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
        List<StackPanel>? _stp = new List<StackPanel>();
        bool IsEditable = true;
        List<StackPanel> StudentPanels { get { return _stp; } set { _stp = value; if (IsEditable) { AddControlBoxes(); } } }
        public StudentPage()
        {
            InitializeComponent();
            //LoadUp();
            ListStudents();
        }
        public StudentPage(Course cs)
        {
            InitializeComponent();
            IsEditable = false;
            //LoadUp();
            foreach (Student st in Person.FullPersonList.Values.OfType<Student>().Where(x => (!cs.StudentList.ContainsKey(x))))
            {
                StudentPanels.Add(st.MinimalListThis(MainList));
            }
        }

        void LoadUp()
        {
            for (int i = 1; i <= 10; i++)
            {
                Student st = new Student(i.ToString()+" Student", i);               
            }
        }
        void AddControlBoxes()
        {
            foreach(StackPanel sp in _stp!)
            {
                sp.MouseDown += Sp_MouseDown;
                sp.MouseEnter += Panel_MouseEnter;
                sp.MouseLeave += Panel_MouseLeave;
            }
        }
        private void Panel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            StackPanel hovered = (StackPanel)sender;
            if (hovered.Background == Brushes.LightPink || hovered.Background == null)
            {
                hovered.Background = null;
            }
            else
            {
                hovered.Background = Brushes.Violet;
            }
        }

        private void Panel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            StackPanel hovered = (StackPanel)sender;
            if (hovered.Background == null)
            {
                hovered.Background = Brushes.LightPink;
            }
            else
            {
                hovered.Background = Brushes.DeepSkyBlue;
            }
        }
        private void Sp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point clicked = e.GetPosition(this);
            StackPanel sp = new StackPanel();
            Label delBlock = new Label()
            {
                Height = 30,
                Width = 100,
                Content = "DELETE",
                Background = Brushes.Red,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2, 2, 2, 2),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Tag = (sender as StackPanel).Tag,
                
            };
            

            Label textBlock1 = new Label()
            {
                Height = 30,
                Width = 100,
                Content = "Show Profile",
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2, 2, 2, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Tag = (sender as StackPanel).Tag
            };
            textBlock1.MouseDown += TextBlock1_MouseDown;
            delBlock.MouseDown += DelBlock_MouseDown;


            sp.Children.Add(textBlock1);
            sp.Children.Add(delBlock);
            Canvas.SetZIndex(sp, 1);
            Canvas.SetTop(sp, clicked.Y);
            Canvas.SetLeft(sp, clicked.X);
            sp.MouseLeave += InfoBlock_MouseLeave;
            MainCanvas.Children.Add(sp);
        }

        private void DelBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(MessageBox.Show("You sure you want to delete this student?","Delete?",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Student st = ((sender as Label)!.Tag as Student)!;
                st.Destroy();
                MainList.Children.Remove(GetPanel(st));
            }
            
        }
        StackPanel GetPanel(Student st)
        {
            StackPanel toReturn = null;
            foreach(StackPanel sp in StudentPanels)
            {
                if(sp.Tag == st)
                {
                    toReturn = sp;
                }
            }
            if(toReturn == null)
            {
                throw new ArgumentNullException(st.ToString());
            }
            return toReturn;
        }
        private void TextBlock1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new PersonInfoWindow((sender as Label).Tag as Student).ShowDialog();
        }

        private void InfoBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            MainCanvas.Children.Remove(sender as StackPanel);
        }

        void ListStudents()
        {
            foreach (Student st in Person.FullPersonList.Values.OfType<Student>())
            {
                StudentPanels.Add(st.MinimalListThis(MainList));
            }
        }
        void FilterAfterNameSearch(string filter)
        {
            List<StackPanel> toReturn = new List<StackPanel>();
            foreach (Student st in Person.FullPersonList.Values.OfType<Student>().Where(x => x.Name.Contains(filter)))
            {
                toReturn.Add(st.MinimalListThis(MainList));
            }
            StudentPanels = toReturn;
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
                StackPanel added = ToAdd.MinimalListThis(MainList);
                added.MouseDown += Sp_MouseDown;
                StudentPanels.Add(added);
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
                FilterAfterNameSearch(context.Text);
            }
        }
    }
}
