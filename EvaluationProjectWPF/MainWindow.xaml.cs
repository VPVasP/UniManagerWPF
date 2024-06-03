﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace EvaluationProjectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginRegisterManager manager;
        private UniversityManager universityManager;
        private UniversityManager.BoardingMember boardingMember;
        private UniversityManager.Cleaner cleaner;
        private UniversityManager.Teacher teacher;
        public MainWindow()
        {
            InitializeComponent();
            manager = new LoginRegisterManager();
            universityManager = new UniversityManager();
            cleaner = new UniversityManager.Cleaner();
            boardingMember = new UniversityManager.BoardingMember();
            teacher = new UniversityManager.Teacher();
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

            if (!manager.DoesUserExistRegister(selectedCategoryAdmin, registerUsername) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "ADMIN") 
            {
               manager.Register(selectedCategoryAdmin,registerUsername, registerPassword);

                //hide the registration and login panel
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;

                //display the registration message 
                RegistrationMessage.Text = "Welcome, ADMIN " + registerUsername + "!";
                RegistrationMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;
  
            }
            if (!manager.DoesUserExistRegister(selectedCategoryTeacher, registerUsername) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "TEACHER")
            {
                manager.Register(selectedCategoryTeacher, registerUsername, registerPassword);
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
            if (!manager.DoesUserExistRegister(selectedCategoryBoardingMember, registerUsername) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "BOARDING MEMBER")
            {
                manager.Register(selectedCategoryBoardingMember, registerUsername, registerPassword);

                //hide the registration and login panel
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;

                //display the registration message 
                RegistrationMessage.Text = "Welcome, BOARDING MEMBER " + registerUsername + "!";
                RegistrationMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;

            }
            if (!manager.DoesUserExistRegister(selectedCategoryCleaner, registerUsername) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "CLEANER")
            {
                manager.Register(selectedCategoryCleaner, registerUsername, registerPassword);
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
            if (!manager.DoesUserExistRegister(selectedCategoryStudent, registerUsername) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "STUDENT")
            {
                manager.Register(selectedCategoryStudent,registerUsername, registerPassword);
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
            
            if (manager.DoesUserExistLogin(selectedCategoryTeacher,loginUsername,loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "TEACHER")
            {
                AddTeacherTeachingCoursesAndHours();
                manager.Login(selectedCategoryTeacher,loginUsername, loginPassword);
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
            if (manager.DoesUserExistLogin(selectedCategoryBoardingMember,loginUsername, loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "BOARDING MEMBER")
            {
                manager.Login(selectedCategoryBoardingMember, loginUsername, loginPassword); 
                RegistrationLoginPanel.Visibility = Visibility.Collapsed;
                LoginMessage.Text = "You are now logged in BOARDING MEMBER " + loginUsername + " Welcome to the application";
                LoginMessage.Visibility = Visibility.Visible;
                UserExistsMessage.Visibility = Visibility.Collapsed;

            }

            if (manager.DoesUserExistLogin(selectedCategoryCleaner, loginUsername, loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "CLEANER")
            {
                manager.Login(selectedCategoryCleaner, loginUsername, loginPassword);
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

            if (manager.DoesUserExistLogin(selectedCategoryStudent,loginUsername, loginPassword) && UserTypeComboBox.SelectedItem != null && ((ComboBoxItem)UserTypeComboBox.SelectedItem).Content.ToString() == "STUDENT")
            {
                
                manager.Login(selectedCategoryTeacher, loginUsername, loginPassword);
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
      

        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}