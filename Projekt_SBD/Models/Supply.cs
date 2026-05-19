using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.Models
{
    public class Supply
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        // Room reference - to indicate where the supplies are stored
    }
}
