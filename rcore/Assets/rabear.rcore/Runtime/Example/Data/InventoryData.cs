using RCore.Data.JObject;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RCore.Example.Data
{
	[Serializable]
	public class InvItemData
	{
		public int id; // Auto increment id
		public int fk; // Foreign key, Id of configured item
	}

	[Serializable]
	public class InventoryData<T> : JObjectCollection where T : InvItemData
	{
		public List<T> items = new List<T>();
		public int lastItemId;
		public List<int> deletedIds = new List<int>();
		public List<int> noticedIds = new List<int>();

		public int Count => items.Count;
		public T this[int index] { get => items[index]; set => items[index] = value; }

		public virtual bool Insert(T pInvItem)
		{
			if (pInvItem.id > 0)
			{
				for (int i = 0; i < items.Count; i++)
					if (items[i].id == pInvItem.id)
					{
						Debug.LogError("Id of inventory item must be unique!");
						return false;
					}
			}
			else
			{
				int newId = lastItemId += 1;
				if (deletedIds.Count > 0)
				{
					newId = deletedIds[deletedIds.Count - 1];
					deletedIds.RemoveAt(deletedIds.Count - 1);
				}

				pInvItem.id = newId;
				noticedIds.Add(newId);
			}

			items.Add(pInvItem);

			if (pInvItem.id > lastItemId)
				lastItemId = pInvItem.id;

			return true;
		}

		public virtual bool Insert(List<T> pInvItems)
		{
			return Insert(pInvItems.ToArray());
		}

		public virtual bool Insert(params T[] pInvItems)
		{
			for (int j = 0; j < pInvItems.Length; j++)
			{
				if (pInvItems[j].id > 0)
				{
					for (int i = 0; i < items.Count; i++)
						if (items[i].id == pInvItems[j].id)
						{
							Debug.LogError("Id of inventory item must be unique!");
							return false;
						}
				}
				else
				{
					int newId = lastItemId += 1;
					if (deletedIds.Count > 0)
					{
						newId = deletedIds[deletedIds.Count - 1];
						deletedIds.RemoveAt(deletedIds.Count - 1);
					}
					pInvItems[j].id = newId;
					noticedIds.Add(newId);
				}
			}

			for (int j = 0; j < pInvItems.Length; j++)
			{
				items.Add(pInvItems[j]);
			}
			return true;
		}

		public virtual bool Update(T pInvItem)
		{
			for (int i = 0; i < items.Count; i++)
				if (items[i].id == pInvItem.id)
				{
					items[i] = pInvItem;
					return true;
				}
			Debug.LogError("Could not update item, because Id is not found!");
			return false;
		}

		public virtual bool Delete(T pInvItem)
		{
			for (int i = 0; i < items.Count; i++)
				if (items[i].id == pInvItem.id)
				{
					deletedIds.Add(items[i].id);
					items.Remove(items[i]);
					return true;
				}
			Debug.LogError("Could not delete item, because Id is not found!");
			return false;
		}

		public virtual bool Delete(int id)
		{
			for (int i = 0; i < items.Count; i++)
				if (items[i].id == id)
				{
					deletedIds.Add(items[i].id);
					items.Remove(items[i]);
					return true;
				}
			Debug.LogError("Could not delete item, because Id is not found!");
			return false;
		}

		public virtual T GetItemByIndex(int pIndex)
		{
			return items[pIndex];
		}

		public virtual T GetItemById(int pId)
		{
			if (pId > 0)
			{
				for (int i = 0; i < items.Count; i++)
					if (items[i].id == pId)
						return items[i];
			}
			return default;
		}

		public virtual void RemoveNoticedId(int pId)
		{
			noticedIds.Remove(pId);
		}

		public virtual List<T> GetNoticedItems()
		{
			var list = new List<T>();
			for (int i = 0; i < items.Count; i++)
			{
				if (noticedIds.Contains(items[i].id))
					list.Add(items[i]);
			}
			return list;
		}

		public virtual void SortById(bool des = false)
		{
			for (int i = 0; i < items.Count - 1; ++i)
			{
				for (int j = i + 1; j < items.Count; ++j)
				{
					if (items[i].id > items[j].id && !des
					    || items[i].id < items[j].id && des)
					{
						(items[i], items[j]) = (items[j], items[i]);
					}
				}
			}
		}

		public virtual void SortByBaseId(bool des = false)
		{
			for (int i = 0; i < items.Count - 1; ++i)
			{
				for (int j = i + 1; j < items.Count; ++j)
				{
					if (items[i].fk > items[j].fk && !des
					    || items[i].fk < items[j].fk && des)
					{
						(items[i], items[j]) = (items[j], items[i]);
					}
				}
			}
		}

		public virtual bool Exist(int pBaseId, out int pId)
		{
			pId = 0;
			for (int i = 0; i < items.Count; i++)
				if (items[i].fk == pBaseId)
				{
					pId = items[i].id;
					return true;
				}
			return false;
		}
	}
}