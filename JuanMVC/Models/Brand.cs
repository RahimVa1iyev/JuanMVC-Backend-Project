﻿using System.ComponentModel.DataAnnotations;

namespace JuanMVC.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
