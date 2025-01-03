using DWM.BusinessLogic;
using DWM.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DWM.API
{
	[Route("api/[controller]")]
	[ApiController]
	public class MonsterDetailsController : ControllerBase
	{
		private MonsterDetailsService _service;


		public MonsterDetailsController(MonsterDetailsService service)
		{
			_service = service;
		}


		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				List<MonsterDetails> monsterDetails =
					_service.GetMonsterDetails();

				if (!(monsterDetails?.Any() ?? false))
				{
					return NotFound();
				}
				else
				{
					return new ObjectResult(monsterDetails);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
