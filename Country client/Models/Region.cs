﻿using System.Collections.Generic;

namespace Client.Models
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Country> Country { get; set; }
    }
}
