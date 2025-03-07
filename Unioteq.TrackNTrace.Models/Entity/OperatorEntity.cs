using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class OperatorEntity
    {
        public long OperatorId { get; set; }
        [Required(ErrorMessage = "Plant Name is required.")]
        [Display(Name = "Plant")]
        public long PlantId { get; set; }
        [Required(ErrorMessage = "ShopFloor Name is required.")]
        [Display(Name = "ShopFloor")]
        public long ShopFloorId { get; set; }
        [Required(ErrorMessage = "Operator Code is required.")]
        [Display(Name = "Operator Code")]
        [StringLength(50, ErrorMessage = "Operator Code cannot be longer than 50 characters.")]
        public string OperatorCode { get; set; }
        [Required(ErrorMessage = "Operator Skill is required.")]
        [Display(Name = "Operator Skill")]
        public long OperatorSkillId { get; set; }
        [Required(ErrorMessage = "Operator Name is required.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Operator Name can only contain alphabetic characters and spaces.")]
        [StringLength(100, ErrorMessage = "CustomerName cannot be longer than 100 characters.")]
        [Display(Name = "Operator Name")] 
        public string OperatorName { get; set; }
        public string PlantName { get; set; }
        public string ShopFloorName { get; set; }
        public string StationName { get; set; }
        public string LineName { get; set; }
        public string OperatorSkillName { get; set; }
        [Required(ErrorMessage = "Operator Image is required.")]
        [Display(Name = "Operator Image")]
        public string OperatorImage { get; set; }
        public bool IsActive { get; set; }
    }
}
   
