using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Machine
{
    public class CompanyView
    {

        [Key]
        public long CompId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompCorpAdd { get; set; }
        public string? CompClientAdd { get; set; }
        public string? CompCode { get; set; }
        public IFormFile? CompLogo { get; set; }
        public string? CompLogoPath { get; set; }


        public bool IsActive { get; set; }
    }
}
