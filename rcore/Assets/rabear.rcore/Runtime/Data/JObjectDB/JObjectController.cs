namespace RCore.Data.JObject
{
	public abstract class JObjectController
	{
		public JObjectDBManager manager;
		public abstract void OnPause(bool pause, int utcNowTimestamp, int offlineSeconds);
		public abstract void OnPostLoad(int utcNowTimestamp, int offlineSeconds);
		public abstract void OnUpdate(float deltaTime);
	}
}