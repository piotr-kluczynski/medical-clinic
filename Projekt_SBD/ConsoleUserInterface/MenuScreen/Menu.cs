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

            Console.WriteLine("Choose an option: ");
            foreach (MenuOption option in menuOptions.Values)
            {
                option.Display();
            }
            Console.WriteLine("In order to leave the menu, type 'exit' and press enter.");

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
                    Console.WriteLine("Command couldn't be recognized.\r\n" +
                                      "To choose an option enter a number ranging from " + 0 + " to " + menuOptions.Count + ".\r\n" +
                                      "In order to leave the menu, type 'exit' and press enter.");
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
