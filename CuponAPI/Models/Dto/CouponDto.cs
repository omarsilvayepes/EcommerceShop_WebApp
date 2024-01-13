namespace CouponAPI.Models.Dto
{
    public class CouponDto
    {
        public int CuponId { get; set; }
        public string CuponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
