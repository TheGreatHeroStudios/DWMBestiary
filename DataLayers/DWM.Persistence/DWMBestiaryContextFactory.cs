using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Persistence
{
	internal class DWMBestiaryContextFactory : IDesignTimeDbContextFactory<DWMBestiarySqliteDbContext>
	{
		public DWMBestiarySqliteDbContext CreateDbContext(string[] args)
		{
			//Create the template file if it doesn't already exist
			if(!File.Exists(PersistenceConstants.SQLITE_TEMPLATE_FILEPATH))
			{
				File.Create(PersistenceConstants.SQLITE_TEMPLATE_FILEPATH);
			}

			//Scaffold the database migration based on the source .db3 file in the project files.
			return
				new DWMBestiarySqliteDbContext(PersistenceConstants.SQLITE_TEMPLATE_FILEPATH, true);
		}
	}
}
