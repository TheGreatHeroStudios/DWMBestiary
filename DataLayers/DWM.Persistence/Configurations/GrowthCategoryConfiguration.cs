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
	public class GrowthCategoryConfiguration : IEntityTypeConfiguration<GrowthCategory>
	{
		public void Configure(EntityTypeBuilder<GrowthCategory> builder)
		{
			//Configure Table Name
			builder.ToTable("GrowthCategory");

			//Configure Primary Key
			builder.HasKey(growthCategory => growthCategory.GrowthCategoryId);

			//Configure Code-First Table Metadata
			builder
				.Property(growthCategory => growthCategory.GrowthStatistics)
				.HasColumnType("nvarchar(500)");
		}
	}
}
