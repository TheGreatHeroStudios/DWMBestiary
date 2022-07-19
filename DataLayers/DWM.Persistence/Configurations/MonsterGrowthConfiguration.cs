using DWM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Persistence.Configurations
{
	public class MonsterGrowthConfiguration : IEntityTypeConfiguration<MonsterGrowth>
	{
		public void Configure(EntityTypeBuilder<MonsterGrowth> builder)
		{
			//Configure Primary Key
			builder.HasKey(monsterGrowth => monsterGrowth.MonsterGrowthId);

			//Configure Navigation Propert(ies)
			builder
				.HasOne(monsterGrowth => monsterGrowth.GrowthCategory)
				.WithMany()
				.HasForeignKey(monsterGrowth => monsterGrowth.GrowthCategoryId);
		}
	}
}
