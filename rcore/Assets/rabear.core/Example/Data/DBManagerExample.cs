using RCore.Data.JObject;
using RCore.Example.Data;
using System;
using UnityEngine.Serialization;

namespace RCore.Example.Data.JObject
{
	public class DBManagerExample : JObjectDBManager
	{
		private static DBManagerExample m_instance;
		public static DBManagerExample Instance => m_instance;

		public InventoryCollection<InvItemData> inventory;
		public InventoryHandler inventoryHandler;
		
		public InventoryRPGCollection<InvRPGItemData> inventoryRpg;
		public InventoryRPGHandler inventoryRpgHandler;
		
		private void Start()
		{
			Init();
		}
		
		protected override void Load()
		{
			// Example of basic inventory collection
			inventory = CreateCollection<InventoryCollection<InvItemData>>("Inventory");
			inventoryHandler = CreateController<InventoryHandler, DBManagerExample>();
			
			// Example of a rpg inventory inventory
			inventoryRpg = CreateCollection<InventoryRPGCollection<InvRPGItemData>>("InventoryRPG");
			inventoryRpgHandler = CreateController<InventoryRPGHandler, DBManagerExample>();
		}
	}
}