using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
   public class OperationsEntity
    
    {
        [Key]
        public long OperationId { get; set; }
      
        [Required]
        [Display(Name = "Operation")]
        public string Operation { get; set; }
        [Required]
        [Display(Name = "Station")]
        public long StationId { get; set; }
        [Required]
        [Display(Name = "Model")]
        public long ModelId { get; set; }
        [Required]
        [Display(Name = "Plant")]
        public long PlantId { get; set; }
        public string PlantCode { get; set; }
        [Required]
        [Display(Name = "ShopFloor")]
        public long ShopFloorId { get; set; }
        public string ShopFloorCode { get; set; }
        [Required]
        [Display(Name = "Line")]
        public long LineId { get; set; }
        public string LineCode { get; set; }
        public string StationCode { get; set; }
        public string ModelCode { get; set; }
        [Required]
        public int Sequence { get; set; }
        [Required]
        public bool IsActive { get; set; }

    }
}
