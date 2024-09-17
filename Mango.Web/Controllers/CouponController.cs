using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _CouponService;
        public CouponController(ICouponService CouponService)
        {
            _CouponService = CouponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDTO>? list = null;
            ResponseDTO? response = await _CouponService.GetAllCouponsAsync();
            //visual no me deja solo poner issucess idk
            if (response != null && response.IsSuccess) {
                //deserealizar funca asi  JsonConvert.DeserializeObject<tipoAConvertir>(loQueSea)
                list = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result));
                return View(list);
            }
            else
            {
                TempData["error"] = response?.Message;
                return View();
            }


        }
        public async Task<IActionResult> CouponCreate() {

            return View();
        }
	
		public async Task<IActionResult> CouponDelete(int CouponId)
		{
            ResponseDTO? response = await _CouponService.GetCouponByIdAsync(CouponId);
			if (response != null && response.IsSuccess)
			{
				CouponDTO? model = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));
                return View(model);
			}
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
		}
		[HttpPost]
		public async Task<IActionResult> CouponDelete(CouponDTO model)
		{
			ResponseDTO? response = await _CouponService.DeleteCouponAsync(model.CouponId);
			if (response != null && response.IsSuccess)
			{
                TempData["success"] = "Cupon borrado con exito";
                return RedirectToAction(nameof(CouponIndex));
			}
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(model);
		}
		[HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDTO model)
        {
            
            if (ModelState.IsValid) {
                ResponseDTO? response = await _CouponService.CreateCouponAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Cupon creado con exito";
                    //deserealizar funca asi  JsonConvert.DeserializeObject<tipoAConvertir>(loQueSea)
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }
    }
}
