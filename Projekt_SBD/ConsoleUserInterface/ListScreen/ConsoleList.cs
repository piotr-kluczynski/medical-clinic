using Projekt_SBD.Data;

namespace Projekt_SBD.ConsoleUserInterface.ListScreen
{
    public class ConsoleList<T> : ConsoleScreen
    {
        private List<T> _items = new List<T>();
        public List<T> Items 
        { 
            get => _items; 
            set 
            { 
                _items = value; 
                if (value != null) OriginalItems = new List<T>(value); 
            } 
        }
        public List<T> OriginalItems { get; private set; } = new List<T>();
        public List<ListColumn<T>> Columns { get; set; }
        public Action<T>? InvestigateAction { get; set; }


        public ConsoleList(ScreensEnum id, string name, string description, ScreensEnum previousScreenId, 
                    Action<T>? investigateAction, List<T> items, List<ListColumn<T>> columns,
                    Action? enterAction = null, Action? leaveAction = null) : base(id, name, description, previousScreenId, enterAction, leaveAction)
        {
            Items = items;
            Columns = columns;
            InvestigateAction = investigateAction;
        }

        public override ScreensEnum Run()
        {
            string userResponse = "";
            if (EnterAction != null)
            {
                EnterAction.Invoke();
            }

            Console.WriteLine(Name);
            Console.WriteLine(Description);

            while (true)
            {
                var userInput = "";
                DrawTable();

                if (InvestigateAction != null)
                {
                    Console.WriteLine("\nAby sprawdzić szczegóły rekordu, wpisz 'badaj'.");
                }
                Console.WriteLine("Aby posortować tabelę, wpisz 'sortuj'.");
                Console.WriteLine("Aby zresetować sortowanie, wpisz 'reset'.");
                Console.WriteLine("Aby wyjść z menu, wpisz '0' lub 'exit'.");
                
                Console.Write("\nWybierz akcję: ");
                userResponse = Console.ReadLine() ?? "";

                if (userResponse == "badaj" && InvestigateAction != null)
                {
                    Console.WriteLine("\nPodaj ID wiersza (Row), który chcesz sprawdzić (wpisz 'wróć' aby anulować):");
                    userInput = Console.ReadLine() ?? "";
                    if (userInput == "wróć") continue;

                    if (int.TryParse(userInput, out int row) && 0 <= row && row < Items.Count)
                    {
                        InvestigateAction.Invoke(Items[row]);
                    }
                    else
                    {
                        Console.WriteLine("\nPodane ID wiersza jest nieprawidłowe. Wciśnij Enter...");
                        Console.ReadLine();
                    }
                }
                else if (userResponse == "sortuj")
                {
                    Console.WriteLine("\nPodaj indeks kolumny, po której chcesz posortować (wpisz 'wróć' aby anulować):");
                    userInput = Console.ReadLine() ?? "";
                    if (userInput == "wróć") continue;

                    if (int.TryParse(userInput, out int column) && 0 <= column && column < Columns.Count)
                    {
                        Console.WriteLine("\nAby posortować rosnąco, wpisz 'asc'.\nAby posortować malejąco, wpisz 'desc'.\n(wpisz 'wróć' aby anulować)");
                        userInput = Console.ReadLine() ?? "";
                        if (userInput == "wróć") continue;

                        if(userInput == "asc")
                        {
                            SortByColumn(column, true);
                        }
                        else if (userInput == "desc")
                        {
                            SortByColumn(column, false);
                        }
                        else
                        {
                            Console.WriteLine("\nPodany kierunek sortowania jest nieprawidłowy. Wciśnij Enter...");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nPodany indeks kolumny jest nieprawidłowy. Wciśnij Enter...");
                        Console.ReadLine();
                    }
                }
                else if (userResponse == "reset")
                {
                    _items = new List<T>(OriginalItems);
                }
                else if (userResponse == "exit" || userResponse == "0")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nNie rozpoznano komendy. Wciśnij Enter, aby kontynuować...");
                    Console.ReadLine();
                }
            }


            if (LeaveAction != null)
            {
                LeaveAction.Invoke();
            }
            return PreviousScreenId;
        }

        public void DrawTable()
        {
            const int width = 20;

            Console.Write($"{"Row",-5}");

            for (int i = 0; i < Columns.Count; i++)
            {
                Console.Write($"[{i}] {Columns[i].Header,-15}");
            }

            Console.WriteLine();
            Console.WriteLine(new string('-', 5 + Columns.Count * width));

            for (int row = 0; row < Items.Count; row++)
            {
                Console.Write($"{row,-5}");

                foreach (var column in Columns)
                {
                    var value = column.GetValueFunc(Items[row]);

                    Console.Write($"{value,-20}");
                }

                Console.WriteLine();
            }
        }

        public void SortByColumn(int columnIndex, bool ascending)
        {
            var column = Columns[columnIndex];

            var sorted = ascending
                ? Items.OrderBy(column.GetValueFunc).ToList()
                : Items.OrderByDescending(column.GetValueFunc).ToList();

            Items.Clear();
            Items.AddRange(sorted);
        }
    }
}
