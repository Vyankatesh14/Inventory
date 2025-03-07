using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Machine
{
    public class MachineView
    {
        [Key]
        public long MachineId { get; set; }
        public string? MachineName { get; set; }
        public string? MachineCode { get; set; }
        public string? MachineDetail { get; set; }
        public IFormFile? MachineManual { get; set; } // For uploading files
        public string? MachineManualPath { get; set; } // For storing file path
        public bool IsActive { get; set; }
        public long LineId { get; set; }
        /*        public string? LineName { get; set; }*/

        public string? MachineExtension { get; set; }

        public string? LineName { get; set; }

    }
}
