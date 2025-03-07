using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class ModelPartsEntity
    {
        [Key]
        public long ModelPartId { get; set; }
        [Required]
        [Display(Name = "Station")]
        public long? StationId { get; set; }
        [Required]
        [Display(Name = "Model")]
        public long? ModelId { get; set; }
        [Required]
        [Display(Name = "Model Part Name")]
        public string ModelPartName { get; set; }

        [Display(Name = "Station Code")]
        public string StationCode { get; set; }

        [Display(Name = "Model Code")]
        public string ModelCode { get; set; }
        [Display(Name = "ModelPart Description")]
        public string ModelPartDescription { get; set; }
        [Required]
        public int Sequence { get; set; }
        [Required]
        [Display(Name = "ModelPart Code")]
        public string ModelPartCode { get; set; }

        [Required(ErrorMessage = "ModelPart Image is Required")]
        [Display(Name = "ModelPart Image")]
        public string ModelPartImage { get; set; }

        [Required]
        [Display(Name = "Line")]
        public long? LineId { get; set; }

        [Display(Name = "Line Code")]
        public string LineCode { get; set; }

        [Required]
        [Display(Name = "ShopFloor")]
        public long? ShopFloorId { get; set; }
        [Display(Name = "ShopFloor Code")]
       public string ShopFloorCode { get; set; }

        [Required]
        [Display(Name = "Plant")]
        public long? PlantId { get; set; }

        [Display(Name = "Plant Code")]
        public string PlantCode { get; set; }
        [Required]
        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }

       
    }
}
