using Last.Data.Master;
using Last.Data.User;
using Last.Interpreter;
using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Bindings;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public enum JobPromotionModes
		{ 
			Bahamut = 0,
			AllJobsItem,
			JobItems,
		}
		public static void GeneratePromoItems()
		{
			// Job Items
			GameData.MessageManager.AddMessage($"MSG_JOBITEM_NAME_{(int)Items.JobAll}", "<IC_IOBJ>All Promotion Jobs");
			GameData.MessageManager.AddMessage($"MSG_JOBITEM_INF_{(int)Items.JobAll}", "All the Promotion Jobs you need. No, Mimic isn't a realy job, stop asking.");
			GameData.MasterManager.GetList<Item>().Add((int)Items.JobAll, new Item($"{(int)Items.JobAll},{(int)Items.JobAll},2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0"));
			GameData.MasterManager.GetList<Content>().Add((int)Items.JobAll, new Content($"{(int)Items.JobAll},MSG_JOBITEM_NAME_{(int)Items.JobAll},None,MSG_JOBITEM_INF_{(int)Items.JobAll},0,1,{(int)Items.JobAll}"));

			List<Items> promoJobs = new() { Items.JobKnight, Items.JobNinja, Items.JobMaster, Items.JobRedWizard, Items.JobWhiteWizard, Items.JobBlackWizard };
			Dictionary<Items, string> jobNames = new() { { Items.JobKnight, "Knight" }, { Items.JobNinja, "Ninja" }, { Items.JobMaster, "Master" }, { Items.JobRedWizard, "Red Wizard" }, { Items.JobWhiteWizard, "White Wizard" }, { Items.JobBlackWizard, "Black Wizard" }, };

			foreach (var job in promoJobs)
			{
				InternalLogger.LogInfo($"Adding Job Items for {jobNames[job]}");

				GameData.MessageManager.AddMessage($"MSG_JOBITEM_NAME_{(int)job}", $"<IC_IOBJ>{jobNames[job]} Job");
				GameData.MessageManager.AddMessage($"MSG_JOBITEM_INF_{(int)job}", $"The {jobNames[job]} Job.");
				GameData.MasterManager.GetList<Item>().Add((int)job, new Item($"{(int)job},{(int)job},2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0"));
				GameData.MasterManager.GetList<Content>().Add((int)job, new Content($"{(int)job},MSG_JOBITEM_NAME_{(int)job},None,MSG_JOBITEM_INF_{(int)job},0,1,{(int)job}"));
			}
		}
		public static void ProcessJobItem(int itemid)
		{
			if (itemid < (int)Items.JobAll || itemid > (int)Items.JobBlackWizard)
			{
				return;
			}

			InternalLogger.LogInfo($"Acquired Job Items for {(Items)itemid}");
			List<int> jobToAdd = new();

			if (itemid == (int)Items.JobAll)
			{
				jobToAdd = new() { 7, 8, 9, 10, 11, 12 };
			}
			else
			{
				jobToAdd = new() { itemid - 494 };
			}

			foreach (var job in jobToAdd)
			{
				for (int i = 0; i < GameData.UserData.OwnedCharacterList.Count; i++)
				{

					GameData.UserData.OwnedCharacterList[i].OwnedJobDataList.Add(new OwnedJobData() { Id = job, Level = 1, CurrentProficiency = 0 });

					if (GameData.UserData.OwnedCharacterList[i].JobId == job - 6)
					{
						GameData.UserData.OwnedCharacterList[i].JobId = job;
					}
				}
			}
		}
		public static void InitializeJobPromotions()
		{
			if (Randomizer.Data.JobPromotion != Randomizer.JobPromotionModes.Bahamut)
			{
				GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.BahamutGivesItem, 1);
			}
			else
			{
				GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.BahamutGivesItem, 0);
			}
		}
	}
}
