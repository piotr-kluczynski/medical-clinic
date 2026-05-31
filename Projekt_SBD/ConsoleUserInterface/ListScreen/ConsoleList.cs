namespace Projekt_SBD.ConsoleUserInterface.ListScreen
{
    public class ConsoleList<T> : IConsoleScreen
    {
        public ScreensEnum Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ScreensEnum PreviousScreenId { get; set; }

        public List<T> Items { get; set; }
        public List<ListColumn<T>> Columns { get; set; }
        public Action<T>? InvestigateAction { get; set; }

        public Action? enterAction;
        public Action? leaveAction;

        public ConsoleList(ScreensEnum id, string name, string description, ScreensEnum previousScreenId, 
                    Action<T>? investigateAction, List<T> items, List<ListColumn<T>> columns,
                    Action? enterAction = null, Action? leaveAction = null)
        {
            Id = id;
            Name = name;
            Description = description;
            PreviousScreenId = previousScreenId;

            Items = items;
            Columns = columns;
            InvestigateAction = investigateAction;

            this.enterAction = enterAction;
            this.leaveAction = leaveAction;
        }

        public ScreensEnum Run()
        {
            string userResponse = "";
            if (enterAction != null)
            {
                enterAction.Invoke();
            }

            Console.WriteLine(Name);
            Console.WriteLine(Description);

            while (true)
            {
                var userInput = "";
                DrawTable();

                // Display the instruction
                if (InvestigateAction != null)
                {
                    Console.WriteLine("To investigate a record, enter 'investigate'.");
                }
                Console.WriteLine("To sort table by column, enter 'sort'.");
                Console.WriteLine("In order to leave the menu, type 'exit' and press enter.");
                userResponse = Console.ReadLine();

                if (userResponse == "investigate" && InvestigateAction != null)
                {
                    Console.WriteLine("Select id of the row you would like to investigate:");
                    userInput = Console.ReadLine();

                    if (int.TryParse(userInput, out int row) && 0 <= row && row < Items.Count)
                    {
                        InvestigateAction.Invoke(Items[row]);
                    }
                    else
                    {
                        Console.WriteLine("Given row Id was invalid.");
                    }

                }
                else if (userResponse == "sort")
                {
                    Console.WriteLine("Select the index of column you would like to sort by:");
                    userInput = Console.ReadLine();

                    if (int.TryParse(userInput, out int column) && 0 <= column && column < Columns.Count)
                    {
                        Console.WriteLine("If you want to sort the column in the ascending order, type 'asc'.\r\n" +
                                          "If you want to sort it in the descending order, type 'desc'.");
                        userInput = Console.ReadLine();
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
                            Console.WriteLine("Given sorting order was invalid.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Given column Id was invalid.");
                    }
                }
                else if (userResponse == "exit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Command couldn't be recognized.\r\n" +
                                      "In order to leave the menu, type 'exit' and press enter.");
                }
                userResponse = Console.ReadLine();
            }


            if (leaveAction != null)
            {
                leaveAction.Invoke();
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
                ? Items.OrderBy(column.GetValueFunc)
                : Items.OrderByDescending(column.GetValueFunc);

            Items.Clear();
            Items.AddRange(sorted);
        }
    }
}
