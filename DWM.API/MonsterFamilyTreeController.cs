using DWM.BusinessLogic;
using DWM.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DWM.API
{
	[Route("api/[controller]")]
	[ApiController]
	public class MonsterFamilyTreeController : ControllerBase
	{
		private MonsterFamilyTreeService _service;


		public MonsterFamilyTreeController(MonsterFamilyTreeService service)
		{
			_service = service;
		}


		[HttpGet("{monsterName}")]
		public IActionResult Get(string monsterName, [FromQuery]int? maxHierarchyLevels = null)
		{
			try
			{
				MonsterFamilyTree? monsterFamilyTree = _service.GetMonsterFamilyTree(monsterName, maxHierarchyLevels);

				if (monsterFamilyTree == null)
				{
					return NotFound();
				}
				else
				{
					return new ObjectResult(monsterFamilyTree);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
