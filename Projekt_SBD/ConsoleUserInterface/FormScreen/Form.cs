using Projekt_SBD.Data;

namespace Projekt_SBD.ConsoleUserInterface.FormScreen
{
    public class Form : ConsoleScreen
    {
        public ScreensEnum SucceedScreenId { get; set; }

        public List<FormField> Fields { get; set; }
        public Action<FormField[]> FormAction;

        public Form(ScreensEnum id, string name, string description, List<FormField> fields, ScreensEnum previousScreenId, 
            ScreensEnum succeedScreenId, Action<FormField[]> formAction,
            Action? enterAction = null, Action? leaveAction = null) : base(id, name, description, previousScreenId, enterAction, leaveAction)
        {
            Fields = fields;
            SucceedScreenId = succeedScreenId;
            FormAction = formAction;
        }

        public override ScreensEnum Run()
        {
            if (EnterAction != null) 
            {
                EnterAction.Invoke();
            }

            Console.WriteLine(Name);
            Console.WriteLine(Description);
            Console.WriteLine("In order to leave the menu, type 'exit' and press enter.");

            foreach (FormField field in Fields) 
            {
                field.Display();
                string givenValue = Console.ReadLine();
                string errorMsg = "";
                if (givenValue == "exit")
                {
                    if (LeaveAction != null)
                    {
                        LeaveAction.Invoke();
                    }
                    return PreviousScreenId;
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
                        if (LeaveAction != null)
                        {
                            LeaveAction.Invoke();
                        }
                        return PreviousScreenId;
                    }
                    else
                    {
                        field.Value = givenValue;
                    }
                }
            }

            FormAction.Invoke(Fields.ToArray());

            if (LeaveAction != null)
            {
                LeaveAction.Invoke();
            }
            return SucceedScreenId;
        }
    }
}
