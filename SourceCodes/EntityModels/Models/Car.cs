using System;
using System.Collections.Generic;

namespace EntityModels.Models
{
    public partial class Car
    {
        public int CarId { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Price { get; set; }
    }
}
