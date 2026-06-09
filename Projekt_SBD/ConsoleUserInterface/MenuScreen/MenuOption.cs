using Projekt_SBD.Data;

namespace Projekt_SBD.ConsoleUserInterface.MenuScreen
{
    public class MenuOption
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? SubTitle { get; set; }
        public ScreensEnum TargetScreenId { get; set; }

        public Action<int>? interactAction;

        public MenuOption(int id, string title, ScreensEnum targetScreenId, string? subTitle = null, Action<int>? interactAction = null)
        {
            Id = id;
            Title = title;
            SubTitle = subTitle;
            TargetScreenId = targetScreenId;

            this.interactAction = interactAction;
        }

        public void Display()
        {
            Console.WriteLine(Id + ". " + Title);
            if (SubTitle != null)
            {
                Console.WriteLine("     " + SubTitle);
            }
        }

        public ScreensEnum Run()
        {
            if (interactAction != null)
            {
                interactAction.Invoke(0);
            }
            return TargetScreenId;
        }
    }
}
