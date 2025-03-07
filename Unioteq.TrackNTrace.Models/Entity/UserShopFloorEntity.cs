using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unioteq.TrackNTrace.Models.Entity
{
   public class UserShopFloorEntity
    {
        [Key]
        public long UserShopFloorId { get; set; }
        public long UserId { get; set; }
        public long ShopFloorId { get; set; }
        public string? ShopFloorCode { get; set; }
        public string? ShopFloorName { get; set; }
    }
}
