﻿using System.Diagnostics;
using System.IO;
using System.Security.Policy;
using Newtonsoft.Json;

namespace EvaluationProjectWPF
{
    internal class LoginRegisterManager
    {
        private UserInfoList userInfoList;
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
        private void SaveUserData()
        {
            string json = JsonConvert.SerializeObject(userInfoList, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(userSavedFile, json);
        }

        // load data from JSON
        private void LoadUserData()
        {
            if (File.Exists(userSavedFile))
            {
                string json = File.ReadAllText(userSavedFile);
                userInfoList = JsonConvert.DeserializeObject<UserInfoList>(json);
            }
        }

        // the user info such as username and password
        public class UserInfo
        {
            public required string Category { get; set; }
            public required string Username { get; set; }
            public required string Password { get; set; }

        }

        // a list of all the userinfo
        public class UserInfoList
        {
            public List<UserInfo> UsersInfoList { get; set; } = new List<UserInfo>();
        }
    }
}
