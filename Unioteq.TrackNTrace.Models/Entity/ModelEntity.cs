using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class ModelEntity
    {
        [Key]
        public long ModelId { get; set; }
        [Required]
        [Display(Name = "Model Name")]
        public string? ModelName { get; set; }
        [Required]
        [Display(Name = "Model Code")]
        public string? ModelCode { get; set; }
        [Display(Name = "Model Description")]
        public string? ModelDescription { get; set; }
        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
