# UniManage
Created a University Login/Registration system with the data being stored locally using JSON. The user can register/login as an Admin or as a User(Student, Cleaner, Boarding Member).

## Project Features
- The user can register/sign in with their own credentials depending on whether they are an admin or a user, and the information is stored locally using JSON.
- The program proceeds only after a successful login or registration; otherwise, it does not advance.
- The admin can view all employees within the program via the UI.
- The student can only see the courses they are enrolled in and their grades.
- The teacher can only see the courses they teach and the times they teach them.
- The cleaner can see which days and what hours they work.
- The boarding member can only see the students and their grades, and the teachers, the courses they teach, and the times they teach them.
- The admin can add a new entry, and the entry is added to the UI during runtime if a check has been made to ensure the user's name exists first.
- The admin can modify a user's name, and it is updated in the UI during runtime if a check has been made to ensure the user's name exists first.
- The admin can delete a user, and before finalizing the action, a message appears asking if they want to delete the user, ensuring a check has been made to verify the user's name exists. If a user is deleted, the UI is updated at that moment.

## How to Install
1. Clone this GitHub Repository to your computer.
2. Open the Project .sln file in Visual Studio and run it.





