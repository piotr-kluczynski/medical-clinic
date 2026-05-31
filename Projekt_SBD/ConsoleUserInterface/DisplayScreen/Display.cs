namespace Projekt_SBD.ConsoleUserInterface.DisplayScreen
{
    public class Display : ConsoleScreen
    {
        public string[][] Content { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Display(ScreensEnum id, string name, string description, ScreensEnum previousScreenId,
            int width, int height, string[][] content,
            Action? enterAction = null, Action? leaveAction = null) : base(id, name, description, previousScreenId, enterAction, leaveAction)
        {
            Width = width;
            Height = height;
            Content = content;
        }

        public override ScreensEnum Run()
        {
            if (EnterAction != null)
            {
                EnterAction.Invoke();
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
            if (LeaveAction != null)
            {
                LeaveAction.Invoke();
            }
            return PreviousScreenId;
        }
    }
}
