using StudentManagerApp.PersonClasses;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentManagerApp
{
    
    public static class MySerializer
    {
        static string CurrentFolder = Directory.GetCurrentDirectory();
        static string PersonFolder = CurrentFolder + @"\Database\Persons";
        static string CourseFolder = CurrentFolder + @"\Database\Courses";
        public static void Serialize(IPerson person)
        {
            SavePersonInfo((Person)person);
        }
        public static void Serialize(ICourse course)
        {
            SaveCourseInfo((Course)course);
        }
        public static void Deserialize(string path)
        {
            if (path.Contains("Persons"))
            {
                DeserializePerson(path);
            }
            else if(path.Contains("Courses"))
            {
                DeserializeCourse(path);
            }           
        }

        private static void DeserializeCourse(string path)
        {
            StreamReader sr = new StreamReader(path);
            string buffer;
            int ID = 0;
            while (!sr.EndOfStream)
            {
                buffer = sr.ReadLine();
                if (buffer == null || buffer == string.Empty)
                {
                    continue;
                }
                if (buffer.Contains("[ID]"))
                {
                    ID = int.Parse(buffer.Split('=')[1].Trim());
                    new Course("_PlaceHolder_", ID);
                    break;
                }             
            }
            Course added = Course.Courses[ID];
            while (!sr.EndOfStream)
            {
                buffer = sr.ReadLine();
                if (buffer == null || buffer == string.Empty)
                {
                    continue;
                }
                if (buffer.Contains("[NAME]"))
                {
                    added.CourseName = buffer.Split("=")[1].Trim();
                    continue;
                }
                if(buffer.Contains("[PROFESSOR LIST]"))
                {
                    string buffer2 = buffer.Split("=")[1].Trim();
                    string[] profs = buffer2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for(int i = 0; i < profs.Length; i++)
                    {
                        added.AssignProf((Professor)Person.FindPersonAfterID(int.Parse(profs[i])));
                    }
                }
                if (buffer.Contains("<<STUDENT LIST>>"))
                {
                    while (!sr.EndOfStream)
                    {
                        buffer = sr.ReadLine();
                        string[] tokens = buffer.Split(" ");
                        Student ToAdd = (Student)Person.FindPersonAfterID(int.Parse(tokens[0]));
                        added.AddStudent(ToAdd);
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            added.StudentList[ToAdd].AddGrade(int.Parse(tokens[i]));
                        }
                    }
                    
                }
            }
        }
        public static void Destroy(Person p)
        {
            string path = PersonFolder + $@"\{p.TrueID}.data";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static void Destroy(Course course)
        {
            string path = CourseFolder + $@"\{course.TrueID}.data";
            if (File.Exists(path)) 
            {
                File.Delete(path);
            }
        }

        private static void DeserializePerson(string path)
        {
            StreamReader sr = new StreamReader(path);
            string buffer;
            Dictionary<string, string> args = new Dictionary<string, string>();
            while (!sr.EndOfStream)
            {
                buffer = sr.ReadLine();
                if (buffer == null || buffer == string.Empty)
                {
                    continue;
                }
                if (buffer.Contains("[ID]"))
                {
                    args.Add("ID", buffer.Split('=')[1].Trim());
                    continue;
                }
                if (buffer.Contains("[FUNCTION]"))
                {
                    args.Add("Type", buffer.Split('=')[1].Trim());
                    continue;
                }
                if (buffer.Contains("[NAME]"))
                {
                    args.Add("Name", buffer.Split('=')[1].Trim());
                    continue;
                }
                if (buffer.Contains("[DATE OF BIRTH]"))
                {
                    args.Add("DoB", buffer.Split('=')[1].Trim());
                    continue;
                }
                if (buffer.Contains("[PERSONAL EMAIL]"))
                {
                    args.Add("Personal Email", buffer.Split('=')[1].Trim());
                    continue;
                }
                if (buffer.Contains("[PHONE NUMBER]"))
                {
                    args.Add("Phone Number", buffer.Split('=')[1].Trim());
                    continue;
                }

            }
            if (args["Type"] == "Student")
            {
                CreateStudent(args);
            }
            else if (args["Type"] == "Professor")
            {
                CreateProfessor(args);
            }

            sr.Close();
        }

        private static void CreateProfessor(Dictionary<string, string> args)
        {
            Professor p = new Professor(args["Name"], int.Parse(args["ID"]));
            try
            {
                if (args["Personal Email"] != null && args["Personal Email"] != string.Empty)
                    p.SetPersonalEmail(args["Personal Email"]);
                if (args["DoB"] != null && args["DoB"] != string.Empty)
                    p.SetDoB(DateTime.Parse(args["DoB"]));
                if (args["Phone Number"] != null && args["Phone Number"] != string.Empty)
                    p.SetPhone(args["Phone Number"]);
            }
            catch (Exception)
            {
                p.FirstName = "ERROR_LOADING_UP_THIS_PERSON";
            }
        }

        private static void CreateStudent(Dictionary<string, string> args)
        {
            Student p = new Student(args["Name"], int.Parse(args["ID"]));
            try
            {
                if (args["Personal Email"] != null && args["Personal Email"] != string.Empty)
                    p.SetPersonalEmail(args["Personal Email"]);
                if (args["DoB"] != null && args["DoB"] != string.Empty)
                    p.SetDoB(DateTime.Parse(args["DoB"]));
                if (args["Phone Number"] != null && args["Phone Number"] != string.Empty)
                    p.SetPhone(args["Phone Number"]);
            }
            catch (Exception)
            {
                p.FirstName = "ERROR_LOADING_UP_THIS_PERSON";
            }
        }

        public static void LoadUp()
        {
            foreach (string file in Directory.EnumerateFiles(PersonFolder, "*.data"))
            {
                Deserialize(file);
            }
            foreach (string file in Directory.EnumerateFiles(CourseFolder, "*.data"))
            {
                Deserialize(file);
            }
        }
        public static void Initialize()
        {
            Directory.CreateDirectory(PersonFolder);
            Directory.CreateDirectory(CourseFolder);
        }
        public static void SaveAllInfo()
        {

        }
        static void SavePersonInfo(Person person)
        {
            string PersonFile = PersonFolder + $@"\{person.TrueID}.data";
            if (File.Exists(PersonFile))
            {
                File.Delete(PersonFile);
            }
            StreamWriter sw = new StreamWriter(PersonFile);
            sw.WriteLine("[ID] = " + person.TrueID);
            sw.WriteLine("[FUNCTION] = " + person.Function);
            sw.WriteLine("[NAME] = " + person.Name);
            sw.WriteLine("[DATE OF BIRTH] = " + person.DateOfBrith);
            sw.WriteLine("[PERSONAL EMAIL] = " + person.PersonalEmail);
            sw.WriteLine("[PHONE NUMBER] = " + person.Phone);
            if(person.GetType() == typeof(Student))
            {
                ContinueSavingAsStudent((Student)person, sw);
            }
            else if(person.GetType() == typeof(Professor))
            {
                ContinueSavingAsProfessor((Professor)person, sw);
            }

            sw.Close();
            
        }

        private static void ContinueSavingAsProfessor(Professor person, StreamWriter sw)
        {
            
        }

        private static void ContinueSavingAsStudent(Student person, StreamWriter sw)
        {
            
        }

        static void SaveCourseInfo(Course cs)
        {
            string PersonFile = CourseFolder + $@"\{cs.TrueID}.data";
            if (File.Exists(PersonFile))
            {
                File.Delete(PersonFile);
            }
            StreamWriter sw = new StreamWriter(PersonFile);
            sw.WriteLine("[ID] = " + cs.TrueID);
            sw.WriteLine("[NAME] = " + cs.CourseName);
            sw.Write("[PROFESSOR LIST] = ");
            foreach(Professor pf in cs.Professors)
            {
                sw.Write(pf.TrueID);
                sw.Write(' ');
            }
            sw.WriteLine();
            sw.WriteLine("<<STUDENT LIST>>");
            foreach (KeyValuePair<Student, Grade> kp in cs.StudentList)
            {
                sw.Write(kp.Key.TrueID);
                sw.Write(kp.Value.ToString());
                sw.WriteLine();
            }

            sw.Close();
        }
    }
}
