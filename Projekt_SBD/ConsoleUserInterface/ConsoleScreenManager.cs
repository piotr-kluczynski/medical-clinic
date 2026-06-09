using Projekt_SBD.Data;

namespace Projekt_SBD.ConsoleUserInterface
{
    public class ConsoleScreenManager
    {
        private Dictionary<ScreensEnum, IConsoleScreen> consoleScreens;
        private ScreensEnum currentScreenId;


        public ConsoleScreenManager(List<IConsoleScreen> consoleScreens, ScreensEnum startingScreen)
        {
            this.consoleScreens = new Dictionary<ScreensEnum, IConsoleScreen>();
            foreach (IConsoleScreen consoleScreen in consoleScreens.ToList())
            {
                this.consoleScreens.Add(consoleScreen.Id, consoleScreen);
            }

            currentScreenId = startingScreen;
        }

        public void Run()
        {
            while (currentScreenId >= 0)
            {
                currentScreenId = consoleScreens[currentScreenId].Run();

                if (currentScreenId == ScreensEnum.Exit)
                {
                    return;
                }
            }
        }
    }
}
