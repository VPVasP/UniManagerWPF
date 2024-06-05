using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static EvaluationProjectWPF.UniversityManager;

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
        private bool yesModify;
        private bool noModify;
        private bool yesDeleteEntity;
        private bool noDeleteEntity;
        TextBlock NewEnityTextBlock = new TextBlock();
        public MainWindow()
        {
            InitializeComponent();
            loginRegisterManager = new LoginRegisterManager();
            universityManager = new UniversityManager();
            cleaner = new UniversityManager.Cleaner();
            boardingMember = new UniversityManager.BoardingMember();
            teacher = new UniversityManager.Teacher();
            string adminRegisterUsername = AdminTextBox.Text;
            string modifyNameBox = ModifyNameTextBox.Text;
            HideAdminUI();
        }



        #region RegistrationLogin

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
                RegistrationMessage.Visibility = Visibility.Visible;
                RegistrationMessage.Text = "Please provide a username and a password";
            }


            if (!loginRegisterManager.DoesUserExistRegister(selectedCategoryAdmin, registerUsername) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "ADMIN")
            {
                loginRegisterManager.Register(selectedCategoryAdmin, registerUsername, registerPassword);

                //hide the registration and login panel
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;

                //display the registration message 
                RegistrationMessage.Text = "Welcome, ADMIN " + registerUsername + "!";
                RegistrationMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;
                ShowAdminUIStats();
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
                ShowUserUI();
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
                ShowUserUI();
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
                ShowUserUI();
            }
            if (!loginRegisterManager.DoesUserExistRegister(selectedCategoryStudent, registerUsername) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "STUDENT")
            {
                loginRegisterManager.Register(selectedCategoryStudent, registerUsername, registerPassword);
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
                ShowUserUI();
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
                ShowAdminUIStats();

                LoginMessage.Text = "You are now logged in ADMIN " + loginUsername + " Welcome to the application";
            }

            else if (loginRegisterManager.DoesUserExistLogin(selectedCategoryTeacher, loginUsername, loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "TEACHER")
            {
                AddTeacherTeachingCoursesAndHours();
                loginRegisterManager.Login(selectedCategoryTeacher, loginUsername, loginPassword);
                string allTeacherCourseandHours = "The working hours and courses you need to teach for  " + loginUsername + " Are: " + ":\n";
                foreach (UniversityManager.Teacher teacher in universityManager.AllTeachers)
                {
                    allTeacherCourseandHours += "  You teach Course: " + teacher.teachingCourseTitle + ", Working Day: " + teacher.teachingWorkingDay + ", Working Hour: " + teacher.teacherWorkingHour + "\n";
                }
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                LoginMessage.Text = "You are now logged in TEACHER " + loginUsername + " Welcome to the application";
                TeacherTeachingDates.Text = allTeacherCourseandHours;
                LoginMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;
                TeacherTeachingDates.Visibility = Visibility.Visible;
                ShowUserUI();
            }
            else if (loginRegisterManager.DoesUserExistLogin(selectedCategoryBoardingMember, loginUsername, loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "BOARDING MEMBER")
            {
                loginRegisterManager.Login(selectedCategoryBoardingMember, loginUsername, loginPassword);
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                LoginMessage.Text = "You are now logged in BOARDING MEMBER " + loginUsername + " You can see all the student info, grades and the Teacher Info!";
                LoginMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;

                ShowBoardingMemberStudentTeacherStats();
                ShowUserUI();
            }

            else if (loginRegisterManager.DoesUserExistLogin(selectedCategoryCleaner, loginUsername, loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "CLEANER")
            {
                loginRegisterManager.Login(selectedCategoryCleaner, loginUsername, loginPassword);
                AddCleanerWokringSchedule();
                string allCleanerSchedule = "The working Schedule for the Cleaner " + loginUsername + " is: " + ":\n";
                foreach (UniversityManager.Cleaner cleaner in universityManager.AllCleaner)
                {
                    allCleanerSchedule += "Working Day: " + cleaner.workingDay + ", Working Length " + cleaner.workingLength + " Hours" + "\n";
                }

                RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                LoginMessage.Text = "You are now logged in CLEANER " + loginUsername + " Welcome to the application";
                CleanerDates.Text = allCleanerSchedule;
                CleanerDates.Visibility = Visibility.Visible;
                LoginMessage.Visibility = Visibility.Visible;
                StudentGrades.Visibility = Visibility.Collapsed;
                UserExistsMessage.Visibility = Visibility.Collapsed;
                ShowUserUI();
            }

            else if (loginRegisterManager.DoesUserExistLogin(selectedCategoryStudent, loginUsername, loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "STUDENT")
            {

                loginRegisterManager.Login(selectedCategoryStudent, loginUsername, loginPassword);
                AddStudentCourses();
                string allCourseGrades = "The Grades for The Student:  " + loginUsername + " Are: " + ":\n";

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
                ShowUserUI();
            }
            else
            {
                LoginMessage.Visibility = Visibility.Visible;
                LoginMessage.Text = "Something is wrong with your credentials";
            }

        }
        #endregion RegistrationLogin

        #region Functions
        private void AddStudentCourses()
        {
            Random random = new Random();
            int[] randomGrades = new int[5];
            randomGrades[0] = random.Next(5, 10);
            randomGrades[1] = random.Next(5, 10);
            randomGrades[2] = random.Next(5, 10);
            randomGrades[3] = random.Next(5, 10);
            randomGrades[4] = random.Next(5, 10);
            universityManager.AddStudentCourse("Mathematics", randomGrades[0], randomGrades[4]);
            universityManager.AddStudentCourse("Physics", randomGrades[1], randomGrades[1]);
            universityManager.AddStudentCourse("Economy", randomGrades[2], randomGrades[3]);
            universityManager.AddStudentCourse("History", randomGrades[3], randomGrades[2]);
            universityManager.AddStudentCourse("English", randomGrades[4], randomGrades[1]);
        }

        private void AddTeacherTeachingCoursesAndHours()
        {

            Random random = new Random();
            int[] randomCourseHours = new int[5];
            randomCourseHours[0] = random.Next(1,7);
            randomCourseHours[1] = random.Next(1, 7);
            randomCourseHours[2] = random.Next(1, 7);
            randomCourseHours[3] = random.Next(1, 7);
            randomCourseHours[4] = random.Next(1, 7);

            universityManager.AddTeachersCourse("Mathematics", "Monday", randomCourseHours[0]);
            universityManager.AddTeachersCourse("Physics", "Tuesday", randomCourseHours[1]);
            universityManager.AddTeachersCourse("Economy", "Wednesday", randomCourseHours[2]);
            universityManager.AddTeachersCourse("History", "Thursday", randomCourseHours[3]);
            universityManager.AddTeachersCourse("English", "Friday", randomCourseHours[4]);
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
        private void UpdateAdminStats()
        {
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

            foreach (var cleanerSchedule in universityManager.AllCleaner)
            {
                allCleanerWorkingSchedule += "Working Day: " + cleanerSchedule.workingDay + ", Working Length " + cleanerSchedule.workingLength + " Hours" + "\n";
            }

            var allStudents = loginRegisterManager.GetAllStudents().ToList();
            allStudents.Reverse();

            var allTeachers = loginRegisterManager.GetAllTeachers().ToList();
            allTeachers.Reverse();

            var allCleaners = loginRegisterManager.GetAllCleaners().ToList();
            allCleaners.Reverse();

            var allBoardingMembers = loginRegisterManager.GetAllBoardingMembers().ToList();
            allBoardingMembers.Reverse();


            StudentInfoStackPanel.Children.Clear();
            TeacherInfoStackPanel.Children.Clear();
            CleanerInfoStackPanel.Children.Clear();
            BoardingMemberInfoStackPanel.Children.Clear();

            foreach (var student in allStudents)
            {

                TextBlock studentTextBlock = new TextBlock();
                studentTextBlock.Text = "STUDENT NAME:  " + student.Username + " " + allCourseGrades;
                if (allStudents.IndexOf(student) == 0)
                {
                    studentTextBlock.Foreground = Brushes.Green;
                }
                StudentInfoStackPanel.Children.Add(studentTextBlock);
            }
            foreach (var teacher in allTeachers)
            {

                TextBlock teacherTextBlock = new TextBlock();
                teacherTextBlock.Text = "TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours;

                if (allTeachers.IndexOf(teacher) == 0)
                {
                    teacherTextBlock.Foreground = Brushes.Green;
                }
                TeacherInfoStackPanel.Children.Add(teacherTextBlock);
            }
            foreach (var cleaner in allCleaners)
            {

                TextBlock cleanerTextBlock = new TextBlock();
                cleanerTextBlock.Text = "CLEANER NAME: " + cleaner.Username + " " + allCleanerWorkingSchedule;

                if (allCleaners.IndexOf(cleaner) == 0)
                {
                    cleanerTextBlock.Foreground = Brushes.Green;
                }
                CleanerInfoStackPanel.Children.Add(cleanerTextBlock);
            }
            foreach (var boardingMember in allBoardingMembers)
            {

                TextBlock boardingMemberTextBlock = new TextBlock();
                boardingMemberTextBlock.Text = "BOARDING MEMBER NAME: " + boardingMember.Username;

                if (allBoardingMembers.IndexOf(boardingMember) == 0)
                {
                    boardingMemberTextBlock.Foreground = Brushes.Green;
                }
                BoardingMemberInfoStackPanel.Children.Add(boardingMemberTextBlock);
            }
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

            foreach (var cleanerSchedule in universityManager.AllCleaner)
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
                cleanerNames += " CLEANER NAME: " + cleaner.Username + " " + allCleanerWorkingSchedule + "\n";
            }

            foreach (var boardingMember in allBoardingMembers)
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
        #endregion Functions

        #region UI
        private void ShowAdminUI()
        {
            AdminPanel.Visibility = Visibility.Visible;
            AddEntityUIButton.Visibility = Visibility.Visible;
            ModifyEntityUIButton.Visibility = Visibility.Visible;
            DeleteEntityButton.Visibility = Visibility.Visible;
            ReturnAdminButton.Visibility = Visibility.Collapsed;
            ExitAdminButton.Visibility = Visibility.Visible;
            SearchModifyEntityButton.Visibility = Visibility.Collapsed;
            YesModifyEntityButton.Visibility = Visibility.Collapsed;
            NoModifyEntityButton.Visibility = Visibility.Collapsed;
            ModifyEntityUIButton.Content = "ModifyEntity";
        }
        private void ShowUserUI()
        {
            ExitButtonUserPanel.Visibility = Visibility.Visible;
            UserExitButton.Visibility = Visibility.Visible;
        }
        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void ReturnAdmin(object sender, RoutedEventArgs e)
        {
            ShowAdminUI();
            HideEntityUI();
        }

        private void AdminTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void UserTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void HideDeletionUI()
        {
            YesDeleteEntityButton.Visibility = Visibility.Collapsed;
            NoDeleteEntityButton.Visibility = Visibility.Collapsed;
            DeleteEntityUsername.Visibility = Visibility.Collapsed;
            DeleteEntityUsername.Visibility = Visibility.Collapsed;
            AdminDeleteTypeComboBox.Visibility = Visibility.Collapsed;
            DeleteEntityUsername.Visibility = Visibility.Collapsed;
            AdminDeleteTextBox.Visibility = Visibility.Collapsed;
            ShowAdminUI();
        }
        private void ShowDeletetionUI()
        {
            YesDeleteEntityButton.Visibility = Visibility.Visible;
            NoDeleteEntityButton.Visibility = Visibility.Visible;
            DeleteEntityUsername.Visibility = Visibility.Visible;
            DeleteEntityUsername.Visibility = Visibility.Visible;
            AdminDeleteTypeComboBox.Visibility = Visibility.Visible;
            DeleteEntityUsername.Visibility = Visibility.Visible;
            AdminDeleteTextBox.Visibility = Visibility.Visible;
        }
        private void AddEntityUI(object sender, RoutedEventArgs e)
        {
            EntityUI();
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
            ModifyEntityUIButton.Visibility = Visibility.Collapsed;
            DeleteEntityButton.Visibility = Visibility.Collapsed;
            ConfrimDeletionMessage.Visibility = Visibility.Collapsed;
            ReturnAdminButton.Visibility = Visibility.Visible;
            ExitAdminButton.Visibility = Visibility.Collapsed;
            AdminUsername.Text = "Username";
            ModificationConfrimationMessage.Visibility = Visibility.Collapsed;
        }
        private void UpdateAddNewEntityUI()
        {
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

            foreach (var cleanerSchedule in universityManager.AllCleaner)
            {
                allCleanerWorkingSchedule += "Working Day: " + cleanerSchedule.workingDay + ", Working Length " + cleanerSchedule.workingLength + " Hours" + "\n";
            }

            var allStudents = loginRegisterManager.GetAllStudents().ToList();
            allStudents.Reverse();

            var allTeachers = loginRegisterManager.GetAllTeachers().ToList();
            allTeachers.Reverse();

            var allCleaners = loginRegisterManager.GetAllCleaners().ToList();
            allCleaners.Reverse();

            var allBoardingMembers = loginRegisterManager.GetAllBoardingMembers().ToList();
            allBoardingMembers.Reverse();


            StudentInfoStackPanel.Children.Clear();
            TeacherInfoStackPanel.Children.Clear();
            CleanerInfoStackPanel.Children.Clear();
            BoardingMemberInfoStackPanel.Children.Clear();

            foreach (var student in allStudents)
            {


                NewEnityTextBlock.Text = "STUDENT NAME:  " + student.Username + " " + allCourseGrades;
                if (allStudents.IndexOf(student) == 0)
                {
                    NewEnityTextBlock.Foreground = Brushes.Green;
                }

            }
            foreach (var teacher in allTeachers)
            {


                NewEnityTextBlock.Text = "TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours;

                if (allTeachers.IndexOf(teacher) == 0)
                {
                    NewEnityTextBlock.Foreground = Brushes.Green;
                }

            }
            foreach (var cleaner in allCleaners)
            {


                NewEnityTextBlock.Text = "CLEANER NAME: " + cleaner.Username + " " + allCleanerWorkingSchedule;

                if (allCleaners.IndexOf(cleaner) == 0)
                {
                    NewEnityTextBlock.Foreground = Brushes.Green;
                }

            }
            foreach (var boardingMember in allBoardingMembers)
            {

                TextBlock boardingMemberTextBlock = new TextBlock();
                NewEnityTextBlock.Text = "BOARDING MEMBER NAME: " + boardingMember.Username;

                if (allBoardingMembers.IndexOf(boardingMember) == 0)
                {
                    boardingMemberTextBlock.Foreground = Brushes.Green;
                }

            }
        }
        private void HideEntityUI()
        {
            AdminUsername.Visibility = Visibility.Collapsed;
            AdminTextBox.Visibility = Visibility.Collapsed;
            AdminTypeComboBox.Visibility = Visibility.Collapsed;
            AddNewEntityButton.Visibility = Visibility.Collapsed;
            AdminPassword.Visibility = Visibility.Collapsed;
            AdminPasswordBox.Visibility = Visibility.Collapsed;
            ConfrimDeletionMessage.Visibility = Visibility.Collapsed;
            ReturnAdminButton.Visibility = Visibility.Collapsed;
            SearchDeleteEntityButton.Visibility = Visibility.Collapsed;
            AdminModifyTypeComboBox.Visibility = Visibility.Collapsed;
            AdminUsername.Visibility = Visibility.Collapsed;
            DeleteEntityUsername.Visibility = Visibility.Collapsed;
            AdminDeleteTextBox.Visibility = Visibility.Collapsed;
            AdminDeleteTypeComboBox.Visibility = Visibility.Collapsed;
            ModificationConfrimationMessage.Visibility = Visibility.Collapsed;
            ModifyEntityUIButton.Visibility = Visibility.Visible;
            AdminNewUsernameBox.Visibility = Visibility.Collapsed;
            ModifyEntityUIButton.Click += ModifyEntityUI;
            yesModify = false;
        }
        private void ModifyEntityUI()
        {
            AdminUsername.Visibility = Visibility.Visible;
            AdminTextBox.Visibility = Visibility.Visible;
            AddEntityUIButton.Visibility = Visibility.Collapsed;
            AdminModifyTypeComboBox.Visibility = Visibility.Visible;
            AddNewEntityButton.Visibility = Visibility.Collapsed;
            AdminPassword.Visibility = Visibility.Collapsed;
            AdminUsername.Text = "Search Name";
            ModifyEntityUIButton.Content = "Search Name";
            ModifyEntityUIButton.Click -= ModifyEntityUI;
            ModifyEntityUIButton.Click += SearchModifyEntity;
            ConfrimDeletionMessage.Visibility = Visibility.Collapsed;
            ReturnAdminButton.Visibility = Visibility.Visible;
            ExitAdminButton.Visibility = Visibility.Collapsed;
            DeleteEntityButton.Visibility = Visibility.Collapsed;
            ModificationConfrimationMessage.Visibility = Visibility.Collapsed;
            ModifyEntityUIButton.Content = "Search Name";
            AddEntityUIButton.Visibility = Visibility.Collapsed;
            DeleteEntityButton.Visibility = Visibility.Collapsed;
            NoDeleteEntityButton.Visibility = Visibility.Collapsed;
            YesDeleteEntityButton.Visibility = Visibility.Collapsed;
            yesModify = false;
        }
        private void HideAdminUI()
        {
            AdminPanel.Visibility = Visibility.Collapsed;
            AdminNewUsernameBox.Visibility = Visibility.Collapsed;
            AdminDeleteTypeComboBox.Visibility = Visibility.Collapsed;
            DeleteEntityButton.Visibility = Visibility.Visible;
            DeleteEntityUsername.Visibility = Visibility.Collapsed;
            YesDeleteEntityButton.Visibility = Visibility.Collapsed;
            NoDeleteEntityButton.Visibility = Visibility.Collapsed;
            AdminNewUsernameBox.Visibility = Visibility.Collapsed;
            AdminDeleteTextBox.Visibility = Visibility.Collapsed;
            SearchDeleteEntityButton.Visibility = Visibility.Collapsed;
            ExitAdminButton.Visibility = Visibility.Collapsed;
            UserExitButton.Visibility = Visibility.Collapsed;
        }
        private void ShowAdminUIStats()
        {
            RegistrationLoginPanel.Visibility = Visibility.Collapsed;
            AdminPanel.Visibility = Visibility.Visible;
            AdminTypeComboBox.Visibility = Visibility.Collapsed;
            AdminTextBox.Visibility = Visibility.Collapsed;
            AdminUsername.Visibility = Visibility.Collapsed;
            AddNewEntityButton.Visibility = Visibility.Collapsed;
            AdminPassword.Visibility = Visibility.Collapsed;
            AdminPasswordBox.Visibility = Visibility.Collapsed;
            AdminModifyTypeComboBox.Visibility = Visibility.Collapsed;
            RegistrationMessage.Visibility = Visibility.Collapsed;
            ReturnAdminButton.Visibility = Visibility.Collapsed;
            ExitAdminButton.Visibility = Visibility.Visible;
            YesModifyEntityButton.Visibility = Visibility.Collapsed;
            NoModifyEntityButton.Visibility = Visibility.Collapsed;
            SearchModifyEntityButton.Visibility = Visibility.Collapsed;
            ModifyNameTextBox.Visibility = Visibility.Collapsed;
            ShowAdminRelatedStats();
        }
        private void ModifyEntityUI(object sender, RoutedEventArgs e)
        {
            ModifyEntityUI();
        }
        private void SearchModifyEntity(object sender, RoutedEventArgs e)
        {
            string adminRegisterUsername = AdminTextBox.Text;
            string modifyNameBox = ModifyNameTextBox.Text;
            string selectedCategory = ((ComboBoxItem)AdminModifyTypeComboBox.SelectedItem)?.Content.ToString() ?? "";
            if (selectedCategory == "TEACHER")
            {
                var matchingValuesTeacher = loginRegisterManager.GetAllTeachers().Where(user => user.Username.Contains(adminRegisterUsername));
                if (matchingValuesTeacher.Any())
                {
                    YesModifyEntityButton.Visibility = Visibility.Visible;
                    NoDeleteEntityButton.Visibility = Visibility.Visible;
                    SearchModifyEntityButton.Visibility = Visibility.Collapsed;

                    AdminTextBox.Visibility = Visibility.Collapsed;
                    ModifyEntityUIButton.Visibility = Visibility.Collapsed;
                    AdminUsername.Text = "New Name";
                    ModifyNameTextBox.Visibility = Visibility.Visible;
                    ModificationConfrimationMessage.Text = "Are you sure you want to  Modify this User: ? " + adminRegisterUsername + " If yes type a new name in the New name input field";
                    ModificationConfrimationMessage.Visibility = Visibility.Visible;
                }
            }
            else if (selectedCategory == "STUDENT")
            {
                var matchingValuesStudent = loginRegisterManager.GetAllStudents().Where(user => user.Username.Contains(adminRegisterUsername));
                if (matchingValuesStudent.Any())
                {
                    YesModifyEntityButton.Visibility = Visibility.Visible;
                    NoDeleteEntityButton.Visibility = Visibility.Visible;
                    SearchModifyEntityButton.Visibility = Visibility.Collapsed;

                    AdminTextBox.Visibility = Visibility.Collapsed;
                    ModifyEntityUIButton.Visibility = Visibility.Collapsed;
                    AdminUsername.Text = "New Name";
                    ModifyNameTextBox.Visibility = Visibility.Visible;
                    ModificationConfrimationMessage.Text = "Are you sure you want to Modify this User: ? " + adminRegisterUsername + " If yes type a new name in the New name input field";
                    ModificationConfrimationMessage.Visibility = Visibility.Visible;
                }
            }
            else if (selectedCategory == "BOARDING MEMBER")
            {
                var matchingValuesBoardingMember = loginRegisterManager.GetAllBoardingMembers().Where(user => user.Username.Contains(adminRegisterUsername));
                if (matchingValuesBoardingMember.Any())
                {
                    YesModifyEntityButton.Visibility = Visibility.Visible;
                    NoDeleteEntityButton.Visibility = Visibility.Visible;
                    SearchModifyEntityButton.Visibility = Visibility.Collapsed;

                    AdminTextBox.Visibility = Visibility.Collapsed;
                    ModifyEntityUIButton.Visibility = Visibility.Collapsed;
                    AdminUsername.Text = "New Name";
                    ModifyNameTextBox.Visibility = Visibility.Visible;
                    ModificationConfrimationMessage.Text = "Are you sure you want to Modify this User: ? " + adminRegisterUsername + " If yes type a new name in the New name input field";
                    ModificationConfrimationMessage.Visibility = Visibility.Visible;
                }
            }
            else if (selectedCategory == "CLEANER")
            {
                var matchingValuesCleaner = loginRegisterManager.GetAllCleaners().Where(user => user.Username.Contains(adminRegisterUsername));
                if (matchingValuesCleaner.Any())
                {
                    YesModifyEntityButton.Visibility = Visibility.Visible;
                    NoDeleteEntityButton.Visibility = Visibility.Visible;
                    SearchModifyEntityButton.Visibility = Visibility.Collapsed;

                    AdminTextBox.Visibility = Visibility.Collapsed;
                    ModifyEntityUIButton.Visibility = Visibility.Collapsed;
                    AdminUsername.Text = "New Name";
                    ModifyNameTextBox.Visibility = Visibility.Visible;
                    ModificationConfrimationMessage.Text = "Are you sure you want to Modify this User: ? " + adminRegisterUsername + " If yes type a new name in the New name input field";
                    ModificationConfrimationMessage.Visibility = Visibility.Visible;
                }

                else
                {
                    ModificationConfrimationMessage.Text  = "No user found with the given username.";
                    ModificationConfrimationMessage.Visibility = Visibility.Visible;
                }
            }
        }
        #endregion UI

        #region Entities
        private void YesModifyEntity(object sender, RoutedEventArgs e)
        {
            string adminRegisterUsername = AdminTextBox.Text;
            string selectedCategory = ((ComboBoxItem)AdminModifyTypeComboBox.SelectedItem)?.Content.ToString() ?? "";
            var matchingValuesTeacher = loginRegisterManager.GetAllTeachers().Where(user => user.Username.Contains(adminRegisterUsername));
            var matchingValuesCleaner = loginRegisterManager.GetAllCleaners().Where(user => user.Username.Contains(adminRegisterUsername));
            var matchingValuesBoardingMember = loginRegisterManager.GetAllBoardingMembers().Where(user => user.Username.Contains(adminRegisterUsername));
            var matchingValuesStudent = loginRegisterManager.GetAllStudents().Where(user => user.Username.Contains(adminRegisterUsername));
            YesModifyEntityButton.Visibility = Visibility.Collapsed;
            NoDeleteEntityButton.Visibility = Visibility.Collapsed;
            SearchModifyEntityButton.Visibility = Visibility.Collapsed;
            ModifyNameTextBox.Visibility = Visibility.Collapsed;
            AdminModifyTypeComboBox.Visibility = Visibility.Collapsed;
            AdminUsername.Visibility = Visibility.Collapsed;
            ShowAdminUI();
            ModifyEntityUIButton.Click += ModifyEntityUI;
            if (selectedCategory == "TEACHER")
            {
                if (matchingValuesTeacher.Any())
                {
                    string newTeacherName = ModifyNameTextBox.Text;
                    matchingValuesTeacher.First().Username = newTeacherName;
                    ModificationConfrimationMessage.Text = "The New username now is " + newTeacherName;
                    loginRegisterManager.SaveUserData();
                }
            }
            else if (selectedCategory == "STUDENT")
            {
                if (matchingValuesStudent.Any())
                {
                    string newStudentName = ModifyNameTextBox.Text;
                    matchingValuesStudent.First().Username = newStudentName;
                    ModificationConfrimationMessage.Text = "The New username now is " + newStudentName;
                    loginRegisterManager.SaveUserData();
                }
            }
            else if (selectedCategory == "CLEANER")
            {
                if (matchingValuesCleaner.Any())
                {
                    string newCleanerName = ModifyNameTextBox.Text;
                    matchingValuesCleaner.First().Username = newCleanerName;
                    ModificationConfrimationMessage.Text = "The New username now is " + newCleanerName;
                    loginRegisterManager.SaveUserData();
                }
            }
            else if (selectedCategory == "BOARDING MEMBER")
            {
                if (matchingValuesBoardingMember.Any())
                {
                    string newBoardingMemberName = ModifyNameTextBox.Text;
                    matchingValuesBoardingMember.First().Username = newBoardingMemberName;
                    ModificationConfrimationMessage.Text = "The New username now is " + newBoardingMemberName;
                    loginRegisterManager.SaveUserData();
                }
            }
        }

        private void NoModifyEntity(object sender, RoutedEventArgs e)
        {
            yesModify = false;
            noModify = true;
        }
        private void YesDeleteEntity(object sender, RoutedEventArgs e)
        {
            yesDeleteEntity = true;
            noDeleteEntity = false;
            Debug.WriteLine("Yes deleteEntity is " + yesDeleteEntity);
            ExecuteDeletionIfConfirmed();
        }

        private void NoDeleteEntity(object sender, RoutedEventArgs e)
        {
            noDeleteEntity = true;
            yesDeleteEntity = false;
            ConfrimDeletionMessage.Visibility = Visibility.Collapsed;
        }

        private void SearchNameDeleteEntity(object sender, RoutedEventArgs e)
        {
            string deleteUserInputFieldUsername = AdminDeleteTextBox.Text;
            string selectedCategory = ((ComboBoxItem)AdminDeleteTypeComboBox.SelectedItem)?.Content.ToString() ?? "";

            if (selectedCategory == "TEACHER")
            {
                var matchingValuesTeacher = loginRegisterManager.GetAllTeachers().Where(user => user.Username.Contains(deleteUserInputFieldUsername));
                if (matchingValuesTeacher.Any())
                {
                    YesDeleteEntityButton.Visibility = Visibility.Visible;
                    NoDeleteEntityButton.Visibility = Visibility.Visible;
                    SearchDeleteEntityButton.Visibility = Visibility.Collapsed;
                    ConfrimDeletionMessage.Text = "Are you sure you want to delete this User?";
                    ConfrimDeletionMessage.Visibility = Visibility.Visible;
                }
            }
            else if (selectedCategory == "CLEANER")
            {
                var matchingValuesCleaner = loginRegisterManager.GetAllCleaners().Where(user => user.Username.Contains(deleteUserInputFieldUsername));
                if (matchingValuesCleaner.Any())
                {
                    YesDeleteEntityButton.Visibility = Visibility.Visible;
                    NoDeleteEntityButton.Visibility = Visibility.Visible;
                    SearchDeleteEntityButton.Visibility = Visibility.Collapsed;
                    ConfrimDeletionMessage.Text = "Are you sure you want to delete this User?";
                    ConfrimDeletionMessage.Visibility = Visibility.Visible;
                }
            }
            else if (selectedCategory == "STUDENT")
            {
                var matchingValuesStudent = loginRegisterManager.GetAllStudents().Where(user => user.Username.Contains(deleteUserInputFieldUsername));
                if (matchingValuesStudent.Any())
                {
                    YesDeleteEntityButton.Visibility = Visibility.Visible;
                    NoDeleteEntityButton.Visibility = Visibility.Visible;
                    SearchDeleteEntityButton.Visibility = Visibility.Collapsed;
                    ConfrimDeletionMessage.Text = "Are you sure you want to delete this User?";
                    ConfrimDeletionMessage.Visibility = Visibility.Visible;
                }
            }
            else if (selectedCategory == "BOARDING MEMBER")
            {
                var matchingValuesBoardingMember = loginRegisterManager.GetAllBoardingMembers().Where(user => user.Username.Contains(deleteUserInputFieldUsername));
                if (matchingValuesBoardingMember.Any())
                {
                    YesDeleteEntityButton.Visibility = Visibility.Visible;
                    NoDeleteEntityButton.Visibility = Visibility.Visible;
                    SearchDeleteEntityButton.Visibility = Visibility.Collapsed;
                    ConfrimDeletionMessage.Text = "Are you sure you want to delete this User?";
                    ConfrimDeletionMessage.Visibility = Visibility.Visible;
                }
            }
            else
            {
                ConfrimDeletionMessage.Text = "No user found with the given username.";
                ConfrimDeletionMessage.Visibility = Visibility.Visible;
            }
        }

        private void AddNewEntity(object sender, RoutedEventArgs e)
        {
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

            foreach (var cleanerSchedule in universityManager.AllCleaner)
            {
                allCleanerWorkingSchedule += "Working Day: " + cleanerSchedule.workingDay + ", Working Length " + cleanerSchedule.workingLength + " Hours" + "\n";
            }

            var allStudents = loginRegisterManager.GetAllStudents().ToList();
            var allTeachers = loginRegisterManager.GetAllTeachers().ToList();
            var allCleaners = loginRegisterManager.GetAllCleaners().ToList();
            var allBoardingMembers = loginRegisterManager.GetAllBoardingMembers().ToList();

            StudentInfoStackPanel.Children.Clear();
            TeacherInfoStackPanel.Children.Clear();
            CleanerInfoStackPanel.Children.Clear();
            BoardingMemberInfoStackPanel.Children.Clear();

            string adminRegisterUsername = AdminTextBox.Text;
            string adminRegisterPassword = AdminPasswordBox.Password;
            string selectedCategory = ((ComboBoxItem)AdminTypeComboBox.SelectedItem)?.Content.ToString();

            if (selectedCategory == "NEW TEACHER")
            {
                loginRegisterManager.Register("TEACHER", adminRegisterUsername, adminRegisterPassword);
                TextBlock newTeacherTextBlock = new TextBlock
                {
                    Text = "New Added TEACHER:  " + adminRegisterUsername,
                    Foreground = Brushes.Green
                };
                TeacherInfoStackPanel.Children.Add(newTeacherTextBlock);
            }
            else if (selectedCategory == "NEW BOARDING MEMBER")
            {
                loginRegisterManager.Register("BOARDING MEMBER", adminRegisterUsername, adminRegisterPassword);
                TextBlock newBoardingMemberTextBlock = new TextBlock
                {
                    Text = "New BOARDING MEMBER:  " + adminRegisterUsername,
                    Foreground = Brushes.Green
                };
                BoardingMemberInfoStackPanel.Children.Add(newBoardingMemberTextBlock);
            }
            else if (selectedCategory == "NEW CLEANER")
            {
                loginRegisterManager.Register("CLEANER", adminRegisterUsername, adminRegisterPassword);
                TextBlock newCleanerTextBlock = new TextBlock
                {
                    Text = "New Added CLEANER:  " + adminRegisterUsername,
                    Foreground = Brushes.Green
                };
                CleanerInfoStackPanel.Children.Add(newCleanerTextBlock);
            }
            else if (selectedCategory == "NEW STUDENT")
            {
                loginRegisterManager.Register("STUDENT", adminRegisterUsername, adminRegisterPassword);
                TextBlock newStudentTextBlock = new TextBlock
                {
                    Text = "New Added STUDENT:  " + adminRegisterUsername + " " + allCourseGrades,
                    Foreground = Brushes.Green
                };
                StudentInfoStackPanel.Children.Add(newStudentTextBlock);
            }
            else if (selectedCategory == "NEW COURSE")
            {
                universityManager.AddStudentCourse(adminRegisterUsername, 0, 0);
            }
        }


        private void DeleteEntity(object sender, RoutedEventArgs e)
        {
            AdminDeleteTypeComboBox.Visibility = Visibility.Visible;
            DeleteEntityButton.Visibility = Visibility.Collapsed;
            DeleteEntityUsername.Visibility = Visibility.Visible;
            ModifyEntityUIButton.Visibility = Visibility.Collapsed;
            AddEntityUIButton.Visibility = Visibility.Collapsed;
            AdminDeleteTextBox.Visibility = Visibility.Visible;

            YesDeleteEntityButton.Visibility = Visibility.Collapsed;
            NoDeleteEntityButton.Visibility = Visibility.Collapsed;
            SearchDeleteEntityButton.Visibility = Visibility.Visible;
            ConfrimDeletionMessage.Visibility = Visibility.Visible;
            ReturnAdminButton.Visibility = Visibility.Visible;
            ExitAdminButton.Visibility = Visibility.Collapsed;

        }
        private void ExecuteDeletionIfConfirmed()
        {
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

            foreach (var cleanerSchedule in universityManager.AllCleaner)
            {
                allCleanerWorkingSchedule += "Working Day: " + cleanerSchedule.workingDay + ", Working Length " + cleanerSchedule.workingLength + " Hours" + "\n";
            }

            var allStudents = loginRegisterManager.GetAllStudents().ToList();
            allStudents.Reverse();

            var allTeachers = loginRegisterManager.GetAllTeachers().ToList();
            allTeachers.Reverse();

            var allCleaners = loginRegisterManager.GetAllCleaners().ToList();
            allCleaners.Reverse();

            var allBoardingMembers = loginRegisterManager.GetAllBoardingMembers().ToList();
            allBoardingMembers.Reverse();


            StudentInfoStackPanel.Children.Clear();
            TeacherInfoStackPanel.Children.Clear();
            CleanerInfoStackPanel.Children.Clear();
            BoardingMemberInfoStackPanel.Children.Clear();
            if (yesDeleteEntity)
            {
                string deleteUserInputFieldUsername = AdminDeleteTextBox.Text;
                string selectedCategory = ((ComboBoxItem)AdminDeleteTypeComboBox.SelectedItem)?.Content.ToString() ?? "";

                if (selectedCategory == "TEACHER")
                {

                    loginRegisterManager.DeleteUser(selectedCategory, deleteUserInputFieldUsername);
                    ConfrimDeletionMessage.Text = "Teacher has been deleted successfully.";
                    TeacherInfoStackPanel.UpdateLayout();
                    HideDeletionUI();

                }
                else if (selectedCategory == "CLEANER")
                {
                    loginRegisterManager.DeleteUser(selectedCategory, deleteUserInputFieldUsername);
                    ConfrimDeletionMessage.Text = "Cleaner has been deleted successfully.";
                    HideDeletionUI();
                }

                else if (selectedCategory == "STUDENT")
                {
                    loginRegisterManager.DeleteUser(selectedCategory, deleteUserInputFieldUsername);
                    ConfrimDeletionMessage.Text = "Student has been deleted successfully.";
                    HideDeletionUI();
                }

                else if (selectedCategory == "BOARDING MEMBER")
                {
                    loginRegisterManager.DeleteUser(selectedCategory, deleteUserInputFieldUsername);
                    ConfrimDeletionMessage.Text = "Boarding Member has been deleted successfully.";
                    HideDeletionUI();
                }
                ConfrimDeletionMessage.Visibility = Visibility.Visible;

            }
            if (noDeleteEntity)
            {
                HideDeletionUI();
            }
        }
    
            }
        }
        #endregion Entities
    