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
	internal class MonsterHierarchyConfiguration : IEntityTypeConfiguration<MonsterHierarchy>
	{
		public void Configure(EntityTypeBuilder<MonsterHierarchy> builder)
		{
			builder.HasNoKey();
		}
	}
}
