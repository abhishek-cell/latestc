using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoolApplicationMain.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoolApplicationMain.Controllers
{
	[Route("api/rest")]
	[ApiController]
	public class RestController : ControllerBase
	{
		private IProductInformation _productRepository;
		public RestController(IProductInformation productRepository)
		{
			_productRepository = productRepository;
		}
		[Produces("application/json")]
		[HttpGet("search")]
		public IActionResult Search()
		{
			try
			{
				var db = _productRepository.GetProductsInformation();
				string term = HttpContext.Request.Query["term"].ToString();
				var names = db.Where(p => p.LongDescription.Contains(term)).Select(p => p.Description).ToList();
				return Ok(names);
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}