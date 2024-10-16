using UnityEngine;

namespace RCore.Data.JObject
{
	public interface IJObjectController
	{
		public void OnPause(bool pause, int utcNowTimestamp, int offlineSeconds);
		public void OnPostLoad(int utcNowTimestamp, int offlineSeconds);
		public void OnUpdate(float deltaTime);
	}
	
	public abstract class JObjectController<T> : IJObjectController where T : JObjectDBManager
	{
		public T manager;
		public abstract void OnPause(bool pause, int utcNowTimestamp, int offlineSeconds);
		public abstract void OnPostLoad(int utcNowTimestamp, int offlineSeconds);
		public abstract void OnUpdate(float deltaTime);
	}
}