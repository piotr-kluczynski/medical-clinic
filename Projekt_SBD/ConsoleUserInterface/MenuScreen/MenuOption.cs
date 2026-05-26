namespace Projekt_SBD.ConsoleUserInterface.MenuScreen
{
    public class MenuOption
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? SubTitle { get; set; }
        public int TargetId { get; set; }
    
        public MenuOption(int id, string title, int targetId, string subTitle = null)
        {
            Id = id;
            Title = title;
            SubTitle = subTitle;
            TargetId = targetId;
        }

        public void Display()
        {
            Console.WriteLine(Id + ". " + Title);
            if (SubTitle != null) 
            {
                Console.WriteLine("     " + SubTitle);
            }
        }
    }
}
