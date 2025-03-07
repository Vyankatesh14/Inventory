using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class CompanyEntity
    {
        [Key]
        public long CompId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompLogo { get; set; }
        public string? CompCorpAdd { get; set; }
        public string? CompClientAdd { get; set; }
        public string? CompCode { get; set; }
         
        public bool IsActive { get; set; }
    }
}
