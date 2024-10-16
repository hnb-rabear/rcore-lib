using RCore.Common;

namespace RCore.Data.JObject
{
	public class UserSessionHandler : JObjectHandler<JObjectDBManager>
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
				var lastActive = TimeHelper.UnixTimestampToDateTime(sessionData.lastActive);
				var utcNow = TimeHelper.UnixTimestampToDateTime(utcNowTimestamp);
				if ((utcNow - lastActive).TotalDays > 1)
					sessionData.daysStreak = 0; //Reset days streak
				if (lastActive.Day != utcNow.Day)
				{
					sessionData.days++;
					sessionData.daysStreak++;
					sessionData.sessionsDaily = 0; // Reset daily sessions count
				}
				if (TimeHelper.GetCurrentWeekNumber(lastActive) != TimeHelper.GetCurrentWeekNumber(utcNow))
					sessionData.sessionsWeekly = 0; // Reset weekly sessions count
				if (lastActive.Month != utcNow.Month)
					sessionData.sessionsMonthly = 0; // Reset monthly sessions count
			}
			sessionData.sessionsTotal++;
			sessionData.sessionsDaily++;
			sessionData.sessionsWeekly++;
			sessionData.sessionsMonthly++;
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
		public override void OnPreSave(int utcNowTimestamp)
		{
			manager.userSessionData.lastActive = utcNowTimestamp;
		}
	}
}