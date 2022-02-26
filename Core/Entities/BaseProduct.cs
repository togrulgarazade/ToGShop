﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class BaseProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDT { get; set; }
        public bool IsDeleted { get; set; }
    }
}
