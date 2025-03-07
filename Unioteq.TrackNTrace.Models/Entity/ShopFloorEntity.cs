using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class ShopFloorEntity
    {

        [Key]
        public long ShopFloorId { get; set; }
        public long PlantId { get; set; }
        public string? ShopFloorName { get; set; }
        public string? PlantName { get; set; }
        public string? ShopFloorCode { get; set; }
        public bool IsActive { get; set; }

        
    }
}
