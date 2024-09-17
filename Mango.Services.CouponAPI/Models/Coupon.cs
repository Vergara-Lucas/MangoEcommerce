using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }
        [Required]
        public string CouponCode{ get; set; }
        [Required]
        public double DiscountAmount{ get; set; }//monto del descuento del cupon
        public int MinAmount { get; set; }//monton minimo
    }
}
