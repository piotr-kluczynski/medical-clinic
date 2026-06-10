using Projekt_SBD.Data;

namespace Projekt_SBD.ConsoleUserInterface.MenuScreen
{
    public class Menu : ConsoleScreen
    {
        private readonly Dictionary<int, MenuOption> menuOptions;

        public Menu(ScreensEnum id, string name, string description, ScreensEnum previousScreenId, 
            List<MenuOption> menuOptions, 
            Action? enterAction = null, Action? leaveAction = null) : base(id, name, description, previousScreenId, enterAction, leaveAction)
        {
            this.menuOptions = new Dictionary<int, MenuOption>();
            foreach (MenuOption menuOption in menuOptions)
            {
                this.menuOptions.Add(menuOption.Id, menuOption);
            }
        }

        public override ScreensEnum Run()
        {
            if (EnterAction != null)
            {
                EnterAction.Invoke();
            }

            Console.WriteLine(Name);
            Console.WriteLine(Description);

            Console.WriteLine("\nWybierz opcję:");
            foreach (MenuOption option in menuOptions.Values)
            {
                option.Display();
            }

            string userResponse = Console.ReadLine();
            int userChoice = 0;
            while (userResponse != "exit")
            {
                if (Int32.TryParse(userResponse, out userChoice))
                {
                    if (LeaveAction != null)
                    {
                        LeaveAction.Invoke();
                    }
                    return menuOptions[userChoice].Run();
                }
                else
                {
                    Console.WriteLine("\nNie rozpoznano komendy.\r\n" +
                                      "Aby wybrać opcję, podaj prawidłowy numer z listy powyżej.");
                }
                userResponse = Console.ReadLine();
            }

            if (LeaveAction != null)
            {
                LeaveAction.Invoke();
            }
            return PreviousScreenId;
        }
    }
}
