using System;
using System.Collections.Generic;
using System.Text;

namespace NewsApp.Models
{
    public class HomeMenuItem
    {
        public int menId { get; set; }
        public string menTitulo { get; set; }
        public string Imagen { get; set; }
        public bool menIsHot { get; set; }
        public int menEstado { get; set; }
    }
}
