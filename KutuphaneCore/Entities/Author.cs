﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneCore.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PlaceofBirth { get; set; }
        public int YearofBirth { get; set; }

    }
}
