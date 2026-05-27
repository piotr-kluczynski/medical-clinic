using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.ConsoleUserInterface.FormScreen
{
    public class NumberField : IField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DataType dataType { get; set; }
        public string? Description { get; set; }

        public NumberField(int id, string name, string? description = null)
        {
            Id = id;
            Name = name;
            Description = description;

            dataType = DataType.Number;
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
            float _ = 0;
            return float.TryParse(Value, out _);
        }
    }
}
