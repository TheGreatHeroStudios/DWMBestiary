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
	public class SkillConfiguration : IEntityTypeConfiguration<Skill>
	{
		public void Configure(EntityTypeBuilder<Skill> builder)
		{
			//Configure Primary Key
			builder.HasKey(skill => skill.SkillId);

			//Configure Navigation Propert(ies)
			builder
				.HasMany(skill => skill.StatRequirements)
				.WithOne(ssr => ssr.TargetSkill)
				.HasForeignKey(ssr => ssr.TargetSkillId);

			builder
				.HasMany(skill => skill.ComboRequirements)
				.WithOne(scr => scr.TargetSkill)
				.HasForeignKey(scr => scr.TargetSkillId);

			builder
				.HasMany(skill => skill.AvailableUpgrades)
				.WithOne(skill => skill.BaseSkill)
				.HasForeignKey(skillUpgrade => skillUpgrade.BaseSkillId);

			//Configure Code-First Table Metadata
			builder
				.Property(skill => skill.SkillName)
				.HasColumnType("nvarchar(20)");

			builder
				.Property(skill => skill.SkillDescription)
				.HasColumnType("nvarchar(200)");
		}
	}
}
