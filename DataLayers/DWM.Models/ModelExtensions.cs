using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models
{
	public static class ModelExtensions
	{
		public static Monster FindByName(this IEnumerable<Monster> monsters, string monsterName)
		{
			return
				monsters
					.Single
					(
						monster =>
							monster
								.MonsterName
								.Equals
								(
									monsterName.Trim(),
									StringComparison.InvariantCultureIgnoreCase
								)
					);
		}
	}
}
