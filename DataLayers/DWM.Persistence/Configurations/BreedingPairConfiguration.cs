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
	public class BreedingPairConfiguration : IEntityTypeConfiguration<BreedingPair>
	{
		public void Configure(EntityTypeBuilder<BreedingPair> builder)
		{
			//Configure Table Name
			builder.ToTable("BreedingPair");

			//Configure Primary Key
			builder.HasKey(breedingPair => breedingPair.BreedingPairId);

			//Configure Navigation Propert(ies)
			builder
				.HasOne(breedingPair => breedingPair.OffspringMonster)
				.WithMany()
				.HasForeignKey(breedingPair => breedingPair.OffspringMonsterId);

			builder
				.HasOne(breedingPair => breedingPair.PedigreeMonster)
				.WithMany()
				.HasForeignKey(breedingPair => breedingPair.PedigreeMonsterId)
				.IsRequired(false);

			builder
				.HasOne(breedingPair => breedingPair.PartnerMonster)
				.WithMany()
				.HasForeignKey(breedingPair => breedingPair.PartnerMonsterId)
				.IsRequired(false);

			//Configure Code-First Table Metadata
			builder
				.Property(breedingPair => breedingPair.OffspringPlusRequirement)
				.IsRequired(false);

			builder
				.Property(breedingPair => breedingPair.PedigreeFamilyId)
				.IsRequired(false);

			builder
				.Property(breedingPair => breedingPair.PedigreePlusRequirement)
				.IsRequired(false);

			builder
				.Property(breedingPair => breedingPair.PartnerFamilyId)
				.IsRequired(false);
		}
	}
}
