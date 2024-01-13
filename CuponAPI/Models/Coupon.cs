using System.ComponentModel.DataAnnotations;

namespace CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public int CuponId { get; set; }
        [Required]
        public string CuponCode { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
