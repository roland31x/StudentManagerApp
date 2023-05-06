using StudentManagerApp.PersonClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentManagerApp
{
    
    public static class MySerializer
    {
        const string PersonFolder = @"Database\\Persons";
        const string CourseFolder = @"Database\\Courses";
        public static void Serialize(IPerson person)
        {
            
        }
        public static void Deserialize(IPerson person)
        {

        }
        public static void LoadUp()
        {

        }
        public static void Initialize()
        {
            Directory.CreateDirectory(PersonFolder);
            Directory.CreateDirectory(CourseFolder);
        }
        public static void SaveAllInfo()
        {

        }
        public static void SavePersonInfo(Person person)
        {
            string PersonFile = Path.Combine(PersonFolder, $@"\\{person.TrueID}.data");
            if (File.Exists(PersonFile))
            {
                File.Delete(PersonFile);
            }
            File.Create(PersonFile).Dispose();
            StreamWriter sw = new StreamWriter(PersonFile);
            sw.WriteLine(person.Name);
            
            
        }
        public static void SaveCourseInfo(Course cs)
        {

        }
    }
}
