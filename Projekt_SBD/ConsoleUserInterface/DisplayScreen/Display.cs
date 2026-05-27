namespace Projekt_SBD.ConsoleUserInterface.DisplayScreen
{
    public class Display : IConsoleScreen
    {
        public ScreensEnum Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ScreensEnum ReturnScreenId { get; set; }

        public string[][] Content { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Action? enterAction;
        public Action? leaveAction;

        public Display(ScreensEnum id, string name, string description, ScreensEnum returnScreenId,
            int width, int height, string[][] content,
            Action? enterAction = null, Action? leaveAction = null)
        {
            Id = id;
            Name = name;
            Description = description;
            ReturnScreenId = returnScreenId;

            Width = width;
            Height = height;
            Content = content;

            this.enterAction = enterAction;
            this.leaveAction = leaveAction;
        }

        public ScreensEnum Run()
        {
            if (enterAction != null)
            {
                enterAction.Invoke();
            }

            Console.WriteLine(Name);
            Console.WriteLine(Description);

            for (int i = 0; i < Content.Length; i++)
            {
                for (int j = 0; j < Content[i].Length; j++)
                {
                    Console.WriteLine(Content[i][j]);
                    Console.Write("  ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Press any button to close the screen.");
            Console.ReadKey();
            if (leaveAction != null)
            {
                leaveAction.Invoke();
            }
            return ReturnScreenId;
        }
    }
}
