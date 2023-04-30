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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentManagerApp
{
    /// <summary>
    /// Interaction logic for CoursesPage.xaml
    /// </summary>
    public partial class CoursesPage : Page
    {
        List<StackPanel>? _cps;
        List<StackPanel> CoursePanels { get { return _cps!; } set { _cps = value; AddControlBoxes(); } }
        public CoursesPage()
        {
            InitializeComponent();
            ListCourses();
        }
        void ListCourses()
        {
            CoursePanels = Course.ListAllCourses(MainList);
        }
        void AddControlBoxes()
        {
            foreach (StackPanel sp in _cps!)
            {
                sp.MouseDown += Sp_MouseDown;
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


            Label profileBlock = new Label()
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
            profileBlock.MouseDown += profileBlock_MouseDown;
            delBlock.MouseDown += DelBlock_MouseDown;


            sp.Children.Add(profileBlock);
            sp.Children.Add(delBlock);
            Canvas.SetZIndex(sp, 1);
            Canvas.SetTop(sp, clicked.Y);
            Canvas.SetLeft(sp, clicked.X);
            sp.MouseLeave += InfoBlock_MouseLeave;
            MainCanvas.Children.Add(sp);
        }
        private void DelBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("You sure you want to delete this course?", "Delete?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Course cs = ((sender as Label)!.Tag as Course)!;
                cs.Destroy();
                MainList.Children.Remove(GetPanel(cs));
            }

        }
        StackPanel GetPanel(Course cs)
        {
            StackPanel toReturn = null;
            foreach (StackPanel sp in CoursePanels)
            {
                if (sp.Tag == cs)
                {
                    toReturn = sp;
                }
            }
            if (toReturn == null)
            {
                throw new ArgumentNullException(cs.ToString());
            }
            return toReturn;
        }
        private void profileBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new CourseInfoWindow((sender as Label).Tag as Course).ShowDialog();
        }

        private void InfoBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            MainCanvas.Children.Remove(sender as StackPanel);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
