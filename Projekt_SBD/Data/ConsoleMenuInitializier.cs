using Projekt_SBD.ConsoleUserInterface;
using Projekt_SBD.ConsoleUserInterface.FormScreen;
using Projekt_SBD.ConsoleUserInterface.MenuScreen;
using Projekt_SBD.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Data
{
    public static class ConsoleMenuInitializer
    {
        public static List<IConsoleScreen> Initialize()
        {
            List<IConsoleScreen> screens = new List<IConsoleScreen>();

            // Main menu
            Menu mainMenu = new Menu(
                ScreensEnum.MainMenu,
                "Main menu",
                "Welcome to the Medical Clinic management system!",
                ScreensEnum.Exit,
                [new MenuOption(0, "Log in", ScreensEnum.LogInForm),
                 new MenuOption(1, "Register", ScreensEnum.RegisterForm),
                 new MenuOption(2, "Patients", ScreensEnum.PatientList),
                 new MenuOption(3, "Workers", ScreensEnum.StaffList),
                 new MenuOption(4, "Equipment", ScreensEnum.EquipmentList),
                 new MenuOption(5, "Supplies", ScreensEnum.SupplyList),
                 new MenuOption(6, "Hospital", ScreensEnum.Hospital)]
            );
            screens.Add(mainMenu);

            // Log In form
            Form logInForm = new Form(
                    ScreensEnum.LogInForm,
                    "Log In Form",
                    "Log into already existing account!",
                    [new FormField(0, "Email address", "",  _ => true),
                     new FormField(1, "Password", "", _ => true)
                    ],
                    ScreensEnum.MainMenu,
                    ScreensEnum.MainMenu,
                    (FormField[] formFields) =>
                    {
                        // Check if the user credentials were correct
                        string email = formFields[0].Value;
                        string password = formFields[1].Value;
                    }
            );
            screens.Add(logInForm);

            // Register form
            Form registerForm = new Form(
                ScreensEnum.RegisterForm,
                "Register Form",
                "Register a new account!",
                [new FormField(0, "First Name", "First Name must be in proper format", RegexUtilities.IsValidName),
                 new FormField(1, "Last Name", "Last Name must be in proper format", RegexUtilities.IsValidName),
                 new FormField(2, "Phone Number", "Phone Number must be in international format: +00123456789", RegexUtilities.IsValidPhoneNumber),
                 new FormField(3, "Email address", "Email address must be in proper format", RegexUtilities.IsValidEmail),
                 new FormField(4, "Password", "Password must: be at least 8 characters long, contain at least 1 small letter, contain at least 1 large letter, contain at least 1 digit, contain at least 1 special character.", RegexUtilities.IsValidPassword)
                ],
                ScreensEnum.MainMenu,
                ScreensEnum.MainMenu,
                (FormField[] formFields) =>
                {
                    // Check if patient with this data doesn't already exist
                    string firstName = formFields[0].Value;
                    string lastName = formFields[1].Value;
                    string phone = formFields[2].Value;
                    string email = formFields[3].Value;
                    string password = formFields[4].Value;

                    // Register new patient
                }
            );
            screens.Add(registerForm);

            return screens;
        }
    }
}
