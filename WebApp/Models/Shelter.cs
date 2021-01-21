using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Shelter
    {
        public int Id { get; set; }
        //Store the number of places for pets in the shelter.
        public int Place { get; set; }
        //Stores the amount of shelter funds
        public double AmountFunds { get; set; }

    }
}
