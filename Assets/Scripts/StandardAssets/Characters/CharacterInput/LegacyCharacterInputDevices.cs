﻿using System;
using Boo.Lang;
using UnityEngine;

namespace StandardAssets.Characters.CharacterInput
{
	[CreateAssetMenu(fileName = "LegacyCharacterInputDevices", menuName = "LegacyInput/Create Input Device Mapping",
		order = 1)]
	public class LegacyCharacterInputDevices : ScriptableObject
	{
		[SerializeField]
		private string macPlatformId = "Mac", windowsPlatformId = "Windows";

		[SerializeField]
		private string xboxOneControllerId = "XBone", xbox360ControllerId = "XBox360", ps4ControllerId = "PS4";

		[SerializeField]
		private string controlConvention = "{controller}{control}{platform}";

		private string convention;

		public string GetAxisName(string axisString)
		{
			string platformId = string.Empty;

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			platformId = macPlatformId;
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
			platformId = windowsPlatformId;
#endif
			string controllerId = string.Empty;
			if (IsXboxOne())
			{
				controllerId = xboxOneControllerId;
			}
			else if (IsXbox360())
			{
				controllerId = xbox360ControllerId;
			}
			else if (IsPS4())
			{
				controllerId = ps4ControllerId;
			}
			else
			{
				return axisString;
			}

			if (string.IsNullOrEmpty(convention))
			{
				convention = controlConvention.Replace("{platform}", "{0}").Replace("{controller}", "{1}")
				                              .Replace("{control}", "{2}");	
			}

			return string.Format(convention, platformId, controllerId, axisString);
		}

		private bool IsXboxOne()
		{
			//TODO: dave
			return false;
		}
		
		private bool IsXbox360()
		{
			//TODO: dave
			return false;
		}
		
		private bool IsPS4()
		{
			//TODO: dave
			return false;
		}
	}
}