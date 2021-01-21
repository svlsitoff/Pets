using System;
using System.ComponentModel.DataAnnotations;


namespace WebApp.Models
{
    public class PetViewModel
    {
        [Required(ErrorMessage = "Please enter pet's name")]
        [Display(Name = "Name")]
        [StringLength(20)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter pet's kind")]
        [Display(Name = "Kind")]
        [StringLength(15)]
        public string Kind { get; set; }

        [Required(ErrorMessage = "Please choose pet image")]
        public string PetPicture { get; set; }
        [Range(1, 17, ErrorMessage = "Please enter valid integer Number")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Please enter pet's helth status")]
        [Display(Name = "Health")]
        [StringLength(500)]
        public string HealthStatus { get; set; }
        public int Health { get; set; }
    }
}
