using DWM.Models;
using DWM.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Persistence.Configurations
{
	public class MonsterConfiguration : IEntityTypeConfiguration<Monster>
	{
		public void Configure(EntityTypeBuilder<Monster> builder)
		{
			//Configure Primary Key
			builder.HasKey(monster => monster.MonsterId);

			//Configure Navigation Propert(ies)
			builder
				.HasMany(monster => monster.NaturalSkills)
				.WithOne()
				.HasForeignKey(naturalSkill => naturalSkill.MonsterId);

			builder
				.HasMany(monster => monster.StatGrowth)
				.WithOne()
				.HasForeignKey(monsterGrowth => monsterGrowth.MonsterId);

			builder
				.HasMany(monster => monster.SkillResistances)
				.WithOne()
				.HasForeignKey(skillResistance => skillResistance.MonsterId);

			//Configure Code-First Table Metadata
			builder
				.Property(monster => monster.MonsterName)
				.HasColumnType("nvarchar(20)");
		}
	}
}
