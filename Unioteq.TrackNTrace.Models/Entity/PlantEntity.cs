using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class PlantEntity
    {


        [Key]
        [DisplayName("Plants")]
        public long PlantId { get; set; }

        [DisplayName("Plant Name")]
        public string? PlantName { get; set; }

        [DisplayName("Plant Code")]
        public string? PlantCode { get; set; }

        public string? PlantAddress { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

       
    }
}
