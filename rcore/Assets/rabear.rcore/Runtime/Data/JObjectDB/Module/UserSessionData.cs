using RCore.Common;
using System;
using UnityEngine;

namespace RCore.Data.JObject
{
	[Serializable]
	public class UserSessionData : JObjectCollection
	{
		public int sessions;
		public int days;
		public float activeTime;
		public int lastActive;
		public int firstActive;
		public override void OnPause(bool pause, int utcNowTimestamp, int offlineSeconds)
		{
			if (!pause)
				lastActive = utcNowTimestamp;
		}
		public override void OnPostLoad(int utcNowTimestamp, int offlineSeconds)
		{
			if (firstActive == 0)
				firstActive = utcNowTimestamp;
			if (lastActive == 0)
				lastActive = utcNowTimestamp;
			sessions++;
		}
		public override void OnUpdate(float deltaTime)
		{
			activeTime += Time.deltaTime;
		}
		public override void Save(int utcNowTimestamp, bool minimizeSize = false)
		{
			lastActive = utcNowTimestamp;
			base.Save(utcNowTimestamp, minimizeSize);
		}
	}
}