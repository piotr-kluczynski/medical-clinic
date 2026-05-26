namespace Projekt_SBD.ConsoleUserInterface
{
    public class ConsoleScreenManager
    {
        private Dictionary<int, IConsoleScreen> consoleScreens;
        private int currentScreenId;


        public ConsoleScreenManager(List<IConsoleScreen> consoleScreens, int startingScreen)
        {
            this.consoleScreens = new Dictionary<int, IConsoleScreen>();
            foreach(IConsoleScreen consoleScreen in consoleScreens)
            {
                consoleScreens[consoleScreen.Id] = consoleScreen; 
            }

            currentScreenId = startingScreen;
        }

        public void Run()
        {
            while(currentScreenId >= 0)
            {
                currentScreenId = consoleScreens[currentScreenId].Run();
            }
        }
    }
}
