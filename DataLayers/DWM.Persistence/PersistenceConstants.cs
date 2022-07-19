using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGH.Common;

namespace DWM.Persistence
{
	public static class PersistenceConstants
	{
		public const string APPLICATION_NAME = "DWMBestiary";

		public const string DEFAULT_DB_NAME = "DWMBestiary.db3";
		
		public static readonly string APPLICATION_DATA_FOLDER =
			$"{FileConstants.APP_DATA_DIRECTORY}" +
			$"{APPLICATION_NAME}" +
			$"{FileConstants.PATH_SEPARATOR}";

		public static readonly string SQLITE_TEMPLATE_FILEPATH =
			$"{FileConstants.EXECUTABLE_DIRECTORY}" +
			$"SourceFiles{FileConstants.PATH_SEPARATOR}" +
			$"{DEFAULT_DB_NAME}";

		public static readonly string SQLITE_DB_TARGET_FILEPATH =
			$"{APPLICATION_DATA_FOLDER}{DEFAULT_DB_NAME}";
	}
}
