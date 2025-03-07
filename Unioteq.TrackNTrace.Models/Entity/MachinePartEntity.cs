using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class MachinePartEntity
    {

        [Key]
        public long MachinePartId { get; set; }
        public string? MachinePartName { get; set; }
        public string? MachinePartImage { get; set; }
        public bool IsActive { get; set; }
        public long MachineId { get; set; }

        public string? MachineName { get; set; }


    }
}
