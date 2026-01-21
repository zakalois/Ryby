using System;

namespace Ryby_a_ulovky.Models
{
    public class Aktualita
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Datum { get; set; } = DateTime.Now;
    }
}