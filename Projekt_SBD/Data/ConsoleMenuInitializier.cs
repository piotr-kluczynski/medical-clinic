using Projekt_SBD.ConsoleUserInterface;
using Projekt_SBD.ConsoleUserInterface.DisplayScreen;
using Projekt_SBD.ConsoleUserInterface.FormScreen;
using Projekt_SBD.ConsoleUserInterface.ListScreen;
using Projekt_SBD.ConsoleUserInterface.MenuScreen;
using Projekt_SBD.Models;
using Projekt_SBD.Utilities;

namespace Projekt_SBD.Data
{
    public enum ScreensEnum
    {
        Exit,
        MainMenu,
        LogInForm,
        RegisterForm,
        PatientList,
        PatientDetails,
        StaffList,
        StaffDetails,
        EquipmentList,
        EquipmentDetails,
        SupplyList,
        SupplyDetails,
        DepartmentList
    }

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
                [
                    new MenuOption(0, "Log in", ScreensEnum.LogInForm),
                    new MenuOption(1, "Register", ScreensEnum.RegisterForm),
                    new MenuOption(2, "Patients", ScreensEnum.PatientList),
                    new MenuOption(3, "Workers", ScreensEnum.StaffList),
                    new MenuOption(4, "Equipment", ScreensEnum.EquipmentList),
                    new MenuOption(5, "Supplies", ScreensEnum.SupplyList),
                    new MenuOption(6, "Departments", ScreensEnum.DepartmentList)]
            );
            screens.Add(mainMenu);

            // Log In form
            Form logInForm = new Form(
                ScreensEnum.LogInForm,
                "Log In Form",
                "Log into already existing account!",
                [
                    new FormField(0, "Email address", "",  _ => true),
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
                [
                    new FormField(0, "First Name", "First Name must be in proper format", RegexUtilities.IsValidName),
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

            // Patient List
            ConsoleList<Patient> patientList = new ConsoleList<Patient>
            (
                ScreensEnum.PatientList,
                "Patient List",
                "View the list of registered patients!",
                ScreensEnum.MainMenu,
                patient => { 
                    /* Open the Patient details screen for chosen patient */ 

                },
                [ /* List of patients */ ], 
                [
                    new ListColumn<Patient>("Patient Id", p => p.Id),
                    new ListColumn<Patient>("First Name", p => p.FirstName),
                    new ListColumn<Patient>("Last Name", p => p.LastName)
                ]
            );
            screens.Add(patientList);

            // Patient Details

            // Staff List
            ConsoleList<Worker> staffList = new ConsoleList<Worker>
            (
                ScreensEnum.StaffList,
                "Staff List",
                "View the list of the clinic staff!",
                ScreensEnum.MainMenu,
                staff => { /* Open the Staff details screen for chosen worker */ },
                [ /* List of workers */ ],
                [
                    new ListColumn<Worker>("Worker Id", w => w.Id),
                    new ListColumn<Worker>("First Name", w => w.FirstName),
                    new ListColumn<Worker>("Last Name", w => w.LastName),
                    new ListColumn<Worker>("Position", w => w.Position)
                ]
            );
            screens.Add(staffList);

            // Staff Details

            // Equipment List
            ConsoleList<Equipment> equipmentList = new ConsoleList<Equipment>
            (
                ScreensEnum.EquipmentList,
                "Equipment List",
                "Examine the equipment available in the clinic!",
                ScreensEnum.MainMenu,
                equipment => { /* Open the Equipment details screen for chosen equipment */ },
                [ /* List of equipment */ ],
                [
                    new ListColumn<Equipment>("Equipment Id", e => e.Id),   
                    new ListColumn<Equipment>("Name", e => e.Name),   
                    new ListColumn<Equipment>("Room Id", e => e.RoomId)  
                ]
            );
            screens.Add(equipmentList);

            // Equipment Details

            // Supply List
            ConsoleList<Supply> supplyList = new ConsoleList<Supply>
            (
                ScreensEnum.SupplyList,
                "Supply List",
                "Examine the supplies available in the clinic!",
                ScreensEnum.MainMenu,
                supplies => { /* Open the Supply details screen for chosen supply */ },
                [ /* List of supplies */ ],
                [
                    new ListColumn<Supply>("Supply Id", s => s.Id),
                    new ListColumn<Supply>("Name", s => s.Name),
                    new ListColumn<Supply>("Quantity", s => s.Quantity)
                ]
            );
            screens.Add(supplyList);

            // Supply Details

            // Department List
            ConsoleList<Department> departmentList = new ConsoleList<Department>
            (
                ScreensEnum.DepartmentList,
                "Departments of our Medical Clinic",
                "View the structure of our medical clinic",
                ScreensEnum.MainMenu,
                department => { /* Open the Department details screen for chosen department */ },
                [ /* List of departments */ ],
                [
                    new ListColumn<Department>("Department Id", d => d.Id),
                    new ListColumn<Department>("Name", d => d.Name)
                ]
            );
            screens.Add(departmentList);

            // Department Details

            return screens;
        }
    }
}
