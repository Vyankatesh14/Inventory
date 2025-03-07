using System;
using System.ComponentModel.DataAnnotations;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class LineEntity
    {
        [Key]
        public long LineId { get; set; }
        public long PlantId { get; set; }
        public long ShopFloorId { get; set; }

        [Required]
        [Display(Name = "Line Name")]
        public string? LineName { get; set; }

        [Display(Name = "ShopFloor Name")]
        public string? ShopFloorName { get; set; }  // Added Property

        [Display(Name = "Plant Name")]
        public string? PlantName { get; set; }  // Added Property

        [Required]
        [Display(Name = "Line Code")]
        public string? LineCode { get; set; }

        [Display(Name = "Line Description")]
        public string? LineDescription { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
