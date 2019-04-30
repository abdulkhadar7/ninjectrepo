using NInject.API.authorizefilters;
using NInject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NInject.API.Controllers
{
    [CustomAuthorize(Roles ="manager")]
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        private readonly IProductServices _productServices;

        public ProductController()
        {
            
        }

        public ProductController(IProductServices productServices)
        {
            _productServices= productServices;
        }

        [AllowAnonymous]
        [Route("getproducts")]
        [HttpGet]
        public HttpResponseMessage GetProducts(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
             
            try
            {
                var q = _productServices.GetAllProducts();
                response = request.CreateResponse(HttpStatusCode.OK, q);
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
