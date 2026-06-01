namespace Projekt_SBD.ConsoleUserInterface.FormScreen
{
    public class FormField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string? Description { get; set; }

        public string InvalidInputMsg { get; set; }
        public Func<string, bool> ValidationFunc { get; set; }

        public FormField(int id, string name, string invalidInputMsg, Func<string, bool> validationFunc, string? description = null)
        {
            Id = id;
            Name = name;
            Description = description;

            InvalidInputMsg = invalidInputMsg;
            ValidationFunc = validationFunc;
        }

        public void Display()
        {
            Console.WriteLine(Name);
            if (Description != null)
            {
                Console.WriteLine("     " + Description + "\r\n");
            }
        }

        public bool Verify(out string invalidInputMsg)
        {
            invalidInputMsg = InvalidInputMsg;
            return ValidationFunc.Invoke(Value);
        }
    }
}
