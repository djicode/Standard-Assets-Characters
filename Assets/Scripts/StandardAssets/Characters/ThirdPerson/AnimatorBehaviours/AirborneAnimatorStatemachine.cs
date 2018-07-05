﻿using UnityEngine;

namespace StandardAssets.Characters.ThirdPerson.AnimatorBehaviours
{
	public class AirborneAnimatorStatemachine : StateMachineBehaviour 
	{
		public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
		{
			var animationController = animator.GetComponent<ThirdPersonAnimationController>();
			if (animationController != null)
			{
				animationController.AirborneStateExit();
			}
		}
	}
}