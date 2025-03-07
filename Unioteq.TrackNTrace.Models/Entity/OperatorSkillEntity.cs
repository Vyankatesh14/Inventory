using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class OperatorSkillEntity
    {
        [Key]
        public long OperatorSkillId { get; set; }
        [Required(ErrorMessage = "Operator Skill is Required")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Operator Name can only contain alphabetic characters and spaces.")]
        [StringLength(100, ErrorMessage = "CustomerName cannot be longer than 100 characters.")]
        [Display(Name = "Operator Skill")]
        public string OperatorSkillName { get; set; }
        public bool IsActive { get; set; }
    }
}
