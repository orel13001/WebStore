﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    public class Employee : Entity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? Patronomic { get; set; }
        public int Age { get; set; }
    }
}
