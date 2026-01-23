using System;

namespace Ryby.Models
{
    public class Aktualita
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Datum { get; set; } = DateTime.Now;
    }
}