
namespace Projekt_SBD.ConsoleUserInterface.MenuScreen
{
    public class Menu : IConsoleScreen
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HelpMessage { get; set; }
        public int PreviousScreenId { get; set; }

        private readonly Dictionary<int, MenuOption> menuOptions;

        public Menu(int id, string name, string description, string helpMessage, int previousScreenId, List<MenuOption> menuOptions)
        {
            Id = id;
            Name = name;
            Description = description;
            HelpMessage = helpMessage;
            PreviousScreenId = previousScreenId;

            this.menuOptions = new Dictionary<int, MenuOption>();
            foreach (MenuOption menuOption in menuOptions)
            {
                this.menuOptions[menuOption.Id] = menuOption;
            }
        }

        public int Run()
        {
            Console.WriteLine(Name);
            Console.WriteLine(Description);

            Console.WriteLine("Choose an option: ");
            foreach (MenuOption option in menuOptions.Values)
            {
                option.Display();
            }
            Console.WriteLine((menuOptions.Count + 1) + ". Help");
            Console.WriteLine("In order to leave the menu, type 'exit' and press enter.");

            string userResponse = Console.ReadLine();
            int userChoice = 0;
            while (userResponse != "exit")
            {
                if (Int32.TryParse(userResponse, out userChoice))
                {
                    // Handling help request from the user
                    if (userChoice == menuOptions.Count + 1)
                    {
                        Console.WriteLine(HelpMessage);
                    }
                    else
                    {
                        return menuOptions[userChoice].TargetId;
                    }
                }
                else
                {
                    Console.WriteLine("Command couldn't be recognized.\r\n" +
                                      "To choose an option enter a number ranging from " + 0 + " to " + menuOptions.Count + ".\r\n" +
                                      "In order to leave the menu, type 'exit' and press enter.");
                }
                userResponse = Console.ReadLine();
            }

            return PreviousScreenId;
        }
    }
}
