namespace Projekt_SBD.ConsoleUserInterface.DisplayScreen
{
    public class DisplayBlock
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string[][] Content { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public DisplayBlock(int id, string? name, int width, int height, string[][] content)
        {
            Id = id;
            Name = name;
            Width = width;
            Height = height;
            Content = content;
        }

        public void Display()
        {
            for (int i = 0; i < Content.Length; i++)
            {
                for (int j = 0; j < Content[i].Length; j++)
                {
                    Console.Write(Content[i][j] + "  ");
                }
                Console.WriteLine();
            }
        }
    }
}
