using RCore.Data.JObject;
using RCore.Example.Data;
using System;

namespace RCore.Example.Data
{
	public class DBManagerExample : JObjectDBManager
	{
		private static DBManagerExample m_instance;
		public static DBManagerExample Instance => m_instance;

		public InventoryData<InvItemData> inventoryData;
		public InventoryDataHandler inventoryDataHandler;
		
		private void Start()
		{
			Init();
		}
		
		protected override void Load()
		{
			inventoryData = CreateCollection<InventoryData<InvItemData>>("InventoryData");
			inventoryDataHandler = CreateController<InventoryDataHandler, DBManagerExample>();
		}
	}
}