﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.Modele
{
    public class Categorie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nume { get; set; }
    }
}
