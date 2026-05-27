namespace Projekt_SBD.ConsoleUserInterface.FormScreen
{
    public class DateTimeField : IField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DataType dataType { get; set; }
        public string? Description { get; set; }

        public DateTimeField(int id, string name, string? description = null)
        {
            Id = id;
            Name = name;
            Description = description;

            dataType = DataType.DateTime;
            Value = "";
        }

        public void Display()
        {
            Console.WriteLine(Name);
            if (Description != null)
            {
                Console.WriteLine("     " + Description + "\r\n");
            }
        }

        public bool Verify()
        {
            DateTime _ = DateTime.Now;
            return DateTime.TryParse(Value, out _);
        }
    }
}
