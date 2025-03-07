using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class MachineEntity
    {
        [Key]
        public long MachineId { get; set; }

        [Required]
        [DisplayName("Machine Name")]
        public string? MachineName { get; set; }

        [Required]
        [DisplayName("Machine Code")]
        public string? MachineCode { get; set; }

        [Required]
        [DisplayName("Machine Detail ")]
        public string? MachineDetail { get; set; }

        [Required]
        [DisplayName("Machine Manual")]
        public string? MachineManual { get; set; }

        [Required]
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        public long LineId { get; set; }

        [NotMapped]  // Optional: Use this attribute if LineName is not a database field.
        public string? LineName { get; set; }
    }
}   
