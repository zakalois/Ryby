using System;

namespace Ryby.Models
{
    public class Ulovek
    {
        public int Id { get; set; }

        // FK na Ryba
        public int RybaId { get; set; }
        public Ryba Ryba { get; set; }

        // FK na Lokalita
        public int LokalitaId { get; set; }
        public Lokalita Lokalita { get; set; }

        public double Delka { get; set; }
        public double Vaha { get; set; }
        public DateTime Datum { get; set; }
        public string Poznamka { get; set; }
    }
}