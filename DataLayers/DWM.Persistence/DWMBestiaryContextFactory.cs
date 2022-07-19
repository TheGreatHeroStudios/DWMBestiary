using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Persistence
{
	internal class DWMBestiaryContextFactory : IDesignTimeDbContextFactory<DWMBestiaryDbContext>
	{
		public DWMBestiaryDbContext CreateDbContext(string[] args)
		{
			//Scaffold the database migration based on the source .db3 file in the project files.
			return
				new DWMBestiaryDbContext(PersistenceConstants.SQLITE_TEMPLATE_FILEPATH, true);
		}
	}
}
