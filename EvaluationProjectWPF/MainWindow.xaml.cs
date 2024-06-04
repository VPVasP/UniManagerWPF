using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EvaluationProjectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginRegisterManager loginRegisterManager;
        private UniversityManager universityManager;
        private UniversityManager.BoardingMember boardingMember;
        private UniversityManager.Cleaner cleaner;
        private UniversityManager.Teacher teacher;
        private AdminManager adminManager;
        public MainWindow()
        {
            InitializeComponent();
            loginRegisterManager = new LoginRegisterManager();
            universityManager = new UniversityManager();
            cleaner = new UniversityManager.Cleaner();
            boardingMember = new UniversityManager.BoardingMember();
            teacher = new UniversityManager.Teacher();
            adminManager = new AdminManager();
            HideAdminStuff();
        }
        private void AdminTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void UserTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        private void RegisterUser(object sender, RoutedEventArgs e)
        {
            string registerUsername = UsernameTextBox.Text;
            string registerPassword = PasswordBox.Password;
            string selectedCategoryAdmin = UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "ADMIN" ? "ADMIN" : "";
            string selectedCategoryTeacher = UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "TEACHER" ? "TEACHER" : "";
            string selectedCategoryBoardingMember = UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "BOARDING MEMBER" ? "BOARDING MEMBER" : "";
            string selectedCategoryCleaner = UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "CLEANER" ? "CLEANER" : "";
            string selectedCategoryStudent = UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "STUDENT" ? "STUDENT" : "";
            if (string.IsNullOrEmpty(registerUsername) || string.IsNullOrEmpty(registerPassword))
            {
                Debug.WriteLine("Please provide username and password");
            }
            ////if the register username exists we show a message
            //if (manager.DoesUserExistRegister(selectedCategoryAdmin,registerUsername)) 
            //{
            //    Debug.WriteLine("Username Already Exists");
            //    UserExistsMessage.Text = "Username Already Exists or you didn't pick your role in the menu...";
            //    UserExistsMessage.Visibility = Visibility.Visible;
            //}

            if (!loginRegisterManager.DoesUserExistRegister(selectedCategoryAdmin, registerUsername) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "ADMIN") 
            {
               loginRegisterManager.Register(selectedCategoryAdmin,registerUsername, registerPassword);

                //hide the registration and login panel
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;

                //display the registration message 
                RegistrationMessage.Text = "Welcome, ADMIN " + registerUsername + "!";
                RegistrationMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;
  
            }
            if (!loginRegisterManager.DoesUserExistRegister(selectedCategoryTeacher, registerUsername) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "TEACHER")
            {
                loginRegisterManager.Register(selectedCategoryTeacher, registerUsername, registerPassword);
                AddTeacherTeachingCoursesAndHours();
                string allTeacherCourseandHours = "The working hours and courses you need to teach for  " + registerUsername + " Are: " + ":\n";
                foreach (UniversityManager.Teacher teacher in universityManager.AllTeachers)
                {
                    allTeacherCourseandHours += "  You teach Course: " + teacher.teachingCourseTitle + ", Working Day: " + teacher.teachingWorkingDay + ", Working Hour: " + teacher.teacherWorkingHour + "\n";
                }
                TeacherTeachingDates.Text = allTeacherCourseandHours;
                //hide the registration and login panel
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;

                //display the registration message 
                RegistrationMessage.Text = "Welcome, TEACHER " + registerUsername + "!";
                RegistrationMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;
                TeacherTeachingDates.Visibility = Visibility.Visible;
            }
            if (!loginRegisterManager.DoesUserExistRegister(selectedCategoryBoardingMember, registerUsername) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "BOARDING MEMBER")
            {
                loginRegisterManager.Register(selectedCategoryBoardingMember, registerUsername, registerPassword);

                //hide the registration and login panel
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;

                //display the registration message 
                RegistrationMessage.Text = "Welcome, BOARDING MEMBER " + registerUsername + "!";
                RegistrationMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;

                ShowBoardingMemberStudentTeacherStats();

            }
            if (!loginRegisterManager.DoesUserExistRegister(selectedCategoryCleaner, registerUsername) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "CLEANER")
            {
                loginRegisterManager.Register(selectedCategoryCleaner, registerUsername, registerPassword);
                AddCleanerWokringSchedule();
                string allCleanerSchedule = "The working Schedule for the Cleaner " + registerUsername + " is: " + ":\n";
                foreach (UniversityManager.Cleaner cleaner in universityManager.AllCleaner)
                {
                    allCleanerSchedule += "Working Day: " + cleaner.workingDay + ", Working Length " + cleaner.workingLength + " Hours" + "\n";
                }
                CleanerDates.Text = allCleanerSchedule;
                CleanerDates.Visibility = Visibility.Visible;
                //hide the registration and login panel
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;

                //display the registration message 
                RegistrationMessage.Text = "Welcome, CLEANER " + registerUsername + "!";
                RegistrationMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;

            }
            if (!loginRegisterManager.DoesUserExistRegister(selectedCategoryStudent, registerUsername) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "STUDENT")
            {
                loginRegisterManager.Register(selectedCategoryStudent,registerUsername, registerPassword);
                AddStudentCourses();
                //hide the registration and login panel
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                string allCourseGrades = "The Grades for The Student:  " + registerUsername + " Are: " + ":\n";

                foreach (Courses course in universityManager.AllCourses)
                {
                    allCourseGrades += "Course Name: " + course.CourseTitle + ", Oral Mark: " + course.OralMark + ", Written Mark: " + course.WritingMark + "\n";
                }
                StudentGrades.Text = allCourseGrades;
                //display the registration message 
                StudentGrades.Visibility = Visibility.Visible;
                RegistrationMessage.Text = "Welcome, STUDENT " + registerUsername + "!";
                RegistrationMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;

            }
        }

        private void LoginUser(object sender, RoutedEventArgs e)
        {
            string loginUsername = UsernameTextBox.Text;
            string loginPassword = PasswordBox.Password;
            string selectedCategoryAdmin = UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "ADMIN" ? "ADMIN" : "";
            string selectedCategoryTeacher = UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "TEACHER" ? "TEACHER" : "";
            string selectedCategoryBoardingMember = UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "BOARDING MEMBER" ? "BOARDING MEMBER" : "";
            string selectedCategoryCleaner = UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "CLEANER" ? "CLEANER" : "";
            string selectedCategoryStudent = UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "STUDENT" ? "STUDENT" : "";
            if (string.IsNullOrEmpty(loginUsername) || string.IsNullOrEmpty(loginPassword))
            {
                Debug.WriteLine("Please provide username and password");
                UserExistsMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Text = "Please provide username and password";
            }

            if (loginRegisterManager.DoesUserExistLogin(selectedCategoryAdmin, loginUsername, loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "ADMIN")
            {
                
                loginRegisterManager.Login(selectedCategoryAdmin, loginUsername, loginPassword);
                ShowAdminStuff();
                LoginMessage.Text = "You are now logged in ADMIN " + loginUsername + " Welcome to the application";

            }

              else  if (loginRegisterManager.DoesUserExistLogin(selectedCategoryTeacher,loginUsername,loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "TEACHER")
            {
                AddTeacherTeachingCoursesAndHours();
                loginRegisterManager.Login(selectedCategoryTeacher,loginUsername, loginPassword);
                string allTeacherCourseandHours = "The working hours and courses you need to teach for  " + loginUsername + " Are: " + ":\n";
                foreach (UniversityManager.Teacher teacher  in universityManager.AllTeachers)
                {
                    allTeacherCourseandHours += "  You teach Course: " + teacher.teachingCourseTitle + ", Working Day: " + teacher.teachingWorkingDay + ", Working Hour: " + teacher.teacherWorkingHour + "\n";
                }
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                LoginMessage.Text = "You are now logged in TEACHER "+ loginUsername + " Welcome to the application";
                TeacherTeachingDates.Text = allTeacherCourseandHours;
                LoginMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;
                TeacherTeachingDates.Visibility = Visibility.Visible;
            }
           else if (loginRegisterManager.DoesUserExistLogin(selectedCategoryBoardingMember,loginUsername, loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "BOARDING MEMBER")
            {
                loginRegisterManager.Login(selectedCategoryBoardingMember, loginUsername, loginPassword); 
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                LoginMessage.Text = "You are now logged in BOARDING MEMBER " + loginUsername + " You can see all the student info, grades and the Teacher Info!";
                LoginMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;

                ShowBoardingMemberStudentTeacherStats();

            }

         else   if (loginRegisterManager.DoesUserExistLogin(selectedCategoryCleaner, loginUsername, loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "CLEANER")
            {
                loginRegisterManager.Login(selectedCategoryCleaner, loginUsername, loginPassword);
                AddCleanerWokringSchedule();
                string allCleanerSchedule = "The working Schedule for the Cleaner " + loginUsername + " is: " + ":\n";
                foreach (UniversityManager.Cleaner cleaner in universityManager.AllCleaner)
                {
                   allCleanerSchedule  += "Working Day: " + cleaner.workingDay + ", Working Length " + cleaner.workingLength + " Hours" +  "\n";
                }
                
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                LoginMessage.Text = "You are now logged in CLEANER " + loginUsername + " Welcome to the application";
                CleanerDates.Text = allCleanerSchedule;
                CleanerDates.Visibility = Visibility.Visible;
                LoginMessage.Visibility = Visibility.Visible;
                StudentGrades.Visibility = Visibility.Collapsed;
                UserExistsMessage.Visibility = Visibility.Collapsed;

            }

          else  if (loginRegisterManager.DoesUserExistLogin(selectedCategoryStudent,loginUsername, loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "STUDENT")
            {
                
                loginRegisterManager.Login(selectedCategoryStudent, loginUsername, loginPassword);
                AddStudentCourses();
                string allCourseGrades = "The Grades for The Student:  " + loginUsername +" Are: " + ":\n";
                
                foreach (Courses course in universityManager.AllCourses)
                {
                    allCourseGrades += "Course Name: " + course.CourseTitle + ", Oral Mark: " + course.OralMark + ", Written Mark: " + course.WritingMark + "\n";
                }
                StudentGrades.Text = allCourseGrades;
                LoginMessage.Text = "You are now logged in STUDENT " + loginUsername + " Welcome to the application";
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                StudentGrades.Visibility = Visibility.Visible;
                CleanerDates.Visibility = Visibility.Collapsed;
                LoginMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;
            }
            //else if (!manager.DoesUserExistLogin(loginUsername, loginPassword))
            //{
            //    LoginMessage.Text = "Incorrect Username or Password or Category";
            //    LoginMessage.Visibility = Visibility.Visible;
            //}
        }

      
        private void AddStudentCourses()
        {
            universityManager.AddStudentCourse("Mathematics", 5, 10);
            universityManager.AddStudentCourse("Physics", 7, 6);
            universityManager.AddStudentCourse("Economy", 4, 7);
            universityManager.AddStudentCourse("History", 2, 4);
            universityManager.AddStudentCourse("English", 5, 7);
        }

        private void AddTeacherTeachingCoursesAndHours()
        {
            universityManager.AddTeachersCourse("Mathematics", "Monday", 1);
            universityManager.AddTeachersCourse("Physics", "Tuesday", 2);
            universityManager.AddTeachersCourse("Economy", "Wednesday", 3);
            universityManager.AddTeachersCourse("History", "Thursday", 4);
            universityManager.AddTeachersCourse("English", "Friday", 5);
        }
        private void AddCleanerWokringSchedule()
        {
            universityManager.AddCleanersHours("Monday", 8);
            universityManager.AddCleanersHours("Tuesday", 8);
            universityManager.AddCleanersHours("Wednesday", 8);
            universityManager.AddCleanersHours("Thursday", 8);
            universityManager.AddCleanersHours("Friday", 8);
            universityManager.AddCleanersHours("Saturday", 4);
        }
      
        private void ShowBoardingMemberStudentTeacherStats()
        {
            AddStudentCourses();
            AddTeacherTeachingCoursesAndHours();
            //hide the registration and login panel
            RegistrationLoginPanel.Visibility = Visibility.Collapsed;
            string allCourseGrades = string.Empty;
            string allTeacherWorkingHours = string.Empty;

            foreach (var teacherHours in universityManager.AllTeachers)
            {
                allTeacherWorkingHours += "  They teach Course: " + teacherHours.teachingCourseTitle + ", Working Day: " + teacherHours.teachingWorkingDay + ", Working Hour: " + teacherHours.teacherWorkingHour + "\n";
            }
            foreach (var course in universityManager.AllCourses)
            {
                allCourseGrades += " Course Name: " + course.CourseTitle + ", Oral Mark: " + course.OralMark + ", Written Mark: " + course.WritingMark + "\n";
            }

            var allStudents = loginRegisterManager.GetAllStudents();
            var allTeachers = loginRegisterManager.GetAllTeachers();

            string teacherNames = "All Teachers Info:\n";
            string studentNames = "All Students Info:\n";


            foreach (var student in allStudents)
            {
                studentNames += " STUDENT NAME:  " + student.Username + " " + allCourseGrades + "\n";
            }
            foreach (var teacher in allTeachers)
            {
                teacherNames += " TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours + "\n";
            }
            StudentInfoText.Text = studentNames;
            StudentInfoText.Visibility = Visibility.Visible;

            TeacherInfoText.Text = teacherNames;
            TeacherInfoText.Visibility = Visibility.Visible;
        }

        private void ShowAdminRelatedStats()
        {
            AddStudentCourses();
            AddTeacherTeachingCoursesAndHours();
            AddCleanerWokringSchedule();
            //hide the registration and login panel
            RegistrationLoginPanel.Visibility = Visibility.Collapsed;
            string allCourseGrades = string.Empty;
            string allTeacherWorkingHours = string.Empty;
            string allCleanerWorkingSchedule = string.Empty;

            foreach (var teacherHours in universityManager.AllTeachers)
            {
                allTeacherWorkingHours += "  They teach Course: " + teacherHours.teachingCourseTitle + ", Working Day: " + teacherHours.teachingWorkingDay + ", Working Hour: " + teacherHours.teacherWorkingHour + "\n";
            }
            foreach (var course in universityManager.AllCourses)
            {
                allCourseGrades += " Course Name: " + course.CourseTitle + ", Oral Mark: " + course.OralMark + ", Written Mark: " + course.WritingMark + "\n";
            }

            foreach(var cleanerSchedule in universityManager.AllCleaner)
            {
                allCleanerWorkingSchedule += "Working Day: " + cleanerSchedule.workingDay + ", Working Length " + cleanerSchedule.workingLength + " Hours" + "\n";
            }

            var allStudents = loginRegisterManager.GetAllStudents();
            var allTeachers = loginRegisterManager.GetAllTeachers();
            var allCleaners = loginRegisterManager.GetAllCleaners();
            var allBoardingMembers = loginRegisterManager.GetAllBoardingMembers();

            string teacherNames = "All Teachers Info:\n";
            string studentNames = "All Students Info:\n";
            string cleanerNames = "All Cleaners Info:\n";
            string boardingMemberNames = "All Boarding Members Info:\n";

            foreach (var student in allStudents)
            {
                studentNames += " STUDENT NAME:  " + student.Username + " " + allCourseGrades + "\n";
            }
            foreach (var teacher in allTeachers)
            {
                teacherNames += " TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours + "\n";
            }
            foreach (var cleaner in allCleaners)
            {
                cleanerNames += " CLEANER NAME: "+ cleaner.Username + " "  + allCleanerWorkingSchedule + "\n";
            }

            foreach(var boardingMember in allBoardingMembers)
            {
                boardingMemberNames += " BOARDING MEMBER NAME: " + boardingMember.Username + "\n";
              
            }

            StudentInfoText.Text = studentNames;
            StudentInfoText.Visibility = Visibility.Visible;

            TeacherInfoText.Text = teacherNames;
            TeacherInfoText.Visibility = Visibility.Visible;

            CleanerInfoText.Text = cleanerNames;
            CleanerInfoText.Visibility = Visibility.Visible;

            BoardingMemberInfoText.Text = boardingMemberNames;
            BoardingMemberInfoText.Visibility = Visibility.Visible;
        }
        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void HideAdminStuff()
        {
            AdminPanel.Visibility = Visibility.Collapsed;
            //AdminUsername.Visibility = Visibility.Collapsed;
            //AdminTextBox.Visibility = Visibility.Collapsed;          
            //AdminTypeComboBox.Visibility = Visibility.Collapsed;
        }
        private void ShowAdminStuff() {
            RegistrationLoginPanel.Visibility = Visibility.Collapsed;
            AdminPanel.Visibility = Visibility.Visible;
            AdminTypeComboBox.Visibility = Visibility.Collapsed;
            AdminTextBox.Visibility = Visibility.Collapsed;
            AdminUsername.Visibility = Visibility.Collapsed;
            AddNewEntityButton.Visibility = Visibility.Collapsed;
            AdminPassword.Visibility = Visibility.Collapsed;
            AdminPasswordBox.Visibility = Visibility.Collapsed;
            AdminModifyTypeComboBox.Visibility = Visibility.Collapsed;
            ShowAdminRelatedStats();
        }
        private void EntityUI()
        {

            AdminUsername.Visibility = Visibility.Visible;
            AdminTextBox.Visibility = Visibility.Visible;
            AddEntityUIButton.Visibility = Visibility.Collapsed;
            AdminTypeComboBox.Visibility = Visibility.Visible;
            AddNewEntityButton.Visibility = Visibility.Visible;
            AdminPassword.Visibility = Visibility.Visible;
            AdminPasswordBox.Visibility = Visibility.Visible;
        }
        private void ModifyEntityUI()
        {
            AdminUsername.Visibility = Visibility.Visible;
            AdminTextBox.Visibility = Visibility.Visible;
            AddEntityUIButton.Visibility = Visibility.Collapsed;
            AdminModifyTypeComboBox.Visibility = Visibility.Visible;
            AddNewEntityButton.Visibility = Visibility.Collapsed;
            AdminUsername.Text = "Search Name";
            ModifyEntityUIButton.Content = "Search Name";
            ModifyEntityUIButton.Click -= ModifyEntityUI;
            ModifyEntityUIButton.Click += SearchName;
        }
        

        private void AddEntityUI(object sender, RoutedEventArgs e)
        {
            EntityUI();
        }
        private void ModifyEntityUI(object sender, RoutedEventArgs e)
        {
            ModifyEntityUI();
        }
        private void SearchName(object sender,RoutedEventArgs e)
        {
            string adminRegisterUsername = AdminTextBox.Text;
            string adminRegisterPassword = AdminPasswordBox.Password;
            string selectedCategoryTeacher = AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminModifyTypeComboBox.SelectedItem).Content.ToString() == "TEACHER" ? "TEACHER" : "";
            string selectedCategoryBoardingMember = AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminModifyTypeComboBox.SelectedItem).Content.ToString() == "BOARDING MEMBER" ? "BOARDING MEMBER" : "";
            string selectedCategoryCleaner = AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminModifyTypeComboBox.SelectedItem).Content.ToString() == "CLEANER" ? "CLEANER" : "";
            string selectedCategoryStudent = AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminModifyTypeComboBox.SelectedItem).Content.ToString() == "STUDENT" ? "STUDENT" : "";

            if (AdminModifyTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminModifyTypeComboBox.SelectedItem).Content.ToString() == "TEACHER")
            {
                var matchingValues = loginRegisterManager.GetAllTeachers().Where(user => user.Username.Contains(adminRegisterUsername));
                if (matchingValues.Any()) 
                {
                    ModifyEntityUIButton.Content = "Found Name";
                }
                else
                {
                    ModifyEntityUIButton.Content = "Didn't Find Name";
                }
            }

            if (AdminModifyTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminModifyTypeComboBox.SelectedItem).Content.ToString() == "BOARDING MEMBER")
            {

                var matchingValues = loginRegisterManager.GetAllBoardingMembers().Where(user => user.Username.Contains(adminRegisterUsername));
                if (matchingValues.Any())
                {
                    ModifyEntityUIButton.Content = "Found Name";
                }
                else
                {
                    ModifyEntityUIButton.Content = "Didn't Find Name";
                }
            }

            if (AdminModifyTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminModifyTypeComboBox.SelectedItem).Content.ToString() == "CLEANER")
            {
                var matchingValues = loginRegisterManager.GetAllCleaners().Where(user => user.Username.Contains(adminRegisterUsername));
                if (matchingValues.Any())
                {
                    ModifyEntityUIButton.Content = "Found Name";
                }
                else
                {
                    ModifyEntityUIButton.Content = "Didn't Find Name";
                }
            }

                if (AdminModifyTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminModifyTypeComboBox.SelectedItem).Content.ToString() == "STUDENT")
                {
                var matchingValues = loginRegisterManager.GetAllStudents().Where(user => user.Username.Contains(adminRegisterUsername));
                if (matchingValues.Any())
                {
                    ModifyEntityUIButton.Content = "Found Name";
                }
                else
                {
                    ModifyEntityUIButton.Content = "Didn't Find Name";
                }
            }
        }
        private void AddNewEntity(object sender, RoutedEventArgs e)
        {
            string adminRegisterUsername = AdminTextBox.Text;
            string adminRegisterPassword = AdminPasswordBox.Password;
            string selectedCategoryTeacher = AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminTypeComboBox.SelectedItem).Content.ToString() == "NEW TEACHER" ? "TEACHER" : "";
            string selectedCategoryBoardingMember = AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminTypeComboBox.SelectedItem).Content.ToString() == "NEW BOARDING MEMBER" ? "BOARDING MEMBER" : "";
            string selectedCategoryCleaner = AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminTypeComboBox.SelectedItem).Content.ToString() == "NEW CLEANER" ? "CLEANER" : "";
            string selectedCategoryStudent = AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminTypeComboBox.SelectedItem).Content.ToString() == "NEW STUDENT" ? "STUDENT" : "";

            if (AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminTypeComboBox.SelectedItem).Content.ToString() == "NEW TEACHER")
            {
                loginRegisterManager.Register(selectedCategoryTeacher, adminRegisterUsername,adminRegisterPassword);
            }

           else if(AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminTypeComboBox.SelectedItem).Content.ToString() == "NEW BOARDING MEMBER"){
                loginRegisterManager.Register(selectedCategoryBoardingMember, adminRegisterUsername, adminRegisterPassword);
            }

            else if (AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminTypeComboBox.SelectedItem).Content.ToString() == "NEW CLEANER")
            {
                loginRegisterManager.Register(selectedCategoryCleaner, adminRegisterUsername, adminRegisterPassword);
            }
            else if (AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminTypeComboBox.SelectedItem).Content.ToString() == "NEW STUDENT")
            {
                loginRegisterManager.Register(selectedCategoryStudent, adminRegisterUsername, adminRegisterPassword);
            }
            else if (AdminTypeComboBox.SelectedItem != null && ((ComboBoxItem)AdminTypeComboBox.SelectedItem).Content.ToString() == "NEW COURSE")
            {

                universityManager.AddStudentCourse(adminRegisterUsername,0,0);
            }
        }

        private void ModifyEntity(object sender, RoutedEventArgs e)
        {

        }
    }
}