using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace IronRod.Models
{
    public class Topic
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
    }
}