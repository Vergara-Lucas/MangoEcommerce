using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> CreateCouponAsync(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data= couponDTO,
                Url = StaticDetail.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDTO?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetail.ApiType.DELETE,
                Url = StaticDetail.CouponAPIBase + "/api/coupon/" + id,
            });
        }

        public async Task<ResponseDTO?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.CouponAPIBase + "/api/coupon/"
            });
        }

        public async Task<ResponseDTO?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDTO() {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.CouponAPIBase + "/api/coupon/GetByCode/" + couponCode,
            });
        }

        public async Task<ResponseDTO?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.CouponAPIBase + "/api/coupon/" + id,
            });
        }

        public async Task<ResponseDTO?> UpdateCouponAsync(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetail.ApiType.PUT,
                Data = couponDTO,
                Url = StaticDetail.CouponAPIBase + "/api/coupon"
            });
        }
    }
}
