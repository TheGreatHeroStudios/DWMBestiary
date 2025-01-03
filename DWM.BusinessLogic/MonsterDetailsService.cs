using DWM.DataTransferObjects;
using DWM.Models;
using DWM.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGH.Common.Repository.Interfaces;

namespace DWM.BusinessLogic
{
	public class MonsterDetailsService
	{
		private IGenericRepository _repo;

		public MonsterDetailsService(IGenericRepository repo)
		{
			_repo = repo;
		}


		public List<MonsterDetails> GetMonsterDetails(int? monsterId = null)
		{
			return
				_repo
					.RetrieveEntities<Monster>
					(
						m => monsterId == null || m.MonsterId == monsterId
					)
					.Select
					(
						m => 
							new MonsterDetails
							{
								MonsterId = m.MonsterId,
								MonsterName = m.MonsterName,
								Family = Enum.GetName(m.FamilyId),
								MaxLevel = m.MaxLevel,
								GenderProbability = Enum.GetName(m.GenderProbabilityId),
								IsFlying = m.IsFlying,
								IsMetal = m.IsMetal
							}
					)
					.ToList();
		}
	}
}
