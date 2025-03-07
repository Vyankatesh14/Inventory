using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class CustomerEntity
    {


        [Key]
        public long CustomerId { get; set; }

        [Required]
        [DisplayName("Plant")]
        public long PlantId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "CustomerName cannot be longer than 100 characters.")]
        [DisplayName("Customer Name")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Customer Name can only contain alphabetic characters and spaces.")]
        public string? CustomerName { get; set; }

        [Required]
        [DisplayName("Contact")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Enter valid contact number!")]
        public string? Contact { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Address cannot be longer than 255 characters.")]
        public string? Address { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [StringLength(100, ErrorMessage = "PlantName cannot be longer than 100 characters.")]
        [DisplayName("Plant Name")]
        public string? PlantName { get; set; }
    }
}
