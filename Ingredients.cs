using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2024PROGPOE1.v3
{
    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double OriginalQuantity { get; set; } // This will store the original quantity
        public string Unit { get; set; }
        public double Calories { get; set; } 
        public string FoodGroup { get; set; } 
    }


}
