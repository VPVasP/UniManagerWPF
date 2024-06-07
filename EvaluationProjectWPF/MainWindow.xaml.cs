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
        string allCourseGrades = string.Empty;
        string allTeacherWorkingHours = string.Empty;
        string allCleanerWorkingSchedule = string.Empty;
        string adminRegisterUsername = string.Empty;
        string teacherNames = string.Empty;
        private int minimumUsernameCharacters =3;
        private int minimumPasswordCharacters= 6;

        public MainWindow()
        {
            InitializeComponent();
            loginRegisterManager = new LoginRegisterManager();
            universityManager = new UniversityManager();
            cleaner = new UniversityManager.Cleaner();
            boardingMember = new UniversityManager.BoardingMember();
            teacher = new UniversityManager.Teacher();
            string modifyNameBox = ModifyNameTextBox.Text;
            InitialiseComponents();
            HideAdminUI();
        }
        private void InitialiseComponents()
        {
            adminRegisterUsername = AdminTextBox.Text;

            teacherNames = "All Teachers Info:\n";
        }


        #region RegistrationLogin


        //register user method
        private void RegisterUser(object sender, RoutedEventArgs e)
        {

            string loginUsername = UsernameTextBox.Text;
            string loginPassword = PasswordBox.Password;
            string selectedCategory = ((ComboBoxItem)UserTypeComboBox.SelectedItem)?.Content.ToString() ?? "";

            if (string.IsNullOrEmpty(loginUsername) || string.IsNullOrEmpty(loginPassword))
            {
                Debug.WriteLine("Please provide username and password");
                UserExistsMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Text = "Please provide username and password";
                return;
            }
            if (loginUsername.Length < minimumUsernameCharacters)
            {
                UserExistsMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Text = "Invalid username length,it must be more than " + minimumUsernameCharacters + " digits"; ;
                return;
            }
          else if (loginPassword.Length < minimumUsernameCharacters)
            {
                UserExistsMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Text = "Invalid password length,it must be more than " + minimumUsernameCharacters + " digits";
                return;
            }
           else if(loginUsername.Length >minimumUsernameCharacters && loginPassword.Length > minimumPasswordCharacters)
            {
                loginRegisterManager.Register(selectedCategory, loginUsername, loginPassword);
            }
 

            switch (selectedCategory)
            {
                case "ADMIN":
                    //when you log in as an admin you can see all the employees info and add/modify/delete entities
                    ShowAdminUIStats();
                    LoginMessage.Text = "You are now logged in as ADMIN " + loginUsername + ". Welcome to the application.";
                    break;
                case "TEACHER":
                    //display only the teacher info

                    //add the teaching random course and hours and display them
                    AddTeacherRandomTeachingCoursesAndHours();
                    string allTeacherCourseandHours = "The working hours and courses you need to teach for  " + loginUsername + " Are: " + ":\n";
                    foreach (UniversityManager.Teacher teacher in universityManager.AllTeachers)
                    {
                        allTeacherCourseandHours += "  You teach Course: " + teacher.teachingCourseTitle + ", Working Day: " + teacher.teachingWorkingDay + ", Working Hour: " + teacher.teacherWorkingHour + "\n";
                    }

                    //ui handling
                    RegistrationLoginPanel.Visibility = Visibility.Collapsed;

                    TeacherTeachingDates.Text = allTeacherCourseandHours;
                    LoginMessage.Visibility = Visibility.Visible;
                    UserExistsMessage.Visibility = Visibility.Collapsed;
                    TeacherTeachingDates.Visibility = Visibility.Visible;
                    ShowUserUI();
                    LoginMessage.Text = "You are now logged in as TEACHER " + loginUsername + ". Welcome to the application.";

                    break;
                case "BOARDING MEMBER":

                    //when you log in as a boarding member you can see the student stats and the teachers
                    RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                    LoginMessage.Text = "You are now logged in BOARDING MEMBER " + loginUsername + " You can see all the student info, grades and the Teacher Info!";
                    LoginMessage.Visibility = Visibility.Visible;
                    UserExistsMessage.Visibility = Visibility.Collapsed;

                    //show the correct ui if you are the boarding member
                    ShowBoardingMemberStudentTeacherStats();
                    ShowUserUI();
                    break;
                case "CLEANER":


                    //add the cleaners working schedule 
                    AddCleanerWokringSchedule();
                    string allCleanerSchedule = "The working Schedule for the Cleaner " + loginUsername + " is: " + ":\n";
                    foreach (UniversityManager.Cleaner cleaner in universityManager.AllCleaner)
                    {
                        allCleanerSchedule += "Working Day: " + cleaner.workingDay + ", Working Length " + cleaner.workingLength + " Hours" + "\n";
                    }

                    //ui handling
                    RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                    LoginMessage.Text = "You are now logged in CLEANER " + loginUsername + " Welcome to the application";
                    CleanerDates.Text = allCleanerSchedule;
                    CleanerDates.Visibility = Visibility.Visible;
                    LoginMessage.Visibility = Visibility.Visible;
                    StudentGrades.Visibility = Visibility.Collapsed;
                    UserExistsMessage.Visibility = Visibility.Collapsed;
                    ShowUserUI();
                    break;
                case "STUDENT":
                    //add the courses and show all the grades of that student
                    AddRandomStudentCourses();
                    string allCourseGrades = "The Grades for The Student:  " + loginUsername + " Are: " + ":\n";

                    foreach (Courses course in universityManager.AllCourses)
                    {
                        allCourseGrades += "Course Name: " + course.CourseTitle + ", Oral Mark: " + course.OralMark + ", Written Mark: " + course.WritingMark + "\n";
                    }

                    //ui handling
                    StudentGrades.Text = allCourseGrades;
                    LoginMessage.Text = "You are now logged in STUDENT " + loginUsername + " Welcome to the application";
                    RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                    StudentGrades.Visibility = Visibility.Visible;
                    CleanerDates.Visibility = Visibility.Collapsed;
                    LoginMessage.Visibility = Visibility.Visible;
                    UserExistsMessage.Visibility = Visibility.Collapsed;
                    ShowUserUI();
                    break;
                default:
                    UserExistsMessage.Visibility = Visibility.Visible;
                    UserExistsMessage.Text = "Not selected category username or put incorrect passwsord";
                    break;

            }
        
}



        //login user method
        private void LoginUser(object sender, RoutedEventArgs e)
        {
            string loginUsername = UsernameTextBox.Text;
            string loginPassword = PasswordBox.Password;
            string selectedCategory = ((ComboBoxItem)UserTypeComboBox.SelectedItem)?.Content.ToString() ?? "";

            if (string.IsNullOrEmpty(loginUsername) || string.IsNullOrEmpty(loginPassword))
            {
                UserExistsMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Text = "Please provide username and password";
                return; 
            }

            if (loginRegisterManager.DoesUserExistLogin(selectedCategory, loginUsername, loginPassword))
            {
                loginRegisterManager.Login(selectedCategory, loginUsername, loginPassword);

                //switch statement that handles the login functionallity for each category

                switch (selectedCategory)
                {
                    case "ADMIN":
                        //when you log in as an admin you can see all the employees info and add/modify/delete entities
                        ShowAdminUIStats();
                        LoginMessage.Text = "You are now logged in as ADMIN " + loginUsername + ". Welcome to the application.";
                        break;
                    case "TEACHER":
                        //display only the teacher info

                        //add the teaching random course and hours and display them
                        AddTeacherRandomTeachingCoursesAndHours();
                        string allTeacherCourseandHours = "The working hours and courses you need to teach for  " + loginUsername + " Are: " + ":\n";
                        foreach (UniversityManager.Teacher teacher in universityManager.AllTeachers)
                        {
                            allTeacherCourseandHours += "  You teach Course: " + teacher.teachingCourseTitle + ", Working Day: " + teacher.teachingWorkingDay + ", Working Hour: " + teacher.teacherWorkingHour + "\n";
                        }

                        //ui handling
                        RegistrationLoginPanel.Visibility = Visibility.Collapsed;

                        TeacherTeachingDates.Text = allTeacherCourseandHours;
                        LoginMessage.Visibility = Visibility.Visible;
                        UserExistsMessage.Visibility = Visibility.Collapsed;
                        TeacherTeachingDates.Visibility = Visibility.Visible;
                        ShowUserUI();
                        LoginMessage.Text = "You are now logged in as TEACHER " + loginUsername + ". Welcome to the application.";

                        break;
                    case "BOARDING MEMBER":

                        //when you log in as a boarding member you can see the student stats and the teachers
                        RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                        LoginMessage.Text = "You are now logged in BOARDING MEMBER " + loginUsername + " You can see all the student info, grades and the Teacher Info!";
                        LoginMessage.Visibility = Visibility.Visible;
                        UserExistsMessage.Visibility = Visibility.Collapsed;

                        //show the correct ui if you are the boarding member
                        ShowBoardingMemberStudentTeacherStats();
                        ShowUserUI();
                        break;
                    case "CLEANER":

                        
                        //add the cleaners working schedule 
                        AddCleanerWokringSchedule();
                        string allCleanerSchedule = "The working Schedule for the Cleaner " + loginUsername + " is: " + ":\n";
                        foreach (UniversityManager.Cleaner cleaner in universityManager.AllCleaner)
                        {
                            allCleanerSchedule += "Working Day: " + cleaner.workingDay + ", Working Length " + cleaner.workingLength + " Hours" + "\n";
                        }

                        //ui handling
                        RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                        LoginMessage.Text = "You are now logged in CLEANER " + loginUsername + " Welcome to the application";
                        CleanerDates.Text = allCleanerSchedule;
                        CleanerDates.Visibility = Visibility.Visible;
                        LoginMessage.Visibility = Visibility.Visible;
                        StudentGrades.Visibility = Visibility.Collapsed;
                        UserExistsMessage.Visibility = Visibility.Collapsed;
                        ShowUserUI();
                        break;
                    case "STUDENT":
                        //add the courses and show all the grades of that student
                        AddRandomStudentCourses();
                        string allCourseGrades = "The Grades for The Student:  " + loginUsername + " Are: " + ":\n";

                        foreach (Courses course in universityManager.AllCourses)
                        {
                            allCourseGrades += "Course Name: " + course.CourseTitle + ", Oral Mark: " + course.OralMark + ", Written Mark: " + course.WritingMark + "\n";
                        }

                        //ui handling
                        StudentGrades.Text = allCourseGrades;
                        LoginMessage.Text = "You are now logged in STUDENT " + loginUsername + " Welcome to the application";
                        RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                        StudentGrades.Visibility = Visibility.Visible;
                        CleanerDates.Visibility = Visibility.Collapsed;
                        LoginMessage.Visibility = Visibility.Visible;
                        UserExistsMessage.Visibility = Visibility.Collapsed;
                        ShowUserUI();
                        break;
                    default:
                        //show the message that something is wrong with the selected category and password
                        UserExistsMessage.Visibility = Visibility.Visible;
                        UserExistsMessage.Text = "Not selected category username or put incorrect passwsord";
                        break;
                }
            }
            else
            {
                //show the message that something is wrong with the user's credentials
                LoginMessage.Visibility = Visibility.Visible;
                LoginMessage.Text = "Something is wrong with your credentials";
            }
        }
    
            #endregion RegistrationLogin

            #region Functions

        //add random grades to the students courses
            private void AddRandomStudentCourses()
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


        //add random course hours to every course
        private void AddTeacherRandomTeachingCoursesAndHours()
        {

            Random random = new Random();
            int[] randomCourseHours = new int[5];
            randomCourseHours[0] = random.Next(1, 7);
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


        //function that adds the cleaners working hours
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
            AddRandomStudentCourses();
            AddTeacherRandomTeachingCoursesAndHours();
            //hide the registration and login panel
            RegistrationLoginPanel.Visibility = Visibility.Collapsed;

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


              ClearStackPanels();

            foreach (var student in allStudents)
            {

                TextBlock studentTextBlock = new TextBlock();
                studentTextBlock.Text = "STUDENT NAME:  " + student.Username + " " + allCourseGrades;

                StudentInfoStackPanel.Children.Add(studentTextBlock);
            }
            //search through the teachers and add the teacher text block to the stack panel
            foreach (var teacher in allTeachers)
            {

                TextBlock teacherTextBlock = new TextBlock();
                teacherTextBlock.Text = "TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours;

                TeacherInfoStackPanel.Children.Add(teacherTextBlock);
            }

            //search through the cleaners and add the cleaners text block to the stack panel
            foreach (var cleaner in allCleaners)
            {

                TextBlock cleanerTextBlock = new TextBlock();
                cleanerTextBlock.Text = "CLEANER NAME: " + cleaner.Username + " " + allCleanerWorkingSchedule;

                CleanerInfoStackPanel.Children.Add(cleanerTextBlock);
            }
            //search through the boarding member and add the teachers text block to the stack panel
            foreach (var boardingMember in allBoardingMembers)
            {

                TextBlock boardingMemberTextBlock = new TextBlock();
                boardingMemberTextBlock.Text = "BOARDING MEMBER NAME: " + boardingMember.Username;

                BoardingMemberInfoStackPanel.Children.Add(boardingMemberTextBlock);
            }
        }

        private void UIAdminStats()
        {
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
            //get all the students,teachers,cleaners and boarding members and store them into variables
            var allStudents = loginRegisterManager.GetAllStudents();
            var allTeachers = loginRegisterManager.GetAllTeachers();
            var allCleaners = loginRegisterManager.GetAllCleaners();
            var allBoardingMembers = loginRegisterManager.GetAllBoardingMembers();

            string teacherNames = "All Teachers Info:\n";
            string studentNames = "All Students Info:\n";
            string cleanerNames = "All Cleaners Info:\n";
            string boardingMemberNames = "All Boarding Members Info:\n";

            //search through the students and add their courses grades 
            foreach (var student in allStudents)
            {
                studentNames += " STUDENT NAME:  " + student.Username + " " + allCourseGrades + "\n";
            }
            //search through the teachers and add their working hours
            foreach (var teacher in allTeachers)
            {
                teacherNames += " TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours + "\n";
            }
            //search through the cleaners and add their working hours
            foreach (var cleaner in allCleaners)
            {
                cleanerNames += " CLEANER NAME: " + cleaner.Username + " " + allCleanerWorkingSchedule + "\n";
            }
            //search through the boarding members and show their username
            foreach (var boardingMember in allBoardingMembers)
            {
                boardingMemberNames += " BOARDING MEMBER NAME: " + boardingMember.Username + "\n";

            }
            //assign to the texts the new values
            StudentInfoText.Text = studentNames;
            StudentInfoText.Visibility = Visibility.Visible;

            TeacherInfoText.Text = teacherNames;
            TeacherInfoText.Visibility = Visibility.Visible;

            CleanerInfoText.Text = cleanerNames;
            CleanerInfoText.Visibility = Visibility.Visible;

            BoardingMemberInfoText.Text = boardingMemberNames;
            BoardingMemberInfoText.Visibility = Visibility.Visible;
        }
        private void ShowAdminRelatedStats()
        {
            AddRandomStudentCourses();
            AddTeacherRandomTeachingCoursesAndHours();
            AddCleanerWokringSchedule();
            //hide the registration and login panel
            RegistrationLoginPanel.Visibility = Visibility.Collapsed;

            //initialise the strings
            string allCourseGrades = string.Empty;
            string allTeacherWorkingHours = string.Empty;
            string allCleanerWorkingSchedule = string.Empty;


            //search through the teachers and add their working hour and courses
            foreach (var teacherHours in universityManager.AllTeachers)
            {
                allTeacherWorkingHours += "  They teach Course: " + teacherHours.teachingCourseTitle + ", Working Day: " + teacherHours.teachingWorkingDay + ", Working Hour: " + teacherHours.teacherWorkingHour + "\n";
            }
            //search through the courses and add their courses 
            foreach (var course in universityManager.AllCourses)
            {
                allCourseGrades += " Course Name: " + course.CourseTitle + ", Oral Mark: " + course.OralMark + ", Written Mark: " + course.WritingMark + "\n";
            }

            //search thorugh the cleaners schedule and add their working info
            foreach (var cleanerSchedule in universityManager.AllCleaner)
            {
                allCleanerWorkingSchedule += "Working Day: " + cleanerSchedule.workingDay + ", Working Length " + cleanerSchedule.workingLength + " Hours" + "\n";
            }

            //get all the students,teachers,cleaners and boarding members and store them into variables
            var allStudents = loginRegisterManager.GetAllStudents();
            var allTeachers = loginRegisterManager.GetAllTeachers();
            var allCleaners = loginRegisterManager.GetAllCleaners();
            var allBoardingMembers = loginRegisterManager.GetAllBoardingMembers();

            string teacherNames = "All Teachers Info:\n";
            string studentNames = "All Students Info:\n";
            string cleanerNames = "All Cleaners Info:\n";
            string boardingMemberNames = "All Boarding Members Info:\n";

            //search through the students and add their courses grades 

            foreach (var student in allStudents)
            {
                studentNames += " STUDENT NAME:  " + student.Username + " " + allCourseGrades + "\n";
            }


            //search through the teachers and add their working hours
            foreach (var teacher in allTeachers)
            {
                teacherNames += " TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours + "\n";
            }
            //search through the cleaners and add their working hours
            foreach (var cleaner in allCleaners)
            {
                cleanerNames += " CLEANER NAME: " + cleaner.Username + " " + allCleanerWorkingSchedule + "\n";
            }

            //search through the boarding members and show their username
            foreach (var boardingMember in allBoardingMembers)
            {
                boardingMemberNames += " BOARDING MEMBER NAME: " + boardingMember.Username + "\n";

            }

            //assign to the texts the new values
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

        //function that shows the Admin UI
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
            ModifyNameTextBox.Visibility = Visibility.Collapsed;
            ModifyEntityUIButton.Content = "ModifyEntity";
        }

        //function that shows the user UI
        private void ShowUserUI()
        {
            ExitButtonUserPanel.Visibility = Visibility.Visible;
            UserExitButton.Visibility = Visibility.Visible;
        }
        //function that exits the application
        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        //function that calls the return admin UI
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
        //function that shows the Hides the deletion UI
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

        //function that shows the deletion UI
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
        //function that handles the Entity UI
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


            

            foreach (var student in allStudents)
            {
                NewEnityTextBlock.Text = "STUDENT NAME:  " + student.Username + " " + allCourseGrades;
            }
            foreach (var teacher in allTeachers)
            {

                NewEnityTextBlock.Text = "TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours;

            }
            foreach (var cleaner in allCleaners)
            {
                NewEnityTextBlock.Text = "CLEANER NAME: " + cleaner.Username + " " + allCleanerWorkingSchedule;

            }
            foreach (var boardingMember in allBoardingMembers)
            {
                NewEnityTextBlock.Text = "BOARDING MEMBER NAME: " + boardingMember.Username;
            }
        }

        /// method that hides the entity UI
  
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
        /// method that handles the modify entity UI
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
            AddEntityUIButton.Visibility = Visibility.Collapsed;
            DeleteEntityButton.Visibility = Visibility.Collapsed;
            NoDeleteEntityButton.Visibility = Visibility.Collapsed;
            YesDeleteEntityButton.Visibility = Visibility.Collapsed;
            YesModifyEntityButton.Visibility = Visibility.Collapsed;
            NoModifyEntityButton.Visibility = Visibility.Collapsed;
            ModifyEntityUIButton.Visibility = Visibility.Visible;
            ModifyNameTextBox.Visibility = Visibility.Collapsed;
            yesModify = false;
        }
        /// method that hides the admin UI
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
        /// method that shows the admin stats
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

        //admin search bad for modifing user info
        private void SearchModifyEntity(object sender, RoutedEventArgs e)
        {
            string adminRegisterUsername = AdminTextBox.Text;
            string modifyNameBox = ModifyNameTextBox.Text;
            string selectedCategory = ((ComboBoxItem)AdminModifyTypeComboBox.SelectedItem)?.Content.ToString() ?? "";
            switch (selectedCategory)
            {
                
                case "TEACHER":
                    //if any of the result results contains the text from the AdminTextBox field
                    var matchingValuesTeacher = loginRegisterManager.GetAllTeachers().Where(user => user.Username.Contains(adminRegisterUsername));
                    if (matchingValuesTeacher.Any())
                    {
                        //ui handling
                        YesModifyEntityButton.Visibility = Visibility.Visible;
                        NoDeleteEntityButton.Visibility = Visibility.Visible;
                        SearchModifyEntityButton.Visibility = Visibility.Collapsed;
                        AdminTextBox.Visibility = Visibility.Collapsed;
                        ModifyEntityUIButton.Visibility = Visibility.Collapsed;
                        AdminUsername.Text = "New Name";
                        ModifyNameTextBox.Visibility = Visibility.Visible;
                        ModificationConfrimationMessage.Text = "Are you sure you want to  Modify this User: ? " + adminRegisterUsername + " If yes type a new name in the New name input field";
                        ModificationConfrimationMessage.Visibility = Visibility.Visible;
                        NoDeleteEntityButton.Visibility = Visibility.Collapsed;
                        NoModifyEntityButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ShowNoUserFoundModifyMessage();
                    }
                    break;
                case "STUDENT":
                    //if any of the result results contains the text from the AdminTextBox field
                    var matchingValuesStudent = loginRegisterManager.GetAllStudents().Where(user => user.Username.Contains(adminRegisterUsername));
                    if (matchingValuesStudent.Any())
                    {
                        //ui handling
                        YesModifyEntityButton.Visibility = Visibility.Visible;
                        NoDeleteEntityButton.Visibility = Visibility.Visible;
                        SearchModifyEntityButton.Visibility = Visibility.Collapsed;

                        AdminTextBox.Visibility = Visibility.Collapsed;
                        ModifyEntityUIButton.Visibility = Visibility.Collapsed;
                        AdminUsername.Text = "New Name";
                        ModifyNameTextBox.Visibility = Visibility.Visible;
                        ModificationConfrimationMessage.Text = "Are you sure you want to Modify this User: ? " + adminRegisterUsername + " If yes type a new name in the New name input field";
                        ModificationConfrimationMessage.Visibility = Visibility.Visible;
                        NoDeleteEntityButton.Visibility = Visibility.Collapsed;
                        NoModifyEntityButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ShowNoUserFoundModifyMessage();
                    }
                    break;

                case "BOARDING MEMBER":
                    //if any of the result results contains the text from the AdminTextBox field
                    var matchingValuesBoardingMember = loginRegisterManager.GetAllBoardingMembers().Where(user => user.Username.Contains(adminRegisterUsername));
                    if (matchingValuesBoardingMember.Any())
                    {
                        //ui handling
                        YesModifyEntityButton.Visibility = Visibility.Visible;
                        NoDeleteEntityButton.Visibility = Visibility.Visible;
                        SearchModifyEntityButton.Visibility = Visibility.Collapsed;

                        AdminTextBox.Visibility = Visibility.Collapsed;
                        ModifyEntityUIButton.Visibility = Visibility.Collapsed;
                        AdminUsername.Text = "New Name";
                        ModifyNameTextBox.Visibility = Visibility.Visible;
                        ModificationConfrimationMessage.Text = "Are you sure you want to Modify this User: ? " + adminRegisterUsername + " If yes type a new name in the New name input field";
                        ModificationConfrimationMessage.Visibility = Visibility.Visible;
                        NoDeleteEntityButton.Visibility = Visibility.Collapsed;
                        NoModifyEntityButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ShowNoUserFoundModifyMessage();
                    }
                    break;
                //if any of the result results contains the text from the AdminTextBox field
                case "CLEANER":
                    var matchingValuesCleaner = loginRegisterManager.GetAllCleaners().Where(user => user.Username.Contains(adminRegisterUsername));
                    if (matchingValuesCleaner.Any())
                    {
                        //ui handling
                        YesModifyEntityButton.Visibility = Visibility.Visible;
                        NoDeleteEntityButton.Visibility = Visibility.Visible;
                        SearchModifyEntityButton.Visibility = Visibility.Collapsed;

                        AdminTextBox.Visibility = Visibility.Collapsed;
                        ModifyEntityUIButton.Visibility = Visibility.Collapsed;
                        AdminUsername.Text = "New Name";
                        ModifyNameTextBox.Visibility = Visibility.Visible;
                        ModificationConfrimationMessage.Text = "Are you sure you want to Modify this User: ? " + adminRegisterUsername + " If yes type a new name in the New name input field";
                        ModificationConfrimationMessage.Visibility = Visibility.Visible;
                        NoDeleteEntityButton.Visibility = Visibility.Collapsed;
                        NoModifyEntityButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ShowNoUserFoundModifyMessage();
                    }
                    break;
                default:
                    ShowNoUserFoundModifyMessage();
                    break;
            }
            
               
        }

        //handles the ui in case a user hasn't been found in the modification method

        private void ShowNoUserFoundModifyMessage()
        {
            ModificationConfrimationMessage.Text = "No user can be found with the given username";
            ModificationConfrimationMessage.Visibility = Visibility.Visible;
            NoModifyEntityButton.Visibility = Visibility.Collapsed;
            YesModifyEntityButton.Visibility = Visibility.Collapsed;
            SearchModifyEntityButton.Visibility = Visibility.Visible;
            AdminTextBox.Visibility = Visibility.Visible;
            ReturnAdminButton.Visibility = Visibility.Visible;
            SearchModifyEntityButton.Visibility = Visibility.Collapsed;
        }
        //handles the ui in case a user hasn't been found in the deletion method
        private void ShowNoUserFoundDeleteMessage()
        {
            ConfrimDeletionMessage.Text = "No user can be found with the given username";
            ConfrimDeletionMessage.Visibility = Visibility.Visible;
            NoDeleteEntityButton.Visibility = Visibility.Collapsed;
            YesDeleteEntityButton.Visibility = Visibility.Collapsed;
        //    AdminTextBox.Visibility = Visibility.Visible;
         //   delete
            SearchDeleteEntityButton.Visibility = Visibility.Visible;
            ReturnAdminButton.Visibility = Visibility.Visible;
        }
        #endregion UI

        //clear the childen of all stack panels function
        private void ClearStackPanels()
        {
            StudentInfoStackPanel.Children.Clear();
            TeacherInfoStackPanel.Children.Clear();
            CleanerInfoStackPanel.Children.Clear();
            BoardingMemberInfoStackPanel.Children.Clear();
        }

        //ui handling if the player choses yes modify entity
        private void YesModifyEntityUI()
        {
            YesModifyEntityButton.Visibility = Visibility.Collapsed;
            NoDeleteEntityButton.Visibility = Visibility.Collapsed;
            SearchModifyEntityButton.Visibility = Visibility.Collapsed;
            ModifyNameTextBox.Visibility = Visibility.Collapsed;
            AdminModifyTypeComboBox.Visibility = Visibility.Collapsed;
            AdminUsername.Visibility = Visibility.Collapsed;
            ShowAdminUI();
            loginRegisterManager.LoadUserData();
            RegistrationLoginPanel.Visibility = Visibility.Collapsed;
        }
        #region Entities


        //function that handles when we click the yes modify entity button
        private void YesModifyEntity(object sender, RoutedEventArgs e)
        {
            allTeacherWorkingHours = string.Empty;
            allCourseGrades = string.Empty;
            allCleanerWorkingSchedule = string.Empty;
            string selectedCategory = ((ComboBoxItem)AdminModifyTypeComboBox.SelectedItem)?.Content.ToString() ?? "";
            //find matching users based on the adminRegisterUsername in each category
            var matchingValuesTeacher = loginRegisterManager.GetAllTeachers().Where(user => user.Username.Contains(adminRegisterUsername));
            var matchingValuesCleaner = loginRegisterManager.GetAllCleaners().Where(user => user.Username.Contains(adminRegisterUsername));
            var matchingValuesBoardingMember = loginRegisterManager.GetAllBoardingMembers().Where(user => user.Username.Contains(adminRegisterUsername));
            var matchingValuesStudent = loginRegisterManager.GetAllStudents().Where(user => user.Username.Contains(adminRegisterUsername));
            //update the ui and load the user data 
            YesModifyEntityUI();
            loginRegisterManager.LoadUserData();

            //gather all the teacher hours
            foreach (var teacherHours in universityManager.AllTeachers)
            {
                allTeacherWorkingHours += "  They teach Course: " + teacherHours.teachingCourseTitle + ", Working Day: " + teacherHours.teachingWorkingDay + ", Working Hour: " + teacherHours.teacherWorkingHour + "\n";
            }
            //gather all the courses info
            foreach (var course in universityManager.AllCourses)
            {
                allCourseGrades += " Course Name: " + course.CourseTitle + ", Oral Mark: " + course.OralMark + ", Written Mark: " + course.WritingMark + "\n";
            }
            //gather all the clenaer schedule info
            foreach (var cleanerSchedule in universityManager.AllCleaner)
            {
                allCleanerWorkingSchedule += "Working Day: " + cleanerSchedule.workingDay + ", Working Length " + cleanerSchedule.workingLength + " Hours" + "\n";
            }

            //set all the users list as variables from the loginregister manager
            var allStudents = loginRegisterManager.GetAllStudents();
            var allTeachers = loginRegisterManager.GetAllTeachers();
            var allCleaners = loginRegisterManager.GetAllCleaners();
            var allBoardingMembers = loginRegisterManager.GetAllBoardingMembers();

            //clear all the stack panels
            ClearStackPanels();


            string teacherNames = "All Teachers Info:\n";

            //gather all the tachers names and working hours info
            foreach (var teacher in allTeachers)
            {
                teacherNames += " TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours + "\n";
            }
            ModifyEntityUIButton.Click += ModifyEntityUI;


            switch (selectedCategory)
            {
                case "TEACHER":

                    string newTeacherName = ModifyNameTextBox.Text;
                    loginRegisterManager.Register("TEACHER", newTeacherName, matchingValuesTeacher.First().Password);
                    if (matchingValuesTeacher.Any())
                    {
                        //delete the old teacher 
                        loginRegisterManager.DeleteUser("TEACHER", matchingValuesTeacher.First().Username);

                        //update the modification messsage
                        ModificationConfrimationMessage.Text = "The New username now is " + newTeacherName;
                        //update the teachers list and display it
                        var updatedAllTeachers = loginRegisterManager.GetAllTeachers();
                        string updatedTeacherName = "All Teachers Info:\n";
                        foreach (var teacher in updatedAllTeachers)
                        {
                            updatedTeacherName += "TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours + "\n";
                        }
                        TeacherInfoText.Text = updatedTeacherName;

                        //save the updated user data
                        loginRegisterManager.SaveUserData();
                    }
                    break;

                case "STUDENT":
                    string newStudentName = ModifyNameTextBox.Text;
                    loginRegisterManager.Register("STUDENT", newStudentName, matchingValuesStudent.First().Password);
                    if (matchingValuesStudent.Any())
                    {
                        //delete the old student
                        loginRegisterManager.DeleteUser("STUDENT", matchingValuesStudent.First().Username);

                        //update the modification messsage
                        ModificationConfrimationMessage.Text = "The New username now is " + newStudentName;

                        //update the students list and display it
                        var updatedAllStudents = loginRegisterManager.GetAllStudents();

                        string updatedStudentName = "All Students Info:\n";
                        foreach (var student in updatedAllStudents)
                        {
                            updatedStudentName += "STUDENT NAME:  " + student.Username + allCourseGrades + "\n";
                        }
                        StudentInfoText.Text = updatedStudentName;

                        //save the updated user data
                        loginRegisterManager.SaveUserData();
                    }
                    break;
                case "CLEANER":
                    string newCleanerName = ModifyNameTextBox.Text;
                    loginRegisterManager.Register("CLEANER", newCleanerName, matchingValuesCleaner.First().Password);
                    if (matchingValuesCleaner.Any())
                    {
                        //delete the old cleaner
                        loginRegisterManager.DeleteUser("CLEANER", matchingValuesCleaner.First().Username);

                        //update the modification messsage
                        ModificationConfrimationMessage.Text = "The New username now is " + newCleanerName;


                        //update the cleaners list and display it
                        var updatedAllCleaners = loginRegisterManager.GetAllCleaners();


                        string updatedCleanerInfo = "All Cleaners Info:\n";
                        foreach (var cleaner in updatedAllCleaners)
                        {
                            updatedCleanerInfo += "CLEANER NAME:  " + cleaner.Username + "  " + allCleanerWorkingSchedule + "\n";
                        }
                        CleanerInfoText.Text = updatedCleanerInfo;

                        //save the updated user data
                        loginRegisterManager.SaveUserData();
                    }
                    break;

                case "BOARDING MEMBER":
                    string newBoardingMemberName = ModifyNameTextBox.Text;
                    loginRegisterManager.Register("BOARDING MEMBER", newBoardingMemberName, matchingValuesBoardingMember.First().Password);
                    if (matchingValuesBoardingMember.Any())
                    {
                        //delete the old boarding member
                        loginRegisterManager.DeleteUser("BOARDING MEMBER", matchingValuesBoardingMember.First().Username);

                        //update the modification messsage
                        ModificationConfrimationMessage.Text = "The New username now is " + newBoardingMemberName;

                        //update the boarding members list and display it
                        var updatedAllBoardingMembers = loginRegisterManager.GetAllBoardingMembers();

                        string updatedBoardingMemberInfo = "All Boarding Members Info:\n";
                        foreach (var boardingMember in updatedAllBoardingMembers)
                        {
                            updatedBoardingMemberInfo += "BOARDING MEMBER NAME:  " + boardingMember.Username + "\n";
                        }
                        BoardingMemberInfoText.Text = updatedBoardingMemberInfo;

                        //save the updated user data
                        loginRegisterManager.SaveUserData();
                    }
                    break;


            }
        }

        //no modify entity function that shows the admin ui and the no modify entity ui
        private void NoModifyEntity(object sender, RoutedEventArgs e)
        {
            yesModify = false;
            noModify = true;
            ShowAdminUI();
            NoModifyEntityUI();
            ModifyEntityUIButton.Click += ModifyEntityUI;
            ModificationConfrimationMessage.Visibility = Visibility.Collapsed;
        }
        //handles the ui in case we chose no in the no modify button
        private void NoModifyEntityUI()
        {
            YesModifyEntityButton.Visibility = Visibility.Collapsed;
            NoDeleteEntityButton.Visibility = Visibility.Collapsed;
            SearchModifyEntityButton.Visibility = Visibility.Collapsed;
            ModifyNameTextBox.Visibility = Visibility.Collapsed;
            AdminModifyTypeComboBox.Visibility = Visibility.Collapsed;
            AdminUsername.Visibility = Visibility.Collapsed;
        }

        //yes delete entity button
        private void YesDeleteEntity(object sender, RoutedEventArgs e)
        {
            yesDeleteEntity = true;
            noDeleteEntity = false;
            Debug.WriteLine("Yes deleteEntity is " + yesDeleteEntity);
            ExecuteDeletionIfConfirmed();
        }

        //no delete entity button
        private void NoDeleteEntity(object sender, RoutedEventArgs e)
        {
            noDeleteEntity = true;
            yesDeleteEntity = false;
            ConfrimDeletionMessage.Visibility = Visibility.Collapsed;
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

        private void SearchNameDeleteEntity(object sender, RoutedEventArgs e)
        {
            string deleteUserInputFieldUsername = AdminDeleteTextBox.Text;
            string selectedCategory = ((ComboBoxItem)AdminDeleteTypeComboBox.SelectedItem)?.Content.ToString() ?? "";
            switch (selectedCategory)
            {
                case "TEACHER":
                    var matchingValuesTeacher = loginRegisterManager.GetAllTeachers().Where(user => user.Username.Contains(deleteUserInputFieldUsername));
                    if (matchingValuesTeacher.Any())
                    {
                        //ui hadling for the deletion of the user
                        YesDeleteEntityButton.Visibility = Visibility.Visible;
                        NoDeleteEntityButton.Visibility = Visibility.Visible;
                        SearchDeleteEntityButton.Visibility = Visibility.Collapsed;
                        ConfrimDeletionMessage.Text = "Are you sure you want to delete this User?";
                        ConfrimDeletionMessage.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        //in case a mistake happens show a message
                        ShowNoUserFoundDeleteMessage();
                    }
                    break;

                case "STUDENT":
                    var matchingValuesStudent = loginRegisterManager.GetAllStudents().Where(user => user.Username.Contains(deleteUserInputFieldUsername));
                    if (matchingValuesStudent.Any())
                    {
                        //ui hadling for the deletion of the user
                        YesDeleteEntityButton.Visibility = Visibility.Visible;
                        NoDeleteEntityButton.Visibility = Visibility.Visible;
                        SearchDeleteEntityButton.Visibility = Visibility.Collapsed;
                        ConfrimDeletionMessage.Text = "Are you sure you want to delete this User?";
                        ConfrimDeletionMessage.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        //in case a mistake happens show a message
                        ShowNoUserFoundDeleteMessage();
                    }
                    break;


                case "CLEANER":
                    var matchingValuesCleaner = loginRegisterManager.GetAllCleaners().Where(user => user.Username.Contains(deleteUserInputFieldUsername));
                    if (matchingValuesCleaner.Any())
                    {
                        //ui hadling for the deletion of the user
                        YesDeleteEntityButton.Visibility = Visibility.Visible;
                        NoDeleteEntityButton.Visibility = Visibility.Visible;
                        SearchDeleteEntityButton.Visibility = Visibility.Collapsed;
                        ConfrimDeletionMessage.Text = "Are you sure you want to delete this User?";
                        ConfrimDeletionMessage.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        //in case a mistake happens show a message
                        ShowNoUserFoundDeleteMessage();
                    }
                    break;

                case "BOARDING MEMBER":
                    var matchingValuesBoardingMember = loginRegisterManager.GetAllBoardingMembers().Where(user => user.Username.Contains(deleteUserInputFieldUsername));
                    if (matchingValuesBoardingMember.Any())
                    {
                        //ui hadling for the deletion of the user
                        YesDeleteEntityButton.Visibility = Visibility.Visible;
                        NoDeleteEntityButton.Visibility = Visibility.Visible;
                        SearchDeleteEntityButton.Visibility = Visibility.Collapsed;
                        ConfrimDeletionMessage.Text = "Are you sure you want to delete this User?";
                        ConfrimDeletionMessage.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        //in case a mistake happens show a message
                        ShowNoUserFoundDeleteMessage();
                    }
                    break;
                default:
                    //in case a mistake happens show a message
                    ShowNoUserFoundDeleteMessage();
                    break;

            }
        }


        //add new entity function
        private void AddNewEntity(object sender, RoutedEventArgs e)
        {
            //load the data from json
            loginRegisterManager.LoadUserData();
            RegistrationLoginPanel.Visibility = Visibility.Collapsed;
            string allCourseGrades = string.Empty;
            string allTeacherWorkingHours = string.Empty;
            string allCleanerWorkingSchedule = string.Empty;

            //gain all teachers working hours information
            foreach (var teacherHours in universityManager.AllTeachers)
            {
                allTeacherWorkingHours += "  They teach Course: " + teacherHours.teachingCourseTitle + ", Working Day: " + teacherHours.teachingWorkingDay + ", Working Hour: " + teacherHours.teacherWorkingHour + "\n";
            }
            //gain all teachers working hours information
            foreach (var course in universityManager.AllCourses)
            {
                allCourseGrades += " Course Name: " + course.CourseTitle + ", Oral Mark: " + course.OralMark + ", Written Mark: " + course.WritingMark + "\n";
            }
            //gain all teachers working hours information
            foreach (var cleanerSchedule in universityManager.AllCleaner)
            {
                allCleanerWorkingSchedule += "Working Day: " + cleanerSchedule.workingDay + ", Working Length " + cleanerSchedule.workingLength + " Hours" + "\n";
            }

            //store all the lists of the loginregister manager in variables
            var allStudents = loginRegisterManager.GetAllStudents();
            var allTeachers = loginRegisterManager.GetAllTeachers();
            var allCleaners = loginRegisterManager.GetAllCleaners();
            var allBoardingMembers = loginRegisterManager.GetAllBoardingMembers();

            ClearStackPanels();


            string adminRegisterUsername = AdminTextBox.Text;
            string adminRegisterPassword = AdminPasswordBox.Password;
            string selectedCategory = ((ComboBoxItem)AdminTypeComboBox.SelectedItem)?.Content.ToString();
            string teacherNames = "All Teachers Info:\n";
            foreach (var teacher in allTeachers)
            {
                teacherNames += " TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours + "\n";
            }
            switch (selectedCategory)
            {
                case "NEW TEACHER":
                    //register a new teacher
                    loginRegisterManager.Register("TEACHER", adminRegisterUsername, adminRegisterPassword);

                    //create a new textblock and disaplay the new added teacher
                    TextBlock newTeacherTextBlock = new TextBlock
                    {
                        Text = "New Added TEACHER:  " + adminRegisterUsername,
                    };
                    TeacherInfoStackPanel.Children.Insert(0, newTeacherTextBlock);
                    TeacherInfoStackPanel.UpdateLayout();


                    var updatedAllTeachers = loginRegisterManager.GetAllTeachers();
                    string updatedTeacherName = "All Teachers Info:\n";
                    foreach (var teacher in updatedAllTeachers)
                    {
                        updatedTeacherName += "TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours + "\n";
                    }
                    TeacherInfoText.Text = updatedTeacherName;
                    //save the user data
                    loginRegisterManager.SaveUserData();

                    break;
            
                case "NEW BOARDING MEMBER":
                    //register a new boarding member
                    loginRegisterManager.Register("BOARDING MEMBER", adminRegisterUsername, adminRegisterPassword);
                    //create a new textblock and disaplay the new added boarding member
                    TextBlock newBoardingMemberTextBlock = new TextBlock
                {
                    Text = "New BOARDING MEMBER:  " + adminRegisterUsername,
                };
                    //instert it to the stack panel at the the first position
                    BoardingMemberInfoStackPanel.Children.Insert(0, newBoardingMemberTextBlock);
                BoardingMemberInfoStackPanel.UpdateLayout();

                var updatedAllBoardingMembers = loginRegisterManager.GetAllBoardingMembers();
                string updatedBoardingMemberName = "All Boarding Name Info:\n";

                foreach (var boardingMember in updatedAllBoardingMembers)
                {
                    updatedBoardingMemberName += "BOARDING MEMBER NAME: " + boardingMember.Username + "\n";
                }

                BoardingMemberInfoText.Text = updatedBoardingMemberName;
                loginRegisterManager.SaveUserData();
                break;

            case "NEW CLEANER":
                    //register a new cleaner
                    loginRegisterManager.Register("CLEANER", adminRegisterUsername, adminRegisterPassword);
                    //create a new textblock and disaplay the new added cleaner
                    TextBlock newCleanerTextBlock = new TextBlock
                {
                    Text = "New Added CLEANER:  " + adminRegisterUsername,
                };
                    //instert it to the stack panel at the the first position
                CleanerInfoStackPanel.Children.Insert(0, newCleanerTextBlock);
                CleanerInfoStackPanel.UpdateLayout();

                var updatedAllCleaners = loginRegisterManager.GetAllCleaners();
                string updatedCleanerName = "All Cleaner Name Info:\n";

                foreach (var cleaner in updatedAllCleaners)
                {
                    updatedCleanerName += "CLEANER NAME: " + cleaner.Username + allCleanerWorkingSchedule + "\n";
                }

                CleanerInfoText.Text = updatedCleanerName;
                loginRegisterManager.SaveUserData();

                break;

            case "NEW STUDENT":
                    //register a new student
                loginRegisterManager.Register("STUDENT", adminRegisterUsername, adminRegisterPassword);
                    //create a new textblock and disaplay the new added cleaner
                    TextBlock newStudentTextBlock = new TextBlock
                {
                    Text = "New Added STUDENT:  " + adminRegisterUsername,
                };
                    //instert it to the stack panel at the the first position
                    StudentInfoStackPanel.Children.Insert(0, newStudentTextBlock);
                StudentInfoStackPanel.UpdateLayout();

                var updateallstudents = loginRegisterManager.GetAllStudents();
                string updatedstudentName = "All Student Name Info:\n";

                foreach (var student in updateallstudents)
                {
                    updatedstudentName += "STUDENT NAME: " + student.Username + allCourseGrades + "\n";
                }

                StudentInfoText.Text = updatedstudentName;
                loginRegisterManager.SaveUserData();
                break;


            case "NEW COURSE":

                universityManager.AddStudentCourse(adminRegisterUsername, 0, 0);
                break;
            }
        }



        //handle the ui in case we delete an entity
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
            ConfrimDeletionMessage.Visibility = Visibility.Collapsed;
        }
        private void ExecuteDeletionIfConfirmed()
        {
            RegistrationLoginPanel.Visibility = Visibility.Collapsed;
            string allCourseGrades = string.Empty;
            string allTeacherWorkingHours = string.Empty;
            string allCleanerWorkingSchedule = string.Empty;
            string deletedInputField = AdminDeleteTextBox.Text;
            var matchingValuesTeacher = loginRegisterManager.GetAllTeachers().Where(user => user.Username.Contains(deletedInputField));
            var matchingValuesCleaner = loginRegisterManager.GetAllCleaners().Where(user => user.Username.Contains(deletedInputField));
            var matchingValuesBoardingMember = loginRegisterManager.GetAllBoardingMembers().Where(user => user.Username.Contains(deletedInputField));
            var matchingValuesStudent = loginRegisterManager.GetAllStudents().Where(user => user.Username.Contains(deletedInputField));
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

            //get all the loginregister manager lists info
            var allStudents = loginRegisterManager.GetAllStudents().ToList();
            allStudents.Reverse();

            var allTeachers = loginRegisterManager.GetAllTeachers().ToList();
            allTeachers.Reverse();

            var allCleaners = loginRegisterManager.GetAllCleaners().ToList();
            allCleaners.Reverse();

            var allBoardingMembers = loginRegisterManager.GetAllBoardingMembers().ToList();
            allBoardingMembers.Reverse();


            ClearStackPanels();
            //if the yesdeleteentity is true
            if (yesDeleteEntity)
            {
                string deleteUserInputFieldUsername = deletedInputField; ;
                string selectedCategory = ((ComboBoxItem)AdminDeleteTypeComboBox.SelectedItem)?.Content.ToString() ?? "";
                switch (selectedCategory)
                {
                    case "TEACHER":

                    if (matchingValuesTeacher.Any())
                    {
                            //delete the teacher 
                        loginRegisterManager.DeleteUser("TEACHER", matchingValuesTeacher.First().Username);
                            //update and display all the teachers info
                        var updatedAllTeachers = loginRegisterManager.GetAllTeachers();
                        string updatedDeletedTeacherName = "All Teachers Info:\n";
                        foreach (var teacher in updatedAllTeachers)
                        {
                            updatedDeletedTeacherName += "TEACHER NAME:  " + teacher.Username + allTeacherWorkingHours + "\n";
                        }
                        
                        TeacherInfoText.Text = updatedDeletedTeacherName;
                        ConfrimDeletionMessage.Text = "Deleted User";
                            //save the updated data
                        loginRegisterManager.SaveUserData();
                    }
                    break;

                    case "CLEANER":
                    if (matchingValuesCleaner.Any())
                    {
                            //delete the cleaner
                            loginRegisterManager.DeleteUser("CLEANER", matchingValuesCleaner.First().Username);

                            //update and display all the cleaners info
                            var updatedAllCleaners = loginRegisterManager.GetAllCleaners();

                        string updatedCleanerInfo = "All Cleaners Info:\n";

                        foreach (var cleaner in updatedAllCleaners)
                        {
                            updatedCleanerInfo += "CLEANER NAME:  " + cleaner.Username + allCleanerWorkingSchedule + "\n";
                        }
                        CleanerInfoText.Text = updatedCleanerInfo;


                        ConfrimDeletionMessage.Text = "Deleted User";

                            //save the updated data
                            loginRegisterManager.SaveUserData();
                    }
                        break;



                    case "STUDENT":
                    if (matchingValuesStudent.Any())
                    {
                            //delete the student
                            loginRegisterManager.DeleteUser("STUDENT", matchingValuesStudent.First().Username);


                            //update and display all the students info
                            var updatedAllStudents = loginRegisterManager.GetAllStudents();


                        string updatedStudentInfo = "All Students Info:\n";
                        foreach (var student in updatedAllStudents)
                        {
                            updatedStudentInfo += "STUDENT NAME:  " + student.Username + allCourseGrades + "\n";
                        }
                        StudentInfoText.Text = updatedStudentInfo;

                        ConfrimDeletionMessage.Text = "Deleted User";

                            //save the updated data
                            loginRegisterManager.SaveUserData();
                    }
                        break;


                    case "BOARDING MEMBER":
                    if (matchingValuesBoardingMember.Any())
                    {
                            //delete the boarding member
                            loginRegisterManager.DeleteUser("BOARDING MEMBER", matchingValuesBoardingMember.First().Username);


                            //update and display all the boarding members info
                            var updatedAllBoardingMembers = loginRegisterManager.GetAllBoardingMembers();


                        string updatedBoardingMemberInfo = "All Boarding Members Info:\n";

                        foreach (var boardingMember in updatedAllBoardingMembers)
                        {
                            updatedBoardingMemberInfo += "BOARDING MEMBER NAME:  " + boardingMember.Username + "\n";
                        }
                        BoardingMemberInfoText.Text = updatedBoardingMemberInfo;


                        ConfrimDeletionMessage.Text = "Deleted User";

                            //save the updated data
                            loginRegisterManager.SaveUserData();
                    }
                        break;
                }
                //if the nodelete entity flag is true then hide the deletion UI
                if (noDeleteEntity)
                {
                    HideDeletionUI();
                }
            }

        }
    }
}
        #endregion Entities
    