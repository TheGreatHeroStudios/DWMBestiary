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
	public class NaturalSkillConfiguration : IEntityTypeConfiguration<NaturalSkill>
	{
		public void Configure(EntityTypeBuilder<NaturalSkill> builder)
		{
			//Configure Primary Key
			builder.HasKey(naturalSkill => naturalSkill.NaturalSkillId);

			//Configure Navigation Propert(ies)
			builder
				.HasOne(naturalSkill => naturalSkill.Skill)
				.WithMany()
				.HasForeignKey(naturalSkill => naturalSkill.SkillId);

			//Configure Code-First Table Metadata
			builder
				.HasOne<Monster>()
				.WithMany()
				.HasForeignKey(naturalSkill => naturalSkill.MonsterId);
		}
	}
}
