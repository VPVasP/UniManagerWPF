   M S B A M L   `   `   `  ����O  KEvaluationProjectWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:EvaluationProjectWPFEvaluationProjectWPF  5   	   %    EvaluationProjectWPF.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:EvaluationProjectWPF     5   	     � Title$  UniversityManager��5      $	��450��6&   $	��800��63    � WindowState$ 	Maximized=�6?   .��5
      � ��5   
   �� 5   
    �� Color$ 	CadetBlue��6   .+�5   
   �� -   5         ��Name$ 
AdminPanel��5      $��Center=�6)   $��Center=�6F   $��10q�6a   .+�5      �� -   5         ��Name$ AdminTypeComboBox��5      $��5q�62   $	��218��6=   .@�5      �� $��NEW TEACHER��6   �� 5      $��NEW BOARDING MEMBER��6   �� 5      $��NEW CLEANER��6   �� 5      $��NEW STUDENT��6   �� 5      $��
NEW COURSE��6   5      �� 5      -   5        $ AdminModifyTypeComboBox��5      $��5q�68   $	��220��6C   .@�5      �� $��TEACHER��6   �� 5      $��BOARDING MEMBER��6   �� 5      $��CLEANER��6   �� 5      $��STUDENT��6   �� 5      $��COURSE��6   5      �� 5      -   5        $ AdminDeleteTypeComboBox��5      $��5q�68   $	��220��6C   .@�5      �� $��TEACHER��6   �� 5       $��BOARDING MEMBER��6   �� 5!      $��CLEANER��6   �� 5"      $��STUDENT��6   �� 5#      $��COURSE��6   5$      �� 5&      -   5         ��Name$ AdminUsername��5&      $��Set new Username:��6/   
�������6H   �� 5'      -   5         ��Name$ AdminTextBox��5'      $��5q�6,   $	��220��67   �� 5(      -   5        $ AdminNewUsernameBox��5(      $��5q�63   $	��220��6>   �� 5)      -   5        $ AdminPassword��5)      $��Set new Password:��6/   
�������6H   G� 5*      -	   5         G�Name$ AdminPasswordBox��5*      $��5q�64   �� 5.      -
   5        $ WrongInfoWarning��5.      $y�	Collapsed=�62   $
��Wrap=�6I   $��Center=�6]   $��Center=�6z   $��-1250, -600, 10, 0q�6�   
�������6�   �� 5/      -   5        $ DeleteEntityUsername��5/      $��Delete using Username:��66   
�������6T   �� 51      -   5        $ AdminDeleteTextBox��51      $��5q�62   $	��220��6=   �� 53      -   5         ��Name$ AddEntityUIButton��53      $��	AddEntity��61   $��5q�6Y   �� 55      -   5        $ ModifyEntityUIButton��55      $��ModifyEntity��64   $��5q�6b   �� 57      -   5        $ ModifyNameTextBox��57      $��5q�61   $	��220��6<   �� 59      -   5        $ AddNewEntityButton��59      $��AddNewEntity��62   $��5q�6^   �� 5;      -   5        $ DeleteEntityButton��5;      $��DeleteEntity��62   $��5q�6^   �� 5=      -   5        $ SearchModifyEntityButton��5=      $��SearchModifyEntity��68   $��5q�6p   �� 5?      -   5        $ SearchDeleteEntityButton��5?      $ ��SearchUsernameDeleteEntity��68   $��5q�6|   �� 5A      -   5        $ YesDeleteEntityButton��5A      $��YesDeleteEntity��65   $��5q�6g   �� 5C      -   5        $ NoDeleteEntityButton��5C      $��NoDeleteEntity��64   $��5q�6d   �� 5F      -   5        $ YesModifyEntityButton��5F      $��YesModifyEntity��65   $��5q�6g   �� 5H      -   5        $ NoModifyEntityButton��5H      $��NoModifyEntity��64   $��5q�6d   �� 5I      -   5        $ ReturnAdminButton��5I      $��Return��61   $��5q�6V   �� 5K      -   5        $ ExitAdminButton��5K      $
��EXIT��6/   $��5q�6V   �� 5M      -   5        $% ModificationConfrimationMessage��5M      $y�	Collapsed=�6A   $
��Wrap=�6X   $��Center=�6l   $��Center=�6�   $��10, 100, 10, 0q�6�   
����  �6�   �� 5O      -   5        $  ConfrimModificationMessage��5O      $y�	Collapsed=�6<   $
��Wrap=�6S   $��Center=�6g   $��Center=�6�   $��10, 100, 10, 0q�6�   
��� ���6�   �� 5Q      -   5        $ ConfrimDeletionMessage��5Q      $y�	Collapsed=�68   $
��Wrap=�6O   $��Center=�6c   $��Center=�6�   $��10, 100, 10, 0q�6�   
��� ���6�   5R      �� 5T   
   -   5        $ RegistrationLoginPanel��5T      $��Center=�65   $��Center=�6R   $��10q�6m   .+�5V   
   �� -   5        $ UserTypeComboBox��5V      $��5q�6-   $	��218��68   .@�5W      �� $��ADMIN��6   �� 5X      $��TEACHER��6   �� 5Y      $��BOARDING MEMBER��6   �� 5Z      $��CLEANER��6   �� 5[      $��STUDENT��6   5\      �� 5_      -   5        $ GeneralUsername��5_      $��	Username:��62   
�������6C   �� 5`   
   -    5        $ UsernameTextBox��5`      $��5q�6+   $	��220��66   �� 5c      -!   5        $ Password��5c      $��	Password:��6*   
�������6;   G� 5d   
   -"   5        $ PasswordBox��5d      $��5q�6+   �� 5g      -#   5        $ RegisterButton��5g      $��Register��6-   $��5q�6U   �� 5h      -$   5        $ LoginButton��5h      $��Login��6*   $��5q�6L   �� 5i      -%   5        $ 
ExitButton��5i      $
��Exit��6*   $��5q�6Q   5k      �� 5m   
   -&   5        $ RegistrationMessage��5m      $y�	Collapsed=�61   $
��Wrap=�6H   $��Center=�6\   $��Center=�6y   $��10, -950, 10, 0q�6�   
��� ���6�   �� 5n   
   -'   5        $ LoginMessage��5n      $y�	Collapsed=�6*   $
��Wrap=�6A   $��Center=�6U   $��Center=�6r   $��10, -950,0, 0q�6�   
��� ���6�   �� 5o   	   -(   5        $ UserExistsMessage��5o      $y�	Collapsed=�6.   $
��Wrap=�6E   $��Center=�6Y   $��Center=�6v   $��10, 500, 10, 0q�6�   
���  ��$��45(�6�   �� 5p   
   -)   5        $ StudentGrades��5p      $y�	Collapsed=�6+   $
��Wrap=�6B   $��Center=�6V   $��Center=�6s   $��10, 500, 10, 0q�6�   
������6�   �� 5q   
   -*   5        $ CleanerDates��5q      $y�	Collapsed=�6*   $
��Wrap=�6A   $��Center=�6U   $��Center=�6r   $��10, 500, 10, 0q�6�   
�������6�   �� 5r   
   -+   5        $ TeacherTeachingDates��5r      $y�	Collapsed=�62   $
��Wrap=�6I   $��Center=�6]   $��Center=�6z   $��10, 500, 10, 0q�6�   
�������6�   �� 5t   
   -,   5        $ StudentInfoText��5t      $y�	Collapsed=�6-   $
��Wrap=�6D   $��Center=�6X   $��Center=�6u   $��-1250, -600, 10, 0q�6�   
�������6�   �� 5u   
   --   5        $ TeacherInfoText��5u      $y�	Collapsed=�6-   $
��Wrap=�6D   $��Center=�6X   $��Center=�6u   $��1150,-00, 10, 0q�6�   
�������6�   �� 5v   
   -.   5        $ CleanerInfoText��5v      $y�	Collapsed=�6-   $
��Wrap=�6D   $��Center=�6X   $��Center=�6u   $��00, -600, 10, 0q�6�   
�������6�   �� 5w   
   -/   5        $ BoardingMemberInfoText��5w      $y�	Collapsed=�64   $
��Wrap=�6K   $��Center=�6_   $��Center=�6|   $��-1350, 600, 10, 0q�6�   
�������6�   �� 5x   
   -0   5        $ StudentInfoStackPanel��5x      $��10q�64   $��Stretch=�6@   $	��Top=�6^   �� 5y   
   -1   5        $ TeacherInfoStackPanel��5y      $��10q�64   $��Stretch=�6@   $	��Top=�6^   �� 5z   
   -2   5        $ CleanerInfoStackPanel��5z      $��10q�64   $��Stretch=�6@   $	��Top=�6^   �� 5|   
   -3   5        $" BoardingMemberInfoStackPanel��5|      $��10q�6;   $��Stretch=�6G   $	��Top=�6e   �� 5   
   -4   5        $ ExitButtonUserPanel��5      $��Center=�62   $��Center=�6O   $��20q�6j   .+�5�      �� -5   5        $ UserExitButton��5�      $
��Exit��6-   $��50q�6T   $	��150��6`   $��50��6l   5�      5�      5�      