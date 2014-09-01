using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleService
{
    public class CarViewModel
    {
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }
    }
}
