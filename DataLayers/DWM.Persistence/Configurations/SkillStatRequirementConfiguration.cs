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
	public class SkillStatRequirementConfiguration : IEntityTypeConfiguration<SkillStatRequirement>
	{
		public void Configure(EntityTypeBuilder<SkillStatRequirement> builder)
		{
			//Configure Primary Key
			builder.HasKey(ssr => ssr.SkillStatRequirementId);

			//Configure Navigation Propert(ies)
			builder
				.HasOne(ssr => ssr.TargetSkill)
				.WithMany(skill => skill.StatRequirements)
				.HasForeignKey(ssr => ssr.TargetSkillId);
		}
	}
}
