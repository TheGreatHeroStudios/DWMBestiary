using DWM.DataLoader.DataLoaders;
using DWM.Models;
using DWM.Persistence;
using TGH.Common.Patterns.IoC;
using TGH.Common.Persistence.Interfaces;
using TGH.Common.Repository.Implementations;
using TGH.Common.Repository.Interfaces;
using TGH.Common.Utilities.Logging;
using TGH.Common.Utilities.Logging.Providers;

#region File-Specific Constant(s)
//Token(s)
const string LOG_FILE_NAME = "DataLoaderLog.txt";
const int BUFFER_HEIGHT = Int16.MaxValue - 1;
const int CONSOLE_WIDTH = 100;
const string DATA_LOADER_APP_NAME = "DWM Bestiary Data Loader";

//Info Message(s)
const string INFO_BEGIN_DB_REBUILD = "Beginning Re-build of Sqlite Database...";
const string INFO_BEGIN_MEMORY_LOAD = "Loading data into memory...";
const string INFO_BEGIN_DB_LOAD = "Loading data into Sqlite Database...";
const string INFO_MEMORY_LOAD_SUCCESSFUL = "Successfully loaded data into memory!";
const string INFO_DB_LOAD_SUCCESSFUL = "Successfully loaded Sqlite Database!";

const string EXIT_PROMPT = "Press any key to exit...";

//Error Message(s)
const string ERROR_DATA_LOADER_FAILED =
	"Data Loader failed to load one or more entities.  DWM Bestiary may not function as expected.";
#endregion



#region Non-Public Member(s)
int _exitCode = 0;
bool _waitOnExit = false;
bool _rebuildDatabase = false;

SkillDataLoader _skillDataLoader;
GrowthCategoryDataLoader _growthCategoryDataLoader;
MonsterDataLoader _monsterDataLoader;
MonsterLocationDataLoader _monsterLocationDataLoader;
BreedingPairDataLoader _breedingPairDataLoader;

IEnumerable<Skill> _skills = new List<Skill>();
IEnumerable<GrowthCategory> _growthCategories = new List<GrowthCategory>();
IEnumerable<Monster> _monsters = new List<Monster>();
IEnumerable<MonsterLocation> _monsterLocations = new List<MonsterLocation>();
IEnumerable<BreedingPair> _breedingPairs = new List<BreedingPair>();
#endregion



#region Entry Point
try
{
	ConfigureMiddleware();
	PrepareConsole(args);

	if(_rebuildDatabase)
	{
		BeginDatabaseRebuild();
	}

	ReadDataIntoMemory();
	LoadDatabase();
}
catch(Exception ex)
{
	Logger.LogError(ex.ToString());
	Logger.LogError(ERROR_DATA_LOADER_FAILED);
	_exitCode = -1;
}
finally
{
	if (_waitOnExit)
	{
		Logger.LogInfo(EXIT_PROMPT);
		Console.Read();
	}

	Logger.UnregisterLoggingProviders();
	Environment.Exit(_exitCode);
}
#endregion



#region Non-Public Method(s)
void ConfigureMiddleware()
{
	//Register logging provider(s)
	Logger.RegisterLoggingProviders
	(
		new LoggingProvider[]
		{
			new FileLoggingProvider
			(
				LOG_FILE_NAME,
				PersistenceConstants.APPLICATION_DATA_FOLDER
			),
			new ConsoleLoggingProvider()
		}
	);

	//Register singleton middleware instances for the necessary data layers
	DependencyManager
		.RegisterService<IDatabaseContext, DWMBestiaryDbContext>
		(
			() =>
				new DWMBestiaryDbContext
				(
					PersistenceConstants.SQLITE_DB_TARGET_FILEPATH,
					true
				),
			ServiceScope.Singleton
		);

	DependencyManager
		.RegisterService<IGenericRepository, GenericRepository>
		(
			ServiceScope.Singleton
		);
}


void PrepareConsole(string[] args)
{
	IEnumerable<string> argsList = args.Select(arg => arg.ToLower());

	if (argsList.Contains("/w") || argsList.Contains("-w")
		|| argsList.Contains("/wait") || argsList.Contains("-wait")
		|| argsList.Contains("/waitonexit") || argsList.Contains("-waitonexit"))
	{
		_waitOnExit = true;
	}

	if (argsList.Contains("/r") || argsList.Contains("-r")
		|| argsList.Contains("/rebuild") || argsList.Contains("-rebuild")
		|| argsList.Contains("/rebuilddatabase") || argsList.Contains("-rebuilddatabase"))
	{
		_rebuildDatabase = true;
	}

	Console.BufferHeight = BUFFER_HEIGHT;
	Console.WindowWidth = CONSOLE_WIDTH;
	Console.Title = DATA_LOADER_APP_NAME;
}


void BeginDatabaseRebuild()
{
	if
	(
		File.Exists(PersistenceConstants.SQLITE_DB_TARGET_FILEPATH) &&
		File.Exists(PersistenceConstants.SQLITE_TEMPLATE_FILEPATH)
	)
	{
		//If the database exists but should be rebuilt, delete it from the 
		//target filepath and re-copy it from the template database filepath.
		Logger.LogInfo(INFO_BEGIN_DB_REBUILD);

		File.Delete(PersistenceConstants.SQLITE_DB_TARGET_FILEPATH);

		File.Copy
		(
			PersistenceConstants.SQLITE_TEMPLATE_FILEPATH,
			PersistenceConstants.SQLITE_DB_TARGET_FILEPATH
		);
	}
}


void ReadDataIntoMemory()
{
	Logger.LogInfo(INFO_BEGIN_MEMORY_LOAD);

	_skillDataLoader = new SkillDataLoader();
	_skills = _skillDataLoader.ReadDataIntoMemory();

	_growthCategoryDataLoader = new GrowthCategoryDataLoader();
	_growthCategories = _growthCategoryDataLoader.ReadDataIntoMemory();

	_monsterDataLoader = new MonsterDataLoader(_skills, _growthCategories);
	_monsters = _monsterDataLoader.ReadDataIntoMemory();

	_monsterLocationDataLoader = new MonsterLocationDataLoader(_monsters);
	_monsterLocations = _monsterLocationDataLoader.ReadDataIntoMemory();

	_breedingPairDataLoader = new BreedingPairDataLoader(_monsters);
	_breedingPairs = _breedingPairDataLoader.ReadDataIntoMemory();

	Logger.LogInfo(INFO_MEMORY_LOAD_SUCCESSFUL);
}


void LoadDatabase()
{
	Logger.LogInfo(INFO_BEGIN_DB_LOAD);

	_skillDataLoader.LoadDataIntoDatabase(_skills);
	_growthCategoryDataLoader.LoadDataIntoDatabase(_growthCategories);
	_monsterDataLoader.LoadDataIntoDatabase(_monsters);
	_monsterLocationDataLoader.LoadDataIntoDatabase(_monsterLocations);
	_breedingPairDataLoader.LoadDataIntoDatabase(_breedingPairs);

	Logger.LogInfo(INFO_DB_LOAD_SUCCESSFUL);
}
#endregion