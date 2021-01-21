using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Disease
    {
        public int Id { get; set; }

        public int IdPet { get; set; }

        public string Description { get; set; }

        public double Cost { get; set; }

    }
}
