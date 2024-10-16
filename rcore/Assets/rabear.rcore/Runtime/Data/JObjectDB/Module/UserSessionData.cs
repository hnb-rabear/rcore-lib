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
		public override void Save(int utcNowTimestamp, bool minimizeSize = false)
		{
			lastActive = utcNowTimestamp;
			base.Save(utcNowTimestamp, minimizeSize);
		}
	}
}