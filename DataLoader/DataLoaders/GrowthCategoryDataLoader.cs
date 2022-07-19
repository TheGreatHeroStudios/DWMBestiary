using DWM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGH.Common;
using TGH.Common.Utilities.DataLoader.Implementations;
using TGH.Common.Utilities.Logging;

namespace DWM.DataLoader.DataLoaders
{
	public class GrowthCategoryDataLoader : DataLoader<GrowthCategory>
	{
		#region File-Specific Constant(s)
		//Token(s)
		private const int GROWTH_CATEGORY_COUNT = 32;
		private const int MAX_LEVEL_COUNT = 99;

		private const int FILE_OFFSET_LEVEL_DATA = 2;

		//File Path(s)
		private static readonly string GROWTH_TABLE_FILEPATH =
			$@"{FileConstants.EXECUTABLE_DIRECTORY}\SourceFiles\GrowthTable.txt";

		//Message(s)
		private const string INFO_GROWTH_CAT_LOADING_SKIPPED =
			"Growth categories have already been loaded into the target database.  Skipping...";

		private const string INFO_BEGIN_GROWTH_CAT_LOADING = "Loading growth categories...";
		#endregion



		#region Abstract Implementation
		public override Func<GrowthCategory, int> KeySelector => growthCategory => growthCategory.GrowthCategoryId;


		public override IEnumerable<GrowthCategory> ReadDataIntoMemory()
		{
			Logger.LogInfo(INFO_BEGIN_GROWTH_CAT_LOADING);

			string[] levelData =
				File
					.ReadAllLines(GROWTH_TABLE_FILEPATH)
					.Skip(FILE_OFFSET_LEVEL_DATA)
					.Take(MAX_LEVEL_COUNT)
					.ToArray();

			int[][] growthMatrix = new int[GROWTH_CATEGORY_COUNT][];

			for(int i = 0; i < GROWTH_CATEGORY_COUNT; i++)
			{
				growthMatrix[i] = new int[MAX_LEVEL_COUNT];
			}

			int currentCategoryIndex;

			for (int currentLevelIndex = 0; currentLevelIndex < levelData.Length; currentLevelIndex++)
			{
				currentCategoryIndex = 0;

				foreach
				(
					int growthDataPoint in
					levelData[currentLevelIndex]
						.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries)
						.Skip(1)
						.Select(dataPointText => int.Parse(dataPointText))
				)
				{
					growthMatrix[currentCategoryIndex][currentLevelIndex] = growthDataPoint;
					currentCategoryIndex++;
				}
			}

			List<GrowthCategory> growthCategories = new List<GrowthCategory>();

			Array
				.ForEach
				(
					growthMatrix,
					growthCategoryData =>
						growthCategories
							.Add
							(
								new GrowthCategory
								{
									GrowthStatistics =
										JsonConvert.SerializeObject(growthCategoryData)
								}
							)
				);

			return growthCategories;
		}
		#endregion



		#region Override(s)
		public override int ExpectedRecordCount => GROWTH_CATEGORY_COUNT;
		#endregion
	}
}
