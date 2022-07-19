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
	public class SkillComboRequirementConfiguration : IEntityTypeConfiguration<SkillComboRequirement>
	{
		public void Configure(EntityTypeBuilder<SkillComboRequirement> builder)
		{
			//Configure Primary Key
			builder.HasKey(scr => scr.SkillComboRequirementId);

			//Configure Navigation Propert(ies)
			builder
				.HasOne(scr => scr.RequiredSkill)
				.WithMany()
				.HasForeignKey(scr => scr.RequiredSkillId);

			builder
				.HasOne(scr => scr.TargetSkill)
				.WithMany(skill => skill.ComboRequirements)
				.HasForeignKey(scr => scr.TargetSkillId);
		}
	}
}
