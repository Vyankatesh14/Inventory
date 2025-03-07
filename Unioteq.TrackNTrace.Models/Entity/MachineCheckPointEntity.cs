using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class MachineCheckPointEntity
    {
        [Key]
        public long MachineCheckPointId { get; set; }

        public string? Effect { get; set; }

        public string? CheckPointName { get; set; }

        public string? ValidationType { get; set; }

        public string? StandardText { get; set; }

        public string? OKText { get; set; }

        public string? NOKText { get; set; }

        public string? ValueUOM { get; set; }

        public string? ValueRangeForm { get; set; }

        public string? ValueRangeTo { get; set; }

        public string CheckMethod { get; set; } = "";  

        public string? Frequecy { get; set; }

        public long MachineId  { get; set; }

        public string? MachineName { get; set; }

        public long MachinePartId { get; set; }

        public string? MachinePartName { get; set; }

        public bool IsActive { get; set; }
    }
}
