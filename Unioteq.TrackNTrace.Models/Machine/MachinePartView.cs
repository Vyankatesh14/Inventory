using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Machine
{
    public class MachinePartView
    {

        [Key]
        public long MachinePartId { get; set; }
        public string? MachinePartName { get; set; }
        public IFormFile? MachinePartImage { get; set; }
        public string? MachinePartImagePath { get; set; }
        public bool IsActive { get; set; }
        public long MachineId { get; set; }
        
        public string? MachineName { get; set; }

      


    }
}
