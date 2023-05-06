using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StudentManagerApp.PersonClasses
{
    public class Grade
    {
        public List<int> Grades { get; private set; }
        public string StringValue
        {
            get
            {
                if (Grades.Count == 0)
                {
                    return "Not graded";
                }
                return ToString();
            }
        }
        public double AVGGrade
        {
            get
            {
                if (Grades.Count == 0)
                {
                    return 0;
                }
                return AverageGrade();
            }
        }
        public Grade()
        {
            Grades = new List<int>();
        }
        public Grade(List<int> grades)
        {
            Grades = grades;
        }
        public double AverageGrade()
        {
            double i = 1;
            double result = 0;
            foreach (int gr in Grades)
            {
                result += gr;
                i++;
            }
            return Math.Round(result / i, 2);
        }
        public void AddGrade(int gr)
        {
            Grades.Add(gr);
        }
        public void CorrectGrade(int which, int newgr) // warning this is experimental
        {
            try
            {
                Grades[which] = newgr;
            }
            catch (Exception)
            {
                MessageBox.Show("grade doesn't exist");
            }
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (int gr in Grades)
            {
                stringBuilder.Append(gr);
                stringBuilder.Append(' ');
            }

            return stringBuilder.ToString();
        }
    }
}
