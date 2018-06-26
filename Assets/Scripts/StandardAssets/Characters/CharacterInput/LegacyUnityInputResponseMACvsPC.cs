﻿using System;
using UnityEngine;


namespace StandardAssets.Characters.CharacterInput
{
	/// <summary>
	/// Default Unity Input System implementation of the InputResponse
	/// </summary>
	[CreateAssetMenu(fileName = "InputResponseXBoneMacAndPC", menuName = "Input Response/Create Default Unity Input Response MAC and PC ",
		order = 2)]
	public class LegacyUnityInputResponseMACvsPC: InputResponse
	{
		/// <summary>
		/// Classification of the type of response
		/// </summary>
		[SerializeField]
		private DefaultInputResponseBehaviour behaviour;

		

		[SerializeField] private String axisRaw;

		[SerializeField]
		private String XBoxAxisMAC;
		[SerializeField]
		private String XBoxAxisWIN;

		/// <summary>
		/// Initializes the polling behaviour for the legacy input system
		/// </summary>
		public override void Init()
		{
			axisRaw = GetAxisXBonePlatform();
			GameObject gameObject = new GameObject();
			gameObject.name = string.Format("LegacyInput_{0}_Poller", name);
			LegacyUnityInputResponsePoller poller = gameObject.AddComponent<LegacyUnityInputResponsePoller>();
			poller.Init(this, behaviour, axisRaw);
		}

		private string GetAxisXBonePlatform()
		{
			//Only works with with XBox One for now
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			return XBoxAxisMAC;
#endif
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
			return XBoxAxisWIN;
#endif
		}

		/// <summary>
		/// Exposes the input start for the poller
		/// </summary>
		public void BroadcastStart()
		{
			OnInputStarted();
		}

		/// <summary>
		/// Exposes the input end for the poller
		/// </summary>
		public void BroadcastEnd()
		{
			OnInputEnded();
		}
	}
	
}