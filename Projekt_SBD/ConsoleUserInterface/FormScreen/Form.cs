using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.ConsoleUserInterface.FormScreen
{
    public class Form : IConsoleScreen
    {
        public ScreensEnum Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ScreensEnum CancelScreenId { get; set; }
        public ScreensEnum SucceedScreenId { get; set; }

        public List<IField> fields { get; set; }
        public Action<IField[]> formAction;

        public Action<int>? enterAction;
        public Action<int>? leaveAction;

        public Form(ScreensEnum id, string name, string description, List<IField> fields, 
            ScreensEnum cancelScreenId, ScreensEnum succeedScreenId, Action<IField[]> formAction,
            Action<int>? enterAction = null, Action<int>? leaveAction = null)
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
                enterAction.Invoke(0);
            }

            Console.WriteLine(Name);
            Console.WriteLine(Description);
            Console.WriteLine("In order to leave the menu, type 'exit' and press enter.");

            foreach (IField field in fields) 
            {
                field.Display();
                string givenValue = Console.ReadLine();
                if (givenValue != "exit")
                {
                    if (leaveAction != null)
                    {
                        leaveAction.Invoke(0);
                    }
                    return CancelScreenId;
                }
                else
                {
                    field.Value = givenValue;
                }

                while (!field.Verify())
                {
                    Console.WriteLine("Given value was incorrect.\r\nTry again.");
                    field.Display();
                    Console.WriteLine("In order to leave the menu, type 'exit' and press enter.");
                    givenValue = Console.ReadLine();

                    if (givenValue != "exit")
                    {
                        if (leaveAction != null)
                        {
                            leaveAction.Invoke(0);
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
                leaveAction.Invoke(0);
            }
            return SucceedScreenId;
        }
    }
}
