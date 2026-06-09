using Projekt_SBD.Data;

namespace Projekt_SBD.ConsoleUserInterface
{
    public abstract class ConsoleScreen : IConsoleScreen
    {
        public ScreensEnum Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ScreensEnum PreviousScreenId { get; set; }
        public Action? EnterAction { get; set; }
        public Action? LeaveAction { get; set; }

        public ConsoleScreen(ScreensEnum id, string name, string description, ScreensEnum previousScreenId, Action? enterAction, Action? leaveAction)
        {
            Id = id;
            Name = name;
            PreviousScreenId = previousScreenId;
            Description = description;
            EnterAction = enterAction;
            LeaveAction = leaveAction;
        }

        public virtual ScreensEnum Run()
        {
            return PreviousScreenId;
        }
    }
}
