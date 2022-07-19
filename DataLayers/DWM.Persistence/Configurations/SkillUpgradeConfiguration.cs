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
	public class SkillUpgradeConfiguration : IEntityTypeConfiguration<SkillUpgrade>
	{
		public void Configure(EntityTypeBuilder<SkillUpgrade> builder)
		{
			//Configure Primary Key
			builder.HasKey(skillUpgrade => skillUpgrade.SkillUpgradeId);

			//Configure Navigation Propert(ies)
			builder
				.HasOne(skillUpgrade => skillUpgrade.UpgradedSkill)
				.WithMany()
				.HasForeignKey(skillUpgrade => skillUpgrade.UpgradedSkillId);

			builder
				.HasOne(skillUpgrade => skillUpgrade.BaseSkill)
				.WithMany(skill => skill.AvailableUpgrades)
				.HasForeignKey(skillUpgrade => skillUpgrade.BaseSkillId);
		}
	}
}
