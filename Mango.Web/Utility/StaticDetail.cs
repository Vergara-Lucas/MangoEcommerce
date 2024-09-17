﻿namespace Mango.Web.Utility
{
    public class StaticDetail
    {
        public static string CouponAPIBase {  get; set; }
        public static string AuthAPIBase { get; set; }
        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JWTToken";
        public enum ApiType { 
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
