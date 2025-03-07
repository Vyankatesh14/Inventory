using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class StationEntity
    {

        [Key]
        public long StationId { get; set; }

        [Required(ErrorMessage = "Plant is required.")]
        [Display(Name = "Plant")]
        public long PlantId { get; set; }

        [Required(ErrorMessage = "ShopFloor is required.")]
        [Display(Name = "ShopFloor")]
        public long ShopFloorId { get; set; }

        [Required(ErrorMessage = "Line is required.")]
        [Display(Name = "Line")]
        public long LineId { get; set; }

        [Required(ErrorMessage = "Station Name is required.")]
        [Display(Name = "Station Name")]
        public string? StationName { get; set; }

        public string? PlantName { get; set; }
        public string? ShopFloorName { get; set; }
        public string? LineName { get; set; }

        [Display(Name = "Station Description")]
        public string? StationDescription { get; set; }

        [Required(ErrorMessage = "Station Code is required.")]
        [Display(Name = "Station Code")]
        [StringLength(50, ErrorMessage = "Station Code cannot be longer than 50 characters.")]
        public string? StationCode { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
