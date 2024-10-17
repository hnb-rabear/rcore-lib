using RCore.Data.JObject;

namespace RCore.Example.Data.JObject
{
	public class JObjectDBManagerExample : JObjectDBManager
	{
		private static JObjectDBManagerExample m_instance;
		public static JObjectDBManagerExample Instance => m_instance;

		public InventoryCollection<InvItemData> inventory;
		public InventoryHandler inventoryHandler;
		
		public InventoryRPGCollection<InvRPGItemData> inventoryRpg;
		public InventoryRPGHandler inventoryRpgHandler;

		public AchievementCollection achievement;
		public AchievementHandler achievementHandler;

		public DailyRewardCollection dailyReward;
		public DailyRewardHandler dailyRewardHandler;
		
		private void Start()
		{
			Init();
		}
		
		protected override void Load()
		{
			// Example of basic inventory collection
			inventory = CreateCollection<InventoryCollection<InvItemData>>("Inventory");
			inventoryHandler = CreateController<InventoryHandler, JObjectDBManagerExample>();
			
			// Example of a rpg inventory collection
			inventoryRpg = CreateCollection<InventoryRPGCollection<InvRPGItemData>>("InventoryRPG");
			inventoryRpgHandler = CreateController<InventoryRPGHandler, JObjectDBManagerExample>();

			// Example of Achievement collection
			achievement = CreateCollection<AchievementCollection>("Achievement");
			achievementHandler = CreateController<AchievementHandler, JObjectDBManagerExample>();
			
			// Example of DailyReward collection
			dailyReward = CreateCollection<DailyRewardCollection>("DailyReward");
			dailyRewardHandler = CreateController<DailyRewardHandler, JObjectDBManagerExample>();
		}
	}
}