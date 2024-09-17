using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDTO _response;
        private readonly IMapper _mapper;
        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDTO();
            _mapper = mapper;
        }
        [HttpGet]
        public ResponseDTO Get() {
            try {
                IEnumerable<Coupon> objList = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDTO>>(objList);
                return _response; 
            } catch (Exception ex) { 
            
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        
        }
        [HttpGet]
        [Route("{id:int}")]
        public ResponseDTO Get(int id) {

            try {
                Coupon obj = _db.Coupons.First(c => c.CouponId == id);
                _response.Result = _mapper.Map<CouponDTO>(obj);
                return _response;
            }catch (Exception ex) { 
                _response.IsSuccess=false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDTO GetByCode(string code)
        {

            try
            {
                Coupon obj = _db.Coupons.First(c => c.CouponCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDTO>(obj);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        //post
        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public ResponseDTO Post([FromBody]CouponDTO coupon)
        {
            try {
                Coupon obj = _mapper.Map<Coupon>(coupon);
                _db.Coupons.Add(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDTO>(obj);
                return _response;
            } catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPut]
		[Authorize(Roles = "ADMIN")]
		public ResponseDTO Put([FromBody] CouponDTO coupon)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(coupon);
                _db.Coupons.Update(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDTO>(obj);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpDelete]
        [Route("{id:int}")]
		[Authorize(Roles = "ADMIN")]
		public ResponseDTO Delete(int id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(c => c.CouponId == id);
                _db.Coupons.Remove(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDTO>(obj);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


    }
}
