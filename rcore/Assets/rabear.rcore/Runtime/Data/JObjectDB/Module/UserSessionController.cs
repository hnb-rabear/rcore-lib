using System;
using RCore.Common;

namespace RCore.Data.JObject
{
	public class UserSessionController : JObjectController<JObjectDBManager>
	{
		public override void OnPostLoad(int utcNowTimestamp, int offlineSeconds)
		{
			var sessionData = manager.userSessionData;
			if (sessionData.firstActive == 0)
				sessionData.firstActive = utcNowTimestamp;
			if (sessionData.lastActive == 0)
			{
				sessionData.lastActive = utcNowTimestamp;
				sessionData.days++;
			}
			else
			{
				var now = TimeHelper.GetUtcNowTimestamp();
				if (now - sessionData.lastActive > 86400)
					sessionData.days++;
				if (sessionData.days == 0)
					sessionData.days = 1;
			}
			sessionData.sessions++;
		}
		public override void OnPause(bool pause, int utcNowTimestamp, int offlineSeconds)
		{
			if (!pause)
				manager.userSessionData.lastActive = utcNowTimestamp;
		}
		public override void OnUpdate(float deltaTime)
		{
			var sessionData = manager.userSessionData;
			sessionData.activeTime += deltaTime;
		}
	}
}