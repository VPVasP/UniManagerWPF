namespace EvaluationProjectWPF
{
    internal class UniversityManager
    {
        public List<Courses> AllCourses = new List<Courses>();
        public List <Teacher> AllTeachers = new List<Teacher>();
        public List<Cleaner> AllCleaner = new List<Cleaner>();
        public Teacher teacher;
        public class Teacher : Employees
        {
            public string? teachingCourseTitle;
            public string? teachingWorkingDay;
            public int teacherWorkingHour;
        }
        public class BoardingMember : Employees
        {
            public string? boardingMemberUsername;

        }
        public class Cleaner : Employees
        {
            public string? workingDay;
            public int workingLength;
        }
        public void AddStudentCourse(string title, int oralMark, int writingMark)
        {
            Courses newCourse = new Courses();
            newCourse.CourseTitle = title;
            newCourse.OralMark = oralMark;
            newCourse.WritingMark = writingMark;

            AllCourses.Add(newCourse);
        }
        public void AdminAddStudentCourse(string title)
        {
            Courses newCourse = new Courses();
            newCourse.CourseTitle = title;

            AllCourses.Add(newCourse);
        }
        public void AddTeachersCourse(string title, string workingday, int workingHour)
        {
            Teacher teacher = new Teacher();
            teacher.teachingCourseTitle = title;
            teacher.teachingWorkingDay = workingday;
            teacher.teacherWorkingHour = workingHour;

            AllTeachers.Add(teacher);
        }

        public void AddCleanersHours(string workingday, int workingLength)
        {
            Cleaner cleaner = new Cleaner();
            cleaner.workingDay = workingday;
            cleaner.workingLength = workingLength;

            AllCleaner.Add(cleaner);
        }
    }

}

