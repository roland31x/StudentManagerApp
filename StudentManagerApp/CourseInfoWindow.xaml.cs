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
    /// Interaction logic for CourseInfoWindow.xaml
    /// </summary>
    public partial class CourseInfoWindow : Window
    {
        public bool isEditing { get; set; }
        Course CurrentCourse { get; set; }
        public CourseInfoWindow(Course cs)
        {
            InitializeComponent();
            Closed += CourseInfoWindow_Closed;
            CurrentCourse = cs;
            isEditing = false;
            DrawCourseInfo();
        }
        void DrawCourseInfo()
        {
            IDLabel.Content = CurrentCourse.TrueID;
            NameLabel.IsEnabled = false;
            Course.BindToControlElement(NameLabel, TextBox.TextProperty, CurrentCourse, "CourseName");
            Course.BindToControlElement(STCount, ContentControl.ContentProperty, CurrentCourse, "StudentCount");
            Course.BindToControlElement(PFCount, ContentControl.ContentProperty, CurrentCourse, "ProfCount");
            // Students box

            UpdateStudentTable();
            // Profs box

            UpdateProfsTable();

        }
        StackPanel AddStudentPanel(Student st, Grade gr)
        {
            Label stname = new Label() { Width = 280, Style = (Style)App.Current.Resources["LabelStyle"], HorizontalContentAlignment = HorizontalAlignment.Center };
            
            Person.BindToControlElement(stname, ContentControl.ContentProperty, st, "Name");
            Label grs = new Label() { Width = 370, Style = (Style)App.Current.Resources["LabelStyle"], HorizontalContentAlignment = HorizontalAlignment.Center } ;

            BindingOperations.SetBinding(grs, ContentControl.ContentProperty, new Binding("StringValue") { Source = gr });

            Label gravg = new Label() { Width = 120, Style = (Style)App.Current.Resources["LabelStyle"], HorizontalContentAlignment = HorizontalAlignment.Center };

            BindingOperations.SetBinding(gravg, ContentControl.ContentProperty, new Binding("AVGGrade") { Source = gr });

            StackPanel created = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Children = { stname, grs, gravg },
                Tag = st,
            };

            return created;
        }
        private void NameValidation(object sender, TextChangedEventArgs e)
        {
            TextBox current = (TextBox)sender;
            if(current.Text == string.Empty)
            {
                current.BorderBrush = Brushes.Red;
            }
            else
            {
                current.BorderBrush = Brushes.Gray;
                CurrentCourse.CourseName = current.Text;
            }
        }

        private void ShowB_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CourseInfoWindow_Closed(object? sender, EventArgs e)
        {
            CurrentCourse.Validate();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isEditing)
            {
                isEditing = false;
                NameLabel.IsEnabled = false;
                (sender as Button)!.Content = "Edit";
                
            }
            else
            {
                isEditing = true;
                NameLabel.IsEnabled = true;
                (sender as Button)!.Content = "Stop Editing";
                
            }
        }

        private void AssignProfClick(object sender, RoutedEventArgs e)
        {
            new ProfessorEnrollmentWindow(CurrentCourse).ShowDialog();
            UpdateProfsTable();
        }

        private void AddStudentClick(object sender, RoutedEventArgs e)
        {
            new StudentEnrollmentWindow(CurrentCourse).ShowDialog();
            UpdateStudentTable();
        }
        void UpdateProfsTable()
        {
            PFInfoPanel.Children.Clear();
            foreach (Professor pf in CurrentCourse.Professors)
            {
                pf.MinimalListThis(PFInfoPanel);
            }
        }

        private void UpdateStudentTable()
        {
            STInfoPanel.Children.Clear();
            foreach (KeyValuePair<Student, Grade> val in CurrentCourse.StudentList)
            {
                STInfoPanel.Children.Add(AddStudentPanel(val.Key, val.Value));
            }
            foreach(StackPanel st in STInfoPanel.Children)
            {
                st.MouseDown += St_MouseDown;
                st.MouseEnter += St_MouseEnter;
                st.MouseLeave += St_MouseLeave;
            }
        }

        private void St_MouseLeave(object sender, MouseEventArgs e)
        {
            StackPanel hovered = (StackPanel)sender;
            hovered.Background = null;
           
        }

        private void St_MouseEnter(object sender, MouseEventArgs e)
        {
            StackPanel hovered = (StackPanel)sender;
            hovered.Background = Brushes.Aquamarine;
        }

        private void St_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point clicked = e.GetPosition(this);
            StackPanel sp = new StackPanel();
            Label delBlock = new Label()
            {
                Height = 30,
                Width = 100,
                Content = "Remove",
                Background = Brushes.Red,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2, 2, 2, 2),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Tag = sender,

            };


            Label grBlock = new Label()
            {
                Height = 30,
                Width = 100,
                Content = "Grade",
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2, 2, 2, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Tag = sender,
            };
            grBlock.MouseDown += grBlock_MouseDown;
            delBlock.MouseDown += DelBlock_MouseDown;


            sp.Children.Add(grBlock);
            sp.Children.Add(delBlock);
            Canvas.SetZIndex(sp, 1);
            Canvas.SetTop(sp, clicked.Y);
            Canvas.SetLeft(sp, clicked.X);
            sp.MouseLeave += InfoBlock_MouseLeave;
            MainCanvas.Children.Add(sp);
        }

        private void grBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel clicked = ((sender as Label).Tag) as StackPanel;
            // GRADEWINDOW LOGIC
        }

        private void DelBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel clicked = ((sender as Label).Tag) as StackPanel;
            if (MessageBox.Show("You sure you want to remove this student from the course?", "Unlist?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                STInfoPanel.Children.Remove(clicked);
                CurrentCourse.RemoveStudent(clicked.Tag as Student);
            }

        }
        private void InfoBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            MainCanvas.Children.Remove(sender as StackPanel);
        }
    }
}
