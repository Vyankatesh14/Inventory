using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
    public class UserEntity
    {
        [Key]
        public long UserId { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string? Contact { get; set; }
        public long DepartmentId { get; set; }
        /*  public string DepartmentName { get; set; }*/
        public long PlantId { get; set; }
        /*  public string PlantName { get; set; }*/
        public long RoleId { get; set; }
        /*  public string RoleName { get; set; }*/
        /*      public string DeviceCode { get; set; }*/
        public bool IsActive { get; set; }
    }
    
}
