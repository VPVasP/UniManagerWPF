namespace EvaluationProjectWPF
{
    internal class UniversityManager
    {
        public List<Courses> AllCourses = new List<Courses>();
        public List <Teacher> AllTeachers = new List<Teacher>();
        public List<Cleaner> AllCleaner = new List<Cleaner>();

        //class teacher that inherits from the class employees
        public class Teacher : Employees
        {
            public string? teachingCourseTitle;
            public string? teachingWorkingDay;
            public int teacherWorkingHour;
        }
        //class boarding member that inherits from the class employees
        public class BoardingMember : Employees
        {
            public string? boardingMemberUsername;


        }
        //class cleaner that inherits from the class employees
        public class Cleaner : Employees
        {
            public string? workingDay;
            public int workingLength;
        }

        //add a student course function
        public void AddStudentCourse(string title, int oralMark, int writingMark)
        {
            Courses newCourse = new Courses();
            newCourse.CourseTitle = title;
            newCourse.OralMark = oralMark;
            newCourse.WritingMark = writingMark;

            AllCourses.Add(newCourse);
        }
        //the admin adds a student course
        public void AdminAddStudentCourse(string title)
        {
            Courses newCourse = new Courses();
            newCourse.CourseTitle = title;

            AllCourses.Add(newCourse);
        }
        //add a teachers's course function
        public void AddTeachersCourse(string title, string workingday, int workingHour)
        {
            Teacher teacher = new Teacher();
            teacher.teachingCourseTitle = title;
            teacher.teachingWorkingDay = workingday;
            teacher.teacherWorkingHour = workingHour;

            AllTeachers.Add(teacher);
        }
        //add the cleaner working hours
        public void AddCleanersHours(string workingday, int workingLength)
        {
            Cleaner cleaner = new Cleaner();
            cleaner.workingDay = workingday;
            cleaner.workingLength = workingLength;

            AllCleaner.Add(cleaner);
        }
    }

}

