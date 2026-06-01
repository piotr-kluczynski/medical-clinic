using System;
using Projekt_SBD.ConsoleUserInterface;
using Projekt_SBD.Data; 

namespace Projekt_SBD
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine("Trwa łączenie z bazą Oracle i zasilanie danymi testowymi...");

     
            using (var context = new PrzychodniaContext())
            {
                DbInitializer.Initialize(context);
            }

            Console.WriteLine("Gotowe! Baza danych została pomyślnie zasilona.");
            Console.WriteLine("Witaj w systemie zarządzania przychodnią!");


            Console.ReadLine();
            */
            List<IConsoleScreen> screens = ConsoleMenuInitializer.Initialize();
            ConsoleScreenManager manager = new ConsoleScreenManager(screens, ScreensEnum.MainMenu);
            manager.Run();
        }
    }
}