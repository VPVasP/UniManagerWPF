using System.Diagnostics;
using System.IO;
using System.Security.Policy;
using Newtonsoft.Json;
using static EvaluationProjectWPF.LoginRegisterManager;

namespace EvaluationProjectWPF
{
    internal class LoginRegisterManager
    {
        public UserInfoList userInfoList;
        private string userSavedFile = "userData.json";
        public LoginRegisterManager()
        {
            userInfoList = new UserInfoList();
            LoadUserData();
        }

        // register method
        public void Register(string category, string registerUsername, string registerPassword)
        {
            if (DoesUserExistRegister(category,registerUsername))
            {
                Debug.WriteLine("Username already exists");
            }
            else
            {
                UserInfo newUser = new UserInfo()
                {
                    Category = category,
                    Username = registerUsername,
                    Password = registerPassword,
                };

                Debug.WriteLine("Registered New User, Also Welcome " + registerUsername);
                userInfoList.UsersInfoList.Add(newUser);
                SaveUserData();
            }
        }

        // login method
        public void Login(string category, string loginUsername, string loginPassword)
        {
            if (DoesUserExistLogin(category,loginUsername, loginPassword))
            {
                Debug.WriteLine("You are now logged in " + loginUsername);
            }
            else
            {
                Debug.WriteLine("Wrong Username or Password");
            }
        }
        public void Logout(string username)
        {
            Debug.WriteLine("Logged out " + username);
        }
        // checking if a username already exists in registration method
        public bool DoesUserExistRegister(string category,string username)
        {
            return userInfoList.UsersInfoList.Exists(user => user.Username == username  &&user.Category == category);
        }

        // checking if a username already exists in login method
        public bool DoesUserExistLogin(string category, string username, string password)
        {
            return userInfoList.UsersInfoList.Exists(user =>user.Category ==category && user.Username == username && user.Password == password);
        }

        // save data to JSON
        public void SaveUserData()
        {
            string json = JsonConvert.SerializeObject(userInfoList, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(userSavedFile, json);
        }

        // load data from JSON
        public void LoadUserData()
        {
            if (File.Exists(userSavedFile))
            {
                string json = File.ReadAllText(userSavedFile);
                userInfoList = JsonConvert.DeserializeObject<UserInfoList>(json);
            }
        }

        public void DeleteUser(string category, string username)
        {
            var userToDelete = userInfoList.UsersInfoList.FirstOrDefault(user => user.Category == category && user.Username == username);
            if (userToDelete != null)
            {
                userInfoList.UsersInfoList.Remove(userToDelete);
                SaveUserData();
                Debug.WriteLine("User deleted: " + username);
            }
            else
            {
                Debug.WriteLine("User not found: " + username);
            }
        }
        // the user info such as username and password
        public class UserInfo
        {
            public string Category { get; set; }
            public  string Username { get; set; }
            public  string Password { get; set; }

        }

        // a list of all the userinfo
        public class UserInfoList
        {
            public List<UserInfo> UsersInfoList { get; set; } = new List<UserInfo>();
        }
        public List<UserInfo> GetAllStudents()
        {
            return userInfoList.UsersInfoList.Where(user => user.Category == "STUDENT").ToList();
        }
        public List<UserInfo> GetAllTeachers()
        {
            return userInfoList.UsersInfoList.Where(user => user.Category == "TEACHER").ToList();

        }
            public List<UserInfo> GetAllCleaners()
            {
                return userInfoList.UsersInfoList.Where(user => user.Category == "CLEANER").ToList();
            }

        public List<UserInfo> GetAllBoardingMembers()
        {
            return userInfoList.UsersInfoList.Where(user => user.Category == "BOARDING MEMBER").ToList();
        }
    }
}
