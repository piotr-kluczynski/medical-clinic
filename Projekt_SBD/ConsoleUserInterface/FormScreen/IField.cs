using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.ConsoleUserInterface.FormScreen
{
    public enum DataType
    {
        Text,
        Number,
        DateTime
    }

    public interface IField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DataType dataType { get; set; }
        public string? Description { get; set; }

        public abstract void Display();
        public bool Verify();
    }
}
