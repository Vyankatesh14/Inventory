using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class DeviceMasterEntity
    {

        [Key]
        public long Id { get; set; }
        public long CompId { get; set; }

        public string? CompanyName { get; set; }
        public long PlantId  { get; set;}
        public string? PlantName { get;set; }
        public long LineId { get; set; }
        public string? LineName { get; set; }
        public string? MacCode { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public string? DeviceCode { get; set; }
        public bool ? IsActive { get; set; }
       
    }
}
