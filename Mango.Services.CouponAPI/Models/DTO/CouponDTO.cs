namespace Mango.Services.CouponAPI.Models.DTO
{
    public class CouponDTO
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }//monto del descuento del cupon
        public int MinAmount { get; set; }//monton minimo
    }
}
