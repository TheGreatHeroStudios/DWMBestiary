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
	public class SkillResistanceConfiguration : IEntityTypeConfiguration<SkillResistance>
	{
		public void Configure(EntityTypeBuilder<SkillResistance> builder)
		{
			//Configure Primary Key
			builder.HasKey(skillResistance => skillResistance.SkillResistanceId);
		}
	}
}
