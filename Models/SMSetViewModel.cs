using System;
using System.Collections.Generic;

namespace IronRod.Models
{
    public class SMSetViewModel
    {
        public int Id {get; set;}
        public string Title {get; set;}
        public List<Verse> Verses { get; set; }
        public List<int> Taken { get; set; }
    }
}