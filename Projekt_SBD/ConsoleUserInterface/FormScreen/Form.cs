namespace Projekt_SBD.ConsoleUserInterface.FormScreen
{
    public class Form : IConsoleScreen
    {
        public ScreensEnum Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ScreensEnum CancelScreenId { get; set; }
        public ScreensEnum SucceedScreenId { get; set; }

        public List<FormField> fields { get; set; }
        public Action<FormField[]> formAction;

        public Action? enterAction;
        public Action? leaveAction;

        public Form(ScreensEnum id, string name, string description, List<FormField> fields, 
            ScreensEnum cancelScreenId, ScreensEnum succeedScreenId, Action<FormField[]> formAction,
            Action? enterAction = null, Action? leaveAction = null)
        {
            Id = id;
            Name = name;
            Description = description;
            this.fields = fields;

            CancelScreenId = cancelScreenId;
            SucceedScreenId = succeedScreenId;
            this.formAction = formAction;

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
            Console.WriteLine("In order to leave the menu, type 'exit' and press enter.");

            foreach (FormField field in fields) 
            {
                field.Display();
                string givenValue = Console.ReadLine();
                string errorMsg = "";
                if (givenValue != "exit")
                {
                    if (leaveAction != null)
                    {
                        leaveAction.Invoke();
                    }
                    return CancelScreenId;
                }
                else
                {
                    field.Value = givenValue;
                }

                while (!field.Verify(out errorMsg))
                {
                    Console.WriteLine(errorMsg + "\r\nTry again.");
                    field.Display();
                    Console.WriteLine("In order to leave the menu, type 'exit' and press enter.");
                    givenValue = Console.ReadLine();

                    if (givenValue == "exit")
                    {
                        if (leaveAction != null)
                        {
                            leaveAction.Invoke();
                        }
                        return CancelScreenId;
                    }
                    else
                    {
                        field.Value = givenValue;
                    }
                }
            }

            formAction.Invoke(fields.ToArray());

            if (leaveAction != null)
            {
                leaveAction.Invoke();
            }
            return SucceedScreenId;
        }
    }
}
