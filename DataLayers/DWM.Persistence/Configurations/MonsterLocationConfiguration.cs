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
	public class MonsterLocationConfiguration : IEntityTypeConfiguration<MonsterLocation>
	{
		public void Configure(EntityTypeBuilder<MonsterLocation> builder)
		{
			//Configure Primary Key
			builder.HasKey(monsterLocation => monsterLocation.MonsterLocationId);

			//Configure Navigation Propert(ies)
			builder
				.HasOne(monsterLocation => monsterLocation.Monster)
				.WithMany()
				.HasForeignKey(monsterLocation => monsterLocation.MonsterId);

			//Configure Code-First Table Metadata
			builder
				.Property(monsterLocation => monsterLocation.TravelersGateName)
				.HasColumnType("nvarchar(50)");
		}
	}
}
