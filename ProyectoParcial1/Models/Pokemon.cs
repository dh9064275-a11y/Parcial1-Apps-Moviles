using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoParcial1.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
